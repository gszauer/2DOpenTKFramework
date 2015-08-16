using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GameFramework {
    class MyApplication {
        [STAThread]
        public static void Main() {
            using (Toolkit.Init(new ToolkitOptions { Backend = PlatformBackend.Default })) {
                // here be games

                using (var game = new GameWindow()) {
                    GraphicsManager.Instance.Initialize(game);
                    TextureManager.Instance.Initialize(game);
                    SoundManager.Instance.Initialize(game);
                    InputManager.Instance.Initialize(game);

                    int tex1 = TextureManager.Instance.LoadTexture("C:/Users/gszauer/Desktop/icon-flareLargeBgColor.png");
                    int tex2 = TextureManager.Instance.LoadTexture("C:/Users/Public/Pictures/11696.gif");
                    int bgMusicId = SoundManager.Instance.LoadMp3("C:/Users/Public/Music/WarChild.mp3");
                    int sfxID = SoundManager.Instance.LoadWav("C:/Users/Public/Music/M1F1-Alaw-AFsp.wav");

                    SoundManager.Instance.PlaySound(bgMusicId);
                    float angle = 0.0f;
                    float bgVolume = 1.0f;


                    game.UpdateFrame += (sender, e) => {
                        InputManager.Instance.Update();

                        if (game.Keyboard[Key.Escape]) {
                            game.Exit();
                        }

                        if (game.Keyboard[Key.Q]) {
                            if (!SoundManager.Instance.IsPlaying(sfxID)) {
                                SoundManager.Instance.PlaySound(sfxID);
                            }
                        }

                        if (game.Keyboard[Key.W]) {
                            SoundManager.Instance.StopSound(bgMusicId);
                        }

                        if (game.Keyboard[Key.E]) {
                            SoundManager.Instance.PlaySound(bgMusicId);
                        }

                        if (game.Keyboard[Key.A]) {
                            bgVolume -= (1.0f / 60.0f);
                            if (bgVolume < 0.0f) {
                                bgVolume = 0.0f;
                            }
                            SoundManager.Instance.SetVolume(bgMusicId, bgVolume);
                        }

                        if (game.Keyboard[Key.S]) {
                            bgVolume += (1.0f / 60.0f);
                            if (bgVolume > 1.0f) {
                                bgVolume = 1.0f;
                            }
                            SoundManager.Instance.SetVolume(bgMusicId, bgVolume);
                        }

                        if (game.Keyboard[Key.D]) {
                            Console.WriteLine("BG Volume: " + SoundManager.Instance.GetVolume(bgMusicId));
                        }

                        if (InputManager.Instance.KeyPressed(Key.P)) {
                            Console.WriteLine("Num game pads: " + InputManager.Instance.NumGamepads);
                        }
                    };

                    game.RenderFrame += (sender, e) => {
                        GraphicsManager.Instance.ClearScreen(Color.Yellow);

                        GraphicsManager.Instance.DrawRect(new Rectangle(0, 0, 300, 300), Color.Blue);
                        GraphicsManager.Instance.DrawRect(new Rectangle(150, 150, 300, 300), Color.Green);
                        GraphicsManager.Instance.DrawRect(new Rectangle(300, 300, 300, 300), Color.Purple);
                        GraphicsManager.Instance.DrawLine(new Point(0, 0), new Point(300, 300), Color.Red);
                        TextureManager.Instance.Draw(tex1, new Point(200, 100), new PointF(0.5f, 0.5f));
                        TextureManager.Instance.Draw(tex2, new Point(450, 300), 5.0f, new Rectangle(4, 987, 80, 76), angle);

                        angle += 90.0f * (1.0f / 60.0f);
                        if (angle >= 360.0f) {
                            angle -= 360.0f;
                        }

                        GraphicsManager.Instance.SwapBuffers();
                    };

                    // Run the game at 60 updates per second
                    game.Run(60.0);
                }
            }
        }
    }
}