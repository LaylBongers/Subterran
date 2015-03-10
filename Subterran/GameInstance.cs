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

		/// <summary>
		///     Initializes a new Subterran game engine instance using the provided game info.
		/// </summary>
		/// <param name="info">The information about the Subterran game.</param>
		public GameInstance(GameInfo info)
		{
			StContract.ArgumentNotNull(info, "info");

			_info = (GameInfo) info.Clone();
		}

		public Collection<object> Services { get; } = new Collection<object>();

		public void Run()
		{
			StartServices();

			StopServices();
		}

		private void StartServices()
		{
			var unsorted = GetServiceConstructors();
			var sorted = SortServiceConstructors(unsorted);

			// Actually construct all the services
			foreach (var constructor in sorted)
			{
				// Get all the requested parameters
				var reqParams = constructor.GetParameters();

				// Find a matching service for each parameter
				var resolvedParams = new List<object>();
				foreach (var reqParam in reqParams)
				{
					// Add the first service matching what's requested to the list of parameters
					resolvedParams.Add(Services.First(s => reqParam.ParameterType.IsInstanceOfType(s)));
				}
				
				Services.Add(constructor.Invoke(resolvedParams.ToArray()));
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
				var constructors = serviceInfo.ServiceType.GetConstructors();

				if (constructors.Length == 0)
					throw new InvalidOperationException("Unable to find any constructors in " + serviceInfo.ServiceType);
				if (constructors.Length > 1)
					throw new InvalidOperationException("More than one constructor found in " + serviceInfo.ServiceType);

				serviceConstructors.Add(constructors[0]);
			}

			return serviceConstructors;
		}

		private List<ConstructorInfo> SortServiceConstructors(List<ConstructorInfo> unsorted)
		{
			// Sort the constructors so dependencies can be resolved
			var sorted = new List<ConstructorInfo>();
			var itemNumber = 0;
			while (unsorted.Count != 0)
			{
				var constructor = unsorted[itemNumber];
				var reqParameters = constructor.GetParameters();

				// If there's no parameters, there's no dependencies to resolve
				if (reqParameters.Length == 0)
					goto DependenciesResolved;

				// Go through all parameters
				foreach (var reqParam in reqParameters)
				{
					var requestedType = reqParam.ParameterType;

					// If this one's in the list of already sorted, it's fine
					if (sorted.Any(c => requestedType.IsAssignableFrom(c.DeclaringType)))
						continue;

					// it's not in the list, so we need to wait
					goto DependenciesUnresolved;
				}

				// There were no problems with the dependencies found, so they're all resolved
				goto DependenciesResolved;
				
				// ==== Unresolved ====
				DependenciesUnresolved:
				// It's not yet in the list of services, we can't construct it yet
				// Let's try the next one instead then
				itemNumber++;

				// If there is no next one, there's a problem
				// This means there's nothing in the list we can construct anymore
				if (itemNumber >= unsorted.Count)
					throw new InvalidOperationException("Can not resolve dependencies for " + constructor.DeclaringType);

				continue;

				// ==== Resolved ====
				DependenciesResolved:
				// All dependencies of this one are resolved, we can add it to the sorted list
				sorted.Add(constructor);
				unsorted.Remove(constructor);

				// Go back to the start for a new pass of checks to see if there's anything we can construct now
				itemNumber = 0;
			}

			return sorted;
		}
	}
}