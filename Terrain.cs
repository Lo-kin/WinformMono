using Cyanen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Editor
{
    internal static class Terrain
    {
        public static Random random = new Random();
        public static int seed = DataBase.GameSeed;
        public static int BlockWidth = GameProperty.BlockWidth;
        public static int BlockHeight = GameProperty.BlockHeight;
        public static int ChunkWidth = GameProperty.ChunkWidth;
        public static int ChunkHeight = GameProperty.ChunkHeight;
        public static int ChunkWidthpx = BlockWidth * ChunkWidth;
        public static int ChunkHeightpx = BlockHeight * ChunkHeight;
        public static int LoadChunkSize = GameProperty.LoadSize;//以本身区块为0，延伸size个区块
        public static Dictionary<Vector2, Block[,]> ChunkTerra = new Dictionary<Vector2, Block[,]>();
        public static Dictionary<Vector2, RenderProrerty[,]> ChunkTerrRp = new Dictionary<Vector2, RenderProrerty[,]>();
        public static List<Vector2> RequestGeneTerr = new List<Vector2>();
        private static List<Vector2> CachedRequestGeneTerr = new List<Vector2>();
        public static Dictionary<Vector2, bool> GeneStat = new Dictionary<Vector2, bool>();
        public static Vector2 SuccnFall = new Vector2();

        public static Vector2 GenePosition = new Vector2(0, 0);
        public static bool IsGeneratorWork = true;

        public static void ListenRequests() 
        {
            while (IsGeneratorWork)
            {
                if (RequestGeneTerr.Count != 0)
                {
                    CachedRequestGeneTerr.AddRange(RequestGeneTerr);
                    RequestGeneTerr = new List<Vector2>();
                    Thread GeneratorThread = new Thread(new ParameterizedThreadStart(Generator))
                    {
                        //IsBackground = true,
                        Name = "Generator"
                    };
                    GeneratorThread.Start(CachedRequestGeneTerr);
                    
                }
                Thread.Sleep(1000);
            }
        }

        public static void LoadTerrain()
        {

        }

        private static Vector2 Submit(Vector2 ChunkPos)
        {
            Vector2 MissnSucc = new Vector2();//成功数与失败数
            if (ChunkTerra.ContainsKey(ChunkPos))
            {
                int Code = DataBase.Register(ChunkPos, false, ChunkTerra[ChunkPos]);
                int RPCode = DataBase.Register(ChunkPos , false, ChunkTerrRp[ChunkPos]);
                
                ChunkTerrRp.Remove(ChunkPos);
                ChunkTerra.Remove(ChunkPos);
                if (Code == 0)
                {
                    MissnSucc.Y++;
                }
                else
                {
                    MissnSucc.X++;
                }
                MissnSucc.X++;
                
            }
            GeneStat[ChunkPos] = true;
            return MissnSucc;
        }

        public static void Generator(object CacheReqTerr)
        {
            var CPGP = CacheReqTerr as List<Vector2>;
            Vector2[] TmpVec2 = CachedRequestGeneTerr.ToArray();
            CachedRequestGeneTerr = new List<Vector2>();
            foreach (Vector2 v2 in TmpVec2) 
            {
                WhiteNoise(v2);
                Coast(v2);
                ChunkRp(v2);
                Submit(v2);
                CachedRequestGeneTerr.Remove(v2);
            }
        }

        public static void WhiteNoise(Vector2 ChunkPos)
        {
            Block[,] TmpChunk = new Block[GameProperty.ChunkWidth, GameProperty.ChunkHeight];
            for (int i = 0;i < GameProperty.ChunkHeight; i++)
            {
                for (int j = 0;j < GameProperty.ChunkWidth; j++)
                {
                    if (random.Next(0,5) == 0)
                    {
                        TmpChunk[j, i] = new Water();
                    }
                    else
                    {
                        TmpChunk[j, i] = new Grass();
                    }
                }
            }
            ChunkTerra[ChunkPos] = TmpChunk;
            //MessageBox.Show("");
        }

        public static void Coast(Vector2 ChunkPos)
        {
            Block[,] TmpChunk = ChunkTerra[ChunkPos];
            for (int i = 0; i < GameProperty.ChunkHeight; i++)
            {
                for (int j = 0; j < GameProperty.ChunkWidth; j++)
                {
                    if (TmpChunk[j, i].TypeId == 4)
                    {
                        for (int k = i - 1; k <= i+1; k++)
                        {
                            for (int l = j - 1; l <= j+1; l++)
                            {
                                if (TmpChunk[l, k].TypeId != 4)
                                {
                                    TmpChunk[l, k] = new Sand();
                                }
                            }
                        }
                    }
                }
            }
            ChunkTerra[ChunkPos] = TmpChunk;
        }

        public static void ChunkRp(Vector2 ChunkPos)
        {
            RenderProrerty[,] TmpChunkRp = new RenderProrerty[GameProperty.ChunkHeight , GameProperty.ChunkWidth];
            for (int i = 0; i < GameProperty.ChunkHeight; i++)
            {
                for (int j = 0; j < GameProperty.ChunkWidth; j++)
                {
                    RenderProrerty TmpRp;
                    TmpRp = new RenderProrerty()
                    {
                        RenderPosition = new Vector2((ChunkPos.X * ChunkWidthpx) + (j * BlockWidth), (ChunkPos.Y * ChunkHeightpx) + (i * BlockHeight)),
                        Texture2DId = (ChunkTerra[ChunkPos])[j, i].TextureIds[0],
                        RenderQuaternion = 0
                    };
                    TmpChunkRp[j, i] = TmpRp;
                }
            }
            ChunkTerrRp[ChunkPos] = TmpChunkRp;
        }

    }
}