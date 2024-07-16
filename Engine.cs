using System.Collections;
using Microsoft.Xna.Framework;
using System.Threading;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using MyLevelEditor;
using System.Windows.Input;
using System.Xml.Linq;
using System.Linq;
using Editor.Controls;
using System.Windows;
using MonoGame.Forms.NET.Components;
using WinFormsApp1;
using Editor;

namespace Cyanen
{
    
    public class Cyan
    {
        static public bool IsGameStop = false;
        static public bool IsGUI = true;
        static private int GameTick = 50;
        static private int SubGameTick = 5;
        static public bool IsEngineStarted = false;

        static private Thread CoreTd;
        static private Thread TerrTd;
        static private Thread STH;
        static private Thread IC;

        public static ILogger logger;
        public static int[] FormSize = [0, 0];


        public static void Init(/*string[] args*/)
        {
            //有两个信息流线程,一个用于日志输出,另一个用作监听用户输入
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddSimpleConsole(options =>
            {
                options.TimestampFormat = "HH:MM:ss ";
                options.SingleLine = true;
            }));

            logger = factory.CreateLogger("Program");

            CoreTd = new Thread(Core)
            {
                Name = "Core",
                IsBackground = true,
            };
            CoreTd.Start();
            IsEngineStarted = true;
            logger.LogInformation("Engine Started");
            
            TerrTd = new Thread(new ThreadStart(Terrain.ListenRequests))
            {
                Name = "TerrainGenerator",
                IsBackground = true
            };
            TerrTd.Start();

            STH = new Thread(new ThreadStart(SubTick))
            {
                IsBackground = true,
                Name = "SubtickHandler"
            };
            STH.Start();

            IC = new Thread(new ThreadStart(InputCommond))
            {
                IsBackground = false,
                Name = "Commond"
            };
            //IC.Start();
        }
        
        private static void InputCommond()
        {
            while (true)
            {
                Console.Write(">");
                var command = Console.ReadLine();

                
                if (command == "exit")
                {
                    ShutDown(command);
                }
                else if (command.StartsWith("move"))
                {
                    logger.LogInformation("Camera Move : " + command);
                }
                else
                {
                    logger.LogInformation("Input : " + command);
                }
            }

        }

        static Dictionary<Vector2 , RenderProrerty[,]> RenderProrertyGroup = new Dictionary<Vector2, RenderProrerty[,]>();
        private static int GenTimes = 0;

        public static Dictionary<Vector2, Block[,]> LoadedTerrain = new Dictionary<Vector2, Block[,]>();

        private static void Core()
        { 
            object[] LoadObject = new object[] { };
            int GameTime = 0;
            int ExitCode = 0;

            bool IsReportGraphicStart = false;

            Random random = new Random();

            while (true)
            {
                GameTime++;
                if (IsGameStop)
                {
                    Terrain.IsGeneratorWork = false;
                    ExitCode = 1;
                    break;
                }
                if (IsReportGraphicStart == false && Graphic.IsStarted) 
                {
                    //Console.CursorLeft = 0;
                    //logger.LogInformation("Graphic Started");
                    IsReportGraphicStart = true;
                }

                var ReqTerr = DataBase.CheckTerrianExist(LoadedTerrain.Keys.ToArray(), DataBase.Entities[DataBase.MainEntityId].ChunkPos);
                if (ReqTerr.Count() != 0)
                {
                    foreach (Vector2 ChunkPos2 in LoadedTerrain.Keys)
                    {
                        Vector2 TmpV2 = ChunkPos2 - DataBase.Entities[DataBase.MainEntityId].ChunkPos;
                        if (TmpV2.X > 3 || TmpV2.X < -3 || TmpV2.Y > 3 || TmpV2.Y < -3)
                        {
                            LoadedTerrain.Remove(ChunkPos2);
                            RenderProrertyGroup.Remove(ChunkPos2);
                        }
                    }
                    foreach (Vector2 v2 in ReqTerr)
                    {
                        LoadedTerrain.Add(v2, DataBase.CachedTerraian[v2]);
                        RenderProrertyGroup[v2] = DataBase.CachedTerraianRP[v2];
                    }
                    Graphic.StaticRenderObj = RenderProrertyGroup.Values.ToArray();
                }

                

                Thread.Sleep(GameTick);
            }
        }

