using Cyanen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.NET.Components;
using MonoGame.Forms.NET.Controls;

using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;

namespace Editor.Controls
{
    public class Graphic : MonoGameControl
    {
        public static bool IsStarted;
        public static bool RenderObjectComplete { get; set; }

        private Texture2D[] Texture2DGroup = new Texture2D[65535];
        public static Camera[] Cameras = new Camera[25565];
        public static Camera2D[] Camera2Ds = new Camera2D[25565];
        bool IsFollowChr = true;

        public static RenderProrerty[] StaticRenderObj = [new RenderProrerty()];
        public static RenderProrerty[] DynamicRenderObj = [new RenderProrerty()];
        public static RenderProrerty[] VitualRenderObj = [new RenderProrerty()];

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
            Camera2Ds[0] = MainCamera;
            Texture2DGroup[0] = Editor.Content.Load<Texture2D>("bin\\Windows\\Content\\grass");
            Texture2DGroup[1] = Editor.Content.Load<Texture2D>("bin\\Windows\\Content\\dirt");
            Texture2DGroup[2] = Editor.Content.Load<Texture2D>("bin\\Windows\\Content\\water");
            Texture2DGroup[65534] = Editor.Content.Load<Texture2D>("bin\\Windows\\Content\\ba");

            //Editor.Content.Load<Texture2D>("");

            // Remove FPS-Panel:
            //Components.Remove(Editor.FPSCounter);

            // Remove Default (Built-In) components (this includes the Camera2D):
            //Editor.RemoveDefaultComponents();
        }

        protected override void Update(GameTime gameTime)
        {

        }

        protected override void Draw()
        {
            Editor.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera2Ds[0].GetTransform());
            for (int i = 0; i < StaticRenderObj.Length; i++)
            {
                var OprateStaticObject = StaticRenderObj[i];
                Editor.spriteBatch.Draw(Texture2DGroup[OprateStaticObject.Texture2DId] ,new Rectangle(OprateStaticObject.RenderPosition.ToPoint() , new Point(32,32)), Color.White);
            }
            for (int i = 0; i < DynamicRenderObj.Length; i++)
            {
                var OprateDynamicObject = DynamicRenderObj[i];
                Editor.spriteBatch.Draw(Texture2DGroup[OprateDynamicObject.Texture2DId], OprateDynamicObject.RenderPosition, null, Color.White, OprateDynamicObject.RenderQuaternion, new Vector2(32, 32), 1, SpriteEffects.None, 0f);

            }
            for (int i = 0; i < VitualRenderObj.Length; i++)
            {

            }
            Editor.spriteBatch.End();
        }
    }
}
