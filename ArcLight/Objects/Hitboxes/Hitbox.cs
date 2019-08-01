using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public abstract class Hitbox
    {
        public abstract bool isRectangle();
        public bool isCircle() { return !isRectangle(); }
        public abstract bool Intersect(Vector2 pos, float angle, Hitbox target, Vector2 target_pos, float target_angle);
        public Vector2 size;
        public float radius;
    }
}
