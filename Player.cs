using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyanen
{
    public class Player
    {
        public int Id = 0;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "None";
        public int Money = 0;

        public Effect[] Effects { get; set; }
        public int BodyId { get; set; }
        public int SoloId { get; set; }

        public Player() 
        {
            Random rad = new Random();
            for (int  i = 0; i < 10; i++)
            {
                Name += Char.ConvertFromUtf32(rad.Next(25565));
            }
            Entity Ent = new Entity(0, Name);
            Camera cam = new Camera(0) {Name = Name};
            BodyId = Ent.Id;
            SoloId = cam.Id;
            DataBase.Cameras[SoloId].FollowEntity(BodyId);
            DataBase.Register(Id, this);
        }

        public Player(int id , string name)
        {
            Name = name;
            Id = id;
            Entity Ent = new Entity(1, Name);
            Camera cam = new Camera(0) { Name = Name };
            BodyId = Ent.Id;
            SoloId = cam.Id;
            DataBase.Cameras[SoloId].FollowEntity(BodyId);
            DataBase.Register(Id , this);
        }

        public Player(int id, string name, string description, int bodyid, int money, int soloid)
        {
            Id = id;
            Name = name;
            Description = description;
            BodyId = bodyid;
            Money = money;
            SoloId = soloid;
        }

        private void EventTrigger()
        {

        }
    }
}
