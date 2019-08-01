using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ArcLight
{
    public partial class LevelOneController : LevelController
    {
        bool mid_summoned = false;
        Enemy mid_enemy3, left_enemy3, right_enemy3;
        int wait_frames;
        float offset = 30;
        public void PerformPattern3()
        {
            if (!mid_summoned)
            {
                mid_enemy3 = controller.AddEnemy(new EnemyM1(new Vector2(Graphics.Width / 2, 136), 0, 0), new Stage1_P3A(1));
                controller.AddEnemy(new EnemyS2(Vector2.Zero, 0 + offset, 1), new Stage1_P3B(1, mid_enemy3));
                controller.AddEnemy(new EnemyS2(Vector2.Zero, 90 + offset, 1), new Stage1_P3B(1, mid_enemy3));
                controller.AddEnemy(new EnemyS2(Vector2.Zero, 180 + offset, 1), new Stage1_P3B(1, mid_enemy3));
                controller.AddEnemy(new EnemyS2(Vector2.Zero, 270 + offset, 1), new Stage1_P3B(1, mid_enemy3));
                mid_summoned = true;
            }
            else
            {
                if (!mid_enemy3.Active() && !spawn_end)
                {
                    if (wait_frames < 60)
                        wait_frames += 1;
                    else
                    {
                        left_enemy3 = controller.AddEnemy(new EnemyM1(new Vector2(Graphics.Width / 4, 208), 0, 1), new Stage1_P3A(1));
                        controller.AddEnemy(new EnemyS2(Vector2.Zero, 0 + offset, 0), new Stage1_P3B(1, left_enemy3));
                        controller.AddEnemy(new EnemyS2(Vector2.Zero, 90 + offset, 0), new Stage1_P3B(1, left_enemy3));
                        controller.AddEnemy(new EnemyS2(Vector2.Zero, 180 + offset, 0), new Stage1_P3B(1, left_enemy3));
                        controller.AddEnemy(new EnemyS2(Vector2.Zero, 270 + offset, 0), new Stage1_P3B(1, left_enemy3));
                        right_enemy3 = controller.AddEnemy(new EnemyM1(new Vector2(Graphics.Width / 4 * 3, 208), 0, 1), new Stage1_P3A(1));
                        controller.AddEnemy(new EnemyS2(Vector2.Zero, 0 + offset, 0), new Stage1_P3B(1, right_enemy3));
                        controller.AddEnemy(new EnemyS2(Vector2.Zero, 90 + offset, 0), new Stage1_P3B(1, right_enemy3));
                        controller.AddEnemy(new EnemyS2(Vector2.Zero, 180 + offset, 0), new Stage1_P3B(1, right_enemy3));
                        controller.AddEnemy(new EnemyS2(Vector2.Zero, 270 + offset, 0), new Stage1_P3B(1, right_enemy3));
                        spawn_end = true;
                    }
                }
            }
            if (frame == 900)
            {
                EnemyAllLeave();
                spawn_end = true;
            }
            if (spawn_end && Core.Controller.enemies.Count(e => e.alive) <= 0)
                pattern_end = true;
        }
    }
}
