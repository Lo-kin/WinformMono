using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Editor.Controls;

using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;
using Editor;

namespace Cyanen
{

    static class DataBase
    {
        public static Random RandomBase = new Random();
        public static int GameSeed = 0;
        public static Dictionary<int, Player> Players = new Dictionary<int, Player>();
        public static Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();
        public static Dictionary<int, Camera> Cameras = new Dictionary<int, Camera>();
        public static int PlayerCount = 25565;
        public static int EntityCount = 25565;
        public static int CameraCount = 25565;
        public static int ScreenCameraId = 1;//决定屏幕使用哪一个摄像机
        public static int MainEntityId = 1;//决定操纵哪一个实体
        public static int MainPlayerId = 1;

        public static Dictionary<Vector2,Block[,]> CachedTerraian = new Dictionary<Vector2, Block[,]>();
        public static List<string> CachedTerraianLog = new List<string>();
        public static Dictionary<Vector2, RenderProrerty[,]> CachedTerraianRP = new Dictionary<Vector2, RenderProrerty[,]>();

        public static Dictionary<int, string> TextrueBase32 = new Dictionary<int, string>();
        public static Dictionary<int, string> FileSource = new Dictionary<int, string>();

        public static MouseData mouseData = new MouseData();
        public static Dictionary<Keys, bool> IsKeyDown = new Dictionary<Keys, bool>();
        public static Dictionary<Keys, object[]> KeyOpration = new Dictionary<Keys, object[]>();
        public static string[] KeyOprateID = new string[] { "Move", "ShutDown" , "Menu"};
        public static Keys[] RegisterKeys = [];

        public static bool IsInit = false;

        static DataBase()
        {
            if (GameSeed == 0)
            {
                GameSeed = RandomBase.Next(1 , int.MaxValue);
            }
            mouseData.Position = mouseData.Offset = new Vector2(0, 0);
            mouseData.IsLeftPress = mouseData.IsRightPress = false;

            Camera DefaultCam = new Camera(1, new Vector2(0, 0)) { };
            Player TestPlayer = new Player() {Description = "Test Player" };
            ScreenCameraId = TestPlayer.SoloId;
            MainEntityId = TestPlayer.BodyId;

            Keys[] keys = new Keys[] { Keys.W, Keys.S, Keys.D, Keys.A, Keys.Escape };
            RegisterKeys = keys;
            KeyOpration.Add(Keys.W, new object[] { KeyOprateID[0], new Vector2(0, -1) });
            KeyOpration.Add(Keys.S, new object[] { KeyOprateID[0], new Vector2(0, 1) });
            KeyOpration.Add(Keys.D, new object[] { KeyOprateID[0], new Vector2(1, 0) });
            KeyOpration.Add(Keys.A, new object[] { KeyOprateID[0], new Vector2(-1, 0) });
            KeyOpration.Add(Keys.Escape, new object[] { KeyOprateID[2], "Main" });
            foreach (Keys Name in keys)
            {
                IsKeyDown.Add(Name, false);
            }
            IsInit = true;
            Thread a = new Thread(test);
            a.Start();
        }

