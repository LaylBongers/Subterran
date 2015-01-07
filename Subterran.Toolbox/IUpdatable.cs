using System;

namespace Subterran.Toolbox
{
	public interface IUpdatable
	{
		void Update(TimeSpan elapsed);
	}
}