using System;
using OpenTK;
using OpenTK.Input;
using System.Drawing;
using GameFramework;

namespace SomeNamespace {
    class MainClass {
        public static OpenTK.GameWindow Window = null;

        public static Rectangle body = new Rectangle(128, 0, 128, 128);
        public static Rectangle head = new Rectangle(0, 64, 64, 64);
        public static Rectangle leg = new Rectangle(64, 0, 64, 64);
        public static Rectangle arm = new Rectangle(64, 0, 64, 64);
        public static Rectangle bicep = new Rectangle(64, 64, 64, 64);
        public static Rectangle hand = new Rectangle(0, 0, 64, 64);

        public static int robot = -1;
        public static float currentRotation = 0.0f;

        public static void Initialize(object sender, EventArgs e) {
            GraphicsManager.Instance.Initialize(Window);
            TextureManager.Instance.Initialize(Window);

            robot = TextureManager.Instance.LoadTexture("Assets/BigRobot.png");
        }

        public static void Update(object sender, FrameEventArgs e) {
            currentRotation += (float)e.Time * 30.0f;
            while (currentRotation > 360.0f) {
                currentRotation -= 360.0f;
            }
        }

        public static void Render(object sender, FrameEventArgs e) {
            GraphicsManager.Instance.ClearScreen(Color.CadetBlue);
            GraphicsManager g = GraphicsManager.Instance;
            TextureManager t = TextureManager.Instance;

            t.Draw(robot, new Point(320, 20));

            t.Draw(robot, new Point(212, 85), 0.75f, leg);
            t.Draw(robot, new Point(172, 85), new PointF(-0.75f, 0.75f), leg);
            t.Draw(robot, new Point(128, 0), 1.0f, body);
            t.Draw(robot, new Point(168, 31), 0.75f, head);
            t.Draw(robot, new Point(219, 41), 0.75f, bicep);
            t.Draw(robot, new Point(165, 41), new PointF(-0.75f, 0.75f), bicep);
            t.Draw(robot, new Point(224, 111), new PointF(0.75f, -0.75f), arm);
            t.Draw(robot, new Point(159, 111), 0.75f, arm, new Point(0, 0), 180.0f);
            t.Draw(robot, new Point(226, 75), 0.75f, hand, currentRotation);
            t.Draw(robot, new Point(157, 75), new PointF(-0.75f, 0.75f), hand, currentRotation);

            GraphicsManager.Instance.SwapBuffers();
        }

        public static void Shutdown(object sender, EventArgs e) {
            TextureManager.Instance.UnloadTexture(robot);

            robot = -1;

            TextureManager.Instance.Shutdown();
            GraphicsManager.Instance.Shutdown();
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
            Window.ClientSize = new Size(800, 600);

            // Run the game at 60 frames per second. This method will NOT return
            // until the window is closed.
            Window.Run(60.0f);

            // If we made it down here the window was closed. Call the windows
            // Dispose method to free any resources that the window might hold
            Window.Dispose();

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}