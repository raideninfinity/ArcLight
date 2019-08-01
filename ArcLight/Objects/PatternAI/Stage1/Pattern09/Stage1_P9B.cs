using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P9B : PatternAI
    {

        Enemy host;
        public Stage1_P9B(int param, Enemy host)
        {
            this.param = param;
            this.host = host;
            f_interval = 8;
        }

        public override void Initialize(Enemy enemy)
        {
            this.enemy = enemy;
            enemy.prepare_frames = 60;
            enemy.hp *= 4;
        }

        public override void UpdateMovement()
        {
            if (!enemy.dying && !enemy.leaving)
            {
                Vector2 center = host.pos;
                Vector2 distance = new Vector2(0, 96);
                enemy.pos = center + distance.TurnAngle(enemy.angle);
                enemy.angle += 0.5f;
                if (host.dying)
                    enemy.Leave();
            }
        }

        public override void UpdateFiring()
        {
            if (f_frame >= f_interval)
            {
                AddEmitter(new EmitterG(enemy, new EBulletVar(enemy, enemy.pos, enemy.angle + 180 + 30, 1, 200f, 6.0f, 10.0f, enemy.polarity)));
                AddEmitter(new EmitterG(enemy, new EBulletVar(enemy, enemy.pos, enemy.angle + 180 - 30, 1, 200f, 6.0f, 10.0f, enemy.polarity)));
                f_frame = 0;
            }
        }

    }
}