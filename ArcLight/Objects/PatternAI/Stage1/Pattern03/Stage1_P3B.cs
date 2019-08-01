using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public class Stage1_P3B : PatternAI
    {

        Enemy host;
        public Stage1_P3B(int param, Enemy host)
        {
            this.param = param;
            this.host = host;
            f_interval = 3;
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
                Vector2 distance = new Vector2(0, 80);
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
                AddEmitter(new EmitterG(enemy, new EBulletVar(enemy, enemy.pos, enemy.angle + 180, 0, 2f, 8.0f, 12.0f, enemy.polarity)));
                f_frame = 0;
            }
        }

    }
}