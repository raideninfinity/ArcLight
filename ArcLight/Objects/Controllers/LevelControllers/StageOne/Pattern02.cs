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
        int phase = 0;
        List<Enemy> enemy_list = new List<Enemy>();
        Enemy e;
        int e_frame;
        public void PerformPattern2()
        {
            if (phase == 0)
            {
                e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2A(1)); enemy_list.Add(e);
                e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2A(2)); enemy_list.Add(e);
                e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2A(3)); enemy_list.Add(e);
                phase = 1; e_frame = 0;
            }
            else if (phase == 1)
            {
                if (enemy_list.Count(e => e.Active()) <= 0 || e_frame == 180)
                {
                    enemy_list.Clear();
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2B(1)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2B(2)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2B(3)); enemy_list.Add(e);
                    phase = 2; e_frame = 0;
                }
            }
            else if (phase == 2)
            {
                if (enemy_list.Count(e => e.Active()) <= 0 || e_frame == 180)
                {
                    enemy_list.Clear();
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2A(1)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2A(2)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2A(3)); enemy_list.Add(e);
                    phase = 3; e_frame = 0;
                }
            }
            else if (phase == 3)
            {
                if (enemy_list.Count(e => e.Active()) <= 0 || e_frame == 180)
                {
                    enemy_list.Clear();
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2B(1)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2B(2)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2B(3)); enemy_list.Add(e);
                    phase = 4; e_frame = 0;
                }
            }
            else if (phase == 4)
            {
                if (enemy_list.Count(e => e.Active()) <= 0 || e_frame == 180)
                {
                    enemy_list.Clear();
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2A(1)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2A(2)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2A(3)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2B(1)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2B(2)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2B(3)); enemy_list.Add(e);
                    phase = 5; e_frame = 0;
                }
            }
            else if (phase == 5)
            {
                if (enemy_list.Count(e => e.Active()) <= 0 || e_frame == 180)
                {
                    enemy_list.Clear();
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2A(1)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2A(2)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2A(3)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 0), new Stage1_P2B(1)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2B(2)); enemy_list.Add(e);
                    e = Core.Controller.AddEnemy(new EnemyS2(new Vector2(-64, 64), 0, 1), new Stage1_P2B(3)); enemy_list.Add(e);
                    phase = 6; e_frame = 0; spawn_end = true;
                }
            }
            e_frame += 1;
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
