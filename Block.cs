/*
 * For all the different kinds of physical blocks
 * that are custom game objects
 */

using SFML.Graphics;
using EksedraEngine;

namespace NewSuperChunks {
    public enum BlockType {
        Single = 0,
        Middle = 5,
        TopLeft = 1, TopRight = 2, BottomLeft = 6, BottomRight = 7,
        Top = 3, Right = 4, Bottom = 9, Left = 8
    }

    public abstract class Solid : GameObject {
        public override void EarlyUpdate(float deltaTime) {}
        public override void LateUpdate(float deltaTime) {}
        public override void OnKeyUp(bool[] keyState) {}
        public override void OnKeyDown(bool[] keyState) {}
        public override void OnKeyOff(bool[]  keyState) {}
        public override void OnKeyHeld(bool[] keyState) {}
        public override void OnTimer(int timerIndex) {}
        public override void OnCollision(GameObject other) {}

        public Solid() {
            Tag = "Solid";
        }

        public override void Draw(RenderTarget target, RenderStates states) {
            if(SpriteIndex != null)
                target.Draw(SpriteIndex);
        }

        public override void Update(float deltaTime) {
            if(RunningEngine.CurrentRoom == "title") {
                X -= 256 * deltaTime;

                if(X + 32 <= 0)
                    X = 1280 / 2;
            }
        }
    }

    public class Box : Solid {
        public Box(int x, int y) : base() {
            X = x;
            Y = y;
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
    }

    public class GrassBlock : Solid {
        public BlockType BlockPosition;

        public GrassBlock(int x, int y, BlockType blockType) : base() {
            X = x;
            Y = y;

            BlockPosition = blockType;
        }

        public override void Init() {
            Tag = "GrassBlock";
            Depth = 1;
            
            ImageSpeed = 0;
            ImageIndex = 0;

            MaskX = -32;
            MaskY = -32;
            MaskWidth = 64;
            MaskHeight = 64;

            switch(BlockPosition) {
                case BlockType.Single:
                case BlockType.TopLeft:
                case BlockType.TopRight:
                case BlockType.Top:
                case BlockType.Right:
                    SpriteIndex = new EksedraSprite(RunningEngine.Images["grass_blocks"],
                                    new IntRect[] { new IntRect(((int) BlockPosition) * 64, 0, 64, 64) });
                    break;

                case BlockType.Middle:
                case BlockType.BottomLeft:
                case BlockType.BottomRight:
                case BlockType.Left:
                case BlockType.Bottom:
                    SpriteIndex = new EksedraSprite(RunningEngine.Images["grass_blocks"],
                                    new IntRect[] { new IntRect((((int) BlockPosition) - 5) * 64, 64, 64, 64) });
                    break;

                default:
                    SpriteIndex = null;
                    break;
            }

            if(SpriteIndex != null)
                SpriteIndex.Smooth = false;
        }
    }

    public class WaterBlock : Solid {
        public BlockType BlockPosition;

        public WaterBlock(int x, int y, BlockType blockType) : base() {
            X = x;
            Y = y;

            BlockPosition = blockType;
        }

        public override void Init() {
            Tag = "WaterBlock";
            Depth = 1;
            
            ImageSpeed = 0;
            ImageIndex = 0;

            MaskX = -32;
            MaskY = -32;
            MaskWidth = 64;
            MaskHeight = 64;

            switch(BlockPosition) {
                case BlockType.Single:
                case BlockType.TopLeft:
                case BlockType.TopRight:
                case BlockType.Top:
                case BlockType.Right:
                    SpriteIndex = new EksedraSprite(RunningEngine.Images["spr_ocean_blocks"],
                                    new IntRect[] { new IntRect(((int) BlockPosition) * 64, 0, 64, 64) });
                    break;

                case BlockType.Middle:
                case BlockType.BottomLeft:
                case BlockType.BottomRight:
                case BlockType.Left:
                case BlockType.Bottom:
                    SpriteIndex = new EksedraSprite(RunningEngine.Images["spr_ocean_blocks"],
                                    new IntRect[] { new IntRect((((int) BlockPosition) - 5) * 64, 64, 64, 64) });
                    break;

                default:
                    SpriteIndex = null;
                    break;
            }

            if(SpriteIndex != null)
                SpriteIndex.Smooth = false;
        }
    }

    public class AirBlock : Solid {
        public BlockType BlockPosition;

        public AirBlock(int x, int y, BlockType blockType) : base() {
            X = x;
            Y = y;

            BlockPosition = blockType;
        }

        public override void Init() {
            Tag = "AirBlock";
            Depth = 1;
            
            ImageSpeed = 0;
            ImageIndex = 0;

            MaskX = -32;
            MaskY = -32;
            MaskWidth = 64;
            MaskHeight = 64;

            switch(BlockPosition) {
                case BlockType.Single:
                case BlockType.TopLeft:
                case BlockType.TopRight:
                case BlockType.Top:
                case BlockType.Right:
                    SpriteIndex = new EksedraSprite(RunningEngine.Images["air_blocks"],
                                    new IntRect[] { new IntRect(((int) BlockPosition) * 64, 0, 64, 64) });
                    break;

                case BlockType.Middle:
                case BlockType.BottomLeft:
                case BlockType.BottomRight:
                case BlockType.Left:
                case BlockType.Bottom:
                    SpriteIndex = new EksedraSprite(RunningEngine.Images["air_blocks"],
                                    new IntRect[] { new IntRect((((int) BlockPosition) - 5) * 64, 64, 64, 64) });
                    break;

                default:
                    SpriteIndex = null;
                    break;
            }

            if(SpriteIndex != null)
                SpriteIndex.Smooth = false;
        }
    }


    public abstract class JumpThrough : GameObject {
        public override void EarlyUpdate(float deltaTime) {}
        public override void LateUpdate(float deltaTime) {}
        public override void OnKeyUp(bool[] keyState) {}
        public override void OnKeyDown(bool[] keyState) {}
        public override void OnKeyOff(bool[]  keyState) {}
        public override void OnKeyHeld(bool[] keyState) {}
        public override void OnTimer(int timerIndex) {}
        public override void OnCollision(GameObject other) {}

        public JumpThrough() {
            Tag = "JumpThrough";
        }

        public override void Draw(RenderTarget target, RenderStates states) {
            if(SpriteIndex != null)
                target.Draw(SpriteIndex);
        }

        public override void Update(float deltaTime) {
            if(RunningEngine.CurrentRoom == "title") {
                X -= 256 * deltaTime;

                if(X + 32 <= 0)
                    X = 1280 / 2;
            }
        }
    }

    public class GrassThrough : JumpThrough {
        public GrassThrough(int x, int y) : base() {
            X = x;
            Y = y;
        }
        
        public override void Init() {
            Tag = "GrassThrough";
            Depth = 1;

            SpriteIndex = new EksedraSprite(RunningEngine.Images["grass_jump"], new IntRect[] { new IntRect(0, 0, 64, 64) });
            SpriteIndex.Smooth = false;
            ImageSpeed = 0;
            ImageIndex = 0;

            MaskX = -32;
            MaskY = -32;
            MaskWidth = 64;
            MaskHeight = 9;
        }
    }

    public class CloudThrough : JumpThrough {
        public CloudThrough(int x, int y) : base() {
            X = x;
            Y = y;
        }
        
        public override void Init() {
            Tag = "CloudThrough";
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
    }
}