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
        bool boss_alerted = false;
        public void PerformBossAlert()
        {
            if (frame >= 30)
            {
                if (!boss_alerted && controller.AnyPlayerAlive())
                {
                    Core.Session.movable = false;
                    controller.enemy_bullets.Clear();
                    ((GameScene)Core.Scene).BossAlert();
                    boss_alerted = true;
                }
                else
                {
                    if (boss_alerted && !((GameScene)Core.Scene).boss_alert)
                    {
                        pattern_end = true;
                    }
                }
            }
        }

        Enemy boss_dummy;
        BossEnemy boss;
        bool boss_summoned;
        bool time_bonus;
        int boss_timer = 0;

        public void PerformBossPattern()
        {
            if (!boss_summoned && frame == 0)
            {
                boss_dummy = controller.AddEnemy(new EnemyB1_Dummy(new Vector2(-100, -100), 0), null);
            }
            if (boss_dummy.dying && !boss_summoned) //30
            {
                boss = (BossEnemy)controller.AddEnemy(new EnemyB1(new Vector2(Graphics.Width / 2, 240), 0), new Stage1_Main_Boss(1));
                controller.AddEnemy(new EnemyB1M(boss, 0), new Stage1_Boss_M(1));
                controller.AddEnemy(new EnemyB1A(boss, 0), new Stage1_Boss_A(1));
                controller.AddEnemy(new EnemyB1B(boss, 0), new Stage1_Boss_B(1));
                controller.AddEnemy(new EnemyB1B(boss, 1), new Stage1_Boss_B(1));
                controller.AddEnemy(new EnemyB1C(boss, 0), new Stage1_Boss_C(1));
                controller.AddEnemy(new EnemyB1C(boss, 1), new Stage1_Boss_C(1));
                boss_summoned = true;
                ((GameScene)Core.Scene).SetBossEnemy(boss);
            }
            if (boss_summoned && !boss.alive && Core.Session.movable)
            {
                frame = 0;
                Core.Session.movable = false;
            }
            if (boss != null && boss.alive)
            {
                boss_timer = boss.timer;
            }
            if (boss_summoned && !boss.alive && frame >= 5 && !time_bonus)
            {
                frame = 0;
                time_bonus = true;
                float value = boss_timer * Status.Boss1TimerScore / (Status.Boss1Timer * 60);
                Core.Session.AllGainScore(value);
                string text = "Time Bonus " + ((int)value).ToString();
                if (value <= 0) text = "Time Bonus Failed";
                ((GameScene)Core.Scene).ShowMessage(text);
                if (value > 0) { Core.Session.SetFlag("boss_defeat", true); }
            }
            if (boss_summoned && !boss.alive && frame >= 30 && time_bonus && !((GameScene)Core.Scene).show_msg)
            {
                stage_end = true;
            }
        }
    }
}
