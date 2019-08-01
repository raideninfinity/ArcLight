using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArcLight
{
    public class Fighter : Entity
    {
        public GamePlayer player;
        public bool movable { get { return Core.Session.movable; } }

        public int fire_delay = 0;
        public int burst_delay = 0;
        int prepare_frame;
        int barrier_time;
        bool focus_mode = false;
        int focus_wpn = 0;
        bool dying = false;
        int death_frames = 0;
        float speed_rate = 6.0f;
        int rotate_frame = 0;

        bool charge_move = false;
        int charge_time = 0;

        int a_hold_length = 0;
        int b_hold_length = 0;
        int a_spread_input = 0;
        int b_spread_input = 0;

        PlayerBullet focus_setup = null;
        PlayerBullet burst_setup = null;
        bool burst_active { get { return burst_setup != null && burst_setup.burst_active; } }
        public bool barrier { get { return barrier_time > 0; } }

        float speed { get { return speed_rate * (focus_mode ? 0.5f : 1.0f); } }

        public Fighter(GamePlayer player)
        {
            this.player = player;
            barrier_time = 150;
            prepare_frame = 30;
            y = Graphics.Height - 100;
            x = (player.index == 0) ? Graphics.Width / 4.0f : Graphics.Width / 4.0f * 3;
        }

        private bool Movable()
        {
            return Active() && movable;
        }

        public bool Active()
        {
            return (prepare_frame <= 0) && !dying && alive;
        }

        public override void Update()
        {
            if (prepare_frame > 0) prepare_frame -= 1;
            if (barrier_time > 0) barrier_time -= 1;
            if (death_frames > 0) death_frames -= 1;
            if (fire_delay > 0) fire_delay -= 1;
            if (burst_delay > 0) burst_delay -= 1;
            if (charge_time > 0) charge_time -= 1;
            if (a_spread_input > 0) a_spread_input -= 1;
            if (b_spread_input > 0) b_spread_input -= 1;

            rotate_frame += 1;
            if (rotate_frame > 60) { rotate_frame = 0; }

            if (focus_setup != null && !focus_setup.alive) { focus_setup = null; }
            if (burst_setup != null && !burst_setup.alive) { burst_setup = null; }

            if (fire_delay <= 0 || burst_active)
            {
                focus_mode = false;
            }

            if (dying && death_frames <= 0)
            {
                alive = false;
                return;
            }
            if (Movable())
            {
                UpdateMovement();
                UpdateBulletFire();
                UpdateBurstFire();
                UpdateBurstCharge();
                UpdateDebug();
            }
            else
            {
                a_hold_length = 0;
                b_hold_length = 0;
                a_spread_input = 0;
                b_spread_input = 0;
            }
            if (!focus_mode)
            {
                focus_wpn = 0;
                if (focus_setup != null)
                {
                    focus_setup.Erase();
                    focus_setup = null;
                }
            }
            UpdateGrazedBullets();
            FixBounds();
        }

        public bool Graze(EnemyBullet b)
        {
            if (grazedBullets.Contains(b)) { return false; }
            CircleHitbox grazeHitbox = new CircleHitbox(34);
            return b.Intersect(grazeHitbox, pos, 0);
        }

        List<EnemyBullet> grazedBullets = new List<EnemyBullet>();
        public void AddGrazedBullet(EnemyBullet b)
        {
            grazedBullets.Add(b);
            Core.Session.GainEnergy(player, b.energy * Status.GrazeEnergyMult);
            Core.Session.GainScore(player, b.energy * Status.GrazeScoreMult);
        }

        void UpdateGrazedBullets()
        {
            grazedBullets.RemoveAll(a => !a.alive);
        }

        public void UpdateMovement()
        {
            float v = (float)(1 / Math.Sqrt(2));
            int dir = Input.KeyboardDir8(player.index == 0 ? Data.settings.p1KeyboardDPad : Data.settings.p2KeyboardDPad);
            if (Input.GamepadOnline(player.index))
            {
                int g_dir = Input.GamepadDir8(player.index);
                if (g_dir != 0) { dir = g_dir; }
            }
            switch (dir)
            {
                case 8: y -= speed; break;
                case 4: x -= speed; break;
                case 6: x += speed; break;
                case 2: y += speed; break;
                case 7: x -= speed * v; y -= speed * v; break;
                case 9: x += speed * v; y -= speed * v; break;
                case 1: x -= speed * v; y += speed * v; break;
                case 3: x += speed * v; y += speed * v; break;
            }
            if (Input.GamepadOnline(player.index))
            {
                //Analog
                Vector2 axes = Input.LStickAxes(player.index);
                if (axes.Length() == 0)
                {
                    Vector2 r_axes = Input.RStickAxes(player.index);
                    if (r_axes.Length() != 0)
                    {
                        Vector2 normalized = Input.RStickNormalized(player.index);
                        x += r_axes.X * speed * Math.Abs(normalized.X);
                        y -= r_axes.Y * speed * Math.Abs(normalized.Y);
                    }
                }
                else
                {
                    Vector2 normalized = Input.LStickNormalized(player.index);
                    x += axes.X * speed * Math.Abs(normalized.X);
                    y -= axes.Y * speed * Math.Abs(normalized.Y);
                }
            }
        }

        public void UpdateBulletFire()
        {
            if (Data.ControlMode(player.index) == 2)
            {
                UpdateBulletFireCM3();
                return;
            }
            if (fire_delay == 0 && !burst_active)
            {
                if (Input.Press(2, player.index))
                {
                    player.b_wpn = 1;
                    if (Input.Press(3, player.index))
                    {
                        if (player.type == 0)
                            FireAF2();
                        else
                            FireBF2();
                    }
                    else
                    {
                        if (player.type == 0)
                            FireAS2();
                        else
                            FireBS2();
                    }
                }
                else if (Input.Press(1, player.index))
                {
                    player.b_wpn = 0;
                    if (Input.Press(3, player.index))
                    {
                        if (player.type == 0)
                            FireAF1();
                        else
                            FireBF1();
                    }
                    else
                    {
                        if (player.type == 0)
                            FireAS1();
                        else
                            FireBS1();
                    }
                }
                else if (Input.Press(3, player.index))
                {
                    if (player.type == 0)
                        FocusBarrierA();
                    else
                        FocusBarrierB();
                }
            }
        }

        public void UpdateBulletFireCM3()
        {
            int focus_hold = 12;

            if (Input.Press(2, player.index))
                b_hold_length += 1;
            else if (Input.Press(1, player.index))
                a_hold_length += 1;
            if (Input.Release(2, player.index))
                b_hold_length = 0;
            else if (Input.Release(1, player.index))
                a_hold_length = 0;

            if (Input.Trigger(2, player.index))
                b_spread_input += 10;
            else if (Input.Trigger(1, player.index))
                a_spread_input += 10;

            a_spread_input = a_spread_input.Clamp(0, 30);
            b_spread_input = b_spread_input.Clamp(0, 30);

            if (fire_delay == 0 && !burst_active)
            {
                if (b_hold_length >= focus_hold)
                {
                    player.b_wpn = 1;
                    if (player.type == 0)
                        FireAF2();
                    else
                        FireBF2();
                }
                else if (a_hold_length >= focus_hold)
                {
                    player.b_wpn = 0;
                    if (player.type == 0)
                        FireAF1();
                    else
                        FireBF1();
                }
                else if (b_spread_input > 0)
                {
                    player.b_wpn = 1;
                    if (player.type == 0)
                        FireAS2();
                    else
                        FireBS2();
                }
                else if (a_spread_input > 0)
                {
                    player.b_wpn = 0;
                    if (player.type == 0)
                        FireAS1();
                    else
                        FireBS1();
                }
            }
        }

        #region Weapons Fire

        public int BurstChain(int mode)
        {
            if (player.mode == mode)
            {
                return player.chain.Clamp(0, 3);
            }
            else
            {
                return (player.chain - 3).Clamp(0, 3);
            }
        }

        //SPREAD

        bool as1_alt = false;
        void FireAS1()
        {
            Audio.PlayTypeAShotSE();
            fire_delay = 6;
            float a = 3.0f;
            if (BurstChain(0) == 0)
            {
                if (as1_alt)
                {
                    AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), a));
                    AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -a));

                }
                else
                {
                    AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), 0));
                    AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), 0));
                }
            }
            else if (BurstChain(0) == 1)
            {
                AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), a));
                AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -a));

                AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), 0));
                AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), 0));
            }
            else if (BurstChain(0) == 2)
            {
                if (as1_alt)
                {
                    AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), a * 2));
                    AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -a * 2));

                    AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), 0));
                    AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), 0));

                }
                else
                {
                    AddBullet(new PBulletA1(this, pos + new Vector2(28f, -21f), a));
                    AddBullet(new PBulletA1(this, pos - new Vector2(28f, 21f), -a));
                }
            }
            else if (BurstChain(0) == 3)
            {
                AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), a * 2));
                AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -a * 2));

                AddBullet(new PBulletA1(this, pos + new Vector2(28f, -21f), a));
                AddBullet(new PBulletA1(this, pos - new Vector2(28f, 21f), -a));

                AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), 0));
                AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), 0));
            }
            AddBullet(new PBulletA1(this, pos + new Vector2(10f, 0), 0));
            AddBullet(new PBulletA1(this, pos - new Vector2(10f, 0), 0));
            as1_alt ^= true;
        }

        bool as2_alt = false;
        void FireAS2()
        {
            Audio.PlayTypeAShotSE();
            fire_delay = 6;
            float main_angle1 = 168.75f; // RIGHT
            float main_angle2 = 168.75f; // LEFT
            float offset = 5.625f;

            Enemy e1 = Core.Controller.GetEnemyForAS2_Right(pos);
            if (e1 != null)
            {
                Vector2 bit_pos = new Vector2(pos.X + 28, pos.Y - 21);
                float angle = (180 - bit_pos.AngleBetweenPoints(e1.pos));
                //angle = angle.Clamp(90, 180 + 4 * offset);
                main_angle1 = angle;
            }

            Enemy e2 = Core.Controller.GetEnemyForAS2_Left(pos);
            if (e2 != null)
            {
                Vector2 bit_pos = new Vector2(pos.X - 28, pos.Y - 21);
                float angle = (180 - bit_pos.AngleBetweenPoints(e2.pos));
                angle = 360 - angle;
                //angle = angle.Clamp(90, 180 + 4 * offset);
                main_angle2 = angle;
            }

            if (BurstChain(0) == 0)
            {
                if (as2_alt)
                {
                    AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle1 - offset)));
                    AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 - offset)));
                    AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle1 + offset)));
                    AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 + offset)));
                }
                else
                {
                    AddBullet(new PBulletA1(this, pos + new Vector2(28f, -21f), main_angle1));
                    AddBullet(new PBulletA1(this, pos - new Vector2(28f, 21f), -main_angle2));
                }
            }
            else if (BurstChain(0) == 1)
            {
                AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle1 - offset)));
                AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 - offset)));
                AddBullet(new PBulletA1(this, pos + new Vector2(28f, -21f), main_angle1));
                AddBullet(new PBulletA1(this, pos - new Vector2(28f, 21f), -main_angle2));
                AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle2 + offset)));
                AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 + offset)));
            }
            else if (BurstChain(0) == 2)
            {
                if (as2_alt)
                {
                    AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle1 - offset)));
                    AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 - offset)));
                    AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle2 + offset)));
                    AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 + offset)));
                }
                else
                {
                    AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle1 - offset * 2)));
                    AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 - offset * 2)));
                    AddBullet(new PBulletA1(this, pos + new Vector2(28f, -21f), main_angle1));
                    AddBullet(new PBulletA1(this, pos - new Vector2(28f, 21f), -main_angle2));
                    AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle2 + offset * 2)));
                    AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 + offset * 2)));
                }

            }
            else if (BurstChain(0) == 3)
            {
                AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle1 - offset * 2)));
                AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 - offset * 2)));
                AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle1 - offset)));
                AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 - offset)));
                AddBullet(new PBulletA1(this, pos + new Vector2(28f, -21f), main_angle1));
                AddBullet(new PBulletA1(this, pos - new Vector2(28f, 21f), -main_angle2));
                AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle1 + offset)));
                AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 + offset)));
                AddBullet(new PBulletA2(this, pos + new Vector2(28f, -21f), (main_angle1 + offset * 2)));
                AddBullet(new PBulletA2(this, pos - new Vector2(28f, 21f), -(main_angle2 + offset * 2)));
            }
            AddBullet(new PBulletA1(this, pos, 0));
            as2_alt ^= true;
        }

        void FireBS1()
        {
            Audio.PlayTypeBShotSE();
            fire_delay = 8;
            if (BurstChain(0) == 1) fire_delay = 8;
            else if (BurstChain(0) == 2) fire_delay = 6;
            else if (BurstChain(0) == 3) fire_delay = 6;

            List<Enemy> enemies = (((GameScene)Core.Scene)).controller.GetEnemiesForBS1(pos);
            if (enemies.Count == 0)
            {
                AddBullet(new PBulletB1(this, pos, 0));
                AddBullet(new PBulletB2(this, pos + new Vector2(12f, 0), 8, null));
                AddBullet(new PBulletB2(this, pos - new Vector2(12f, 0), -8, null));
            }
            else
            {
                int count = 0;
                int max = 3;
                foreach (Enemy enemy in enemies)
                {
                    if (count >= max) break;
                    float a = 180 - pos.AngleBetweenPoints(enemy.pos);
                    AddBullet(new PBulletB1(this, pos, a));
                    AddBullet(new PBulletB2(this, pos + new Vector2(12f, 0), a + 15, enemy));
                    AddBullet(new PBulletB2(this, pos - new Vector2(12f, 0), a - 15, enemy));
                    if (BurstChain(0) >= 3)
                    {
                        AddBullet(new PBulletB2(this, pos + new Vector2(16f, 0), a + 30, enemy));
                        AddBullet(new PBulletB2(this, pos - new Vector2(16f, 0), a - 30, enemy));
                    }
                    count += 1;
                }
            }
        }

        void FireBS2()
        {
            Audio.PlayTypeBShotSE();
            fire_delay = 10;
            if (BurstChain(0) == 3) fire_delay = 8;

            Enemy enemy = (((GameScene)Core.Scene)).controller.GetFrontNearestEnemy(pos);
            Enemy enemy2 = (((GameScene)Core.Scene)).controller.GetBackNearestEnemy(pos);

            if (enemy == null) { enemy = enemy2; }
            if (enemy2 == null) { enemy2 = enemy; }

            Vector2 r = new Vector2(21f, 17f);
            Vector2 l = new Vector2(21f, -17f);

            if (BurstChain(0) == 0)
            {
                AddBullet(new PBulletB3(this, pos, 0, enemy));

                AddBullet(new PBulletB3(this, pos + r, 180 - 24, enemy2));
                AddBullet(new PBulletB3(this, pos + r, 180 - 48, enemy2));

                AddBullet(new PBulletB3(this, pos - l, 180 + 24, enemy2));
                AddBullet(new PBulletB3(this, pos - l, 180 + 48, enemy2));
            }
            else if (BurstChain(0) == 1)
            {
                AddBullet(new PBulletB3(this, pos, 0, enemy));

                AddBullet(new PBulletB3(this, pos + r, 180 - 16, enemy2));
                AddBullet(new PBulletB3(this, pos + r, 180 - 32, enemy2));
                AddBullet(new PBulletB3(this, pos + r, 180 - 48, enemy2));

                AddBullet(new PBulletB3(this, pos - l, 180 + 16, enemy2));
                AddBullet(new PBulletB3(this, pos - l, 180 + 32, enemy2));
                AddBullet(new PBulletB3(this, pos - l, 180 + 48, enemy2));
            }
            else if (BurstChain(0) >= 2)
            {
                AddBullet(new PBulletB3(this, pos, 0, enemy));
                AddBullet(new PBulletB3(this, pos + new Vector2(4f, 0), 16, enemy));
                AddBullet(new PBulletB3(this, pos - new Vector2(4f, 0), -16, enemy));

                AddBullet(new PBulletB3(this, pos + r, 180 - 16, enemy2));
                AddBullet(new PBulletB3(this, pos + r, 180 - 32, enemy2));
                AddBullet(new PBulletB3(this, pos + r, 180 - 48, enemy2));

                AddBullet(new PBulletB3(this, pos - l, 180 + 16, enemy2));
                AddBullet(new PBulletB3(this, pos - l, 180 + 32, enemy2));
                AddBullet(new PBulletB3(this, pos - l, 180 + 48, enemy2));
            }
        }

        //FOCUS

        void FireAF1()
        {
            Audio.PlayTypeAFocusSE();
            focus_mode = true;
            fire_delay = 10;
            if (focus_setup != null)
            {
                if (focus_wpn != 1)
                {
                    focus_setup.Erase();
                    focus_setup = new PLaserA1(this, Vector2.Zero, 0);
                    AddBullet(focus_setup);
                }
            }
            else
            {
                focus_setup = new PLaserA1(this, Vector2.Zero, 0);
                AddBullet(focus_setup);
            }
            focus_wpn = 1;
        }

        void FireAF2()
        {
            Audio.PlayTypeAFocusSE();
            focus_mode = true;
            fire_delay = 10;
            if (focus_setup != null)
            {
                if (focus_wpn != 2)
                {
                    focus_setup.Erase();
                    focus_setup = new PLaserA2(this, Vector2.Zero, 0);
                    AddBullet(focus_setup);
                }
            }
            else
            {
                focus_setup = new PLaserA2(this, Vector2.Zero, 0);
                AddBullet(focus_setup);
            }
            focus_wpn = 2;
        }

        void FireBF1()
        {
            Audio.PlayTypeBFocusSE();
            focus_mode = true;
            fire_delay = 10;
            if (focus_setup != null)
            {
                if (focus_wpn != 1)
                {
                    focus_setup.Erase();
                    focus_setup = new PLaserB1(this, Vector2.Zero, 0);
                    AddBullet(focus_setup);
                }
            }
            else
            {
                focus_setup = new PLaserB1(this, Vector2.Zero, 0);
                AddBullet(focus_setup);
            }
            focus_wpn = 1;
        }

        void FireBF2()
        {
            Audio.PlayTypeBFocusSE();
            focus_mode = true;
            fire_delay = 10;
            if (focus_setup != null)
            {
                if (focus_wpn != 2)
                {
                    focus_setup.Erase();
                    focus_setup = new PLSwordB1(this, Vector2.Zero, 0);
                    AddBullet(focus_setup);
                }
            }
            else
            {
                focus_setup = new PLSwordB1(this, Vector2.Zero, 0);
                AddBullet(focus_setup);
            }
            focus_wpn = 2;
        }


        void FocusBarrierA()
        {
            focus_mode = true;
            fire_delay = 10;
            if (focus_setup != null)
            {
                focus_setup.Erase();
            }
            focus_wpn = 3;
        }

        void FocusBarrierB()
        {
            focus_mode = true;
            fire_delay = 10;
            if (focus_setup != null)
            {
                focus_setup.Erase();
            }
            focus_wpn = 3;
        }

        #endregion

        public void UpdateBurstFire()
        {
            if (burst_delay > 0) return;
            if (player.energy < 10000) return;
            //Control Mode 1
            if (Data.ControlMode(player.index) != 1)
            {
                if (Input.Press(4, player.index))
                {
                    ConsumeBurstEnergy();
                    if (player.mode == 0)
                    {
                        if (player.type == 0)
                        {
                            FireBurstAS1();
                        }
                        else if (player.type == 1)
                        {
                            FireBurstBS1();
                        }
                    }
                    else
                    {
                        if (player.type == 0)
                        {
                            FireBurstAF1();
                        }
                        else if (player.type == 1)
                        {
                            FireBurstBF1();
                        }
                    }
                    return;
                }
                else if (Input.Press(5, player.index))
                {
                    ConsumeBurstEnergy();
                    if (player.mode == 0)
                    {
                        if (player.type == 0)
                        {
                            FireBurstAS2();
                        }
                        else if (player.type == 1)
                        {
                            FireBurstBS2();
                        }
                    }
                    else
                    {
                        if (player.type == 0)
                        {
                            FireBurstAF2();
                        }
                        else if (player.type == 1)
                        {
                            FireBurstBF2();
                        }
                    }
                    return;
                }
            }
            if ((Data.ControlMode(player.index) == 1 && (Input.Press(4, player.index) || Input.Press(5, player.index))) || (Data.ControlMode(player.index) == 2) && Input.Press(3, player.index))
            {
                ConsumeBurstEnergy();
                if (player.mode == 0)
                {
                    if (player.b_wpn == 0)
                    {
                        if (player.type == 0)
                        {
                            FireBurstAS1();
                        }
                        else if (player.type == 1)
                        {
                            FireBurstBS1();
                        }
                    }
                    else
                    {
                        if (player.type == 0)
                        {
                            FireBurstAS2();
                        }
                        else if (player.type == 1)
                        {
                            FireBurstBS2();
                        }
                    }
                }
                else
                {
                    if (player.b_wpn == 0)
                    {
                        if (player.type == 0)
                        {
                            FireBurstAF1();
                        }
                        else if (player.type == 1)
                        {
                            FireBurstBF1();
                        }
                    }
                    else
                    {
                        if (player.type == 0)
                        {
                            FireBurstAF2();
                        }
                        else if (player.type == 1)
                        {
                            FireBurstBF2();
                        }
                    }
                }
            }
        }

        #region Burst Fire
        public void FireBurstAS1()
        {
            Audio.PlayBurstChargeSE();
            charge_move = true;
            charge_time = 30;
            AddBullet(new PBurstBulletA1(this, pos, 0));
            burst_delay = 60;
        }

        public void FireBurstAS2()
        {
            Audio.PlayBurstFireSE();
            barrier_time += 60;
            AddBullet(new PBurstBulletA2(this, pos, -120));
            AddBullet(new PBurstBulletA2(this, pos, 120));
            AddBullet(new PBurstBulletA2(this, pos, 0, false));
            burst_delay = 120;
            AddBurstChain();
        }

        public void FireBurstAF1()
        {
            Audio.PlayBurstChargeSE();
            charge_move = true;
            charge_time = 30;
            burst_delay = Status.PBurstLaserA1Time + 30;
        }

        public void FireBurstAF2()
        {
            barrier_time += 60;
            AddBullet(new PBurstFieldA1(this, pos, 0));
            burst_delay = Status.PBurstFieldA1Time / 3 * 2;
            AddBurstChain();
        }

        public void FireBurstBS1()
        {
            Audio.PlayBurstFireSE();
            barrier_time += 60;
            List<Enemy> enemies = (((GameScene)Core.Scene)).controller.GetEnemiesForBS1(pos);
            if (enemies.Count == 0)
            {
                AddBullet(new PBurstBulletB1(this, pos, 0));
            }
            else
            {
                int count = 0;
                int max = 3;
                foreach (Enemy enemy in enemies)
                {
                    if (count >= max) break;
                    float a = 180 - pos.AngleBetweenPoints(enemy.pos);
                    AddBullet(new PBurstBulletB1(this, pos, a));
                    count += 1;
                }
            }
            burst_delay = 90;
            AddBurstChain();
        }

        public void FireBurstBS2()
        {
            Audio.PlayBurstChargeSE();
            charge_move = true;
            charge_time = 30;
            burst_delay = Status.PBurstBarrierB1_Time + 30;
        }

        public void FireBurstBF1()
        {
            burst_setup = new PBurstBarrierB2(this, Vector2.Zero, 0);
            AddBullet(burst_setup);
            burst_delay = Status.PBurstBarrierB2_Time;
            AddBurstChain();
        }

        public void FireBurstBF2()
        {
            Audio.PlayBurstChargeSE();
            charge_move = true;
            charge_time = 30;
            burst_delay = Status.PBurstSwordB1Time + 30;
        }

        public void UpdateBurstCharge()
        {
            if (charge_move && charge_time == 0)
            {
                if (player.type == 0)
                {
                    if (player.mode == 0)
                        InvokeBurstAS1();
                    else
                        InvokeBurstAF1();
                }
                else if (player.type == 1)
                {
                    if (player.mode == 0)
                        InvokeBurstBS2();
                    else
                        InvokeBurstBF2();
                }
                charge_move = false;
            }
        }

        public void InvokeBurstAS1()
        {
            AddBurstChain();
            barrier_time += 30;
        }

        public void InvokeBurstAF1()
        {
            AddBurstChain();
            burst_setup = new PBurstLaserA1(this, Vector2.Zero, 0);
            AddBullet(burst_setup);
        }

        public void InvokeBurstBS2()
        {
            AddBurstChain();
            burst_setup = new PBurstBarrierB1(this, Vector2.Zero, 0);
            AddBullet(burst_setup);
        }

        public void InvokeBurstBF2()
        {
            AddBurstChain();
            burst_setup = new PBurstSwordB1(this, Vector2.Zero, 0);
            AddBullet(burst_setup);
        }

        #endregion

        public void AddBullet(PlayerBullet bullet)
        {
            Core.Controller.AddPlayerBullet(bullet);
        }

        public void FixBounds()
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x > 480) x = 480;
            if (y > 640) y = 640;
        }

        public void UpdateDebug()
        {
            if ((player.index == 0 && Input.KeyTrigger(Keys.D1)) || (player.index == 1 && Input.KeyTrigger(Keys.D2)))
            {
                Kill();
            }
            else if ((player.index == 0 && Input.KeyTrigger(Keys.D9)) || (player.index == 1 && Input.KeyTrigger(Keys.D0)))
            {
                AddBurstChain();
            }
            else if ((player.index == 0 && Input.KeyTrigger(Keys.D3)) || (player.index == 1 && Input.KeyTrigger(Keys.D4)))
            {
                player.energy += 10000;
                barrier_time += 300;
            }
        }

        public void ConsumeBurstEnergy()
        {
            player.energy -= 10000;
        }

        public void AddBurstChain()
        {
            Core.Session.AddBurstChain(player);
        }

        protected override Hitbox GetHitbox()
        {
            return new CircleHitbox(2);
        }

        public override bool Intersect(Entity e)
        {
            return hitbox.Intersect(pos, angle, e.hitbox, e.pos, e.angle);
        }

        public override bool Intersect(Hitbox h, Vector2 h_pos, float h_angle)
        {
            return hitbox.Intersect(pos, angle, h, h_pos, h_angle);
        }

        int GetPolarity()
        {
            if (focus_mode) return 1;
            else return 0;
        }

        public bool IntersectAbsorb(EnemyBullet e)
        {
            if (e.polarity != GetPolarity())
                return false;
            CircleHitbox barrier_hitbox = new CircleHitbox(34);
            return barrier_hitbox.Intersect(pos, angle, e.hitbox, e.pos, e.angle);
        }

        public override void Erase()
        {
            alive = false;
        }

        public override void Kill()
        {
            if (burst_setup != null && burst_setup.priority == 91)
            {
                burst_setup.Erase();
                burst_setup = null;
                burst_delay = 0;
                AddBullet(new PBurstEffectB2(this, pos, 0));
                return;
            }
            dying = true;
            Audio.PlayFighterExplodeSE();
            //remove burst
            Core.Session.ResetBurstChain(player);
            focus_mode = false;
            if (focus_setup != null) focus_setup.Erase();
            focus_wpn = 0;
            death_frames = 60;
            player.miss += 1;
            Core.Controller.player_bullets.RemoveAll(b => b.owner == this);
        }

        public override void Draw()
        {
            if (this == null) return;
            Texture2D texture = GetFighterTexture();
            Texture2D barrier = Cache.Texture("player/invincible_barrier", 1);
            Texture2D s_barrier = Cache.Texture("player/spread_barrier", 1);
            Texture2D f_barrier = Cache.Texture("player/focus_barrier", 1);
            Vector2 pos = new Vector2(x, y + 200 * (prepare_frame / 30.0f));
            Graphics.spriteBatch.Begin();
            float alpha = Math.Abs((float)Math.Sin(((float)Graphics.totalMs * 0.2f).ToRad()));
            alpha = 0.6f + alpha * 0.4f;
            if (!dying)
            {
                if (barrier_time > 0 || burst_active)
                    Graphics.spriteBatch.Draw(barrier, pos, barrier.GetRect(), Color.White * alpha, 0, barrier.GetCenter(), 1.0f, SpriteEffects.None, 0);
                else if (focus_mode)
                    Graphics.spriteBatch.Draw(f_barrier, pos, f_barrier.GetRect(), Color.White * alpha, 0, f_barrier.GetCenter(), 1.0f, SpriteEffects.None, 0);
                else
                    Graphics.spriteBatch.Draw(s_barrier, pos, s_barrier.GetRect(), Color.White * alpha, 0, s_barrier.GetCenter(), 1.0f, SpriteEffects.None, 0);
                if (charge_time > 0)
                    DrawBurstCharge();
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, 0, texture.GetCenter(), 1, SpriteEffects.None, 0);
                if (focus_mode)
                {
                    Texture2D ring = player.type == 0 ? Cache.Texture("player/type_a_focus_aura", 1) : Cache.Texture("player/type_b_focus_aura", 1);
                    float angle = rotate_frame * 3.0f;
                    Graphics.spriteBatch.Draw(ring, pos, ring.GetRect(), Color.White, angle.ToRad(), ring.GetCenter(), 1.0f, SpriteEffects.None, 0);
                }
                if (player.type == 0)
                    DrawTypeABit();
                else
                    DrawTypeBBit();
            }
            else
                DrawDeathRing();
            DrawDebug();
            Graphics.spriteBatch.End();
        }

        public void DrawTypeABit()
        {
            Vector2 pos = new Vector2(x, y + 200 * (prepare_frame / 30.0f));
            Texture2D texture = (player.color == 0) ? Cache.Texture("player/type_a_0_bit", 1) : Cache.Texture("player/type_a_1_bit", 1);
            if (focus_mode && focus_wpn == 1)
            {
                pos += new Vector2(0, -36);
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, 0, texture.GetCenter(), 1, SpriteEffects.None, 0);
            }
            else if (focus_mode && focus_wpn == 2 && focus_setup != null && focus_setup.alive)
            {
                PLaserA2 laser = (PLaserA2)focus_setup;
                pos = laser.left_sub.pos;
                pos.Y -= 8;
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, 180f.ToRad(), texture.GetCenter(), 1, SpriteEffects.None, 0);
                pos = laser.right_sub.pos;
                pos.Y -= 8;
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, 180f.ToRad(), texture.GetCenter(), 1, SpriteEffects.None, 0);
                }
            else
            {
                pos += new Vector2(28, -21);
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, 0, texture.GetCenter(), 1, SpriteEffects.None, 0);
                pos += new Vector2(-(28 * 2), 0);
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, 0, texture.GetCenter(), 1, SpriteEffects.None, 0);
            }
        }

        public void DrawTypeBBit()
        {
            Vector2 pos = new Vector2(x, y + 200 * (prepare_frame / 30.0f));
            Texture2D texture = (player.color == 0) ? Cache.Texture("player/type_b_0_bit", 1) : Cache.Texture("player/type_b_1_bit", 1);
            if (focus_mode && focus_wpn == 1 && focus_setup != null && focus_setup.alive)
            {
                PLaserB1 laser = (PLaserB1)focus_setup;
                Vector2 displace = (new Vector2(0, -34)).TurnAngle(laser.laser_angle);
                Graphics.spriteBatch.Draw(texture, pos + displace, texture.GetRect(), Color.White, laser.laser_angle.ToRad(), texture.GetCenter(), 1, SpriteEffects.None, 0);
            }
            else if (focus_mode && focus_wpn == 2)
            {
                Vector2 displace = new Vector2(0, -34);
                Graphics.spriteBatch.Draw(texture, pos + displace, texture.GetRect(), Color.White, 0, texture.GetCenter(), 1, SpriteEffects.None, 0);
                Vector2 rotated = displace.TurnAngle(120);
                Graphics.spriteBatch.Draw(texture, pos + rotated, texture.GetRect(), Color.White, 120f.ToRad(), texture.GetCenter(), 1, SpriteEffects.None, 0);
                rotated = displace.TurnAngle(-120);
                Graphics.spriteBatch.Draw(texture, pos + rotated, texture.GetRect(), Color.White, -120f.ToRad(), texture.GetCenter(), 1, SpriteEffects.None, 0);
            }
            else
            {
                pos += new Vector2(21, 17);
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, 135f.ToRad(), texture.GetCenter(), 1, SpriteEffects.None, 0);
                pos += new Vector2(-(21 * 2), 0);
                Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White, -135f.ToRad(), texture.GetCenter(), 1, SpriteEffects.None, 0);
            }
        }

        public void DrawDebug()
        {

        }

        public void DrawDeathRing()
        {
            Texture2D d_ring = Cache.Texture("player/death_ring", 1);
            float expl_time = 15f;
            int frame = 30 - (death_frames - 30);
            if (death_frames >= 30)
            {
                float scale = 6.0f * (frame / expl_time);
                float scale2 = 4.0f * (frame / expl_time);
                float alpha = 1f - 0.5f * (frame / expl_time);
                Graphics.spriteBatch.Draw(d_ring, pos, d_ring.GetRect(), Color.White * alpha, 0, d_ring.GetCenter(), scale, SpriteEffects.None, 0);
                Graphics.spriteBatch.Draw(d_ring, pos, d_ring.GetRect(), Color.White * alpha, 0, d_ring.GetCenter(), scale2, SpriteEffects.None, 0);
            }
        }

        public void DrawBurstCharge()
        {
            Texture2D texture = null;
            if (player.type == 0) texture = Cache.Texture("player_bullet/type_a_burst/charge", 1);
            else if (player.type == 1) texture = Cache.Texture("player_bullet/type_b_burst/charge", 1);
            float scale = 0.2f + 0.8f * (charge_time / 30f);
            float alpha = 1.0f - (charge_time / 30f) * 0.5f;
            Graphics.spriteBatch.Draw(texture, pos, texture.GetRect(), Color.White * alpha, 0, texture.GetCenter(), scale, SpriteEffects.None, 0);
        }

        private Texture2D GetFighterTexture()
        {
            if (player.type == 0)
            {
                return (player.color == 0) ? Cache.Texture("player/type_a_0", 1) : Cache.Texture("player/type_a_1", 1);
            }
            else
            {
                return (player.color == 0) ? Cache.Texture("player/type_b_0", 1) : Cache.Texture("player/type_b_1", 1);
            }
        }

    }
}
