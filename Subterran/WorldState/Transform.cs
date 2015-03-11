using OpenTK;

namespace Subterran.WorldState
{
	public sealed class Transform
	{
		private Vector3 _scale;

		public Transform()
		{
			// Scale by default needs to be 1 because 0 will give invisible entities
			Scale = new Vector3(1, 1, 1);
		}

		internal Entity OwningEntity { get; set; }
		public Vector3 Position { get; set; }
		public Vector3 Rotation { get; set; }

		public Vector3 Scale
		{
			get { return _scale; }
			set
			{
				_scale = value;
				InverseScale = new Vector3(1/value.X, 1/value.Y, 1/value.Z);
			}
		}

		public Vector3 InverseScale { get; private set; }

		public Vector3 WorldPosition
		{
			get
			{
				return OwningEntity.Parent != null
					? Vector3.Transform(Position, OwningEntity.Parent.Transform.WorldMatrix)
					: Position;
			}
		}

		public Matrix4 Matrix
		{
			get
			{
				// Create a multiply matrix representing this entity
				return
					Matrix4.CreateScale(Scale)*
					Matrix4.CreateRotationX(Rotation.X)*
					Matrix4.CreateRotationY(Rotation.Y)*
					Matrix4.CreateRotationZ(Rotation.Z)*
					Matrix4.CreateTranslation(Position);
			}
		}

		public Matrix4 WorldMatrix
		{
			get
			{
				// Multiply the entity matrix with the parent's world matrix
				return OwningEntity.Parent != null
					? OwningEntity.Parent.Transform.WorldMatrix*Matrix
					: Matrix;
			}
		}
	}
}