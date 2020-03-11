/*
 * This file is used to make custom game objects (children of base GameObject)
 * These are instantiated in room files at the start of the application
 * 
 * In here are all the persistant game objects like the control and player
 */
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;
using System;
using EksedraEngine;
using SFML.System;

namespace NewSuperChunks {
    public class ControlObject : GameObject {
        public override void EarlyUpdate(float deltaTime) {}
        public override void LateUpdate(float deltaTime) {}
        public override void OnKeyUp(bool[] keyState) {}
        public override void OnKeyOff(bool[]  keyState) {}
        public override void OnKeyHeld(bool[] keyState) {}
        public override void OnCollision(GameObject other) {}
        public override void OnTimer(int timerIndex) {}
        
        public override void Init() {
            Tag = "Control";
        }

        public override void Update(float deltaTime) {
            if(RunningEngine.CurrentRoom == "title" && RunningEngine.Audio["Chunks-Title"].Status != SoundStatus.Playing) {
                RunningEngine.Audio["Chunks-Title"].Loop = true;
                RunningEngine.Audio["Chunks-Title"].Play();
            } else if(RunningEngine.CurrentRoom != "title") {
                RunningEngine.Audio["Chunks-Title"].Loop = false;
                RunningEngine.Audio["Chunks-Title"].Stop();
            }
            
            if(RunningEngine.CurrentRoom == "test" && RunningEngine.Audio["Chunks-Intro-Level"].Status != SoundStatus.Playing) {
                RunningEngine.Audio["Chunks-Intro-Level"].Loop = true;
                RunningEngine.Audio["Chunks-Intro-Level"].Play();
            } else if(RunningEngine.CurrentRoom != "test") {
                RunningEngine.Audio["Chunks-Intro-Level"].Loop = false;
                RunningEngine.Audio["Chunks-Intro-Level"].Stop();
            }

            switch(RunningEngine.CurrentRoom) {
                case "title":
                    RunningEngine.Background = Color.Blue;
                    break;

                default:
                    RunningEngine.Background = Color.Yellow;
                    break;
            }
        }

        public override void OnKeyDown(bool[] keyState) {
            if(keyState[(int) Keyboard.Key.Escape])
                RunningEngine.SetQuit(true);

            if(keyState[(int) Keyboard.Key.Space])
                RunningEngine.CurrentRoom = "test";
        }
        
        public ControlObject() {
            Persistant = true;
            Cull = false;
        }

        public override void Draw(RenderTarget target, RenderStates states) {
            /*Text text = new Text("Eksedra Engine Test Demo", RunningEngine.Fonts["JosefinSans"], 20);
            text.Position = new Vector2f(RunningEngine.ViewPort.Left + 32, RunningEngine.ViewPort.Top + 32);
            text.FillColor = Color.Black;

            target.Draw(text);*/

            if(RunningEngine.CurrentRoom == "title") {
                Text text = new Text("SUPER CHUNKS", RunningEngine.Fonts["Pixeled"], 36);
                text.Position = new Vector2f(112, 720 - 64 * 5);
                text.FillColor = Color.White;
                
                target.Draw(text);
            }
        }
    }

    public class Player : GameObject {
        private float MoveSpeed = 300;
        private float Gravity = 3500;
        private float MaxVSpeed = 5000;
        private float JumpSpeed = 900;
        private bool IsGrounded = false;

        public override void EarlyUpdate(float deltaTime) {}
        public override void LateUpdate(float deltaTime) {}
        public override void OnKeyUp(bool[]  keyState) {}
        public override void OnTimer(int timerIndex) {}
        public override void OnCollision(GameObject other) {}

        private EksedraSprite PlayerStand, PlayerJump, PlayerFall, PlayerRun;

        public Player(int x, int y) {
            X = x;
            Y = y;
            Persistant = true;
        }

        public override void Init() {
            Tag = "Player";
            Depth = 0;

            PlayerStand = new EksedraSprite(RunningEngine.Images["spr_chunks"], new IntRect[] {
                                                new IntRect(0, 0, 64, 64)
                                            });
            PlayerStand.Smooth = false;
            PlayerJump = new EksedraSprite(RunningEngine.Images["spr_chunks"], new IntRect[] {
                                                new IntRect(72, 0, 64, 64)
                                            });
            PlayerJump.Smooth = false;
            PlayerFall = new EksedraSprite(RunningEngine.Images["spr_chunks"], new IntRect[] {
                                                new IntRect(144, 0, 64, 64)
                                            });
            PlayerFall.Smooth = false;
            PlayerRun = new EksedraSprite(RunningEngine.Images["spr_chunks"], new IntRect[] {
                                                new IntRect(216, 0, 64, 64),
                                                new IntRect(288, 0, 64, 64),
                                                new IntRect(364, 0, 64, 64),
                                                new IntRect(432, 0, 64, 64),
                                            });
            PlayerRun.Smooth = false;

            SpriteIndex = PlayerStand;
            ImageSpeed = 10;
            MaskX = -20;
            MaskY = -30;
            MaskWidth = 40;
            MaskHeight = 52;
        }

        public override void Draw(RenderTarget target, RenderStates states) {
            target.Draw(SpriteIndex);

            /*RectangleShape mask = new RectangleShape();
            mask.FillColor = Color.Black;
            mask.Position = new Vector2f(X + MaskX, Y + MaskY);
            mask.Size = new Vector2f(MaskWidth, MaskHeight);
            target.Draw(mask);*/
        }

