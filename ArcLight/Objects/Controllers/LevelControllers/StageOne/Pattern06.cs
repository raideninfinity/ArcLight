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
        public void PerformPattern6()
        {
            if (frame == 0)
            {
                Vector2 center = new Vector2(Graphics.Width / 2.0f, 280);
                for (int i = 0; i < 6; i++)
                    controller.AddEnemy(new EnemyS1(center, 0, 0), new Stage1_P6(i * 60, 1, 1));
            }
            else if (frame == 60)
            {
                Vector2 center = new Vector2(Graphics.Width / 2.0f, 280);
                for (int i = 0; i < 6; i++)
                    controller.AddEnemy(new EnemyS1(center, 0, 1), new Stage1_P6(i * 60, 2, 1));
            }
            else if (frame == 90)
            {
                Vector2 center = new Vector2(Graphics.Width / 2.0f, 280);
                for (int i = 0; i < 6; i++)
                    controller.AddEnemy(new EnemyS1B(center, 0, 0), new Stage1_P6(i * 60, 1, 0));

            }
            else if (frame == 150)
            {
                Vector2 center = new Vector2(Graphics.Width / 2.0f, 280);
                for (int i = 0; i < 6; i++)
                    controller.AddEnemy(new EnemyS1B(center, 0, 1), new Stage1_P6(i * 60 + 30, 2, 0));
                spawn_end = true;
            }
            if (spawn_end && Core.Controller.enemies.Count(e => e.alive) <= 0) { pattern_end = true; }
        }
    }
}
