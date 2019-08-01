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
        bool depth_msg_shown = false;
        public void PerformPattern1()
        {
            if (frame % 20 == 0)
            {
                if (param1 >= 0 && param1 < 12)
                    controller.AddEnemy(new EnemyS1(new Vector2(-64, -64), 0, 0), new Stage1_P1A(1));
                else if (param1 >= 12 && param1 < 24)
                    controller.AddEnemy(new EnemyS1(new Vector2(-64, -64), 0, 1), new Stage1_P1A(2));
                else if (param1 >= 24 && param1 < 26)
                {

                }
                else if (param1 >= 26 && param1 < 32)
                {
                    controller.AddEnemy(new EnemyS1B(new Vector2(-64, -64), 0, 0), new Stage1_P1B(1));
                    controller.AddEnemy(new EnemyS1B(new Vector2(-64, -64), 0, 0), new Stage1_P1B(2));
                }
                else if (param1 >= 32 && param1 < 36)
                {

                }
                else if (param1 >= 36 && param1 < 42)
                {
                    controller.AddEnemy(new EnemyS1B(new Vector2(-64, -64), 0, 1), new Stage1_P1B(1));
                    controller.AddEnemy(new EnemyS1B(new Vector2(-64, -64), 0, 1), new Stage1_P1B(2));
                }
                else if (param1 >= 42 && param1 < 46)
                {

                }
                else if (param1 >= 46 && param1 < 58)
                {
                    controller.AddEnemy(new EnemyS1(new Vector2(-64, -64), 0, 1), new Stage1_P1A(1));
                    controller.AddEnemy(new EnemyS1(new Vector2(-64, -64), 0, 0), new Stage1_P1A(2));
                }
                else if (param1 == 58)
                    spawn_end = true;
                param1 += 1;
            }
            if (spawn_end && Core.Controller.enemies.Count(e => e.alive) <= 0)
            {
                if (param2 < 30) param2 += 1;
                else
                {
                    if (!depth_msg_shown)
                    {
                        ((GameScene)Core.Scene).ShowMessage("Stage 1");
                        depth_msg_shown = true;
                    }
                    else
                    {
                        if (((GameScene)Core.Scene).frame >= 180)
                        {
                            pattern_end = true;
                        }
                    }
                }
            }
        }
    }
}
