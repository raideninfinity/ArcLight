using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EmitterB1A : Emitter
    {
        int interval = 10;
        public EmitterB1A(Enemy owner, Vector2 pos, float angle)
        {
            this.owner = owner;
            this.x = pos.X;
            this.y = pos.Y;
            frame = interval;
            this.angle = angle;
        }

        int count = 3;
        public override void Update()
        {
            if (owner.dying || Utility.CheckCircleOutOfBounds(pos.X, pos.Y, owner.hitbox.radius) || !Core.Controller.AnyPlayerAlive())
            {
                Erase();
                return;
            }
            else
            {
                this.x = owner.x;
                this.y = owner.y;
            }
            if (alive)
            {
                if (frame >= interval)
                {
                    float offset = 20f;
                    polarity = (owner.GetHPRatio() <= 0.5f) ? 1 : 0;
                    float a = angle;
                    AddBullet(new EBulletVar(owner, pos, a, 0, 200f, 3.0f, 12.0f, polarity, 100));
                    a = angle + offset;
                    AddBullet(new EBulletVar(owner, pos, a, 0, 200f, 3.0f, 12.0f, polarity, 100));
                    a = angle + offset * 2;
                    AddBullet(new EBulletVar(owner, pos, a, 0, 200f, 3.0f, 12.0f, polarity, 100));
                    a = angle - offset;
                    AddBullet(new EBulletVar(owner, pos, a, 0, 200f, 3.0f, 12.0f, polarity, 100));
                    a = angle - offset * 2;
                    AddBullet(new EBulletVar(owner, pos, a, 0, 200f, 3.0f, 12.0f, polarity, 100));
                    count -= 1;
                    frame = 0;
                }
                if (count == 0) Erase();
            }
            base.Update();
        }
    }
}
