using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace GameFramework {
    public class GraphicsManager {
        private static GraphicsManager instance = null;
        public static GraphicsManager Instance {
            get {
                if (instance == null) {
                    instance = new GraphicsManager();
                }
                return instance;
            }
        }

        private GraphicsManager() {

        }

        // For font rendering
        private int originalW = 0;
        private int originalH = 0;
        private int fontWidth = 0;
        private int fontHeight = 0;
        private int fontHandle = 0;
        private int charWidth = 0;
        private int charHeight = 0;

        // TODO: Add Font DATA

        public float Depth {
            get {
                return currentDepth;
            }
        }

        private Color lastClear = Color.Red;
        private OpenTK.GameWindow game = null;
        private float currentDepth = -1.0f;
        private bool isInitialized = false;

        private void Error(string error) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = old;
        }

        public void Initialize(OpenTK.GameWindow window) {
            if (isInitialized) {
                Error("Trying to double intialize graphics manager!");
            }

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            lastClear = Color.CadetBlue;
            game = window;

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);

            isInitialized = true;

            SetScreenSize(game.Width, game.Height);

            game.Load += (sender, e) => {
                game.VSync = OpenTK.VSyncMode.On;
            };

            game.Resize += (sender, e) => {
                SetScreenSize(game.Width, game.Height);
            };
        }

        public void Shutdown() {
            if (!isInitialized) {
                Error("Trying to shut down a non initialized graphics manager!");
            }
            game = null;
            isInitialized = false;
        }

        public void SetScreenSize(int width, int height) {
            if (!isInitialized) {
                Error("Trying to set screen size without intializing graphics manager!");
            }
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, width, height, 0, -1.0, 1.0);
            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        public void ClearScreen(Color clearColor) {
            if (!isInitialized) {
                Error("Trying to clear screen size without intializing graphics manager!");
            }
            if (clearColor != lastClear) {
                GL.ClearColor(clearColor);
            }
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            currentDepth = -1.0f;
        }

        public void SwapBuffers() {
            if (!isInitialized) {
                Error("Trying to swap buffers without intializing graphics manager!");
            }

            game.SwapBuffers();
        }

        public void IncreaseDepth() {
            if (!isInitialized) {
                Error("Trying to increase depth without intializing graphics manager!");
            }
            currentDepth += 0.0005f;
            if (currentDepth > 1.0f) {
                currentDepth = 1.0f;
            }
        }

        public void DrawRect(Rectangle rect, Color c) {
            RectangleF rf = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            DrawRect(rf, c);
        }

        public void DrawLine(Point p1, Point p2, Color c) {
            PointF pf1 = new PointF(p1.X, p1.Y);
            PointF pf2 = new PointF(p2.X, p2.Y);
            DrawLine(pf1, pf2, c);
        }

        public void DrawRect(RectangleF rect, Color c) {
            if (!isInitialized) {
                Error("Trying to draw rect without intializing graphics manager!");
            }
            IncreaseDepth();

            GL.Color3(c.R, c.G, c.B);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(rect.X, rect.Y + rect.Height, currentDepth);
            GL.Vertex3(rect.X + rect.Width, rect.Y + rect.Height, currentDepth);
            GL.Vertex3(rect.X + rect.Width, rect.Y, currentDepth);
            GL.Vertex3(rect.X, rect.Y, currentDepth);
            GL.End();
        }

        public void DrawLine(PointF p1, PointF p2, Color c) {
            if (!isInitialized) {
                Error("Trying to draw line without intializing graphics manager!");
            }
            IncreaseDepth();

            GL.Color3(c.R, c.G, c.B);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(p1.X, p1.Y, currentDepth);
            GL.Vertex3(p2.X, p2.Y, currentDepth);
            GL.End();
        }

        public void DrawString(Point position, Color color, string str) {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();

            IncreaseDepth();

            // TODO: ZDepth accurate text
            float charPieceX = ((float)originalW / (float)fontWidth) / 95.0f;
            float[] vertices = new float[str.Length * 2 * 4];
            float[] texcoords = new float[str.Length * 2 * 4];
            float[] colors = new float[str.Length * 3 * 4];
            for (int i = 0; i < str.Length * 4 * 3; i += 3) {
                colors[i + 0] = (float)color.R / 255.0f;
                colors[i + 1] = (float)color.G / 255.0f;
                colors[i + 2] = (float)color.B / 255.0f;
            }
            GL.BindTexture(TextureTarget.Texture2D, fontHandle);

            for (int count = 0; count < str.Length; count++) { 
                vertices[count * 2 * 4 + 0] = position.X + charWidth * count;
                vertices[count * 2 * 4 + 1] = position.Y;
                //vertices[count * 2 * 4 + 1] = Depth;
                texcoords[count * 2 * 4 + 0] = charPieceX * (str[count] - 32);
                texcoords[count * 2 * 4 + 1] = 0.0f;
                vertices[count * 2 * 4 + 2] = position.X + charWidth * count;
                vertices[count * 2 * 4 + 3] = position.Y + charHeight;
                //vertices[count * 2 * 4 + 3] = Depth;
                texcoords[count * 2 * 4 + 2] = charPieceX * (str[count] - 32);
                texcoords[count * 2 * 4 + 3] = (float)originalH / (float)fontHeight;
                vertices[count * 2 * 4 + 4] = position.X + charWidth * (count + 1);
                vertices[count * 2 * 4 + 5] = position.Y + charHeight;
                //vertices[count * 2 * 4 + 5] = Depth;
                texcoords[count * 2 * 4 + 4] = charPieceX * (str[count] - 32 + 1);
                texcoords[count * 2 * 4 + 5] = (float)originalH / (float)fontHeight;
                vertices[count * 2 * 4 + 6] = position.X + charWidth * (count + 1);
                vertices[count * 2 * 4 + 7] = position.Y;
                //vertices[count * 2 * 4 + 7] = Depth;
                texcoords[count * 2 * 4 + 6] = charPieceX * (str[count] - 32 + 1);
                texcoords[count * 2 * 4 + 7] = 0.0f;
            }

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.VertexPointer(2, VertexPointerType.Float, 0, vertices);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, texcoords);
            GL.ColorPointer(3, ColorPointerType.Float, 0, colors);
            GL.DrawArrays(PrimitiveType.Quads, 0, str.Length * 4);
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);

            GL.PopMatrix();
        }
    }
}
