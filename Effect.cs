using MyLevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Editor.Controls;

namespace Cyanen
{
    public class Effect
    {
        public int BindIndex;
        public string EventName;
        public string EventDescription;
        public string EventType;
        public string EventSource;
        public string EventTarget;
        public int TriggerSound = 1;

        public int DisposeAfterTiggerCount = -1;
        public bool DisposeTiggerBool = false;

        public List<Player> PlayerList;

        public Effect() 
        {

        }

        public virtual void TiggerPorperty()
        {
            if (DisposeAfterTiggerCount <= -1)
            {

            }
            else if (DisposeAfterTiggerCount > 0)
            {
                DisposeAfterTiggerCount--;
            }
            else if (DisposeAfterTiggerCount == 0)
            {
                DisposeTiggerBool = true;
            }

        }

        public string Name { get; set; }

        public virtual Player Trigger(Player p , int chg) 
        {
            return p;
        }

        public void Dispose() 
        {

        }
    }

    class ChangeMoney : Effect
    {
        int TotalCount = 5;
        int ChangeCount = 1;
        public override Player Trigger(Player p , int Changecount)
        {
            //base.TiggerPorperty();

            //if (DisposeTiggerBool == false)
            //{
                if (TotalCount < ChangeCount || TotalCount == 0)
                {
                    return p;
                }
                else if (TotalCount <= -1)
                {

                }
                else
                {
                    TotalCount -= Changecount;
                }
                p.Money += Changecount;
            //}
            return p;
        }

        public void Trigger(Player p) 
        {
            p.Money += ChangeCount;
        }

    }

    class PlaySound : Effect
    {
        int SoundId = 1;
        int DisposeAfterTiggerCount = 1;
        public override Player Trigger(Player p , int Id)
        {
            base.TiggerPorperty();

            if (DisposeAfterTiggerCount != 0)
            {
                //Graphic.SoundEffectPlayList.Add(Id);
            }

            return p;
        }
    }
}
