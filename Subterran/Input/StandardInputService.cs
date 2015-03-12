using OpenTK.Input;

namespace Subterran.Input
{
	public sealed class StandardInputService : IInputService
	{
		public bool IsKeyDown(Key escape)
		{
			var keyState = Keyboard.GetState();
			return keyState.IsKeyDown(escape);
		}
	}
}