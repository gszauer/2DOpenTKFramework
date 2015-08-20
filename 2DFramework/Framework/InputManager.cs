using System;
using System.Drawing;
using OpenTK.Input;
using System.Collections.Generic;

namespace GameFramework {
    class InputManager {
        public class ControllerMapping {
            public JoystickButton[] Buttons = new JoystickButton[] {
                JoystickButton.Button1,
                JoystickButton.Button2,
                JoystickButton.Button3,
                JoystickButton.Button4,
                JoystickButton.Button5,
                JoystickButton.Button6,
                JoystickButton.Button7,
                JoystickButton.Button8,
                JoystickButton.Button9,
                JoystickButton.Button10,
                JoystickButton.Button11,
                JoystickButton.Button12,
                JoystickButton.Button13,
                JoystickButton.Button14
            };
            public JoystickAxis[] Axis = new JoystickAxis[] {
                JoystickAxis.Axis0,
                JoystickAxis.Axis1,
                JoystickAxis.Axis2,
                JoystickAxis.Axis3
            };
            public bool[] HasButtons = new bool[15];
            public bool[] HasAxis = new bool[4];

            public JoystickButton A { get { return Buttons[0]; } set { Buttons[0] = value; HasButtons[0] = true; } }
            public JoystickButton B { get { return Buttons[1]; } set { Buttons[1] = value; HasButtons[1] = true; } }
            public JoystickButton X { get { return Buttons[2]; } set { Buttons[2] = value; HasButtons[2] = true; } }
            public JoystickButton Y { get { return Buttons[3]; } set { Buttons[3] = value; HasButtons[3] = true; } }
            public JoystickButton Start { get { return Buttons[4]; } set { Buttons[4] = value; HasButtons[4] = true; } }
            public JoystickButton Select { get { return Buttons[5]; } set { Buttons[5] = value; HasButtons[5] = true; } }
            public JoystickButton Up { get { return Buttons[6]; } set { Buttons[6] = value; HasButtons[6] = true; } }
            public JoystickButton Down { get { return Buttons[7]; } set { Buttons[7] = value; HasButtons[7] = true; } }
            public JoystickButton Left { get { return Buttons[8]; } set { Buttons[8] = value; HasButtons[8] = true; } }
            public JoystickButton Right { get { return Buttons[9]; } set { Buttons[9] = value; HasButtons[9] = true; } }
            public JoystickButton Home { get { return Buttons[10]; } set { Buttons[10] = value; HasButtons[10] = true; } }
            public JoystickButton L1 { get { return Buttons[11]; } set { Buttons[11] = value; HasButtons[11] = true; } }
            public JoystickButton L2 { get { return Buttons[12]; } set { Buttons[12] = value; HasButtons[12] = true; } }
            public JoystickButton R1 { get { return Buttons[13]; } set { Buttons[13] = value; HasButtons[13] = true; } }
            public JoystickButton R2 { get { return Buttons[14]; } set { Buttons[14] = value; HasButtons[14] = true; } }

            public JoystickAxis LeftAxisX { get { return Axis[0]; } set { Axis[0] = value; HasAxis[0] = true; } }
            public JoystickAxis LeftAxisY { get { return Axis[1]; } set { Axis[1] = value; HasAxis[1] = true; } }
            public JoystickAxis RightAxisX { get { return Axis[2]; } set { Axis[2] = value; HasAxis[2] = true; } }
            public JoystickAxis RightAxisY { get { return Axis[3]; } set { Axis[3] = value; HasAxis[3] = true; } }

