using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyanen
{
    public struct RenderProrerty
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public Vector2 RenderPosition { get; set; }
        public Vector2 RenderSize { get; set;}
        public float RenderQuaternion { get; set; }
        public int Texture2DId { get; set; }
    }
}
