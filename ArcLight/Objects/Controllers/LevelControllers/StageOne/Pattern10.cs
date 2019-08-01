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
        public void PerformPattern10()
        {
            Vector2 center1 = new Vector2(Graphics.Width / 2, 268);
            Vector2 center2 = center1 + new Vector2(0, 32);
            Vector2 center3 = center1 - new Vector2(0, 16);
            Vector2 radius = new Vector2(0, -150);
            Vector2 radius2 = new Vector2(0, -180);
            Vector2 radius3 = new Vector2(0, -210);
            Vector2 pos;

            switch (frame)
            {
                //Wave 1
                case 0: pos = center1 + radius.TurnAngle(-60 + 20 * 0); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                case 5: pos = center1 + radius.TurnAngle(-60 + 20 * 1); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                case 10: pos = center1 + radius.TurnAngle(-60 + 20 * 2); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                case 15: pos = center1 + radius.TurnAngle(-60 + 20 * 3); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                case 20: pos = center1 + radius.TurnAngle(-60 + 20 * 4); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                case 25: pos = center1 + radius.TurnAngle(-60 + 20 * 5); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                case 30: pos = center1 + radius.TurnAngle(-60 + 20 * 6); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                //Wave 2
                case 45: pos = center2 + radius.TurnAngle(180 + (60 - 20 * 6)); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                case 50: pos = center2 + radius.TurnAngle(180 + (60 - 20 * 5)); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                case 55: pos = center2 + radius.TurnAngle(180 + (60 - 20 * 4)); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                case 60: pos = center2 + radius.TurnAngle(180 + (60 - 20 * 3)); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                case 65: pos = center2 + radius.TurnAngle(180 + (60 - 20 * 2)); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                case 70: pos = center2 + radius.TurnAngle(180 + (60 - 20 * 1)); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                case 75: pos = center2 + radius.TurnAngle(180 + (60 - 20 * 0)); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                //Wave 3
                case 90: pos = center3 + radius2.TurnAngle(-90 + -60 + 20 * 0); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                case 100: pos = center3 + radius2.TurnAngle(-90 + -60 + 20 * 1); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                case 110: pos = center3 + radius2.TurnAngle(-90 + -60 + 20 * 2); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                case 120: pos = center3 + radius2.TurnAngle(-90 + -60 + 20 * 3); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                case 130: pos = center3 + radius2.TurnAngle(-90 + -60 + 20 * 4); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                case 140: pos = center3 + radius2.TurnAngle(-90 + -60 + 20 * 5); controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1)); break;
                //Wave 4
                case 160: pos = center3 + radius2.TurnAngle(-90 + 180 + (60 - 20 * 6)); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                case 170: pos = center3 + radius2.TurnAngle(-90 + 180 + (60 - 20 * 5)); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                case 180: pos = center3 + radius2.TurnAngle(-90 + 180 + (60 - 20 * 4)); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                case 190: pos = center3 + radius2.TurnAngle(-90 + 180 + (60 - 20 * 3)); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                case 200: pos = center3 + radius2.TurnAngle(-90 + 180 + (60 - 20 * 2)); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
                case 210: pos = center3 + radius2.TurnAngle(-90 + 180 + (60 - 20 * 1)); controller.AddEnemy(new EnemyS3(pos, 0, 0), new Stage1_P10(1)); break;
            }
            //Wave 5
            if (frame >= 240 && frame < 240 + (20 * 28))
            {
                if (frame % 20 == 0)
                {
                    pos = center3 + radius3.TurnAngle(((frame - 270) / 30) * 20);
                    controller.AddEnemy(new EnemyS3(pos, 0, 1), new Stage1_P10(1));
                }
            }
            else if (frame == 240 + (20 * 29)) spawn_end = true;
            if (spawn_end && Core.Controller.enemies.Count(e => e.alive) <= 0) { pattern_end = true; }
        }

    }
}
