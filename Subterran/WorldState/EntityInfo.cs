using System.Collections.ObjectModel;

namespace Subterran.WorldState
{
	public class EntityInfo
	{
		public string Name { get; set; }
		public Collection<EntityInfo> Children { get; } = new Collection<EntityInfo>();

		public Entity ToEntity()
		{
			var entity = new Entity
			{
				Name = Name
			};

			foreach (var child in Children)
			{
				entity.Children.Add(child.ToEntity());
			}

			return entity;
		}
	}
}