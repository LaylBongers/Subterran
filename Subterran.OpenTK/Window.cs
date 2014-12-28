using System;
using System.ComponentModel;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;

namespace Subterran.OpenTK
{
	public sealed class Window : Disposable
	{
		public Window(ScreenSize size)
		{
			OpenTkWindow = new GameWindow(
				size.X, size.Y,
				// Deferred rendering so no samples.
				// If you want AA, it has to be post-process.
				new GraphicsMode(32, 16, 0, 0),
				"Subterran",
				GameWindowFlags.FixedWindow)
			{
				Visible = true,
				VSync = VSyncMode.Adaptive
			};
			OpenTkWindow.Closing += OnClosing;
		}

		internal GameWindow OpenTkWindow { get; set; }

		public string Title
		{
			get { return OpenTkWindow.Title; }
			set { OpenTkWindow.Title = value; }
		}

		public ScreenSize Size
		{
			get { return new ScreenSize(OpenTkWindow.Width, OpenTkWindow.Height); }
			set { OpenTkWindow.Size = new Size(value.X, value.Y); }
		}

		public event EventHandler Closing = (s, e) => { };

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				OpenTkWindow.Dispose();
			}

			base.Dispose(managed);
		}

		public void ProcessEvents()
		{
			OpenTkWindow.ProcessEvents();
		}

		public void SwapBuffers()
		{
			OpenTkWindow.SwapBuffers();
		}

		public void MakeCurrent()
		{
			if (!OpenTkWindow.Context.IsCurrent)
			{
				OpenTkWindow.MakeCurrent();
			}
		}

		private void OnClosing(object sender, CancelEventArgs args)
		{
			Closing(this, EventArgs.Empty);
		}
	}
}