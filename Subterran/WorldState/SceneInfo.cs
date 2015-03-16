namespace Subterran.WorldState
{
	public class SceneInfo
	{
		public EntityInfo Root { get; set; }

		public Entity ToWorld()
		{
			return Root.ToEntity();
		}
	}
}