        public override void Update(float deltaTime) {

            //Console.WriteLine(RunningEngine.GetWindowWidth() + ", " + RunningEngine.GetWindowHeight());
            if(X + MaskX + MaskWidth > RunningEngine.GetRoomSize().X) {
                X = RunningEngine.GetRoomSize().X - MaskX - MaskWidth;
                HSpeed = 0;
            } else if(X + MaskX < 0) {
                X = -MaskX;
                HSpeed = 0;
            } else if(Y + MaskY + MaskHeight > RunningEngine.GetRoomSize().Y) {
                Y = RunningEngine.GetRoomSize().Y - MaskY - MaskHeight;
                VSpeed = 0;
            } else if(Y + MaskY < 0) {
                Y = -MaskY;
                VSpeed = 0;
            }
            
            if(VSpeed < MaxVSpeed && !IsGrounded)
                VSpeed += Gravity * deltaTime;

            // Horizontal collision
            GameObject other = null;
            if(HSpeed > 0 && RunningEngine.CheckCollision(X + HSpeed * deltaTime, Y - 0.1f, this, typeof(Solid), (self, otra) => true, ref other)) {
                X = other.X + other.MaskX - (MaskX + MaskWidth);
                HSpeed = 0;
            }
            
            if(HSpeed < 0 && RunningEngine.CheckCollision(X + HSpeed * deltaTime, Y - 0.1f, this, typeof(Solid), (self, otra) => true, ref other)) {
                X = other.X + other.MaskX + other.MaskWidth - MaskX;
                HSpeed = 0;
            }

            // Vertical Collision
            if(VSpeed > 0 && RunningEngine.CheckCollision(X, Y + VSpeed * deltaTime, this, typeof(Solid), 
                    (self, otra) => self.Y + self.MaskY + self.MaskHeight <= otra.Y + otra.MaskY, ref other)) {
                Y = other.Y + other.MaskY - (MaskY + MaskHeight);
                VSpeed = 0;
                IsGrounded = true;
            } else if(VSpeed > 0 && RunningEngine.CheckCollision(X, Y + VSpeed * deltaTime, this, typeof(CloudThrough), 
                    (self, otra) => self.Y + self.MaskY + self.MaskHeight <= otra.Y + otra.MaskY, ref other)) {
                Y = other.Y + other.MaskY - (MaskY + MaskHeight);
                VSpeed = 0;
                IsGrounded = true;
            } else if(!RunningEngine.CheckCollision(X, Y + 1, this, typeof(Solid), 
                        (self, otra) => self.Y + self.MaskY + self.MaskHeight <= otra.Y + otra.MaskY, ref other)
                    && !RunningEngine.CheckCollision(X, Y + 1, this, typeof(CloudThrough), 
                        (self, otra) => self.Y + self.MaskY + self.MaskHeight <= otra.Y + otra.MaskY, ref other))
                IsGrounded = false;
            
            if(VSpeed < 0 && RunningEngine.CheckCollision(X, Y + VSpeed * deltaTime, this, typeof(Solid), 
                    (self, otra) => self.Y + self.MaskY >= otra.Y + otra.MaskY + otra.MaskHeight, ref other)) {
                Y = other.Y + other.MaskY + other.MaskHeight - MaskY;
                VSpeed = 0;
            }

            // Animate
            if(IsGrounded)
                SpriteIndex = Math.Abs(HSpeed) > 0 ? PlayerRun : PlayerStand;
            else
                SpriteIndex = VSpeed > 0 ? PlayerFall : PlayerJump;
            
            if(HSpeed > 0)
                ImageScaleX = Math.Abs(ImageScaleX);
            else if(HSpeed < 0)
                ImageScaleX = -Math.Abs(ImageScaleX);
            
            
            if(RunningEngine.CurrentRoom == "title")
                SpriteIndex = PlayerRun;

            // Move the view
            if(X - RunningEngine.ViewPort.Width / 2 < 0)
                RunningEngine.ViewPort.Left = 0;
            else if(X + RunningEngine.ViewPort.Width / 2 > RunningEngine.GetRoomSize().X)
                RunningEngine.ViewPort.Left = RunningEngine.GetRoomSize().X - RunningEngine.ViewPort.Width;
            else
                RunningEngine.ViewPort.Left = X - RunningEngine.ViewPort.Width / 2;

            if(Y - RunningEngine.ViewPort.Height / 2 < 0)
                RunningEngine.ViewPort.Top = 0;
            else if(Y + RunningEngine.ViewPort.Height / 2 > RunningEngine.GetRoomSize().Y)
                RunningEngine.ViewPort.Top = RunningEngine.GetRoomSize().Y - RunningEngine.ViewPort.Height;
            else
                RunningEngine.ViewPort.Top = Y - RunningEngine.ViewPort.Height / 2;
        }

        public override void OnKeyDown(bool[] keyState) {
            if(RunningEngine.CurrentRoom == "title") {
                SpriteIndex = PlayerRun;
                return;
            }

            if(keyState[(int) Keyboard.Key.Up] && IsGrounded) {
                VSpeed = -JumpSpeed;
                IsGrounded = false;

                RunningEngine.Audio["270337__littlerobotsoundfactory__pickup-00"].Play();
            }
        }

        public override void OnKeyHeld(bool[] keyState) {
            if(RunningEngine.CurrentRoom == "title") {
                SpriteIndex = PlayerRun;
                return;
            }

            if(keyState[(int) Keyboard.Key.Left])
                HSpeed = -MoveSpeed;
            else if(keyState[(int) Keyboard.Key.Right])
                HSpeed = MoveSpeed;
        }
        
        public override void OnKeyOff(bool[] keyState) {
            if(keyState[(int) Keyboard.Key.Left] && HSpeed < 0)
                HSpeed = 0;
            else if(keyState[(int) Keyboard.Key.Right] && HSpeed > 0)
                HSpeed = 0;
        }
    }
}