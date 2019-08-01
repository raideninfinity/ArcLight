using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{
    public partial class LevelOneController : LevelController
    {
        bool w1_5_summoned = false;
        bool w2_5_summoned = false;
        Enemy e1_5, e2_5, e3_5, e4_5;
        public void PerformPattern5()
        {
            if (!w1_5_summoned)
            {
                e1_5 = controller.AddEnemy(new EnemyM1B(new Vector2(90, 120), 120, 1), new Stage1_P5A(1));
                e2_5 = controller.AddEnemy(new EnemyM1B(new Vector2(Graphics.Width - 90, 120), 240, 0), new Stage1_P5A(2));
                w1_5_summoned = true;
            }
            else
            {
                if (!e1_5.alive && !e2_5.alive && !spawn_end)
                {
                    wait_frames += 1;
                    if (wait_frames >= 60 && !w2_5_summoned)
                    {
                        e3_5 = controller.AddEnemy(new EnemyM1B(new Vector2(186, 90), 180, 0), new Stage1_P5B(1));
                        e4_5 = controller.AddEnemy(new EnemyM1B(new Vector2(Graphics.Width - 186, 90), 180, 1), new Stage1_P5B(2));
                        w2_5_summoned = true;
                    }
                    else if (wait_frames == 240)
                    {
                        spawn_end = true;
                    }
                }
            }
            if (frame == 1500)
            {
                EnemyAllLeave();
                spawn_end = true;
            }
            if (spawn_end && Core.Controller.enemies.Count(e => e.alive) <= 0)
                pattern_end = true;
        }
    }
}
