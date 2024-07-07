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

namespace Cyanen
{
    
    public class Cyan
    {
        static public bool IsGraphicExit = false;
        static public bool IsGameStop = false;
        static public bool IsLoaded = false;
        static public bool IsGUI = true;
        static private int GameTick = 50;
        static private int SubGameTick = 5;
        static private int GameTimer = 0;
        static public bool TerrGeneStat;
        static internal int WindowWidth = 1280;
        static internal int WindowHeight = 720;
        static public bool IsEngineStarted = false;
        static public GameTime gameTime;

        static public ArrayList LogSave = new ArrayList();

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
            
            TerrTd = new Thread(TerrGenerator)
            {
                Name = "Gntr",
                IsBackground = true
            };
            TerrTd.Start();

            STH = new Thread(new ThreadStart(SubTick))
            {
                IsBackground = true,
                Name = "SubTickHandler"
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

        private void UpdateData()
        {
            while (true)
            {
                Thread.Sleep(100);
            }
        }

        private object GphTranslater()
        {
            try
            {

            }
            catch 
            {
                
            }
            return null;
        }

        public static Mutex PaintMutlock = new Mutex();
                                                  //区块长与宽，长宽包含块数 物体长与宽 最后是区块像素长与宽
        private static int[] GeneProperties = new int[7] {3 , 16 , 16 , 16 , 16 , 1 , 1};
        private static Vector3 GenePosition = new Vector3(0 , 0 , 0);//生成的起点，即生成矩形的中央
        public float[][,] TerrProperties { get; set; }
        //一个核心参数与下者的数据一致，但是为了方便理解
        static StaticObject[,][,] CyanObjOutput;
        static RenderProrerty[] RenderProrertyGroup = [new RenderProrerty()];

        private static bool IsCoreGetMut = true;
        private static bool IsUpdateStatic = true;
        private static int GenTimes = 0;

        /*
            for (int i = 0;i < cyo.lenth;i++)
            {
                
            }
        */
        private static bool IsTerrGeneAlive = true;

        private int RandomWeight(params float[] Weights)
        {
            foreach (float weight in Weights)
            {

            }
            return 0;
        }

        private static void TerrGenerator()
        {
            var ChunkArg = GeneProperties[0];
            float ChunkArgF = GeneProperties[0];
            float BlockWidth = GeneProperties[3];
            float BlockHeight = GeneProperties[4];
            var Width = GeneProperties[1];
            var Height = GeneProperties[2];

            var ChunkWidth = GeneProperties[5] = GeneProperties[1] * GeneProperties[3];
            var ChunkHeight = GeneProperties[6] = GeneProperties[2] * GeneProperties[4];
            StaticObject[,][,] output = new StaticObject[ChunkArg , ChunkArg][,];
            RenderProrerty[] tmpPropertyGroup = new RenderProrerty[ChunkArg * ChunkArg * Width * Height];

            Random Chaos = new Random();
            StaticObject CurrentInfo;
            if (IsGUI)
            {
                
                while (true)
                {
                    int BlockNum = 0;
                    if (!IsTerrGeneAlive)
                    {
                        break;
                    }
                    if (IsUpdateStatic)
                    {
                        GenTimes ++;
                        int BlockCount = 0;
                        for (int ChunkIndexY = 0; ChunkIndexY < ChunkArgF; ChunkIndexY++)
                        {
                            //现根据区块坐标,再根据物块相对于每个区块的坐标位移,最后设置出生点,即摄像头的位置
                            //单位都采用像素
                            //初始化算法：1 都向右上方位移
                            for (int ChunkIndexX = 0; ChunkIndexX < ChunkArgF; ChunkIndexX++)
                            {
                                float ChunkX = ChunkIndexX * ChunkWidth;
                                float ChunkY = ChunkIndexY * ChunkHeight;
                                var CurrentChunkVec3 = new Vector2(ChunkX, ChunkY);
                                var Temp2Mat = new StaticObject[Width, Height];

                                for (int y = 0; y < Height; y++)
                                {
                                    float BaseYHeight = ChunkY + (y * BlockHeight);
                                    for (int x = 0; x < Width; x++)
                                    {
                                        BlockNum++;

                                        float BaseXWidth = ChunkX + (x * BlockWidth);
                                        float LocalBlockXPos = BaseXWidth;
                                        float LocalBlockYPos = BaseYHeight;

                                        Vector2 CurrentBlockVec3 = new Vector2(x, y);
                                        Vector2 CurrentTransVec3 = new Vector2(LocalBlockXPos, LocalBlockYPos);

                                        int Noise = Chaos.Next(0, 5);
                                        if (Noise == 1) { Noise = 1; }
                                        if (Noise == 2) { Noise = 2; }
                                        else { Noise = 0; }
                                        //后台信息
                                        CurrentInfo = new StaticObject(Noise, CurrentChunkVec3, CurrentBlockVec3);
                                        if (Noise == 0)
                                        {
                                            CurrentInfo.Effects = [];
                                        }
                                        CurrentInfo.Position = CurrentTransVec3;

                                        Temp2Mat[x, y] = CurrentInfo;
                                        //渲染信息
                                        tmpPropertyGroup[BlockCount] = new RenderProrerty()
                                        {
                                            RenderPosition = CurrentTransVec3,
                                            Texture2DId = Noise,
                                            RenderQuaternion = 0,
                                        };
                                        BlockCount++;
                                    }
                                }
                                output[ChunkIndexX, ChunkIndexY] = Temp2Mat;
                            }
                        }
                        bool IsBaseLine = true;

                        if (IsBaseLine)
                        {
                            
                        }
                        RenderProrertyGroup = tmpPropertyGroup;
                        CyanObjOutput = output;
                        TerrGeneStat = true;
                        IsUpdateStatic = false;
                        Graphic.StaticRenderObj = tmpPropertyGroup;
                    }
                    Thread.Sleep(50);
                }
            }
            else
            {
                while (true)
                {
                    for (int ChunkIndY = 0; ChunkIndY < ChunkArgF ; ChunkIndY++)
                    {
                        for (int ChunkIndX = 0; ChunkIndX < ChunkArgF; ChunkIndX++)
                        {
                            var Temp2Mat = new StaticObject[Width - 1, Height - 1];
                            for (int y = 0; y < Width - 1; y++)
                            {
                                for (int x = 0; x < Height - 1; x++)
                                {

                                    if (Math.Sqrt(x) + Math.Sqrt(y) <= 3)
                                    {
                                        Temp2Mat[x, y].Id = 0;
                                    }
                                    else
                                    {
                                        Temp2Mat[x, y].Id = 1;
                                    }
                                }
                            }
                            output[ChunkIndX , ChunkIndY] = Temp2Mat;
                        }
                    }

                    TerrGeneStat = true;
                }
            }


        }

        //2个对象,前1个是属性,后面的是事件
        static public object[] EventObjs { get; set; }
        private static StaticObject[] objects { get; set; }

        public static bool IsReqUpdate = true;
        public static bool IsRightClick = true;
        public static bool IsInitGraphic = true;

        public static List<Entity> TestAddEnt { get; set; }
        public static List<int> TestAddEntStat { get; set; }

        private static void Core()
        { 
            object[] LoadObject = new object[] { };
            int GameTime = 0;
            int ExitCode = 0;
            IsLoaded = true;

            PaintMutlock.WaitOne();

            bool OwnMut;
            PaintMutlock = new Mutex(true, "Paint", out OwnMut);

            EventObjs = new Entity[] { };

            var ChunkArg = GeneProperties[0];
            float ChunkArgF = GeneProperties[0];
            float BlockWidth = GeneProperties[3];
            float BlockHeight = GeneProperties[4];
            var Width = GeneProperties[1];
            var Height = GeneProperties[2];

            bool CameraEntity = true;

            GeneProperties[5] = GeneProperties[1] * GeneProperties[3];
            GeneProperties[6] = GeneProperties[2] * GeneProperties[4];
            var ChunkWidthpx = GeneProperties[5];
            var ChunkHeightpx = GeneProperties[6];
            bool IsReportGraphicStart = false;
            TestAddEnt = new List<Entity>();
            TestAddEntStat = new List<int>();

            Random random = new Random();

            while (true)
            {
                GameTime++;

                if (IsGraphicExit)
                {
                    ExitCode = 0;
                    break;
                }
                if (IsGameStop)
                {
                    IsTerrGeneAlive = false;
                    ExitCode = 0;
                    break;
                }
                if (IsReportGraphicStart == false && Graphic.IsStarted) 
                {
                    //Console.CursorLeft = 0;
                    //logger.LogInformation("Graphic Started");
                    IsReportGraphicStart = true;
                }


                if (TestAddEnt.LongCount() != 0)
                {
                    for (int i = 0; i < TestAddEnt.LongCount(); i++)
                    {
                        TestAddEntStat.Add(DataBase.Register(0, TestAddEnt[i]));
                        TestAddEnt.Remove(TestAddEnt[i]);
                    }
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
                                DataBase.Entities[DataBase.MainEntityId].PreMove += (Vector2)keyOprate[1];
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
                        float xPos = DataBase.mouseData.Position.X - (WindowWidth / 2);
                        float yPos = DataBase.mouseData.Position.Y - (WindowHeight / 2);
                        var BaseVector = new Vector2(xPos, yPos);
                        BaseVector.Normalize();
                        Graphic.Camera2Ds[0].Position += BaseVector * 3;
                        DataBase.Cameras[DataBase.ScreenCameraId].Position = Graphic.Camera2Ds[0].Position;
                    }
                    else
                    {
                        DataBase.Cameras[DataBase.ScreenCameraId].FollowEntity(DataBase.Cameras[DataBase.ScreenCameraId].FollowEntityId);
                    }
                }

                //摄像机事件处理wwww
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
                    RenderProrerty[] EntityRP = new RenderProrerty[DataBase.Entities.Count()];
                    if (entity != null)
                    {
                        if (CheckCrashBox())
                        {
                            entity.Position += entity.PreMove;
                            entity.PreMove = new Vector2();
                            EntityRP[entityCount] = new RenderProrerty()
                            {
                                RenderPosition = entity.Position,
                                RenderSize = new Vector2(16,16),
                                Texture2DId = 65534,
                            };
                        }

                    }
                    Graphic.DynamicRenderObj = EntityRP;
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
            Position -= (Position - DataBase.Entities[FollowEntityId].Position) * 0.06f ;
        }
    }
    
