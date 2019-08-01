using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    class TestBullet : EnemyBullet
    {

        public TestBullet(Enemy owner, Vector2 pos, float angle, int polarity = 2) : base(owner, pos, angle)
        {
            this.owner = owner;
            this.angle = angle;
            this.polarity = polarity;
            x = pos.X;
            y = pos.Y;
            type = 3;
            speed = 3.0f;
            size = 12.0f;
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            priority = 130;
        }
    }
}
