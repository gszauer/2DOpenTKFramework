using System;
using OpenTK;
using OpenTK.Input;
using System.Drawing;
using GameFramework;
using System.Collections.Generic;

namespace SomeNamespace {
    class MainClass {
        public static OpenTK.GameWindow Window = null;

        private static RectangleF position = new RectangleF(100, 100, 20, 20);
        private static Color[] colors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Yellow };
        private static int currentColor = 0;

        public static void Initialize(object sender, EventArgs e) {
            GraphicsManager.Instance.Initialize(Window);
            InputManager.Instance.Initialize(Window);
        }

        public static void Update(object sender, FrameEventArgs e) {
            InputManager.Instance.Update();

            if (InputManager.Instance.APressed(0)) {
                currentColor += 1;
                if (currentColor >= colors.Length) {
                    currentColor = 0;
                }
            }

            if (InputManager.Instance.LeftStickX(0) < 0) { // < 0 = left
                position.X -= 80.0f * Math.Abs(InputManager.Instance.LeftStickX(0)) * (float)e.Time;
            }
            else if (InputManager.Instance.LeftStickX(0) > 0) { // < 0 = right
                position.X += 80.0f * Math.Abs(InputManager.Instance.LeftStickX(0)) * (float)e.Time;
            }

            if (InputManager.Instance.LeftStickY(0) < 0) { // y < 0 = down
                position.Y += 80.0f * Math.Abs(InputManager.Instance.LeftStickY(0)) * (float)e.Time;
            }
            else if (InputManager.Instance.LeftStickY(0) > 0) { // y > 0 = up
                position.Y -= 80.0f * Math.Abs(InputManager.Instance.LeftStickY(0)) * (float)e.Time;
            }
        }

        public static void Render(object sender, FrameEventArgs e) {
            GraphicsManager.Instance.ClearScreen(Color.CadetBlue);

            if (!InputManager.Instance.IsConnected(0)) {
                GraphicsManager.Instance.DrawString("Please connect a controller", new PointF(10, 10), Color.Red);
            }
            else if (!InputManager.Instance.HasAButton(0)) {
                GraphicsManager.Instance.DrawString("A button not mapped, press A button", new PointF(10, 10), Color.Red);
                JoystickButton newAButton = JoystickButton.Button0; // Some default

                if (InputManager.Instance.GetButton(0, ref newAButton)) {
                    InputManager.Instance.GetMapping(0).A = newAButton;
                }
            }
            else if (!InputManager.Instance.HasLeftStick(0)) {
                InputManager.ControllerMapping map = InputManager.Instance.GetMapping(0);

                if (!map.HasLeftAxisX) {
                    GraphicsManager.Instance.DrawString("X axis not found, move left stick horizontally (sizeways)", new PointF(10, 10), Color.Red);
                }
                else if (!map.HasLeftAxisY) {
                    GraphicsManager.Instance.DrawString("Y axis not found, move left stick vertically (up or down)", new PointF(10, 10), Color.Red);
                }

                JoystickAxis newAxis = JoystickAxis.Axis0; // Some default
                if (map.HasLeftAxisX) { // We have X, need Y
                    if (InputManager.Instance.GetAxis(0, ref newAxis, map.LeftAxisX)) {
                        map.LeftAxisY = newAxis;
                    }
                }
                else {
                    if (InputManager.Instance.GetAxis(0, ref newAxis)) {
                        map.LeftAxisX = newAxis;
                    }
                }
            }
            else {
                GraphicsManager.Instance.DrawRect(position, colors[currentColor]);
            }

            int y = 0;
            JoystickState state = Joystick.GetState(0);
            InputManager.ControllerMapping buttons = InputManager.Instance.GetMapping(0);
            foreach (JoystickAxis enumVal in Enum.GetValues(typeof(JoystickAxis))) {
                Color clr = Color.Black;
                if (enumVal == buttons.LeftAxisX) {
                    clr = Color.Purple;
                }
                if (enumVal == buttons.LeftAxisY) {
                    clr = Color.Blue;
                }
                GraphicsManager.Instance.DrawString(enumVal.ToString() + ": " + state.GetAxis(enumVal), new PointF(10, 30 + y), clr);
                y += 20;
            }

            GraphicsManager.Instance.SwapBuffers();
        }

        public static void Shutdown(object sender, EventArgs e) {
            InputManager.Instance.Shutdown();
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