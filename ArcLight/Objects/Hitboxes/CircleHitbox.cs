using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ArcLight
{
    public class CircleHitbox : Hitbox
    {

        public CircleHitbox(float radius)
        {
            this.radius = radius;
        }

        public override bool isRectangle()
        {
            return false;
        }

        public override bool Intersect(Vector2 pos, float angle, Hitbox target, Vector2 target_pos, float target_angle)
        {
            if (target.isCircle()) return this.CheckCollision(pos, (CircleHitbox)target, target_pos);
            if (target.isRectangle()) return this.CheckCollision(pos, (RectangleHitbox)target, target_pos, target_angle);
            return false;
        }
    }
}
