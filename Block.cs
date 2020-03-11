/*
 * For all the different kinds of physical blocks
 * that are custom game objects
 */

using SFML.Graphics;
using EksedraEngine;

namespace NewSuperChunks {
    public class JumpThrough : GameObject {
        public override void EarlyUpdate(float deltaTime) {}
        public override void Update(float deltaTime) {}
        public override void LateUpdate(float deltaTime) {}
        public override void OnKeyUp(bool[] keyState) {}
        public override void OnKeyDown(bool[] keyState) {}
        public override void OnKeyOff(bool[]  keyState) {}
        public override void OnKeyHeld(bool[] keyState) {}
        public override void OnTimer(int timerIndex) {}
        
        public override void OnCollision(GameObject other) {
            //Console.WriteLine("Rock collision with: " + other.Tag);
        }

        public JumpThrough(int x, int y) {
            X = x;
            Y = y;
        }
        
        public override void Init() {
            Tag = "JumpThrough";
            Depth = 1;

            SpriteIndex = new EksedraSprite(RunningEngine.Images["air_blocks"], new IntRect[] { new IntRect(0, 0, 64, 64) });
            SpriteIndex.Smooth = false;
            ImageSpeed = 0;
            ImageIndex = 0;

            MaskX = -32;
            MaskY = -32;
            MaskWidth = 64;
            MaskHeight = 9;
        }

        public override void Draw(RenderTarget target, RenderStates states) {
            target.Draw(SpriteIndex);
        }
    }

    public class Rock : GameObject {
        public override void EarlyUpdate(float deltaTime) {}
        public override void LateUpdate(float deltaTime) {}
        public override void OnKeyUp(bool[] keyState) {}
        public override void OnKeyDown(bool[] keyState) {}
        public override void OnKeyOff(bool[]  keyState) {}
        public override void OnKeyHeld(bool[] keyState) {}
        public override void OnTimer(int timerIndex) {}
        
        public override void OnCollision(GameObject other) {
            //Console.WriteLine("Rock collision with: " + other.Tag);
        }

        public Rock(int x, int y) {
            X = x;
            Y = y;
        }
        
        public override void Init() {
            Tag = "Rock";
            Depth = 1;

            SpriteIndex = new EksedraSprite(RunningEngine.Images["spr_block_2"], new IntRect[] { new IntRect(0, 0, 64, 64) });
            SpriteIndex.Smooth = false;
            ImageSpeed = 0;
            ImageIndex = 0;

            MaskX = -32;
            MaskY = -32;
            MaskWidth = 64;
            MaskHeight = 64;
        }

        public override void Draw(RenderTarget target, RenderStates states) {
            target.Draw(SpriteIndex);
        }

        public override void Update(float deltaTime) {
            //Console.WriteLine("Rock: { X: " + X + ", Y: " + Y + ", HSpeed: " + HSpeed + ", VSpeed: " + VSpeed + " }");
        }
    }
}