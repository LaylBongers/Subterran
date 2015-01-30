using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subterran.Toolbox.SimplePhysics
{
	public class PhysicsWorldComponent :EntityComponent, IUpdatable
	{
		private TimeSpan _accumulator;

		public PhysicsWorldComponent()
		{
			// Default values
			Timestep = TimeSpan.FromSeconds(0.01);
		}

		public TimeSpan Timestep { get; set; }

		public void Update(TimeSpan elapsed)
		{
			var rigidBodies = Entity
				.Children
				.Select(e => new Tuple<Entity, RigidbodyComponent>(e, e.GetComponent<RigidbodyComponent>()))
				.Where(t => t.Item2 != null)
				.ToList();

			_accumulator += elapsed;
			while (_accumulator > Timestep)
			{
				_accumulator -= Timestep;

				foreach (var body in rigidBodies)
				{
					body.Item1.Position += Timestep.PerSecond(body.Item2.Velocity);
				}
			}
		}
	}
}
