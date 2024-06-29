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
        public string Name { get; set; } = "John";
        public string Description { get; set; } = "None";
        public int Money = 0;

        public Effect[] Effects { get; set; }
        public Entity Body { get; set; }
        public Camera Solo { get; set; }

        public Player() 
        {
            Entity Ent = new Entity(0, Name);
            Camera cam = new Camera(0);
            Body = Ent;
            Solo = cam;
            Solo.FollowEntity(Body.Id);
        }

        public Player(int id, string name, string description, Entity body, int money, Camera solo)
        {
            Id = id;
            Name = name;
            Description = description;
            Body = body;
            Money = money;
            Solo = solo;
        }

        private void EventTrigger()
        {
            if (Body != null)
            {

            }
        }
    }
}
