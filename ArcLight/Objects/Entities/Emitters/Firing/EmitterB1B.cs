using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EmitterB1B : Emitter
    {
        int interval = 5;
        public EmitterB1B(Enemy owner, Vector2 pos, float angle, int polarity)
        {
            this.owner = owner;
            this.x = pos.X;
            this.y = pos.Y;
            frame = interval;
            this.angle = angle;
            this.polarity = polarity;
        }

        int count = 5;
        int phase = 0;
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
                    float offset = 5f;
                    float a = angle;
                    if (phase == 1)
                    {
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                        a = angle + offset;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                        a = angle + offset * 2;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                        a = angle + offset * 3;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                        a = angle - offset;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                        a = angle - offset * 2;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                        a = angle - offset * 3;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                    }
                    else
                    {
                        a = angle + offset * 0.5f;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                        a = angle + offset * 1.5f;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                        a = angle + offset * 2.5f;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                        a = angle - offset * 0.5f;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                        a = angle - offset * 1.5f;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                        a = angle - offset * 2.5f;
                        AddBullet(new EBulletVar(owner, pos, a, 1, 100f, 3.0f, 8.0f, polarity, 100));
                    }
                    count -= 1;
                    frame = 0;
                    phase += 1;
                    if (phase > 1) phase = 0;
                }
                if (count == 0) Erase();
            }
            base.Update();
        }
    }
}