    public class Entity
    {
        public int Id;
        public string Name = "Default";
        public string Description;
        public float Speed = 20;//unit px
        
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

    class StaticObject
    {
        public int Id;
        public string Name = "Default";
        public Effect[] Effects = new Effect[] {new ChangeMoney() , new PlaySound()};
        public string Description;
        public bool CrossAble = true;
        public Vector2 Position;
        public Vector2 Size = new Vector2(32, 32);
        public Vector2 ChunkPosition = new Vector2();
        public Vector2 BlockPosition = new Vector2();
        public Microsoft.Xna.Framework.Color Color;
        public int TextrueID = 1;
        string TextureName;
        public object[] Detail;
        
        public StaticObject(int id , string name , string description , Vector2 Cposition , Vector2 Bposition , object[] detail )
        {
            Id=id; 
            Name = name;
            description = Description;
            ChunkPosition = Cposition;
            BlockPosition = Bposition;
            Detail = detail;
        }

        public StaticObject(int id , Vector2 Cposition , Vector2 Bposition)
        {
            Id = id;
            var InitDetail = (new ObjectCollection());
            Detail = InitDetail.Objects[Id];
            Description = Detail[4] as string;
            CrossAble = (bool)Detail[3];
            Name = Detail[2] as string;
            TextrueID = (int)Detail[0];
            ChunkPosition = Cposition;
            BlockPosition = Bposition;
            TextureName = (string)Detail[1];
        }

        public Player EventTrigger(Player ply)
        {
            foreach (Effect e in Effects)
            {
                if (e.DisposeTiggerBool != true)
                {
                    if (e.DisposeTiggerBool != true)
                    {
                        e.DisposeAfterTiggerCount--;
                        if (e.DisposeAfterTiggerCount != 0)
                        {
                            e.DisposeTiggerBool = true;
                            ply = e.Trigger(ply, 1);
                        }
                    }

                    
                }
                else
                {
                    //销毁事件
                }
            }
            return ply;
        }
    }

    class ObjectCollection
    {
        public Dictionary<int, object[]> Objects ;
        public ObjectCollection()
        {   //物体id , 资源路径名称 , 名称 , 可穿过 , 描述 ,  速度衰减
            Objects = new Dictionary<int, object[]>
            {
                { 0, new object[] { 0, "grass" , "Grass" , false , "Vividly" ,  10} },
                { 1, new object[] { 1, "dirt" ,  "Dirt"  , false, "Poor" , 20} },
                { 2, new object[] { 2 , "wall" , "Wall"  , false , "Tight" , 0}}
            };
        }
    }

    class GraphicOpration
    {

    }

}