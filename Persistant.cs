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

        EksedraSprite PlayButtonOff, PlayButtonOn;
        
        public override void Init() {
            Tag = "Control";

            PlayButtonOff = new EksedraSprite(RunningEngine.Images["play_button"], new IntRect[] { new IntRect(0, 0, 512, 128) });
            PlayButtonOn = new EksedraSprite(RunningEngine.Images["play_button"], new IntRect[] { new IntRect(0, 128, 512, 128) });
            PlayButtonOff.MoveTo(1280 / 4, 3 * 720 / 4);
            PlayButtonOff.Smooth = true;
            PlayButtonOff.SetScale(0.5f, 0.5f);
            PlayButtonOn.MoveTo(1280 / 4, 3 * 720 / 4);
            PlayButtonOn.Smooth = true;
            PlayButtonOn.SetScale(0.5f, 0.5f);
        }

        public override void Update(float deltaTime) {
            if(RunningEngine.CurrentRoom == "title" && RunningEngine.Audio["Chunks-Title"].Status != SoundStatus.Playing) {
                RunningEngine.Audio["Chunks-Title"].Loop = true;
                RunningEngine.Audio["Chunks-Title"].Volume = 50;
                RunningEngine.Audio["Chunks-Title"].Play();
            } else if(RunningEngine.CurrentRoom != "title") {
                RunningEngine.Audio["Chunks-Title"].Loop = false;
                RunningEngine.Audio["Chunks-Title"].Stop();
            }
            
            if(RunningEngine.CurrentRoom == "grass-world" && RunningEngine.Audio["Chunks-Intro-Level"].Status != SoundStatus.Playing) {
                RunningEngine.Audio["Chunks-Intro-Level"].Loop = true;
                RunningEngine.Audio["Chunks-Intro-Level"].Volume = 50;
                RunningEngine.Audio["Chunks-Intro-Level"].Play();
            } else if(RunningEngine.CurrentRoom != "grass-world") {
                RunningEngine.Audio["Chunks-Intro-Level"].Loop = false;
                RunningEngine.Audio["Chunks-Intro-Level"].Stop();
            }

            switch(RunningEngine.CurrentRoom) {
                case "title":
                case "grass-world":
                    RunningEngine.Background = new Color(102, 161, 255);
                    break;

                default:
                    RunningEngine.Background = Color.Yellow;
                    break;
            }

            if(Mouse.IsButtonPressed(Mouse.Button.Left)) {
                //Console.WriteLine("CLICK!");
                Vector2i mousePos = Mouse.GetPosition() - RunningEngine.GetWindow().Position;
                if(mousePos.X > 1280 / 4 + 64 && mousePos.X < 1280 / 4 + 576
                        && mousePos.Y > (3 * 720 / 4) - 256 && mousePos.Y < (3 * 720 / 4) - 128) {
                    RunningEngine.FindGameObjectsWithTag("Player")[0].X = 32 + 64 * 9.5f;
                    RunningEngine.FindGameObjectsWithTag("Player")[0].Y = 27 * 64;
                    RunningEngine.CurrentRoom = "grass-world";
                }
            }
        }

        public override void OnKeyDown(bool[] keyState) {
            if(keyState[(int) Keyboard.Key.Escape])
                RunningEngine.SetQuit(true);

            /*if(keyState[(int) Keyboard.Key.Space])
                RunningEngine.CurrentRoom = "test";*/
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

                Vector2i mousePos = Mouse.GetPosition() - RunningEngine.GetWindow().Position;
                if(mousePos.X > 1280 / 4 + 64 && mousePos.X < 1280 / 4 + 576
                        && mousePos.Y > (3 * 720 / 4) - 256 && mousePos.Y < (3 * 720 / 4) - 128)
                    target.Draw(PlayButtonOn);
                else
                    target.Draw(PlayButtonOff);

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

        private bool DoubleJumpUnlocked, PunchUnlocked;
        private bool DoubleJumped = false, CanPunch = true;
        public bool Punched = false;                            // Public so it can break blocks
        private float PunchTime = 0.175f, PunchSpeed = 1800;

        public override void EarlyUpdate(float deltaTime) {}
        public override void LateUpdate(float deltaTime) {}
        public override void OnKeyUp(bool[]  keyState) {}
        public override void OnCollision(GameObject other) {}

        private EksedraSprite PlayerStand, PlayerJump, PlayerFall, PlayerRun, PlayerSuperJump, PlayerPunch, PlayerPunchDone;

        public Player(int x, int y) {
            X = x;
            Y = y;
            Persistant = true;
        }

        public override void Init() {
            Tag = "Player";
            Depth = 0;

            PlayerStand = new EksedraSprite(RunningEngine.Images["spr_chunks"], new IntRect[] {
                                                new IntRect(4, 4, 64, 64)
                                            });
            PlayerStand.Smooth = false;
            PlayerJump = new EksedraSprite(RunningEngine.Images["spr_chunks"], new IntRect[] {
                                                new IntRect(76, 4, 64, 64)
                                            });
            PlayerJump.Smooth = false;
            PlayerFall = new EksedraSprite(RunningEngine.Images["spr_chunks"], new IntRect[] {
                                                new IntRect(148, 4, 64, 64)
                                            });
            PlayerFall.Smooth = false;
            PlayerRun = new EksedraSprite(RunningEngine.Images["spr_chunks"], new IntRect[] {
                                                new IntRect(220, 4, 64, 64),
                                                new IntRect(292, 4, 64, 64),
                                                new IntRect(364, 4, 64, 64),
                                                new IntRect(436, 4, 64, 64),
                                            });
            PlayerRun.Smooth = false;
            PlayerSuperJump = new EksedraSprite(RunningEngine.Images["spr_chunks"], new IntRect[] {
                                                new IntRect(652, 4, 64, 64)
                                            });
            PlayerSuperJump.Smooth = false;
            PlayerPunch = new EksedraSprite(RunningEngine.Images["spr_chunks"], new IntRect[] {
                                                new IntRect(508, 4, 64, 64)
                                            });
            PlayerPunch.Smooth = false;
            PlayerPunchDone = new EksedraSprite(RunningEngine.Images["spr_chunks"], new IntRect[] {
                                                new IntRect(508, 76, 64, 64)
                                            });
            PlayerPunchDone.Smooth = false;

            SpriteIndex = PlayerStand;
            ImageSpeed = 10;
            MaskX = -20;
            MaskY = -32;
            MaskWidth = 40;
            MaskHeight = 52;

            // All false when game is done
            DoubleJumpUnlocked = true;
            PunchUnlocked = true;
        }

        public override void Draw(RenderTarget target, RenderStates states) {
            target.Draw(SpriteIndex);

            /*RectangleShape mask = new RectangleShape();
            mask.FillColor = Color.Black;
            mask.Position = new Vector2f(X + MaskX, Y + MaskY);
            mask.Size = new Vector2f(MaskWidth, MaskHeight);
            target.Draw(mask);*/

            if(Punched) {
                EksedraSprite animeLines = new EksedraSprite(RunningEngine.Images["anime-lines"], new IntRect[] { new IntRect(0, 0, 640, 360) });
                animeLines.MoveTo(RunningEngine.ViewPort.Left + 320, RunningEngine.ViewPort.Top + 180);
                animeLines.Smooth = true;
                target.Draw(animeLines);
            }
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
            
            if(VSpeed < MaxVSpeed && !IsGrounded && !Punched)
                VSpeed += Gravity * deltaTime;

            if(Punched)
                HSpeed = Math.Sign(ImageScaleX) * PunchSpeed;
            else if(!CanPunch)
                HSpeed = Math.Sign(ImageScaleX) * MoveSpeed;

            // Horizontal collision
            GameObject other = null;
            if(HSpeed > 0 && RunningEngine.CheckCollision(X + HSpeed * deltaTime, Y - 0.1f, this, typeof(Solid),
                    (self, otra) => self.Y < otra.Y + otra.MaskY + otra.MaskHeight 
                                    && (otra as Solid).BlockPosition != BlockType.PassThrough, ref other)) {
                X = other.X + other.MaskX - (MaskX + MaskWidth);
                HSpeed = 0;
            }
            
            if(HSpeed < 0 && RunningEngine.CheckCollision(X + HSpeed * deltaTime, Y - 0.1f, this, typeof(Solid),
                    (self, otra) => self.Y < otra.Y + otra.MaskY + otra.MaskHeight 
                                    && (otra as Solid).BlockPosition != BlockType.PassThrough, ref other)) {
                //Console.WriteLine("Left Side: " + (Y + MaskY + MaskHeight) + " > Right Side: " + (other.Y + other.MaskY - 1));
                X = other.X + other.MaskX + other.MaskWidth - MaskX;
                HSpeed = 0;
            }
            
            if(VSpeed > 0 && RunningEngine.CheckCollision(X - Math.Sign(ImageScaleX) * 1, Y + VSpeed * deltaTime, this, typeof(Solid),
                    (self, otra) => self.Y + self.MaskY + self.MaskHeight <= otra.Y + otra.MaskY 
                                    && (otra as Solid).BlockPosition != BlockType.PassThrough, ref other)) {
                Y = other.Y + other.MaskY - (MaskY + MaskHeight);
                VSpeed = 0;
                IsGrounded = true;
            } else if(VSpeed > 0 && RunningEngine.CheckCollision(X - Math.Sign(ImageScaleX) * 1, Y + VSpeed * deltaTime, this, typeof(JumpThrough), 
                    (self, otra) => self.Y + self.MaskY + self.MaskHeight <= otra.Y + otra.MaskY, ref other)) {
                Y = other.Y + other.MaskY - (MaskY + MaskHeight);
                VSpeed = 0;
                IsGrounded = true;
            } else if(!RunningEngine.CheckCollision(X - Math.Sign(ImageScaleX) * 1, Y + 1, this, typeof(Solid), 
                        (self, otra) => self.Y + self.MaskY + self.MaskHeight <= otra.Y + otra.MaskY 
                                    && (otra as Solid).BlockPosition != BlockType.PassThrough, ref other)
                    && !RunningEngine.CheckCollision(X - Math.Sign(ImageScaleX) * 1, Y + 1, this, typeof(JumpThrough), 
                        (self, otra) => self.Y + self.MaskY + self.MaskHeight <= otra.Y + otra.MaskY, ref other))
                IsGrounded = false;
            
            if(VSpeed < 0 && RunningEngine.CheckCollision(X - Math.Sign(ImageScaleX) * 1, Y + VSpeed * deltaTime, this, typeof(Solid), 
                    (self, otra) => self.Y + self.MaskY >= otra.Y + otra.MaskY + otra.MaskHeight 
                                    && (otra as Solid).BlockPosition != BlockType.PassThrough, ref other)) {
                Y = other.Y + other.MaskY + other.MaskHeight - MaskY;
                VSpeed = 0;
            }

            // Animate
            if(Punched)
                SpriteIndex = PlayerPunch;
            else if(IsGrounded) {
                SpriteIndex = Math.Abs(HSpeed) > 0 ? PlayerRun : PlayerStand;

                DoubleJumped = false;
                if(!Punched)
                    CanPunch = true;
            } else
                SpriteIndex = !CanPunch ? (!Punched ? PlayerPunchDone : PlayerPunch) : (VSpeed > 0 ? PlayerFall : (DoubleJumped ? PlayerSuperJump : PlayerJump));
            
            if(HSpeed > 0 && CanPunch)
                ImageScaleX = Math.Abs(ImageScaleX);
            else if(HSpeed < 0 && CanPunch)
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

            if(keyState[(int) Keyboard.Key.Up] && IsGrounded && CanPunch) {
                    VSpeed = -JumpSpeed;
                    IsGrounded = false;

                    RunningEngine.Audio["270337__littlerobotsoundfactory__pickup-00"].Play();
            } else if(keyState[(int) Keyboard.Key.Up] && !IsGrounded && DoubleJumpUnlocked && !DoubleJumped && CanPunch) {
                VSpeed = -JumpSpeed * 1.5f;
                DoubleJumped = true;
                RunningEngine.Audio["270337__littlerobotsoundfactory__pickup-00"].Play();
            }

            if(keyState[(int) Keyboard.Key.Space] && PunchUnlocked && !Punched && CanPunch) {
                Timers[0] = PunchTime;
                Punched = true;
                CanPunch = false;
                RunningEngine.Audio["270310__littlerobotsoundfactory__explosion-04"].Play();
            }
        }

        public override void OnTimer(int timerIndex) {
            if(timerIndex == 0)
                Punched = false;
        }

        public override void OnKeyHeld(bool[] keyState) {
            if(RunningEngine.CurrentRoom == "title") {
                SpriteIndex = PlayerRun;
                return;
            }

            if(keyState[(int) Keyboard.Key.Left] && CanPunch)
                HSpeed = -MoveSpeed;
            else if(keyState[(int) Keyboard.Key.Right] && CanPunch)
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