            public bool HasA { get { return HasButtons[0]; } set { HasButtons[0] = value; } }
            public bool HasB { get { return HasButtons[1]; } set { HasButtons[1] = value; } }
            public bool HasX { get { return HasButtons[2]; } set { HasButtons[2] = value; } }
            public bool HasY { get { return HasButtons[3]; } set { HasButtons[3] = value; } }
            public bool HasStart { get { return HasButtons[4]; } set { HasButtons[4] = value; } }
            public bool HasSelect { get { return HasButtons[5]; } set { HasButtons[5] = value; } }
            public bool HasUp { get { return HasButtons[6]; } set { HasButtons[6] = value; } }
            public bool HasDown { get { return HasButtons[7]; } set { HasButtons[7] = value; } }
            public bool HasLeft { get { return HasButtons[8]; } set { HasButtons[8] = value; } }
            public bool HasRight { get { return HasButtons[9]; } set { HasButtons[9] = value; } }
            public bool HasHome { get { return HasButtons[10]; } set { HasButtons[10] = value; } }
            public bool HasL1 { get { return HasButtons[11]; } set { HasButtons[11] = value; } }
            public bool HasL2 { get { return HasButtons[12]; } set { HasButtons[12] = value; } }
            public bool HasR1 { get { return HasButtons[13]; } set { HasButtons[13] = value; } }
            public bool HasR2 { get { return HasButtons[14]; } set { HasButtons[14] = value; } }

            public bool HasLeftAxisX { get { return HasAxis[0]; } set { HasAxis[0] = value; } }
            public bool HasLeftAxisY { get { return HasAxis[1]; } set { HasAxis[1] = value; } }
            public bool HasRightAxisX { get { return HasAxis[2]; } set { HasAxis[2] = value; } }
            public bool HasRightAxisY { get { return HasAxis[3]; } set { HasAxis[3] = value; } }

            // TODO: ToString
            // TODO: FromString
        }

        public class ControllerState {
            public bool[] Buttons = new bool[15];

            public bool A       { get { return Buttons[0]; } set { Buttons[0] = value; } }
            public bool B       { get { return Buttons[1]; } set { Buttons[1] = value; } }
            public bool X       { get { return Buttons[2]; } set { Buttons[2] = value; } }
            public bool Y       { get { return Buttons[3]; } set { Buttons[3] = value; } }
            public bool Start   { get { return Buttons[4]; } set { Buttons[4] = value; } }
            public bool Select  { get { return Buttons[5]; } set { Buttons[5] = value; } }
            public bool Up      { get { return Buttons[6]; } set { Buttons[6] = value; } }
            public bool Down    { get { return Buttons[7]; } set { Buttons[7] = value; } }
            public bool Left    { get { return Buttons[8]; } set { Buttons[8] = value; } }
            public bool Right   { get { return Buttons[9]; } set { Buttons[9] = value; } }
            public bool Home    { get { return Buttons[10]; } set { Buttons[10] = value; } }
            public bool L1      { get { return Buttons[11]; } set { Buttons[11] = value; } }
            public bool L2      { get { return Buttons[12]; } set { Buttons[12] = value; } }
            public bool R1      { get { return Buttons[13]; } set { Buttons[13] = value; } }
            public bool R2      { get { return Buttons[14]; } set { Buttons[14] = value; } }

            public PointF LeftAxis = new PointF(0, 0);
            public PointF RightAxis = new PointF(0, 0);
        }

        private static InputManager instance = null;
        public static InputManager Instance {
            get {
                if (instance == null) {
                    instance = new InputManager();
                }
                return instance;
            }
        }

        private OpenTK.GameWindow game = null;
        private bool isInitialized = false;
        private bool[] prevKeysDown = null;
        private bool[] curKeysDown = null;
        private bool[] prevMouseDown = null;
        private bool[] curMouseDown = null;
        private Point mousePosition = new Point(0, 0);
        private PointF mouseDeltaPos = new PointF(0, 0);
        private int numJoysticks = 0;
        private ControllerMapping[] joyMapping = null;
        private ControllerState[] prevJoyDown = null;
        private ControllerState[] curJoyDown = null;
        private float[] joyDeadZone = null;

        private InputManager() {

        }

