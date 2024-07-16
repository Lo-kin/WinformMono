using Cyanen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.NET.Components;
using MonoGame.Forms.NET.Controls;

using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;
using Microsoft.Extensions.Logging;

namespace Editor.Controls
{
    public class Graphic : MonoGameControl
    {
        Random random = new Random();
        public static bool IsStarted;
        public static bool RenderObjectComplete { get; set; }

        private Texture2D[] Texture2DGroup = new Texture2D[65535];
        public static Camera[] Cameras = new Camera[25565];
        public static Camera2D[] Camera2Ds = new Camera2D[25565];
        bool IsFollowChr = true;

        public static RenderProrerty[][,] StaticRenderObj;
        public static RenderProrerty[] DynamicRenderObj;
        public static RenderProrerty[,] VitualRenderObj;
        public static bool DynamicChanged = true;

        // Fields & Properties here!
        private const string WelcomeMessage = "Welcome to MonoGame.Forms!";

        protected override void Initialize()
        {
            // Initialization & Content-Loading here!

            SetMultiSampleCount(8);
            RenderObjectComplete = false;
            IsStarted = true;
            Camera2D MainCamera = new Camera2D(Editor.GraphicsDevice);
            MainCamera.Position = new Vector2(0, 0);
            MainCamera.Zoom(1.5f);
            Camera2Ds[0] = MainCamera;
            Texture2DGroup[1] = Editor.Content.Load<Texture2D>("bin\\movement");
            Texture2DGroup[2] = Editor.Content.Load<Texture2D>("bin\\texture");
            for (int i = 0; i < Movements.Length; i++)
            {
                Movements[i] = new Frame();
            }
            //Editor.Content.Load<Texture2D>("");

            // Remove FPS-Panel:
            //Components.Remove(Editor.FPSCounter);

            // Remove Default (Built-In) components (this includes the Camera2D):
            //Editor.RemoveDefaultComponents();
        }

        int GraphicGameTime = 0;
        Frame[] Movements = new Frame[8];

        protected override void Update(GameTime gameTime)
        {
            GraphicGameTime++;
            if (DynamicChanged)
            {
                Array.Resize(ref Movements, DynamicRenderObj.Length);

                DynamicChanged = false;
            }
            if (GraphicGameTime % 10 == 0)
            {
                foreach (Frame frame in Movements)
                {
                    frame.Record();
                }
            }
        }
        
        protected override void Draw()
        {
            Editor.GraphicsDevice.Clear(Color.DarkSlateGray);
            Editor.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera2Ds[0].GetTransform());

            if (StaticRenderObj != null)
            {
                foreach (var objGrp in StaticRenderObj)
                {
                    foreach (var obj in objGrp)
                    {
                        Editor.spriteBatch.Draw(Texture2DGroup[2], new Rectangle(obj.RenderPosition.ToPoint(), new Point(16, 16)), TextureBase.TextureRegion[obj.Texture2DId], Color.White);
                    }
                    
                }
            }

            int Count = -1;
            if (DynamicRenderObj != null)
            {
                foreach (var obj in DynamicRenderObj)
                {
                    Count++;
                    Editor.spriteBatch.Draw(Texture2DGroup[1], obj.RenderPosition, TextureBase.MovementTextureRegion[Movements[Count].FramePosition, 0], Color.White, obj.RenderQuaternion, new Vector2(16, 16), 1, SpriteEffects.None, 0f);
                }
            }

            /*
            for (int i = 0; i < StaticRenderObj.Length; i++)
            {
                var obj = StaticRenderObj[i];
                Editor.spriteBatch.Draw(Texture2DGroup[2] ,new Rectangle(obj.RenderPosition.ToPoint(), new Point(16, 16)), TextureBase.TextureRegion[obj.Texture2DId], Color.White);
            }
            for (int i = 0; i < DynamicRenderObj.Length; i++)
            {
                var obj = DynamicRenderObj[i];
                Editor.spriteBatch.Draw(Texture2DGroup[1], obj.RenderPosition, TextureBase.MovementTextureRegion[Movements[i].FramePosition,0], Color.White, obj.RenderQuaternion, new Vector2(16, 16), 1, SpriteEffects.None, 0f);
            }
            for (int i = 0; i < VitualRenderObj.Length; i++)
            {

            }
            */
            Editor.spriteBatch.End();
        }
    }

    class Frame
    {
        public int MaxFrame = 8;
        public int FramePosition { get; set; } = 0;
        public int Record()
        {
            if (FramePosition == MaxFrame -1)
            {
                FramePosition = 0;
            }
            else
            {
                FramePosition++;
            }
            return FramePosition;
        }
    }
}