        private static void test()
        {
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        public static bool DeleteEntity(int id)
        {
            if (Entities.Keys.Contains(id))
            {
                Entities.Remove(id);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int Register(Vector2 ChunkPosition , bool ForcedOverwrite , Block[,] ChunkContent)
        {
            CachedTerraianLog.Add("add");
            if (CachedTerraian.ContainsKey(ChunkPosition) == true)
            {
                if (ForcedOverwrite == true)
                {
                    CachedTerraian[ChunkPosition] = ChunkContent;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                CachedTerraian[ChunkPosition] = ChunkContent;
            }
            return 0;
        }

        public static int Register(Vector2 ChunkPosition , bool ForcedOverwrite , RenderProrerty[,] RP)
        {
            if (!ForcedOverwrite == true)
            {
                CachedTerraianRP[ChunkPosition] = RP;
                return 1;
            }
            return 0;
        }

        public static int Register(int id, Entity Content)
        {
            if (Entities.Keys.Contains(id))
            {
                return -1;
            }
            else if (id == 0)
            {
                int Num = RandomBase.Next(1, EntityCount + 1);
                while (true)
                {
                    if (Entities.Keys.Contains(Num))
                    {
                        Num = RandomBase.Next(1, EntityCount + 1);
                    }
                    else
                    {
                        if (Content.SpiritStat == false)
                        {
                            //AddRectCrash(Content.Id, Content.RectCrashBox);
                        }
                        Entities.Add(Num, Content);
                        break;
                    }
                }
                return Num;
            }
            else
            {
                Entities.Add(id, Content);
                return 0;
            }
        }

        public static int Register(int id, Player Content)
        {
            if (Players.Keys.Contains(id))
            {
                return -1;
            }
            else if (id == 0)
            {
                int Num = RandomBase.Next(1, PlayerCount + 1);
                while (true)
                {
                    if (Players.Keys.Contains(Num))
                    {
                        Num = RandomBase.Next(1, PlayerCount + 1);
                    }
                    else
                    {
                        Players.Add(Num, Content);
                        return Num;
                    }
                }
            }
            else
            {
                Players.Add(id, Content);
                return 0;
            }
        }

        public static int Register(int id, Camera Content)
        {
            if (Cameras.Keys.Contains(id))
            {
                return -1;
            }
            else if (id == 0)
            {
                int Num = RandomBase.Next(1, CameraCount + 1);
                while (true)
                {
                    if (Cameras.Keys.Contains(Num))
                    {
                        Num = RandomBase.Next(1, CameraCount + 1);
                    }
                    else
                    {
                        Cameras.Add(Num, Content);
                        break;
                    }
                }
                return Num;
            }
            else
            {
                Cameras.Add(id, Content);
                return 0;
            }
        }

        static public List<Vector2> CheckTerrianExist(Vector2[] ChunkExsist ,Vector2 ChunkPos)//检查视距内区块是否需要更新，如果需要，则报告进行生成或是直接返回需要的区块
        {
            List<Vector2> CacheExistTerrian = new List<Vector2>();
            List<Vector2> CacheNotExistTerrian = new List<Vector2>();
            List<Vector2> NoNeedTerrain = new List<Vector2>();

            foreach (Vector2 ChunkPos2 in ChunkExsist)
            {
                Vector2 TmpV2 = ChunkPos2 - ChunkPos;
                if (!(TmpV2.X > 3 || TmpV2.X < -3 || TmpV2.Y > 3 || TmpV2.Y < -3)) 
                {
                    NoNeedTerrain.Add(ChunkPos2);
                }
            }

            for (float i = ChunkPos.Y - 3; i < ChunkPos.Y + 3; i++)
            {
                for (float j = ChunkPos.X - 3; j < ChunkPos.X + 3; j++)
                {
                    Vector2 TmpVector2 = new Vector2(j, i);
                    if (!NoNeedTerrain.Contains(TmpVector2))
                    {
                        if (CachedTerraian.Keys.Contains(TmpVector2))
                        {
                            CacheExistTerrian.Add(TmpVector2);
                        }
                        else
                        {
                            if (!Terrain.RequestGeneTerr.Contains(TmpVector2))
                            {
                                CacheNotExistTerrian.Add(TmpVector2);
                            }
                        }
                    }
                }
            }

            Terrain.RequestGeneTerr.AddRange(CacheNotExistTerrian);
            return CacheExistTerrian;
        }

        static public void DeleteCrashBox(int Id)
        {
            
            
        }
    }

    public struct MouseData
    {
        public Vector2 Position;
        public Vector2 Offset;
        public bool IsRightPress;
        public bool IsLeftPress;
    }

    public static class TextureBase
    {
        public static Rectangle[] TextureRegion = new Rectangle[1024];
        public static Rectangle[,] MovementTextureRegion = new Rectangle[8, 1024];//每个动作分配八个帧，预留256个动作
        static TextureBase()
        {
            int TextureIdCount = -1;
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    TextureIdCount++;
                    Rectangle tmpRect = new Rectangle(new Point(j*16,i*16),new Point(16,16));
                    TextureRegion[TextureIdCount] = tmpRect;
                }
            }
            int MovementIdCount = -1;
            for (int i = 0;i < 256;i++)
            {
                for (int j = 0;j < 8;j++)
                {
                    MovementIdCount++;
                    Rectangle tmpRect = new Rectangle(new Point(j * 16, i * 16), new Point(16, 16));
                    MovementTextureRegion[j,i] = tmpRect;
                }
            }
        }
    }
}