        public void Initialize(OpenTK.GameWindow window) {
            game = window;
            
            prevKeysDown = new bool[game.Keyboard.NumberOfKeys];
            curKeysDown = new bool[game.Keyboard.NumberOfKeys];
            for (int i = 0; i < game.Keyboard.NumberOfKeys; ++i) {
                prevKeysDown[i] = curKeysDown[i] = false;
            }

            prevMouseDown = new bool[game.Mouse.NumberOfButtons];
            curMouseDown = new bool[game.Mouse.NumberOfButtons];
            for (int i = 0; i < game.Mouse.NumberOfButtons; ++i) {
                prevMouseDown[i] = curMouseDown[i] = false;
            }

            numJoysticks = game.Joysticks.Count;
            joyMapping = new ControllerMapping[numJoysticks];
            prevJoyDown = new ControllerState[numJoysticks];
            curJoyDown = new ControllerState[numJoysticks];
            joyDeadZone = new float[numJoysticks];
            for (int i = 0; i < numJoysticks; ++i) {
                joyMapping[i] = new ControllerMapping();
                prevJoyDown[i] = new ControllerState();
                curJoyDown[i] = new ControllerState();
                joyDeadZone[i] = 0.0f;
            }

            isInitialized = true;
        }
        
        public void Shutdown() {
            prevKeysDown = null;
            curKeysDown = null;
            prevMouseDown = null;
            curMouseDown = null;
            joyMapping = null;
            prevJoyDown = null;
            curJoyDown = null;
            joyDeadZone = null;
            isInitialized = false;
        }

        public void Update() {
            for (int i = 0; i < curKeysDown.Length; ++i) {
                prevKeysDown[i] = curKeysDown[i];
                curKeysDown[i] = game.Keyboard[(Key)i];
            }
            for (int i = 0; i < curMouseDown.Length; ++i) {
                prevMouseDown[i] = curMouseDown[i];
                curMouseDown[i] = game.Mouse[(MouseButton)i];
            }
            mousePosition.X = game.Mouse.X;
            mousePosition.Y = game.Mouse.Y;
            mouseDeltaPos.X = ((float)game.Mouse.XDelta) / ((float)game.ClientSize.Width);
            mouseDeltaPos.Y = ((float)game.Mouse.YDelta) / ((float)game.ClientSize.Height);
            
            for (int i = 0; i < numJoysticks; ++i) {
                if (IsConnected(i)) {
                    JoystickState state = Joystick.GetState(i);

                    for (int j = 0; j < curJoyDown[i].Buttons.Length; ++j) {
                        prevJoyDown[i].Buttons[j] = curJoyDown[i].Buttons[j];
                        curJoyDown[i].Buttons[j] = joyMapping[i].HasButtons[j] ? state.GetButton(joyMapping[i].Buttons[j]) == ButtonState.Pressed : false;
                    }
                    prevJoyDown[i].LeftAxis.X = curJoyDown[i].LeftAxis.X;
                    prevJoyDown[i].LeftAxis.Y = curJoyDown[i].LeftAxis.Y;
                    prevJoyDown[i].RightAxis.X = curJoyDown[i].RightAxis.X;
                    prevJoyDown[i].RightAxis.Y = curJoyDown[i].RightAxis.Y;

                    curJoyDown[i].LeftAxis.X = joyMapping[i].HasLeftAxisX ? state.GetAxis(joyMapping[i].LeftAxisX) : 0.0f;
                    curJoyDown[i].LeftAxis.Y = joyMapping[i].HasLeftAxisY ? state.GetAxis(joyMapping[i].LeftAxisY) : 0.0f;
                    curJoyDown[i].RightAxis.X = joyMapping[i].HasRightAxisX ? state.GetAxis(joyMapping[i].RightAxisX) : 0.0f;
                    curJoyDown[i].RightAxis.Y = joyMapping[i].HasRightAxisY ? state.GetAxis(joyMapping[i].RightAxisY) : 0.0f;

                    if (curJoyDown[i].LeftAxis.X < joyDeadZone[i]) {
                        curJoyDown[i].LeftAxis.X = 0.0f;
                    }
                    if (curJoyDown[i].LeftAxis.Y < joyDeadZone[i]) {
                        curJoyDown[i].LeftAxis.Y = 0.0f;
                    }
                    if (curJoyDown[i].RightAxis.X < joyDeadZone[i]) {
                        curJoyDown[i].RightAxis.X = 0.0f;
                    }
                    if (curJoyDown[i].RightAxis.Y < joyDeadZone[i]) {
                        curJoyDown[i].RightAxis.Y = 0.0f;
                    }
                }
            }
        }

