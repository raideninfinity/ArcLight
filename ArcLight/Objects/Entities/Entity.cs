using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public abstract class Entity
    {
        public float x;
        public float y;
        public float angle;
        public bool alive = true;
        public Vector2 pos { get { return new Vector2(x, y); } set { x = value.X; y = value.Y; } }
        public Hitbox hitbox { get { return GetHitbox(); } }
        protected abstract Hitbox GetHitbox();
        public abstract void Update();
        public abstract void Draw();
        public abstract void Kill();
        public abstract void Erase();
        public abstract bool Intersect(Entity e);
        public abstract bool Intersect(Hitbox h, Vector2 h_pos, float h_angle);
    }
}