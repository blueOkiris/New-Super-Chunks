/*
 * This file contains all the blocks that do special things or are simply tiles
 * Includes breakables, boundaries, ladders, water, etc.
 */

using SFML.Graphics;
using EksedraEngine;
using System;

namespace NewSuperChunks {
    public class BoundaryBlock : GameObject {
        public BlockType BlockRotation;

        public BoundaryBlock(float x, float y, BlockType blockType) {
            X = x;
            Y = y;
            BlockRotation = blockType;
        }

        public override void Init() {
            Tag = "Boundary";
            switch(BlockRotation) {
                case BlockType.Left:
                    SpriteIndex = new EksedraSprite(RunningEngine.Images["boundary-block"], new IntRect[] { new IntRect(128, 0, 128, 64) });
                    break;

                case BlockType.Right:
                    SpriteIndex = new EksedraSprite(RunningEngine.Images["boundary-block"], new IntRect[] { new IntRect(128, 64, 128, 64) });
                    break;

                case BlockType.Top:
                    SpriteIndex = new EksedraSprite(RunningEngine.Images["boundary-block"], new IntRect[] { new IntRect(0, 0, 64, 128) });
                    break;

                case BlockType.Bottom:
                    SpriteIndex = new EksedraSprite(RunningEngine.Images["boundary-block"], new IntRect[] { new IntRect(64, 0, 64, 128) });
                    break;

                default:
                    SpriteIndex = null;
                    break;
            }

            Cull = false;
            if(SpriteIndex != null)
                SpriteIndex.Smooth = false;
        }

        public override void Draw(RenderTarget target, RenderStates states){
            if(SpriteIndex != null)
                target.Draw(SpriteIndex);
        }

        public override void EarlyUpdate(float deltaTime) {}
        public override void LateUpdate(float deltaTime) {}
        public override void OnCollision(GameObject other) {}
        public override void OnKeyDown(bool[] keyState) {}
        public override void OnKeyHeld(bool[] keyState) {}
        public override void OnKeyOff(bool[] keyState) {}
        public override void OnKeyUp(bool[] keyState) {}
        public override void OnTimer(int timerIndex) {}
        public override void Update(float deltaTime) {}
    }

    public class Box : Solid {
        public Box(int x, int y) : base() {
            X = x;
            Y = y;

            BlockPosition = BlockType.Single;
        }

        public override void Init() {
            Tag = "Box";
            Depth = 1;
            
            ImageSpeed = 0;
            ImageIndex = 0;

            MaskX = -32;
            MaskY = -32;
            MaskWidth = 64;
            MaskHeight = 64;

            SpriteIndex = new EksedraSprite(RunningEngine.Images["box"], new IntRect[] { new IntRect(0, 0, 64, 64) });
            SpriteIndex.Smooth = false;
        }

        public override void OnCollision(GameObject other) {
            if(other.Tag == "Player" && (other as Player).Punched && BlockPosition != BlockType.PassThrough) {
                BlockPosition = BlockType.PassThrough;
                SpriteIndex = new EksedraSprite(RunningEngine.Images["box"], new IntRect[] { new IntRect(64, 0, 64, 64) });
                SpriteIndex.Smooth = false;

                (other as Player).Punched = false;
                other.X -= Math.Sign(other.ImageScaleX) * 112;
                other.VSpeed = -300;
            }
        }
    }

    public class LadderBlock : GameObject {
        public override void EarlyUpdate(float deltaTime) {}
        public override void Update(float deltaTime) {}
        public override void LateUpdate(float deltaTime) {}
        public override void OnCollision(GameObject other) {}
        public override void OnKeyDown(bool[] keyState) {}
        public override void OnKeyHeld(bool[] keyState) {}
        public override void OnKeyOff(bool[] keyState) {}
        public override void OnKeyUp(bool[] keyState) {}
        public override void OnTimer(int timerIndex) {}

        public LadderBlock(int x, int y) {
            X = x;
            Y = y;
        }

        public override void Init() {
            Tag = "Ladder";
            Depth = 1;

            SpriteIndex = new EksedraSprite(RunningEngine.Images["ladder"], new IntRect[] { new IntRect(0, 0, 64, 64) });
            SpriteIndex.Smooth = false;

            MaskX = -20;
            MaskY = -32;
            MaskWidth = 52;
            MaskHeight = 64;
        }

        public override void Draw(RenderTarget target, RenderStates states) {
            target.Draw(SpriteIndex);
        }
    }

    public class Water : GameObject {
        public override void EarlyUpdate(float deltaTime) {}
        public override void Update(float deltaTime) {}
        public override void LateUpdate(float deltaTime) {}
        public override void OnCollision(GameObject other) {}
        public override void OnKeyDown(bool[] keyState) {}
        public override void OnKeyHeld(bool[] keyState) {}
        public override void OnKeyOff(bool[] keyState) {}
        public override void OnKeyUp(bool[] keyState) {}
        public override void OnTimer(int timerIndex) {}

        public Water(int x, int y) {
            X = x;
            Y = y;
        }

        public override void Init() {
            Tag = "Water";
            Depth = 2;

            MaskX = -32;
            MaskY = -32;
            MaskWidth = 64;
            MaskHeight = 64;

            SpriteIndex = new EksedraSprite(RunningEngine.Images["water"], new IntRect[] { new IntRect(0, 0, 64, 64) });
            SpriteIndex.Smooth = false;
        }

        public override void Draw(RenderTarget target, RenderStates states) {
            target.Draw(SpriteIndex);
        }
    }
}
