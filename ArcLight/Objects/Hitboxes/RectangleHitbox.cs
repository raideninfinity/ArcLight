using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ArcLight
{
    public class RectangleHitbox : Hitbox
    {
        public float inner_radius;

        public RectangleHitbox(float width, float height)
        {
            size = new Vector2(width, height);
            radius = size.Length() / 2.0f;
            inner_radius = (width < height) ? width / 2 : height / 2;
        }

        public override bool isRectangle()
        {
            return true;
        }

        public override bool Intersect(Vector2 pos, float angle, Hitbox target, Vector2 target_pos, float target_angle)
        {
            if (target.isCircle()) return ((CircleHitbox)target).CheckCollision(pos, this, target_pos, target_angle);
            if (target.isRectangle())
            {
                if (angle == 0 && target_angle == 0)
                {
                    Rectangle rect1 = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
                    Rectangle rect2 = new Rectangle((int)target_pos.X, (int)target_pos.Y, (int)target.size.X, (int)target.size.Y);
                    return rect1.Intersects(rect2);
                }
                else
                {
                    //Not Implemented - rotated X rotated / rotated X unrotated
                }
            }
            return false;
        }
    }
}
