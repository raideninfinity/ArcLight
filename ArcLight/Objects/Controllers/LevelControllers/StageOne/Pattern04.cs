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
        public void PerformPattern4()
        {
            float x = Graphics.Width / 6.0f;
            float y = 80;
            if (frame == 0)
            {
                for (int j = 0; j < 10; j++)
                {
                    float dy = 0 - y * j;
                    if (j % 2 == 0)
                    {
                        controller.AddEnemy(new EnemyS1(new Vector2(x * (0 + 1f), dy), 0, 0), new Stage1_P4(1));
                        controller.AddEnemy(new EnemyS1(new Vector2(x * (1 + 1f), dy), 0, 1), new Stage1_P4(1));
                        controller.AddEnemy(new EnemyS1(new Vector2(x * (2 + 1f), dy), 0, 0), new Stage1_P4(1));
                        controller.AddEnemy(new EnemyS1(new Vector2(x * (3 + 1f), dy), 0, 1), new Stage1_P4(1));
                    }
                    else
                    {
                        controller.AddEnemy(new EnemyS1(new Vector2(x * (0 + 2f), dy), 0, 1), new Stage1_P4(2));
                        controller.AddEnemy(new EnemyS1(new Vector2(x * (1 + 2f), dy), 0, 0), new Stage1_P4(2));
                        controller.AddEnemy(new EnemyS1(new Vector2(x * (2 + 2f), dy), 0, 1), new Stage1_P4(2));
                        controller.AddEnemy(new EnemyS1(new Vector2(x * (3 + 2f), dy), 0, 0), new Stage1_P4(2));
                    }
                }
            }
            else if (frame == 240)
            {
                spawn_end = true;
            }
            else if (frame == 1200)
            {
                EnemyAllLeave();
            }
            if (spawn_end && Core.Controller.enemies.Count(e => e.alive) <= 0) { pattern_end = true; }
        }
    }
}
