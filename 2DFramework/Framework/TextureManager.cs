//#define SAVE_PNG_BYTES
using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace GameFramework {
    public class TextureManager {
        // TextureInstance: Provides a reference counted instance to a texture
        // we keep track of the hardware accelerated textureId (glHandle),
        // the original path of the texture, it's width and height
        // We also keep track of refCount, this is how many times the texture
        // is loaded. At the end of the application, refCount should be 0
        // if the refCount of a texture is 0, it is no longer in use, we
        // know that it's safe to recycle the texture when it's ref count
        // is 0
        private class TextureInstance {
            public int glHandle = -1;
            public string path = string.Empty;
            public int refCount = 0;
            public int width = 0;
            public int height = 0;
        }

        // A list (vector) of all the texture instances currently available
        // not every texture instance has a ref count > 0. Look at the 
        // LoadTexture function for details on how this works.
        private List<TextureInstance> managedTextures = null;

        // Helper used to warn if any method is called without first
        // intilizing the manager
        private bool isInitialized = false;

        // The ONLY instance of TextureManager. No class outside of the
        // manager can access this instance. This variable, the Instance getter
        // and private constructor make this class a singleton.
        private static TextureManager instance = null;

        // Lazy accessor, no instance of TextureManager exists until the very
        // first time a user tries to access Instance. Once Instance is accessed
        // a TextureManager will exist until the application quits
        public static TextureManager Instance {
            get {
                if (instance == null) {
                    instance = new TextureManager();
                }
                return instance;
            }
        }

        // The constructor is private to prevent anything other than the Instance
        // getter from creating a new instance of TextureManager, this is what
        // makes this class a singleton.
        private TextureManager() {

        }

        // Utility function to log red text to the console
        private void Error(string error) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = old;
        }

        // Utility function to log yellow text to the console
        private void Warning(string error) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(error);
            Console.ForegroundColor = old;
        }

        // Set up all necesary member variables of the texture manager
        // The argument this manager takes is ignored, we include it to
        // make all managers have a similar interface. Usually you only
        // initialize a manager at the start of the application.
        public void Initialize(OpenTK.GameWindow window) {
            if (isInitialized) {
                Error("Trying to double intialize texture manager!");
            }
            managedTextures = new List<TextureInstance>();
            // List.Capacity = Vector.Reserve
            // We reserve enough room for 100 textures, if 101
            // textures are loaded, room will be reserved for 200
            // textures. This will keep doubling as vectors do
            managedTextures.Capacity = 100;
            isInitialized = true;
        }

        // Deallocate all memory the texture manager is currently using, flip the
        // initialization flag back to false. After calling shutdown the texture
        // manager is invalid, unless Initialize is called again. Usually you only
        // shut down a manager at the end of the application.
        public void Shutdown() {
            if (!isInitialized) {
                Error("Trying to shut down a non initialized texture manager!");
            }
            // Loop trough all loaded textures
            for (int i = 0; i < managedTextures.Count; ++i) {
                // We expect the texture count to be 0, if it's not then the textures
                // where not unloaded properly. We will delete them anyway, but the 
                // user should really, REALLY fix their code.
                if (managedTextures[i].refCount != 0) {
                    Warning("Texture reference is > 0: " + managedTextures[i].path);
                }
                // Delete the texture from graphics memory
                GL.DeleteTexture(managedTextures[i].glHandle);
                // This isn't needed, but let's set the textures element in the array
                // to null, to give GC a hint that it is to be collected.
                managedTextures[i] = null;
            }
            // .Clear and = null are not both needed. Usually = null is enough. But we
            // want to make sure that GC gets the hint, so we agressivley clear the list
            managedTextures.Clear();
            managedTextures = null;
            isInitialized = false;
        }

        // Helper function to determine if a given texture is a power of two or not
        // Graphics cards work a LOT faster when tehy have Power of Two (POT) textures
        private bool IsPowerOfTwo(int x) {
            // If X was an unsigned long (or int), we could do this:
            // return (x & (x - 1)) == 0;
            // ^ would be a lot faster. But our handle is an integer, so use a general
            // purpose method of figuring this out. 0 will be a POT.
            if (x > 1) {
                while (x % 2 == 0) {
                    x >>= 1;
                }
            }
            return x == 1;
        }

        // Given a file name, this function will load the texture into system memory, then
        // upload it to GPU memory, and let the system memory become eligable for garbage collection
        // We really only need a reference to the texture on the GPU, as everything we do is going to
        // be hardware accelerated (happen on the GPU). The function will return a handle to the GPU
        // instance of the texture, as well as the width and height f the texutre
        private int LoadGLTexture(string filename, out int width, out int height) {
            if (string.IsNullOrEmpty(filename)) {
                Error("Load texture file path was null");
                throw new ArgumentException(filename);
            }

            // Generate a handle on the GPU
            int id = GL.GenTexture();
            // Bind the handle to the be the active texture.
            GL.BindTexture(TextureTarget.Texture2D, id);

            // Ming & Mag filters are needed to figure out how to interpolate scaling. If you don't 
            // provide them the GPU will not draw your texture. Trilinear is the nicest looking,
            // but most expensive. Linear is kind of standard.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Allocate system memory for the image
            Bitmap bmp = new Bitmap(filename);
            // Load the image into system memory
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // If the texture is non POT, or bigger than 2048 warn the user so they can fix
            // the texutre during development
            if (!IsPowerOfTwo(bmp.Width)) {
                Warning("Texture width non power of two: " + filename);
            }

            if (!IsPowerOfTwo(bmp.Height)) {
                Warning("Texture height  non power of two: " + filename);
            }

            if (bmp.Width > 2048) {
                Warning("Texture width > 2048: " + filename);
            }

            if (bmp.Height > 2048) {
                Warning("Texture height > 2048: " + filename);
            }

            // Upload the image data to the GPU
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

#if SAVE_PNG_BYTES
            byte[] byteArray = new byte[bmp_data.Width * bmp_data.Height * 4];
            System.Runtime.InteropServices.Marshal.Copy(bmp_data.Scan0, byteArray, 0, byteArray.Length);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("{\n");
            for (int i = 0; i < byteArray.Length; ++i) {
                sb.Append("0x");
                sb.AppendFormat("{0:x2}", byteArray[i]);
                if (i != byteArray.Length - 1) {
                    sb.Append(", ");
                }
                if (i != 0 && i%16 == 0) {
                    sb.Append("\n");
                }
            }
            sb.Append("\n};");

            string directory = System.IO.Path.GetDirectoryName(filename);
            string file = System.IO.Path.GetFileNameWithoutExtension(filename);

            System.IO.File.WriteAllText(directory + "/" + file + ".array", sb.ToString());
#endif

            // Mark system memory eligable for GC
            bmp.UnlockBits(bmp_data);

            // Return the textures width, height and GPU ID
            width = bmp.Width;
            height = bmp.Height;
            return id;
        }

        // Public facing interface for loading a texture. Given a texture path it will return a handle
        // (index into managedTextures vector) which can later be used to draw the texture.
        public int LoadTexture(string texturePath) {
            if (!isInitialized) {
                Error("Trying to load texture without intializing texture manager!");
            }

            // First, check if the texture is already being managed. If it is, increase ref count and
            // return the textures index.
            for (int i = 0; i < managedTextures.Count; ++i) {
                if (managedTextures[i].path == texturePath) {
                    managedTextures[i].refCount += 1;
                    return i;
                }
            }

            // If the texture was not being tracked, go trough the list and look for an open space, if an
            // open space is found, unload it's texture from GPU memory, and load our new texture, override
            // the reference count, path, width, height and texture handle with new values
            for (int i = 0; i < managedTextures.Count; ++i) {
                if (managedTextures[i].refCount <= 0) {
                    GL.DeleteTexture(managedTextures[i].glHandle);
                    managedTextures[i].glHandle = LoadGLTexture(texturePath, out managedTextures[i].width, out managedTextures[i].height);
                    managedTextures[i].refCount = 1;
                    managedTextures[i].path = texturePath;
                    return i;
                }
            }

            // Finally we get here if the texture we are trying to load is not an already managed texture
            // and we have no open spots for new textures to be managed. Here we just create a new texture
            // and add it to the managed textures vector
            TextureInstance newTexture = new TextureInstance();
            newTexture.refCount = 1;
            newTexture.glHandle = LoadGLTexture(texturePath, out newTexture.width, out newTexture.height);
            newTexture.path = texturePath;
            managedTextures.Add(newTexture);
            return managedTextures.Count - 1;
        }

        // This function decreases the textures reference count. It does not de-allocate the texture
        // memory on the GPU. The reason for this is that there is a decent chance that some textures
        // will stay around between scene loads. This way if a texture's reference count reaches 0,
        // and it hasn't been overwritten yet, if we load it again we get that load for free.
        public void UnloadTexture(int textureId) {
            if (!isInitialized) {
                Error("Trying to unload texture without intializing texture manager!");
            }
            managedTextures[textureId].refCount -= 1;

            // If we go below -1, no problem. But the system is intended to go only to 0, so lets
            // warn the user that they cleaned up wrong
            if (managedTextures[textureId].refCount < 0) {
                Error("Ref count of texture is less than 0: " + managedTextures[textureId].path);
            }
        }

        // Simple getter to access the width of a texture
        public int GetTextureWidth(int textureId) {
            if (!isInitialized) {
                Error("Trying to access texture width without intializing texture manager!");
            }
            return managedTextures[textureId].width;
        }

        // Simple getter to access the height of a texture
        public int GetTextureHeight(int textureId) {
            if (!isInitialized) {
                Error("Trying to access texture height without intializing texture manager!");
            }
            return managedTextures[textureId].height;
        }

        // Simple getter to access the size of a texture
        public Size GetTextureSize(int textureId) {
            if (!isInitialized) {
                Error("Trying to access texture size without intializing texture manager!");
            }
            return new Size(managedTextures[textureId].width, managedTextures[textureId].height);
        }

        // Given a texture id, draw it at the specified screen position. This will draw the
        // entire texture at that position, nothing gets cut off
        public void Draw(int textureId, Point screenPosition) {
            if (!isInitialized) {
                Error("Trying to draw texture without intializing texture manager!");
            }
            // Let the graphics manager know that we are drawing on top of everything else
            GraphicsManager.Instance.IncreaseDepth();
            // Save the current transform matrix
            GL.PushMatrix();

            // Build out the rectangle we will be drawing
            float left = 0.0f;
            float top = 0.0f;
            float right = left + managedTextures[textureId].width;
            float bottom = top + managedTextures[textureId].height;

            // Because blending is enabled, we want to blend the color of the texture with
            // just straight white. That way we get the correct color back
            GL.Color3(1.0f, 1.0f, 1.0f);

            // Bind the texture we want to draw to be active
            GL.BindTexture(TextureTarget.Texture2D, managedTextures[textureId].glHandle);

            // Offset to the correct position to draw at
            GL.Translate(screenPosition.X, screenPosition.Y, GraphicsManager.Instance.Depth);

            // Draw a quad
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1); // What part of the texture to draw
            GL.Vertex3(left, bottom, 0.0f); // Where on screen to draw it
            GL.TexCoord2(1, 1);
            GL.Vertex3(right, bottom, 0.0f);
            GL.TexCoord2(1, 0);
            GL.Vertex3(right, top, 0.0f);
            GL.TexCoord2(0, 0);
            GL.Vertex3(left, top, 0.0f);
            GL.End();

            // Restore the saved transform matrix
            GL.PopMatrix();
            // Unbind any active textures
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        // Given a texture id, draw it at the specified screen position and scale it.
        // This will draw the entire texture at that position, nothing gets cut off,
        // but it is possible to scale the image down or up
        public void Draw(int textureId, Point screenPosition, float scale) {
            if (!isInitialized) {
                Error("Trying to draw texture without intializing texture manager!");
            }
            Draw(textureId, screenPosition, new PointF(scale, scale));
        }

        // Given a texture id, draw it at the specified screen position and scale it.
        // This will draw the entire texture at that position, nothing gets cut off,
        // but it is possible to scale the image down or up
        public void Draw(int textureId, Point screenPosition, PointF scale) {
            if (!isInitialized) {
                Error("Trying to draw texture without intializing texture manager!");
            }
            GraphicsManager.Instance.IncreaseDepth();
            GL.PushMatrix();

            float left = 0.0f;
            float top = 0.0f;
            float right = left + managedTextures[textureId].width;
            float bottom = top + managedTextures[textureId].height;

            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.BindTexture(TextureTarget.Texture2D, managedTextures[textureId].glHandle);
            GL.Translate(screenPosition.X, screenPosition.Y, GraphicsManager.Instance.Depth);
            GL.Scale(scale.X, scale.Y, 1.0f);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 1);
            GL.Vertex3(left, bottom, 0.0f);
            GL.TexCoord2(1, 1);
            GL.Vertex3(right, bottom, 0.0f);
            GL.TexCoord2(1, 0);
            GL.Vertex3(right, top, 0.0f);
            GL.TexCoord2(0, 0);
            GL.Vertex3(left, top, 0.0f);
            GL.End();

            GL.PopMatrix();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        // Given a texture id, draw a sub-section of that texture at the specified screen position 
        // and possibly scale it. This will not draw the whole texture, just a specific rectangle form it
        public void Draw(int textureId, Point screenPosition, float scale, Rectangle sourceSection) {
            Draw(textureId, screenPosition, new PointF(scale, scale), sourceSection);
        }

        // Given a texture id, draw a sub-section of that texture at the specified screen position 
        // and possibly scale it. This will not draw the whole texture, just a specific rectangle form it
        public void Draw(int textureId, Point screenPosition, PointF scale,  Rectangle sourceSection) {
            if (!isInitialized) {
                Error("Trying to draw texture without intializing texture manager!");
            }
            GraphicsManager.Instance.IncreaseDepth();
            GL.PushMatrix();

            float left = 0.0f;
            float top = 0.0f;
            float right = left + sourceSection.Width;
            float bottom = top + sourceSection.Height;

            float wRecip = 1.0f / ((float)managedTextures[textureId].width);
            float hRecip = 1.0f / ((float)managedTextures[textureId].height);

            float uvLeft = ((float)sourceSection.X) * wRecip;
            float uvTop = ((float)sourceSection.Y) * hRecip;
            float uvRight = uvLeft + ((float)sourceSection.Width) * wRecip;
            float uvBottom = uvTop + ((float)sourceSection.Height) * hRecip;

            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.BindTexture(TextureTarget.Texture2D, managedTextures[textureId].glHandle);
            GL.Translate(screenPosition.X, screenPosition.Y, GraphicsManager.Instance.Depth);
            GL.Scale(scale.X, scale.Y, 1.0f);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(uvLeft, uvBottom);
            GL.Vertex3(left, bottom, 0.0f);
            GL.TexCoord2(uvRight, uvBottom);
            GL.Vertex3(right, bottom, 0.0f);
            GL.TexCoord2(uvRight, uvTop);
            GL.Vertex3(right, top, 0.0f);
            GL.TexCoord2(uvLeft, uvTop);
            GL.Vertex3(left, top, 0.0f);
            GL.End();

            GL.PopMatrix();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        // Given a texture id, draw a sub-section of that texture at the specified screen position 
        // and possibly scale it. This will not draw the whole texture, just a specific rectangle form it.
        // The resulting image can then be rotated about it's center by any angle.
        public void Draw(int textureId, Point screenPosition, float scale, Rectangle sourceSection, float rotation) {
            Point rotationCenter  = new Point(sourceSection.Width / 2, sourceSection.Height / 2);
            Draw(textureId, screenPosition, new PointF(scale, scale), sourceSection, rotationCenter, rotation);
        }

        // Given a texture id, draw a sub-section of that texture at the specified screen position 
        // and possibly scale it. This will not draw the whole texture, just a specific rectangle form it.
        // The resulting image can then be rotated about it's center by any angle.
        public void Draw(int textureId, Point screenPosition, PointF scale, Rectangle sourceSection, float rotation) {
            Point rotationCenter = new Point(sourceSection.Width / 2, sourceSection.Height / 2);
            Draw(textureId, screenPosition, scale, sourceSection, rotationCenter, rotation);
        }

        // Given a texture id, draw a sub-section of that texture at the specified screen position 
        // and possibly scale it. This will not draw the whole texture, just a specific rectangle form it.
        // The resulting image can then be rotated about a specified point by any angle.
        public void Draw(int textureId, Point screenPosition, float scale, Rectangle sourceSection, Point rotationCenter, float rotation = 0.0f) {
            Draw(textureId, screenPosition, new PointF(scale, scale), sourceSection, rotationCenter, rotation);
        }

        // Given a texture id, draw a sub-section of that texture at the specified screen position 
        // and possibly scale it. This will not draw the whole texture, just a specific rectangle form it.
        // The resulting image can then be rotated about a specified point by any angle.
        public void Draw(int textureId, Point screenPosition, PointF scale, Rectangle sourceSection, Point rotationCenter, float rotation = 0.0f) {
            if (!isInitialized) {
                Error("Trying to draw texture without intializing texture manager!");
            }
            GraphicsManager.Instance.IncreaseDepth();
            GL.PushMatrix();

            float left = 0.0f;
            float top = 0.0f;
            float right = left + sourceSection.Width;
            float bottom = top + sourceSection.Height;

            float wRecip = 1.0f / ((float)managedTextures[textureId].width);
            float hRecip = 1.0f / ((float)managedTextures[textureId].height);

            float uvLeft = ((float)sourceSection.X) * wRecip;
            float uvTop = ((float)sourceSection.Y) * hRecip;
            float uvRight = uvLeft + ((float)sourceSection.Width) * wRecip;
            float uvBottom = uvTop + ((float)sourceSection.Height) * hRecip;

            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.BindTexture(TextureTarget.Texture2D, managedTextures[textureId].glHandle);

            GL.Translate(screenPosition.X, screenPosition.Y, GraphicsManager.Instance.Depth);

            GL.Translate(((float)rotationCenter.X) * scale.X, ((float)rotationCenter.Y) * scale.Y, 0.0f);
            GL.Rotate(rotation, 0.0f, 0.0f, 1.0f);
            GL.Translate(-((float)rotationCenter.X) * scale.X, -((float)rotationCenter.Y)  * scale.Y, 0.0f);

            GL.Scale(scale.X, scale.Y, 1.0f);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(uvLeft, uvBottom);
            GL.Vertex3(left, bottom, 0.0f);
            GL.TexCoord2(uvRight, uvBottom);
            GL.Vertex3(right, bottom, 0.0f);
            GL.TexCoord2(uvRight, uvTop);
            GL.Vertex3(right, top, 0.0f);
            GL.TexCoord2(uvLeft, uvTop);
            GL.Vertex3(left, top, 0.0f);
            GL.End();

            GL.PopMatrix();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
