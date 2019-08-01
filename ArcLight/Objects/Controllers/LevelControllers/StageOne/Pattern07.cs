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
        public void PerformPattern7()
        {
            if (frame == 15)
            {
                Vector2 center = new Vector2(Graphics.Width / 2, Graphics.Height / 2);
                float distance = 200;
                Vector2 pos = center + new Vector2(0, distance).TurnAngle(0);
                controller.AddEnemy(new EnemyM2(pos, 180, 1), new Stage1_P7(1));
                pos = center + new Vector2(0, distance).TurnAngle(60);
                controller.AddEnemy(new EnemyM2(pos, 240, 0), new Stage1_P7(2));
                pos = center + new Vector2(0, distance).TurnAngle(120);
                controller.AddEnemy(new EnemyM2(pos, 300, 1), new Stage1_P7(1));
                pos = center + new Vector2(0, distance).TurnAngle(180);
                controller.AddEnemy(new EnemyM2(pos, 0, 0), new Stage1_P7(2));
                pos = center + new Vector2(0, distance).TurnAngle(240);
                controller.AddEnemy(new EnemyM2(pos, 60, 1), new Stage1_P7(1));
                pos = center + new Vector2(0, distance).TurnAngle(300);
                controller.AddEnemy(new EnemyM2(pos, 120, 0), new Stage1_P7(2));
            }
            else if (frame == 240)
            {
                spawn_end = true;
            }
            else if (frame >= 1200)
            {
                EnemyAllLeave();
                pattern_end = true;
            }
            if (spawn_end && Core.Controller.enemies.Count(e => e.alive) <= 0)
                pattern_end = true;
        }
    }
}
