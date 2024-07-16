using MyLevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Editor.Controls;

namespace Cyanen
{
    public class Event
    {
        public int BindIndex;
        public string EventName;
        public string EventDescription;
        public string EventType;
        public string EventSource;
        public string EventTarget;
        public int TriggerSound = 1;
        public bool Visibility { get; set; } = true;

        public int DisposeAfterTiggerCount = -1;
        public bool IsDisposeTigger = false;

        public List<Player> PlayerList;

        public Event() 
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
                IsDisposeTigger = true;
            }

        }

        public string Name { get; set; }

        public virtual bool Trigger(int pId , int chg) 
        {
            return true;
        }

        public void Dispose() 
        {

        }
    }

    class ChangeMoney : Event
    {
        int TotalCount = 5;
        int ChangeCount = 1;
         
        public override bool Trigger(int PlayerId , int Changecount)
        {
            TiggerPorperty();

            if (IsDisposeTigger == false)
            {
                if (TotalCount < ChangeCount || TotalCount == 0)
                {
                    return true;
                }
                else if (TotalCount <= -1)
                {

                }
                else
                {
                    TotalCount -= Changecount;
                    DataBase.Players[PlayerId].Money += Changecount;
                }
            
            }
            return true;
        }

        public void Trigger(Player p) 
        {
            p.Money += ChangeCount;
        }

    }

    class ChangeSpeed :Event
    {
        int SpeedCount = 0;

        public override bool Trigger(int pId, int chg)
        {
            
            return true;
        }
    }

    class PlaySound : Event
    {
        int SoundId = 1;
        int DisposeAfterTiggerCount = 1;
        public override bool Trigger(int PlayerId , int Id)
        {
            base.TiggerPorperty();

            if (DisposeAfterTiggerCount != 0)
            {
                //Graphic.SoundEffectPlayList.Add(Id);
            }

            return true;
        }
    }
}
