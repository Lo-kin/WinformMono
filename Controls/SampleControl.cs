using Cyanen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Forms.NET.Controls;

using Color = Microsoft.Xna.Framework.Color;

namespace Editor.Controls
{
    public class Graphic : MonoGameControl
    {
        public static bool IsStarted;
        public static bool RenderObjectComplete { get; set; }

        private Texture2D[] Texture2DGroup = new Texture2D[65535];
        private SoundEffect[] SoundSource = new SoundEffect[65535];
        private Song[] SongSource = new Song[65535];
        public static Camera[] Cameras = new Camera[25565];
        public static Vector2 CameraPostion = Vector2.Zero;
        public static Vector2 spriteSpeed = new Vector2(2, 2);
        public static Vector2 WorldPos = new Vector2(+320 - 32, 180 - 32);
        public static List<int> SoundEffectPlayList = new List<int>();
        bool IsFollowChr = true;

        public static RenderProrerty[] StaticRenderObj = [new RenderProrerty()];
        public static RenderProrerty[] DynamicRenderObj = [new RenderProrerty()];
        public static RenderProrerty[] VitualRenderObj = [new RenderProrerty()];

        private SoundEffect mySound;
        public static bool statp = true;

        // Fields & Properties here!
        private const string WelcomeMessage = "Welcome to MonoGame.Forms!";

        protected override void Initialize()
        {
            // Initialization & Content-Loading here!

            SetMultiSampleCount(8);
            RenderObjectComplete = false;
            IsStarted = true;
            Camera cm = new Camera(0);
            Cameras[0] = cm;
            Texture2DGroup[0] = Editor.Content.Load<Texture2D>("bin\\Windows\\Content\\grass");
            Texture2DGroup[1] = Editor.Content.Load<Texture2D>("bin\\Windows\\Content\\dirt");
            Texture2DGroup[2] = Editor.Content.Load<Texture2D>("bin\\Windows\\Content\\water");
            Texture2DGroup[65534] = Editor.Content.Load<Texture2D>("bin\\Windows\\Content\\ba");
            SoundSource[1] = Editor.Content.Load<SoundEffect>("bin\\Windows\\Content\\untitled");
            SongSource[0] = Editor.Content.Load<Song>("bin\\Windows\\Content\\MAIN");
            /*
            while (true)
            {
                if (Cyan.TerrGeneStat == true)
                {
                    break;
                }
                Thread.Sleep(200);
            }
            */
            MediaPlayer.IsRepeating = true;//重复播放背景音乐
            //MediaPlayer.Play(SongSource[0]);//播放背景音乐
            int WaitCount = 0;
            /*
            while (true)
            {
                WaitCount++;
                if (WaitCount >= 600)
                {

                }
                if (RenderObjectComplete == true)
                {

                }
                Thread.Sleep(100);
            }
            */
            //Editor.Content.Load<Texture2D>("");

            // Remove FPS-Panel:
            //Components.Remove(Editor.FPSCounter);

            // Remove Default (Built-In) components (this includes the Camera2D):
            //Editor.RemoveDefaultComponents();
        }

        protected override void Update(GameTime gameTime)
        {
            // Updating here!
            Cyan.gameTime = gameTime;
            if (SoundEffectPlayList.Count != 0)
            {
                for (int i = 0; i <= SoundEffectPlayList.Count; i++)
                {
                    SoundEffectPlayList.Remove(i);

                }
            }
        }

        protected override void Draw()
        {
            //默认的camera在四象限
            //Camera对象移动的方向与要绘制的对象反向
            Editor.spriteBatch.Begin();
            for (int i = 0; i < StaticRenderObj.Length; i++)
            {
                var OprateStaticObject = StaticRenderObj[i];
                Editor.spriteBatch.Draw(Texture2DGroup[OprateStaticObject.Texture2DId], OprateStaticObject.RenderPosition - Cameras[0].Position, Color.White);

            }
            for (int i = 0; i < DynamicRenderObj.Length; i++)
            {
                var OprateDynamicObject = DynamicRenderObj[i];
                Editor.spriteBatch.Draw(Texture2DGroup[OprateDynamicObject.Texture2DId], OprateDynamicObject.RenderPosition - Cameras[0].Position, null, Color.White, OprateDynamicObject.RenderQuaternion, new Vector2(32, 32), 1, SpriteEffects.None, 0f);
            }
            for (int i = 0; i < VitualRenderObj.Length; i++)
            {

            }
            Editor.spriteBatch.End();
        }

        static public double test = 0d;
        private void ViewPort2D(Vector2 CameraPosition)
        {

        }
    }
}
