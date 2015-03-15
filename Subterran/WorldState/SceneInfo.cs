namespace Subterran.WorldState
{
	public class SceneInfo
	{
		public EntityInfo Root { get; set; }

		public Entity ToWorld()
		{
			return EntityInfoToEntity(Root);
		}

		private static Entity EntityInfoToEntity(EntityInfo entityInfo)
		{
			var entity = new Entity
			{
				Name = entityInfo.Name
			};

			return entity;
		}
	}
}