using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EBulletVar : EnemyBullet
    {

        public EBulletVar(Enemy owner, Vector2 pos, float angle, int type, float energy, float speed, 
            float size, int polarity = 2, int priority = 100) : base(owner, pos, angle)
        {
            this.owner = owner;
            this.angle = angle;
            this.polarity = polarity;
            x = pos.X;
            y = pos.Y;
            this.energy = energy;
            this.type = type;
            this.speed = speed;
            this.size = size;
            Vector2 v = new Vector2(0, -speed);
            velocity = Vector2.Transform(v, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
            priority = 100;
        }
    }
}