        private static void SubTick()
        {
            while (true)
            {
                //键盘事件处理
                foreach (Keys KeyName in DataBase.RegisterKeys)
                {
                    if (DataBase.IsKeyDown[KeyName] == true)
                    {
                        object[] keyOprate = DataBase.KeyOpration[KeyName];
                        if (keyOprate != null && keyOprate.Length > 0)
                        {
                            if (keyOprate[0] as string == "Move")
                            {
                                DataBase.Entities[DataBase.MainEntityId].PreMove += (Vector2)keyOprate[1]* DataBase.Entities[DataBase.MainEntityId].Speed;
                            }
                            else if (keyOprate[0] as string == "ShutDown")
                            {
                                ShutDown((string)keyOprate[1]);
                            }
                            else if (keyOprate[0] as string == "Menu")
                            {
                                if (keyOprate[1] as string == "Main")
                                {
                                    bool MainIsPressed;
                                }
                            }
                        }
                    }
                }

                //鼠标事件处理
                if (Graphic.Camera2Ds[0] != null)
                {
                    if (DataBase.mouseData.IsLeftPress == true)
                    {
                        DataBase.Cameras[DataBase.ScreenCameraId].UnfowllowEntity();
                        float xPos = DataBase.mouseData.Position.X - (GameProperty.WindowWidth / 2);
                        float yPos = DataBase.mouseData.Position.Y - (GameProperty.WindowHeight / 2);
                        var BaseVector = new Vector2(xPos, yPos);
                        BaseVector.Normalize();
                        Graphic.Camera2Ds[0].Position += BaseVector * 3;
                        DataBase.Cameras[DataBase.ScreenCameraId].Position = Graphic.Camera2Ds[0].Position;
                    }
                    else
                    {
                        DataBase.Cameras[DataBase.ScreenCameraId].FollowEntity(DataBase.Cameras[DataBase.ScreenCameraId].FollowEntityId);
                    }
                    if (DataBase.mouseData.IsRightPress == true)
                    {
                        Graphic.Camera2Ds[0].Zoom(0.01f+Graphic.Camera2Ds[0].GetZoom());
                    }
                    else
                    {
                        Graphic.Camera2Ds[0].Zoom(0.1f);
                    }
                }

                //摄像机事件处理
                foreach (int CameraId in DataBase.Cameras.Keys)
                {
                    Camera OprateCamera = DataBase.Cameras[CameraId];
                    if (OprateCamera != null)
                    {
                        if (OprateCamera.IsFollow == true)
                        {
                            DataBase.Cameras[CameraId].UpdatePosition();

                            if (Graphic.Camera2Ds[0] != null && DataBase.mouseData.IsLeftPress != true)
                            {
                                Graphic.Camera2Ds[0].Position = DataBase.Cameras[DataBase.ScreenCameraId].Position;
                            }
                        }
                    }
                }

                //处理实体事件
                int entityCount = -1;
                foreach (Entity entity in DataBase.Entities.Values)
                {

                    entityCount++;
                    entity.ChunkPos = GameData.ChunkPositioin(entity.Position);
                    RenderProrerty[] EntityRP = new RenderProrerty[DataBase.Entities.Count()];
                    if (entity != null)
                    {
                        if (CheckCrashBox())
                        {
                            entity.Position += entity.PreMove;
                            entity.PreMove = new Vector2();
                            entity.BlockPos = entity.Position;
                            entity.ChunkPos = GameData.ChunkPositioin(entity.Position);
                            EntityRP[entityCount] = new RenderProrerty()
                            {
                                RenderPosition = entity.Position,
                                RenderSize = new Vector2(16,16),
                                Texture2DId = 1,
                            };
                        }

                    }
                    Graphic.DynamicRenderObj = EntityRP;
                    Graphic.DynamicChanged = true;//临时方案
                }

                //Graphic.DynamicRenderObj = tmg;
                Thread.Sleep(SubGameTick);
            }

        }

