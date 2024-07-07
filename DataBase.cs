using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Editor.Controls;

namespace Cyanen
{

    static class DataBase
    {
        public static Random RandomBase = new Random();
        public static Dictionary<int, Player> Players = new Dictionary<int, Player>();
        public static Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();
        public static Dictionary<int, Camera> Cameras = new Dictionary<int, Camera>();
        public static int PlayerCount = 25565;
        public static int EntityCount = 25565;
        public static int CameraCount = 25565;
        public static int ScreenCameraId = 1;//决定屏幕使用哪一个摄像机
        public static int MainEntityId = 1;//决定操纵哪一个实体

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
}