        public bool KeyDown(OpenTK.Input.Key key) {
            return curKeysDown[(int)key];
        }

        public bool KeyUp(OpenTK.Input.Key key) {
            return !curKeysDown[(int)key];
        }

        public bool KeyPressed(OpenTK.Input.Key key) {
            return (!prevKeysDown[(int)key]) && curKeysDown[(int)key];
        }

        public bool KeyReleased(OpenTK.Input.Key key) {
            return prevKeysDown[(int)key] && (!curKeysDown[(int)key]);
        }

        public bool MouseDown(OpenTK.Input.MouseButton button) {
            return curMouseDown[(int)button];
        }

        public bool MouseUp(OpenTK.Input.MouseButton button) {
            return !curMouseDown[(int)button];
        }

        public bool MousePressed(OpenTK.Input.MouseButton button) {
            return (!prevMouseDown[(int)button]) && curMouseDown[(int)button];
        }

        public bool MouseReleased(OpenTK.Input.MouseButton button) {
            return prevMouseDown[(int)button] && (!curMouseDown[(int)button]);
        }

        public int MouseX {
            get {
                return mousePosition.X;
            }
        }

        public int MouseY {
            get {
                return mousePosition.Y;
            }
        }

        public Point MousePosition {
            get {
                return mousePosition;
            }
        }

        public float MouseDeltaX {
            get {
                return mouseDeltaPos.X;
            }
        }

        public float MouseDeltaY {
            get {
                return mouseDeltaPos.Y;
            }
        }

        public PointF MouseDelta {
            get {
                return mouseDeltaPos;
            }
        }

        public void SetMousePosition(Point newPos) {
            OpenTK.Input.Mouse.SetPosition(newPos.X, newPos.Y);
        }

        public void CenterMouse() {
            double x = game.X + game.Width / 2;
            double y = game.Y + game.Height / 2;
            OpenTK.Input.Mouse.SetPosition(x, y);
        }

        public float GetDeadzone(int controllerNum) {
            return joyDeadZone[controllerNum];
        }

        public void SetDeadzone(int controller, float value) {
            joyDeadZone[controller] = value;
        }

