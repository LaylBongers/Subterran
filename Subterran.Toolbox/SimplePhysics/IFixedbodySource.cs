using System.Collections.Generic;

namespace Subterran.Toolbox.SimplePhysics
{
	/// <summary>
	///     Provides an interface for components to provide a large amount of fixedbody colliders.
	/// </summary>
	public interface IFixedbodySource
	{
		IEnumerable<CubeCollider> Colliders { get; }
	}
}