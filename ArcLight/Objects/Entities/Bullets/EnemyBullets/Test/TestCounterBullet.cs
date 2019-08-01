using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    class TestCounterBullet : EnemyBullet
    {

        public TestCounterBullet(Enemy owner, Vector2 pos, float angle, int polarity = 2) : base(owner, pos, angle)
        {
            this.owner = owner;
            this.angle = angle;
            this.polarity = polarity;
            x = pos.X;
            y = pos.Y;
            type = 1;
            speed = 6.0f;
            size = 8.0f;
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            priority = 90;
        }
    }
}
