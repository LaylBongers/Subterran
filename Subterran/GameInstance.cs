using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Subterran
{
	/// <summary>
	///     Represents an instance of the Subterran game engine.
	/// </summary>
	public class GameInstance
	{
		private readonly GameInfo _info;
		private readonly Collection<object> _services = new Collection<object>();
		private IGameLoop _gameLoop;

		/// <summary>
		///     Initializes a new Subterran game engine instance using the provided game info.
		/// </summary>
		/// <param name="info">The information about the Subterran game.</param>
		public GameInstance(GameInfo info)
		{
			StContract.ArgumentNotNull(info, "info");

			Name = info.Name;
			Services = new ReadOnlyCollection<object>(_services);

			_info = (GameInfo) info.Clone();
		}

		/// <summary>
		///     Gets the collection of services actively in use by this instance.
		/// </summary>
		public ReadOnlyCollection<object> Services { get; }

		/// <summary>
		///     Gets the name of the game.
		/// </summary>
		public string Name { get; }

		/// <summary>
		///     Runs this instance of the Subterran game engine.
		/// </summary>
		public void Run()
		{
			StartServices();

			_gameLoop = (IGameLoop) ConstructUsingDependencies(_info.GameLoopType, CreateDependenciesList().ToList());
			_gameLoop.Run();
			_gameLoop = null;

			StopServices();
		}

		private IEnumerable<object> CreateDependenciesList()
		{
			return _services
				.ConcatOne(this)
				.ConcatOneIfNotDefault(_gameLoop);
		}

		private void StartServices()
		{
			var unsorted = GetServiceConstructors();
			var sorted = SortServiceConstructors(unsorted);

			// Actually construct all the services
			foreach (var constructor in sorted)
			{
				var service = ConstructUsingDependencies(constructor,
					CreateDependenciesList()
						// If a ServiceInfo is requested it should give its own info
						.ConcatOne(_info.Services.First(s => s.ServiceType == constructor.DeclaringType))
						.ToList());
				_services.Add(service);
			}
		}

		private void StopServices()
		{
			// We can only stop services that are disposable
			foreach (var service in Services.OfType<IDisposable>())
			{
				service.Dispose();
			}
		}

		private List<ConstructorInfo> GetServiceConstructors()
		{
			// Detect all the services
			var serviceConstructors = new List<ConstructorInfo>();
			foreach (var serviceInfo in _info.Services)
			{
				serviceConstructors.Add(GetConstructorForType(serviceInfo.ServiceType));
			}

			return serviceConstructors;
		}

		private static ConstructorInfo GetConstructorForType(Type type)
		{
			var constructors = type.GetConstructors();

			if (constructors.Length == 0)
				throw new InvalidOperationException("Unable to find any constructors in " + type);
			if (constructors.Length > 1)
				throw new InvalidOperationException("More than one constructor found in " + type);

			return constructors[0];
		}

		private static List<ConstructorInfo> SortServiceConstructors(List<ConstructorInfo> unsorted)
		{
			// Sort the constructors so dependencies can be resolved
			var sorted = new List<ConstructorInfo>();
			var itemNumber = 0;
			while (unsorted.Count != 0)
			{
				var constructor = unsorted[itemNumber];
				var available = sorted
					.Select(c => c.DeclaringType)
					// Service info is always available as special dependency
					.ConcatOne(typeof (ServiceInfo))
					.ToList();

				if (AreDependenciesAvailable(constructor, available))
				{
					// All dependencies of this one are resolved, we can add it to the sorted list
					sorted.Add(constructor);
					unsorted.Remove(constructor);

					// Go back to the start for a new pass of checks to see if there's anything we can construct now
					itemNumber = 0;
				}
				else
				{
					// It's not yet in the list of services, we can't construct it yet
					// Let's try the next one instead then
					itemNumber++;

					// If there is no next one, there's a problem
					// This means there's nothing in the list we can construct anymore
					if (itemNumber >= unsorted.Count)
					{
						throw new InvalidOperationException(
							"Can not resolve dependencies for " + constructor.DeclaringType + ".");
					}
				}
			}

			return sorted;
		}

		private static bool AreDependenciesAvailable(ConstructorInfo constructor, List<Type> availableTypes)
		{
			var reqParameters = constructor.GetParameters();

			// If there's no parameters, there's no dependencies to resolve
			if (reqParameters.Length == 0)
				return true;

			// Go through all parameters
			foreach (var reqParam in reqParameters)
			{
				var requestedType = reqParam.ParameterType;

				// Check if the requested time is available
				if (availableTypes.Any(c => requestedType.IsAssignableFrom(c)))
					continue;

				// It's not in the list of available parameters, so dependencies are not available
				return false;
			}

			// No unavailable dependencies were found
			return true;
		}

		private static object ConstructUsingDependencies(Type type, IList<object> availableDependencies)
		{
			var constructor = GetConstructorForType(type);
			return ConstructUsingDependencies(constructor, availableDependencies);
		}

		private static object ConstructUsingDependencies(ConstructorInfo constructor, IList<object> availableDependencies)
		{
			// Get all the requested parameters
			var reqParams = constructor.GetParameters();

			// Find a matching service for each parameter
			var resolvedParams = new List<object>();
			foreach (var reqParam in reqParams)
			{
				// Add the first dependency matching what's requested to the list of parameters
				var availableParam = availableDependencies.FirstOrDefault(s => reqParam.ParameterType.IsInstanceOfType(s));

				if (availableParam == null)
					throw new InvalidOperationException("Unable to resolve dependency " + reqParam.ParameterType + ".");

				resolvedParams.Add(availableParam);
			}

			return constructor.Invoke(resolvedParams.ToArray());
		}
	}
}