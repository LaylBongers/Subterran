using System.Collections.Generic;

namespace Subterran.Toolbox.SimplePhysics
{
	/// <summary>
	///     Provides an interface for components to provide a large amount of
	///     fixedbody colliders with a smart lookup for collision checking.
	/// </summary>
	public interface ISmartFixedbodySource
	{
		IEnumerable<BoundingBox> GetBoundingBoxesWithin(BoundingBox collisionArea);
	}
}