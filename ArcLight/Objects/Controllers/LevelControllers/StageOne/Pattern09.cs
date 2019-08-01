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
        bool wave1_9 = false;
        Enemy enemy_9;
        float offset1 = 30;
        int p2_frame = 0;
        int spawn9_1 = 16;
        int spawn9_2 = 16;
        public void PerformPattern9()
        {
            if (!wave1_9)
            {
                enemy_9 = controller.AddEnemy(new EnemyM2(new Vector2(Graphics.Width / 2, 228), 0, 2), new Stage1_P9A(1));
                controller.AddEnemy(new EnemyS2(Vector2.Zero, 0 + offset1, 1), new Stage1_P9B(1, enemy_9));
                controller.AddEnemy(new EnemyS2(Vector2.Zero, 60 + offset1, 0), new Stage1_P9B(1, enemy_9));
                controller.AddEnemy(new EnemyS2(Vector2.Zero, 120 + offset1, 1), new Stage1_P9B(1, enemy_9));
                controller.AddEnemy(new EnemyS2(Vector2.Zero, 180 + offset1, 0), new Stage1_P9B(1, enemy_9));
                controller.AddEnemy(new EnemyS2(Vector2.Zero, 240 + offset1, 1), new Stage1_P9B(1, enemy_9));
                controller.AddEnemy(new EnemyS2(Vector2.Zero, 300 + offset1, 0), new Stage1_P9B(1, enemy_9));
                wave1_9 = true;
            }
            else
            {
                if (!enemy_9.alive && !spawn_end)
                {
                    if (p2_frame == 0)
                    {
                        controller.AddEnemy(new EnemyM1(new Vector2(66, 112), 0, 0), new Stage1_P9C(1));
                        controller.AddEnemy(new EnemyM1(new Vector2(Graphics.Width - 66, 112), 0, 1), new Stage1_P9C(1));
                    }
                    if (spawn9_1 > 0 && spawn9_2 > 0)
                    {
                        if (p2_frame % 15 == 0)
                        {
                            if (spawn9_1 > 0)
                            {
                                controller.AddEnemy(new EnemyS1(new Vector2(-64, -64), 0, 1), new Stage1_P9D(1));
                                spawn9_1 -= 1;
                            }
                            if (spawn9_2 > 0 && p2_frame > (15 * 8))
                            {
                                controller.AddEnemy(new EnemyS1(new Vector2(-64, -64), 0, 0), new Stage1_P9D(2));
                                spawn9_2 -= 1;
                            }
                        }
                    }
                    else
                    {
                        spawn_end = true;
                    }
                    p2_frame += 1;
                }
            }
            if (frame == 1200)
            {
                EnemyAllLeave();
                spawn_end = true;
            }
            if (spawn_end && Core.Controller.enemies.Count(e => e.alive) <= 0)
                pattern_end = true;
        }
    }
}