        static public void ShutDown(string ExitMsg)
        {
            logger.LogInformation("Engine Shut Down By : " + ExitMsg);
            Environment.Exit(0);
        }
        static public bool CheckCrashBox()
        {
            return true;
        }
    }

    public class Camera
    {
        public Vector2 Position { get; set; } = new Vector2();
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int FollowEntityId { get; set; }
        public bool IsFollow;
        public float Speed { get; set; }
        public Camera(int id)
        {
            Position = new Vector2();
            Id = id;
            int StatCode = DataBase.Register(Id, this);
            if (StatCode == -1)
            {
                Id = DataBase.Register(0, this);
            }
            else if (StatCode > 0)
            {
                Id = StatCode;
            }
        }

        public Camera(int id , Vector2 Pos)
        {
            Position = Pos;
            Id = id;
            int StatCode = DataBase.Register(Id, this);
            if (StatCode == -1)
            {
                Id = DataBase.Register(0, this);
            }
            else if (StatCode > 0)
            {
                Id = StatCode;
            }
        }

        public int GetId()
        {
            return Id;
        }

        public void FollowEntity(int id)
        {
            FollowEntityId = id;
            IsFollow = true;
            UpdatePosition();
        }

        public void UnfowllowEntity()
        {
            IsFollow = false;
        }

        public void UpdatePosition()
        {
            //Style 1:直接移动到目标
            //Position = DataBase.Entities[FollowEntityId].Position;
            //Style 2:平滑移动
            Position -= (Position - DataBase.Entities[FollowEntityId].Position) * 0.02f ;
        }
    }
    
    public class Entity
    {
        public int Id;
        public string Name = "Default";
        public string Description;
        public float Speed = 10;//unit px
        
        public int TextrueID = 65534;
        public bool IsUsing = true;
        public Vector2 Size = new Vector2(32 , 32);
        public Vector2 TransSize = new Vector2();
        public Vector2 Position = new Vector2(0 , 0);
        public Vector2 Rotation = new Vector2();
        public Vector2 Crashbox = new Vector2(32 , 32);
        public Vector2 ChunkPos = new Vector2();
        public Vector2 BlockPos = new Vector2();
        public Vector2 PreMove = new Vector2();
        public bool SpiritStat { get; set; } = false;
        public int code;
        public Entity(int id , string name)
        {
            Id = id;
            Name = name;
            int StatCode = DataBase.Register(Id, this);
            if (StatCode == -1)
            {
                Id = DataBase.Register(0, this);
            }
            else if (StatCode > 0)
            {
                Id = StatCode;
            }
        }

        public Entity(int id)
        {
            Id = id;
            int StatCode = DataBase.Register(Id, this);
            if (StatCode == -1)
            {
                Id = DataBase.Register(0, this);
            }
            else if (StatCode > 0)
            {
                Id = StatCode;
            }
        }

        public void Spirit()
        {
            Crashbox = Vector2.Zero;
            DataBase.DeleteCrashBox(Id);
            SpiritStat = true;
        }

        public void UnSpirit()
        {
            Crashbox = Vector2.Zero;
            SpiritStat = true;
        }
    }

    static class GameData
    {
        static public Vector2 ChunkPositioin(Vector2 AbsolutelyPos)
        {
            int ChunkPosX = (int)Math.Floor(AbsolutelyPos.X / (GameProperty.BlockWidth * GameProperty.ChunkWidth));
            int ChunkPosY = (int)Math.Floor(AbsolutelyPos.Y / (GameProperty.BlockHeight * GameProperty.ChunkHeight));
            Vector2 ChunkPos = new Vector2(ChunkPosX, ChunkPosY);
            return ChunkPos;
        }
    }

    class GraphicOpration
    {

    }

    public struct GameProperty
    {
        public static int WindowWidth = 1080;
        public static int WindowHeight = 720;

        public static int BlockWidth = 16;
        public static int BlockHeight = 16;
        public static int ChunkWidth = 16;
        public static int ChunkHeight = 16;

        public static int LoadSize = 3;//unit:Chunk

    }



}