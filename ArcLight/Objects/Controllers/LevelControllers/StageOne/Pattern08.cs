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
        public void PerformPattern8()
        {
            if (frame <= 600 && frame % 12 == 0)
            {
                controller.AddEnemy(new EnemyS1B(new Vector2(60, 0), 0, 1), new Stage1_P8(1));
                controller.AddEnemy(new EnemyS1B(new Vector2(Graphics.Width - 60, Graphics.Height), 0, 0), new Stage1_P8(2));
            }
            else if (frame == 620) { spawn_end = true; }
            if (spawn_end && Core.Controller.enemies.Count(e => e.alive) <= 0) { pattern_end = true; }
        }
    }
}