        public bool GetButton(int joystick, ref JoystickButton button) {
            for (int i = 0; i < numJoysticks; ++i) {
                if (IsConnected(i)) {
                    JoystickState state = Joystick.GetState(i);
                    foreach (JoystickButton enumVal in Enum.GetValues(typeof(JoystickButton))) {
                        if (state.GetButton(enumVal) == ButtonState.Pressed) {
                            button = enumVal;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool GetAxis(int joystick, JoystickAxis axis) {
            for (int i = 0; i < numJoysticks; ++i) {
                if (IsConnected(i)) {
                    JoystickState state = Joystick.GetState(i);
                    foreach (JoystickAxis enumVal in Enum.GetValues(typeof(JoystickAxis))) {
                        if (state.GetAxis(enumVal) != 0.0f) { // TODO: Set this to be a deadzone
                            axis = enumVal;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Dictionary<int, string> Gamepads {
            get {
                Dictionary<int, string> result = new Dictionary<int, string>();

                for (int i = 0; i < numJoysticks; i++) {
                    JoystickCapabilities caps = Joystick.GetCapabilities(i);
                    if (caps.IsConnected) {
                        result.Add(i, game.Joysticks[i].DeviceType + ": " + game.Joysticks[i].Description);
                    }
                }

                return result;
            }
        }

        public int NumGamepads {
            get {
                int count = 0;

                for (int i = 0; i < numJoysticks; i++) {
                    JoystickCapabilities caps = Joystick.GetCapabilities(i);
                    
                    if (caps.IsConnected) {
                        count += 1;
                        Console.WriteLine("Joystick: " + i);
                        Console.WriteLine("\tDescription: " + game.Joysticks[i].Description);
                        Console.WriteLine("\tType: " + game.Joysticks[i].DeviceType);
                        Console.WriteLine("\tString: " + game.Joysticks[i].ToString());
                    }
                }
                
                return count;
            }
        }

        bool IsConnected(int padNum) {
            JoystickCapabilities caps = Joystick.GetCapabilities(padNum);
            return caps.IsConnected;
        }

        bool HasAButton(int padNum) {
            return joyMapping[padNum].HasA;
        }

        bool HasBButton(int padNum) {
            return joyMapping[padNum].HasB;
        }

        bool HasXButton(int padNum) {
            return joyMapping[padNum].HasX;
        }

        bool HasYButton(int padNum) {
            return joyMapping[padNum].HasY;
        }

        bool HasSelectButton(int padNum) {
            return joyMapping[padNum].HasSelect;
        }

        bool HasStartButton(int padNum) {
            return joyMapping[padNum].HasStart;
        }

        bool HasHomeButton(int padNum) {
            return joyMapping[padNum].HasHome;
        }

        bool HasDPad(int padNum) {
            return joyMapping[padNum].HasUp && joyMapping[padNum].HasDown && joyMapping[padNum].HasLeft && joyMapping[padNum].HasRight;
        }

        bool HasL1(int padNum) {
            return joyMapping[padNum].HasL1;
        }

        bool HasL2(int padNum) {
            return joyMapping[padNum].HasL2;
        }

        bool HasR1(int padNum) {
            return joyMapping[padNum].HasR1;
        }

        bool HasR2(int padNum) {
            return joyMapping[padNum].HasR2;
        }

        bool HasLeftStick(int padNum) {
            return joyMapping[padNum].HasLeftAxisX && joyMapping[padNum].HasLeftAxisY;
        }

        bool HasRightStick(int padNum) {
            return joyMapping[padNum].HasRightAxisX && joyMapping[padNum].HasRightAxisY;
        }       

        public bool ADown(int padNum) {
            return curJoyDown[padNum].A;
        }

        public bool AUp(int padNum) {
            return !curJoyDown[padNum].A;
        }

        public bool APressed(int padNum) {
            return (!prevJoyDown[padNum].A) && curJoyDown[padNum].A;
        }

        public bool AReleased(int padNum) {
            return prevJoyDown[padNum].A && (!curJoyDown[padNum].A);
        }

        public bool BDown(int padNum) {
            return curJoyDown[padNum].B;
        }

        public bool BUp(int padNum) {
            return !curJoyDown[padNum].B;
        }

        public bool BPressed(int padNum) {
            return (!prevJoyDown[padNum].B) && curJoyDown[padNum].B;
        }

        public bool BReleased(int padNum) {
            return prevJoyDown[padNum].B && (!curJoyDown[padNum].B);
        }

        public bool XDown(int padNum) {
            return curJoyDown[padNum].X;
        }

        public bool XUp(int padNum) {
            return !curJoyDown[padNum].X;
        }

        public bool XPressed(int padNum) {
            return (!prevJoyDown[padNum].X) && curJoyDown[padNum].X;
        }

        public bool XReleased(int padNum) {
            return prevJoyDown[padNum].X && (!curJoyDown[padNum].X);
        }

        public bool YDown(int padNum) {
            return curJoyDown[padNum].Y;
        }

        public bool YUp(int padNum) {
            return !curJoyDown[padNum].Y;
        }

        public bool YPressed(int padNum) {
            return (!prevJoyDown[padNum].Y) && curJoyDown[padNum].Y;
        }

        public bool YReleased(int padNum) {
            return prevJoyDown[padNum].Y && !curJoyDown[padNum].Y;
        }

        public bool SelectDown(int padNum) {
            return curJoyDown[padNum].Select;
        }

        public bool SelectUp(int padNum) {
            return !curJoyDown[padNum].Select;
        }

        public bool SelectPressed(int padNum) {
            return (!prevJoyDown[padNum].Select) && curJoyDown[padNum].Select;
        }

        public bool SelectReleased(int padNum) {
            return prevJoyDown[padNum].Select && (!curJoyDown[padNum].Select);
        }

        public bool StartDown(int padNum) {
            return curJoyDown[padNum].Start;
        }

        public bool StartUp(int padNum) {
            return !curJoyDown[padNum].Start;
        }

        public bool StartPressed(int padNum) {
            return (!prevJoyDown[padNum].Start) && curJoyDown[padNum].Start;
        }

        public bool StartReleased(int padNum) {
            return prevJoyDown[padNum].Start && (!curJoyDown[padNum].Start);
        }

        public bool L1Down(int padNum) {
            return curJoyDown[padNum].L1;
        }

        public bool L1Up(int padNum) {
            return !curJoyDown[padNum].L1;
        }

        public bool L1Pressed(int padNum) {
            return (!prevJoyDown[padNum].L1) && curJoyDown[padNum].L1;
        }

        public bool L1Released(int padNum) {
            return prevJoyDown[padNum].L1 && (!curJoyDown[padNum].L1);
        }

        public bool L2Down(int padNum) {
            return curJoyDown[padNum].L2;
        }

        public bool L2Up(int padNum) {
            return !curJoyDown[padNum].L2;
        }

        public bool L2Pressed(int padNum) {
            return (!prevJoyDown[padNum].L2) && curJoyDown[padNum].L2;
        }

        public bool L2Released(int padNum) {
            return prevJoyDown[padNum].L2 && (!curJoyDown[padNum].L2);
        }

        public bool R1Down(int padNum) {
            return curJoyDown[padNum].R1;
        }

        public bool R1Up(int padNum) {
            return !curJoyDown[padNum].R1;
        }

        public bool R1Pressed(int padNum) {
            return (!prevJoyDown[padNum].R1) && curJoyDown[padNum].R1;
        }

        public bool R1Released(int padNum) {
            return prevJoyDown[padNum].R1 && (!curJoyDown[padNum].R1);
        }

        public bool UpDown(int padNum) {
            return curJoyDown[padNum].Up;
        }

        public bool UpUp(int padNum) {
            return !curJoyDown[padNum].Up;
        }

        public bool UpPressed(int padNum) {
            return (!prevJoyDown[padNum].Up) && curJoyDown[padNum].Up;
        }

        public bool UpReleased(int padNum) {
            return prevJoyDown[padNum].Up && (!curJoyDown[padNum].Up);
        }

        public bool DownDown(int padNum) {
            return curJoyDown[padNum].Down;
        }

        public bool DownUp(int padNum) {
            return !curJoyDown[padNum].Down;
        }

        public bool DownPressed(int padNum) {
            return (!prevJoyDown[padNum].Down) && curJoyDown[padNum].Down;
        }

        public bool DownReleased(int padNum) {
            return prevJoyDown[padNum].Down && (!curJoyDown[padNum].Down);
        }

        public bool LeftDown(int padNum) {
            return curJoyDown[padNum].Left;
        }

        public bool LeftUp(int padNum) {
            return !curJoyDown[padNum].Left;
        }

        public bool LeftPressed(int padNum) {
            return (!prevJoyDown[padNum].Left) && curJoyDown[padNum].Left;
        }

        public bool LeftReleased(int padNum) {
            return prevJoyDown[padNum].Left && (!curJoyDown[padNum].Left);
        }

        public bool RightDown(int padNum) {
            return curJoyDown[padNum].Right;
        }

        public bool RightUp(int padNum) {
            return !curJoyDown[padNum].Right;
        }

        public bool RightPressed(int padNum) {
            return (!prevJoyDown[padNum].Right) && curJoyDown[padNum].Right;
        }

        public bool RightReleased(int padNum) {
            return prevJoyDown[padNum].Right && (!curJoyDown[padNum].Right);
        }

        public float LeftStickX(int padNum) {
            return curJoyDown[padNum].LeftAxis.X;
        }

        public float LeftStickY(int padNum) {
            return curJoyDown[padNum].LeftAxis.Y;
        }

        public PointF LeftStick(int padNum) {
            return curJoyDown[padNum].LeftAxis;
        }

        public float RightStickX(int padNum) {
            return curJoyDown[padNum].RightAxis.X;
        }

        public float RightStickY(int padNum) {
            return curJoyDown[padNum].RightAxis.Y;
        }
        
        public PointF RightStick(int padNum) {
            return curJoyDown[padNum].RightAxis;
        }
    }
}