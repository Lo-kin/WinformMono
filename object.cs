using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyLevelEditor
{
    internal class DynamicObject
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Vector2 StartPosition { get; set; }
        public Vector2 NowPosition { get; set; }
        public Vector2 StartOrientationVector2 { get; set; }
        public Vector2 EndPosition { get; set; }

        public DynamicObject() { }

        public void UpdateRenderPosition()
        {

        }
    }

    class Bullet : DynamicObject
    {
        public new void UpdateRenderPosition()
        {
            StartPosition = MoveType.Stright(NowPosition , StartOrientationVector2);
        }
    }

    public static class MoveType
    {
        public static float GameUpdateTick { get; set; }
        public static Vector2 Stright(Vector2 SourceVector2 , Vector2 MoveVector2)
        {
            return SourceVector2 + MoveVector2;
        }

        public static Vector2 Circle(Vector2 SourceVector2 , float Radius , Vector2 StartOrientationVector2 , Vector2 EndOrientationVector2 , bool CycleNegative)//如果结束方向向量为0,则表示不使用结束
        {
            double AngleBet = /*Vector.AngleBetween(new Vector(StartOrientationVector2.X, StartOrientationVector2.Y), new Vector(EndOrientationVector2.X, EndOrientationVector2.Y))*/0;
            if (EndOrientationVector2.X !=0 && EndOrientationVector2.Y !=0)
            {
                if (AngleBet == 0)//先判断始终角是否一致,节约性能
                {
                    return SourceVector2;
                }
                float Spd = StartOrientationVector2.Length() / GameUpdateTick;
                float Period = 2 * (float)Math.PI * Radius / Spd;
                if (CycleNegative)
                {
                    if (AngleBet > 0)
                    {
                        //施工
                    }
                }
                else
                {
                    if (AngleBet < 0)
                    {

                    }
                }

            }

            Vector2 End;
            return new Vector2();
        }
    }

    internal struct DynamicObjectTypes
    {
        public string Bullet;

    }
}
