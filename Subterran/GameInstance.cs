using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Subterran
{
	public class GameInstance
	{
		private readonly GameInfo _info;

		public GameInstance(GameInfo info)
		{
			_info = info;
		}

		public Collection<object> Services { get; } = new Collection<object>();

		public void Run()
		{
			StartServices();

			StopServices();
		}

		private void StartServices()
		{
			// Initialize all the services
			foreach (var serviceInfo in _info.Services)
			{
				var constructor = serviceInfo.ServiceType.GetConstructor(new Type[0]);

				if (constructor == null)
					throw new NotSupportedException("Dependency injection is not yet supported.");

				Services.Add(constructor.Invoke(new object[] {}));
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
	}
}