using System;
using OpenTK;
using OpenTK.Input;
using System.Drawing;
using GameFramework;

namespace SomeNamespace {
    class MainClass {
        public static OpenTK.GameWindow Window = null;

        public static void Initialize(object sender, EventArgs e) {
            // INITIALIZE GAME
        }

        public static void Update(object sender, FrameEventArgs e) {
            // UPDATE GAME
        }

        public static void Render(object sender, FrameEventArgs e) {
            // RENDER GAME
        }

        public static void Shutdown(object sender, EventArgs e) {
            // SHUTDOWN GAME
        }

        [STAThread]
        public static void Main() {
            // Create static (global) window instance
            Window = new OpenTK.GameWindow();

            // Hook up the initialize callback
            Window.Load += new EventHandler<EventArgs>(Initialize);
            // Hook up the update callback
            Window.UpdateFrame += new EventHandler<FrameEventArgs>(Update);
            // Hook up the render callback
            Window.RenderFrame += new EventHandler<FrameEventArgs>(Render);
            // Hook up the shutdown callback
            Window.Unload += new EventHandler<EventArgs>(Shutdown);

            // Set window title and size
            Window.Title = "Game Name";
            Window.Size = new Size(800, 600);

            // Run the game at 60 frames per second. This method will NOT return
            // until the window is closed.
            Window.Run(60.0f);

            // If we made it down here the window was closed. Call the windows
            // Dispose method to free any resources that the window might hold
            Window.Dispose();
        }
    }
}