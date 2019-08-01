using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcLight
{
    public class GameController
    {
        public List<Fighter> fighters = new List<Fighter>();
        public List<Enemy> enemies = new List<Enemy>();
        public Queue<Enemy> enemy_queue = new Queue<Enemy>();
        public List<PlayerBullet> player_bullets = new List<PlayerBullet>();
        public Queue<PlayerBullet> player_bullet_queue = new Queue<PlayerBullet>();
        public List<EnemyBullet> enemy_bullets = new List<EnemyBullet>();
        public Queue<EnemyBullet> enemy_bullet_queue = new Queue<EnemyBullet>();
        public List<Emitter> emitters = new List<Emitter>();
        public int elapsed_frames;
        public LevelController levelController;
        public bool StageEnd { get { return levelController.stage_end; } }

        bool enemy_bullet_clear_flag = false;

        public GameController()
        {
            Core.Session.InitStage();
            //SummonTestEnemy();
            switch (Core.Session.stage)
            {
                case 1: levelController = new LevelOneController(this); break;
            }
        }

        public void SummonTestEnemy()
        {
            Enemy enemy = AddEnemy(new EnemyS1B(new Vector2(96, 327), 0, 1), new TestAI());
            enemy.hp = 1000;
            enemy = AddEnemy(new EnemyS1(new Vector2(368, 175), 0, 1), new TestAI());
            enemy.hp = 1000;
            enemy = AddEnemy(new EnemyM2(new Vector2(247, 476), 0, 1), new TestAI());
            enemy.hp = 1000;
        }

        public void Update()
        {
            UpdateFighterSpawn();
            Core.Session.UpdateBurstChain();
            if (elapsed_frames >= 60) levelController.Update();
            //Update Components
            fighters.ForEach(a => a.Update());
            player_bullets.ForEach(a => a.Update());
            enemies.ForEach(a => a.Update());
            enemy_bullets.ForEach(a => a.Update());
            emitters.ForEach(a => a.Update());
            //Remove Components
            fighters.RemoveAll(a => !a.alive);
            player_bullets.RemoveAll(a => !a.alive);
            enemies.RemoveAll(a => !a.alive);
            enemy_bullets.RemoveAll(a => !a.alive);
            emitters.RemoveAll(a => !a.alive);
            if (enemy_bullet_clear_flag)
            {
                enemy_bullets.Clear();
                enemy_bullet_clear_flag = false;
            }
            //Finalize
            UpdatePlayerBulletCollision();
            UpdatePlayerEnemyCollision();
            UpdateEnemyBulletClearing();
            UpdateEnemyBulletCollision();
            UpdatePlayerBulletQueue();
            UpdateEnemyBulletQueue();
            UpdateEnemyQueue();
            elapsed_frames += 1;
        }

        void UpdateFighterSpawn()
        {
            if (Core.Session.Player1Online() && (fighters.Find(a => a.player == Core.Session.player1) == null))
            {
                Core.Session.ConsumeLife(Core.Session.player1);
                if (Core.Session.Player1Online())
                    fighters.Add(new Fighter(Core.Session.player1));
            }
            if (Core.Session.Player2Online() && (fighters.Find(a => a.player == Core.Session.player2) == null))
            {
                Core.Session.ConsumeLife(Core.Session.player2);
                if (Core.Session.Player2Online())
                    fighters.Add(new Fighter(Core.Session.player2));
            }
        }

        public Enemy AddEnemy(Enemy enemy, PatternAI pattern_ai = null)
        {
            if (pattern_ai != null)
            {
                enemy.SetPatternAI(pattern_ai);
                pattern_ai.Initialize(enemy);
            }
            enemy_queue.Enqueue(enemy);
            return enemy;
        }

        public void AddEmitter(Emitter emitter)
        {
            emitters.Add(emitter);
        }

        public void ClearEnemyBullets(Enemy e)
        {
            emitters.RemoveAll(a => a.owner == e);
            enemy_bullets.RemoveAll(a => a.owner == e);
        }

        void UpdatePlayerBulletQueue()
        {
            while (player_bullet_queue.Count > 0)
            {
                player_bullets.Add(player_bullet_queue.Dequeue());
            }
        }

        void UpdateEnemyBulletQueue()
        {
            while (enemy_bullet_queue.Count > 0)
            {
                enemy_bullets.Add(enemy_bullet_queue.Dequeue());
            }
        }

        void UpdateEnemyQueue()
        {
            while (enemy_queue.Count > 0)
            {
                enemies.Add(enemy_queue.Dequeue());
            }
        }

        void UpdatePlayerBulletCollision()
        {
            foreach (PlayerBullet bullet in player_bullets)
            {
                foreach (Enemy e in enemies)
                {
                    if (!e.Active() || bullet.is_setup) continue;
                    if (bullet.Intersect(e))
                    {
                        e.Damage(bullet);
                        if (!bullet.pierce)
                        {
                            bullet.alive = false;
                        }
                        if (bullet.on_hit)
                        {
                            bullet.OnHit();
                        }
                        if (!e.dying) { Audio.PlayBulletHitSE(); }
                    }
                }
            }
        }

        void UpdateEnemyBulletClearing()
        {
            foreach (EnemyBullet bullet in enemy_bullets)
            {
                foreach (PlayerBullet b in player_bullets)
                {
                    if (b.is_counter)
                    {
                        if (b.Intersect(bullet.hitbox, bullet.pos, bullet.angle))
                        {
                            bullet.Erase();
                            FireCounterB1(b);
                        }
                    }
                    else if (b.is_burst)
                    {
                        if (b.Intersect(bullet.hitbox, bullet.pos, bullet.angle))
                        {
                            bullet.Erase();
                        }
                    }
                }
            }
        }

        void FireCounterB1(PlayerBullet b)
        {
            Enemy e = GetNearestEnemy(b.pos);
            float angle = 0;
            if (e != null)
                angle = (180 - b.pos.AngleBetweenPoints(e.pos));
            AddPlayerBullet(new PBurstBulletB2(b.owner, b.pos, angle, e));
        }

        void UpdatePlayerEnemyCollision()
        {
            if (!Core.Session.movable) return;
            foreach (Enemy e in enemies)
            {
                if (!e.Active()) continue;
                foreach (Fighter f in fighters)
                {
                    if (!f.Active()) continue;
                    if (f.Intersect(e))
                    {
                        PlayerDeath(f, e);
                    }
                }
            }
        }

        void UpdateEnemyBulletCollision()
        {
            if (!Core.Session.movable) return;
            foreach (EnemyBullet bullet in enemy_bullets)
            {
                foreach (Fighter f in fighters)
                {
                    if (!f.Active()) continue;
                    if (f.IntersectAbsorb(bullet))
                    {
                        bullet.Erase();
                        float mult = 1.0f;
                        if (bullet.polarity < 2 && bullet.polarity != f.player.mode) mult = Status.AbsorbEnergyBonusMult;
                        Audio.PlayBulletAbsorbSE();
                        Core.Session.GainEnergy(f.player, bullet.energy * mult);
                        Core.Session.GainScore(f.player, bullet.energy * Status.AbsorbScoreMult);
                        continue;
                    }
                    if (bullet.Intersect(f))
                    {
                        PlayerDeath(f, null);
                        continue;
                    }
                    if (f.Graze(bullet))
                    {
                        f.AddGrazedBullet(bullet);
                    }
                }
            }
        }

        bool CheckIfBurstProtected(Fighter f)
        {
            return (player_bullets.Count(b => b.is_burst && b.Intersect(f)) > 0);
        }

        public void PlayerDeath(Fighter f, Enemy e = null)
        {
            if (!CheckIfBurstProtected(f) && !f.barrier && !Core.god_mode)
            {
                f.Kill();
                var list = player_bullets.FindAll(b => b.owner == f);
                list.ToList<PlayerBullet>().ForEach(b => b.Erase());
                player_bullets.RemoveAll(b => b.owner == f);
                if (!AnyPlayerAlive()) { enemy_bullet_clear_flag = true; }
                if (e != null)
                {
                    if (e.GetMaxHP() <= 500) e.Damage(null, 100);
                }
            }
        }

        public bool AnyPlayerAlive()
        {
            return (fighters.Count(f => f.Active()) > 0);
        }

        public void AddPlayerBullet(PlayerBullet bullet)
        {
            player_bullet_queue.Enqueue(bullet);
        }

        public void AddEnemyBullet(EnemyBullet bullet)
        {
            enemy_bullet_queue.Enqueue(bullet);
        }

        public void Draw()
        {
            enemies.ForEach(a => a.Draw());
            player_bullets = player_bullets.OrderBy(a => a.priority).ToList();
            player_bullets.ForEach(a => a.Draw());
            fighters.ForEach(a => a.Draw());
            enemy_bullets = enemy_bullets.OrderBy(a => a.priority).ToList();
            enemy_bullets.ForEach(a => a.Draw());
        }

        #region Targeting Algorithms

        public List<Enemy> GetEnemiesForBS1(Vector2 pos)
        {
            List<Enemy> e_list = enemies.FindAll(a => a.y < (pos.Y - 48) && ForwardPosOk(a, pos, Utility.tan45) && a.Active());
            e_list = e_list.OrderBy(a => (pos - a.pos).Length()).ToList<Enemy>();
            return e_list;
        }

        public bool ForwardPosOk(Enemy a, Vector2 pos, float angle)
        {
            float r_border_x = (a.pos.Y - pos.Y) / -angle + pos.X;
            float l_border_x = (a.pos.Y - pos.Y) / angle + pos.X;
            return (a.pos.X >= l_border_x && a.pos.X <= r_border_x);
        }

        public bool SidePosOk(Enemy a, Vector2 pos, float angle)
        {
            float u_border_y = (a.pos.X - pos.X) * -angle + pos.Y;
            float d_border_y = (a.pos.X - pos.X) * angle + pos.Y;
            if (a.pos.X < pos.X)
                return (a.pos.Y <= u_border_y && a.pos.Y >= d_border_y);
            else
                return (a.pos.Y >= u_border_y && a.pos.Y <= d_border_y);
        }

        public Enemy GetNearestEnemyForBF1(Vector2 pos)
        {
            float angle = Utility.tan30;
            List<Enemy> e_list = enemies.FindAll(a => (a.y <= pos.Y - 48) && ForwardPosOk(a, pos, angle) && a.Active());
            if (e_list.Count == 0) { return null; }
            e_list = e_list.OrderBy(a => Math.Abs(FixAngle(pos.AngleBetweenPoints(a.pos)))).ToList<Enemy>();
            return e_list[0];
        }

        public Enemy GetEnemyForAS2_Left(Vector2 pos)
        {
            /*Enemy enemy = null;
            RectangleHitbox hitbox = new RectangleHitbox(pos.X + 28, Graphics.Height - (pos.Y - 21));
            Vector2 bit_pos = new Vector2(pos.X - 28, pos.Y - 21);
            Vector2 hitbox_pos = new Vector2(hitbox.size.X / 2, (pos.Y - 21) + hitbox.size.Y / 2);
            List<Enemy> e_list = enemies.FindAll(a => a.Active());
            e_list = e_list.FindAll(a => a.Intersect(hitbox, hitbox_pos, 0));
            e_list = e_list.OrderBy(a => (bit_pos - a.pos).Length()).ToList();
            if (e_list.Count > 0) enemy = e_list[0];
            return enemy;*/
            Enemy enemy = null;
            Vector2 bit_pos = new Vector2(pos.X - 28, pos.Y - 21);
            List<Enemy> left = enemies.FindAll(a => a.pos.X <= pos.X);
            left = left.OrderBy(a => (bit_pos - a.pos).Length()).ToList();
            if (left.Count > 0) enemy = left[0];
            if (enemy != null)
                return enemy;
            List<Enemy> right = enemies.FindAll(a => a.pos.X > pos.X);
            right = right.OrderBy(a => (bit_pos - a.pos).Length()).ToList();
            if (right.Count > 0) enemy = right[0];
            return enemy;
        }

        public Enemy GetEnemyForAS2_Right(Vector2 pos)
        {
            /*Enemy enemy = null;
            RectangleHitbox hitbox = new RectangleHitbox(Graphics.Width - (pos.X - 28), Graphics.Height - (pos.Y - 21));
            Vector2 bit_pos = new Vector2(pos.X + 28, pos.Y - 21);
            Vector2 hitbox_pos = new Vector2((pos.X + 28) + hitbox.size.X / 2, (pos.Y - 21) + hitbox.size.Y / 2);
            List<Enemy> e_list = enemies.FindAll(a => a.Active());
            e_list = e_list.FindAll(a => a.Intersect(hitbox, hitbox_pos, 0));
            e_list = e_list.OrderBy(a => (bit_pos - a.pos).Length()).ToList();
            if (e_list.Count > 0) enemy = e_list[0];
            return enemy;*/
            Enemy enemy = null;
            Vector2 bit_pos = new Vector2(pos.X + 28, pos.Y - 21);
            List<Enemy> right = enemies.FindAll(a => a.pos.X > pos.X && a.Active());
            right = right.OrderBy(a => (bit_pos - a.pos).Length()).ToList();
            if (right.Count > 0) enemy = right[0];
            if (enemy != null)
                return enemy;
            List<Enemy> left = enemies.FindAll(a => a.pos.X <= pos.X && a.Active());
            left = left.OrderBy(a => (bit_pos - a.pos).Length()).ToList();
            if (left.Count > 0) enemy = left[0];
            return enemy;
        }

        public float FixAngle(float angle)
        {
            angle = 180 - angle;
            if (angle > 180) angle = -(360 - angle);
            return angle;
        }

        public Fighter GetNearestFighter(Vector2 pos)
        {
            List<Fighter> list = fighters.FindAll(a => a.Active() && a.Active());
            if (list.Count == 0) { return null; }
            list = list.OrderBy(a => (pos - a.pos).Length()).ToList();
            return list[0];
        }

        public Fighter GetFurthestFighter(Vector2 pos)
        {
            List<Fighter> list = fighters.FindAll(a => a.Active() && a.Active());
            if (list.Count == 0) { return null; }
            list = list.OrderBy(a => (pos - a.pos).Length()).Reverse().ToList();
            return list[0];
        }

        public Enemy GetNearestEnemy(Vector2 pos)
        {
            List<Enemy> list = enemies.FindAll(a => a.Active());
            if (list.Count == 0) { return null; }
            list = list.OrderBy(a => (pos - a.pos).Length()).ToList();
            return list[0];
        }

        public Enemy GetFrontNearestEnemy(Vector2 pos)
        {
            List<Enemy> e_list = enemies.FindAll(a => (a.y <= pos.Y) && a.Active());
            if (e_list.Count == 0) { return null; }
            e_list = e_list.OrderBy(a => (pos - a.pos).Length()).ToList<Enemy>();
            return e_list[0];
        }

        public Enemy GetBackNearestEnemy(Vector2 pos)
        {
            List<Enemy> e_list = enemies.FindAll(a => a.y > pos.Y && a.Active());
            if (e_list.Count == 0) { return null; }
            e_list = e_list.OrderBy(a => (pos - a.pos).Length()).ToList<Enemy>();
            return e_list[0];
        }

        public Enemy GetLeftNearestEnemy(Vector2 pos)
        {
            List<Enemy> e_list = enemies.FindAll(a => (a.x <= pos.X - 48) && SidePosOk(a, pos, Utility.tan60) && a.Active());
            if (e_list.Count == 0) { return null; }
            e_list = e_list.OrderBy(a => (pos - a.pos).Length()).ToList<Enemy>();
            return e_list[0];
        }

        public Enemy GetRightNearestEnemy(Vector2 pos)
        {
            List<Enemy> e_list = enemies.FindAll(a => (a.x >= pos.X + 48) && SidePosOk(a, pos, Utility.tan60) && a.Active());
            if (e_list.Count == 0) { return null; }
            e_list = e_list.OrderBy(a => (pos - a.pos).Length()).ToList<Enemy>();
            return e_list[0];
        }

        public float GetTypeALaserLength(Vector2 pos, float width, out Enemy target)
        {
            float length = 0;
            RectangleHitbox hitbox = new RectangleHitbox(width, Graphics.Height - (Graphics.Height - pos.Y));
            Vector2 xpos = pos - new Vector2(0, hitbox.size.Y * 0.5f);
            List<Enemy> e_list = enemies.FindAll(a => a.Active() && a.hitbox.Intersect(a.pos, 0, hitbox, xpos, 0));
            e_list = e_list.OrderBy(a => (pos - a.pos).Length()).ToList<Enemy>();
            if (e_list.Count > 0)
            {
                Enemy enemy = e_list[0];
                target = enemy;
                for (int i = 0; i < enemy.hitbox.radius; i++)
                {
                    if (((CircleHitbox)enemy.hitbox).PointInCircle(enemy.pos, new Vector2(xpos.X + 0.5f * hitbox.size.X, enemy.pos.Y + enemy.hitbox.radius - i)))
                        return pos.Y - (enemy.pos.Y + enemy.hitbox.radius - i);
                    if (((CircleHitbox)enemy.hitbox).PointInCircle(enemy.pos, new Vector2(xpos.X, enemy.pos.Y + enemy.hitbox.radius - i)))
                        return pos.Y - (enemy.pos.Y + enemy.hitbox.radius - i);
                    if (((CircleHitbox)enemy.hitbox).PointInCircle(enemy.pos, new Vector2(xpos.X - 0.5f * hitbox.size.X, enemy.pos.Y + enemy.hitbox.radius - i)))
                        return pos.Y - (enemy.pos.Y + enemy.hitbox.radius - i);
                }
            }
            target = null;
            return length;
        }

        public int GetFighterCount()
        {
            return fighters.FindAll(f => f.Active()).Count;
        }

    }

    #endregion

}