using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = Microsoft.Xna.Framework.Point;

namespace Cyanen
{
    public class Block
    {
        public int Id;
        public string Name;
        public string Description;
        public int TypeId;
        public int[] TextureIds = [255, 255];
        public Point Size = new Point(16,16);
        public Event[] Events;
        public bool Crossable { get; set; } = true;
        public Block()
        {

        }

        public virtual void LoadTexture()
        {

        }

        public int GetTextureId()
        {
            return TextureIds[0];
        }

        public int EventTrigger(int plyId)
        {
            foreach (Event e in Events)
            {
                if (e.IsDisposeTigger == false)
                {
                    e.IsDisposeTigger = true;
                    e.Trigger(plyId, 1);
                }
                else
                {
                    
                }
            }
            return 0;
        }
    }

    class Grass : Block
    {
        public string Name = "Grass";
        public int TypeId = 1;

        public Grass()
        {
            Event[] TmpEvents = [];
            LoadTexture();
        }

        public void Trigger(int EntityId)
        {

        }

        public override void LoadTexture()
        {
            TextureIds = [0 , 32 ,64 , 96];
        }
    }

    class Dirt : Block
    {
        public string Name = "Dirt";
        public int TypeId = 2;

        public Dirt()
        {
            Event[] TmpEvents = [];
            LoadTexture();
        }

        public Dirt(Point ChunkPos, Point BlockPos)
        {
            Event[] TmpEvents = [];
            LoadTexture();
        }

        public void Trigger(int EntityId)
        {

        }

        public override void LoadTexture()
        {
            TextureIds = [2];
        }
    }

    class Water : Block
    {
        public string Name = "Water";
        public int TypeId = 3;

        public Water()
        {
            Event[] TmpEvents = [];
            LoadTexture();
        }

        public void Trigger(int EntityId)
        {

        }

        public override void LoadTexture()
        {
            TextureIds = [1];
        }
    }

    class Sand : Block
    {
        public string Name = "Sand";
        public int TypeId = 4;

        public Sand()
        {
            Event[] TmpEvents = [];
            LoadTexture();
        }

        public void Trigger(int EntityId)
        {

        }

        public override void LoadTexture()
        {
            TextureIds = [3];
        }
    }
}
