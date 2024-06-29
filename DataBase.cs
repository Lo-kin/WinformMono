using Cyanen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xna.Framework;

namespace Cyanen
{

    static class DataBase
    {
        public static Random RandomBase = new Random();
        public static bool test;
        public static bool test1;
        public static Microsoft.Xna.Framework.Point MouseMoveOffset = new Microsoft.Xna.Framework.Point(0, 0);
        public static Microsoft.Xna.Framework.Point MousePosition = new Microsoft.Xna.Framework.Point(0, 0);
        public static Rectangle[] RectCrashBox = new Rectangle[25565];
        public static List<int> RectCrashBoxId = new List<int>();
        public static Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();
        public static Dictionary<int, Camera> Cameras = new Dictionary<int, Camera>();
        public static Dictionary<int, string> TextrueBase32 = new Dictionary<int, string>();
        public static Dictionary<int, string> FileSource = new Dictionary<int, string>();
        public static Dictionary<Keys, bool> IsKeyDown = new Dictionary<Keys, bool>();
        public static Dictionary<Keys, object[]> KeyOpration = new Dictionary<Keys, object[]>();
        public static string[] KeyOprateID = new string[] { "Move", "ShutDown" , "Menu"};
        public static Microsoft.Xna.Framework.Point MousePrevPosition = new Microsoft.Xna.Framework.Point(0, 0);
        public static Keys[] RegisterKeys = new Keys[0];
        public static int EntityCount = 25565;
        public static int CameraCount = 25565;
        public static bool IsInit = false;

        static DataBase()
        {
            var DefaultCamera = new Camera(0);
            DefaultCamera.Description = "A Prepare Entity For Camera";
            Register(DefaultCamera.Id, DefaultCamera);
            var TestEt = new Entity(1, "Test");
            TestEt.Position = new Vector2(32 + 1000, 32 + 1000);
            TestEt.Description = "A Prepare Entity For Test";
            Register(TestEt.Id, TestEt);

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

        static public void AddRectCrash(int id, Rectangle rect)
        {
            RectCrashBoxId.Add(id);
            RectCrashBox[RectCrashBoxId.Count - 1] = rect;
        }

        public static bool Register(int id, Entity Content)
        {
            if (Entities.Keys.Contains(id))
            {
                return false;
            }
            else if (id == 0)
            {
                int Num = RandomBase.Next(0, EntityCount + 1);
                while (true)
                {
                    if (Entities.Keys.Contains(Num))
                    {
                        Num = RandomBase.Next(0, EntityCount + 1);
                    }
                    else
                    {
                        if (Content.SpiritStat == false)
                        {
                            AddRectCrash(Content.Id, Content.RectCrashBox);
                        }
                        Entities.Add(Num, Content);
                        break;
                    }
                }
                return true;
            }
            else
            {
                Entities.Add(id, Content);
                return true;
            }
        }

        public static bool Register(int id, Camera Content)
        {
            if (Cameras.Keys.Contains(id))
            {
                return false;
            }
            else if (id == 0)
            {
                int Num = RandomBase.Next(0, CameraCount + 1);
                while (true)
                {
                    if (Cameras.Keys.Contains(Num))
                    {
                        Num = RandomBase.Next(0, CameraCount + 1);
                    }
                    else
                    {
                        Cameras.Add(Num, Content);
                        break;
                    }
                }
                return true;
            }
            else
            {
                Cameras.Add(id, Content);
                return true;
            }
        }

        static public void DeleteCrashBox(int Id)
        {
            
            RectCrashBox[RectCrashBoxId.IndexOf(Id)] = null;
        }
    }
}
