using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class EmitterG : Emitter
    {
        EBulletVar bullet;
        public EmitterG(Enemy owner, EBulletVar b)
        {
            this.owner = owner;
            bullet = b;
        }

        public override void Update()
        {
            if (owner.dying || Utility.CheckCircleOutOfBounds(pos.X, pos.Y, owner.hitbox.radius) || !Core.Controller.AnyPlayerAlive())
            {
                Erase();
                return;
            }
            if (alive)
            {
                AddBullet(bullet);
                Erase();
            }
        }
    }
}