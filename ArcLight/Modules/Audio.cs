using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace ArcLight
{
    public static class Audio
    {

        static Song title_bgm;
        static Song result_bgm;

        static Song stage_bgm;
        static Song boss_bgm;

        static bool fade_out = false;
        static bool half_fade_out = false;
        static bool half_fade_in = false;
        static int fade_time = 0;
        static int fade_duration = 0;
        static float current_volume;

        static float bgmVolume { get { return Data.settings.bgmVolume; } }
        static float seVolume { get { return Data.settings.seVolume; } }

        static Dictionary<SoundEffect, List<SoundEffectInstance>> se_list = new Dictionary<SoundEffect, List<SoundEffectInstance>>();
        static int max_se = 10;

        public static void Initialize()
        {
            InitBGM();
        }

        public static void Update()
        {
            UpdateBGM();
            UpdateSE();
        }

        public static void UpdateBGMVolume()
        {
            if (fade_duration <= 0)
            {
                MediaPlayer.Volume = bgmVolume;
            }
        }

        #region BGM

        static void InitBGM()
        {
            LoadGameBGM();
            MediaPlayer.Volume = bgmVolume;
            MediaPlayer.IsRepeating = true;
        }

        static void UpdateBGM()
        {
            if (half_fade_out)
            {
                if (fade_time > 0)
                {
                    MediaPlayer.Volume = current_volume * (0.25f + (fade_time / (float)fade_duration) * 0.75f);
                    fade_time -= 1;
                }
                else
                {
                    MediaPlayer.Volume = bgmVolume * 0.25f;
                    half_fade_out = false;
                }
            }
            else if (half_fade_in)
            {
                if (fade_time > 0)
                {
                    MediaPlayer.Volume = current_volume * (((fade_duration - fade_time) / (float)fade_duration) * 4.0f);
                    fade_time -= 1;
                }
                else
                {
                    MediaPlayer.Volume = bgmVolume;
                    half_fade_in = false;
                }
            }
            else if (fade_out)
            {
                if (fade_time > 0)
                {
                    MediaPlayer.Volume = current_volume * (fade_time / (float)fade_duration);
                    fade_time -= 1;
                }
                else
                {
                    MediaPlayer.Volume = 0;
                    fade_out = false;
                }
            }
        }

        static void LoadGameBGM()
        {
            title_bgm = Cache.BGM("bgm/Blue_Ever", 0);
            result_bgm = Cache.BGM("bgm/Cyberspace", 0);
        }

        public static void LoadStage1BGM()
        {
            stage_bgm = Cache.BGM("bgm/Electric_Highway", 1);
            boss_bgm = Cache.BGM("bgm/DEAD_HEAT_MAX", 1);
        }

        public static void PlayTitleBGM()
        {
            MediaPlayer.Volume = bgmVolume;
            MediaPlayer.Play(title_bgm);
        }

        public static void PlayResultBGM()
        {
            MediaPlayer.Volume = bgmVolume;
            MediaPlayer.Play(result_bgm);
        }

        public static void PlayStageBGM()
        {
            MediaPlayer.Volume = bgmVolume;
            MediaPlayer.Play(stage_bgm);
        }

        public static void PlayBossBGM()
        {
            MediaPlayer.Volume = bgmVolume;
            MediaPlayer.Play(boss_bgm);
        }

        public static void BGMFadeOut(int frames)
        {
            if (!fade_out)
            {
                current_volume = MediaPlayer.Volume;
                fade_time = fade_duration = frames;
                fade_out = true;
            }
        }

        public static void BGMHalfFadeOut(int frames)
        {
            if (!half_fade_out)
            {
                current_volume = MediaPlayer.Volume;
                fade_time = fade_duration = frames;
                half_fade_out = true;
            }
        }

        public static void BGMHalfFadeIn(int frames)
        {
            if (!half_fade_in)
            {
                current_volume = MediaPlayer.Volume;
                fade_time = fade_duration = frames;
                half_fade_in = true;
            }
        }

        #endregion

        #region SE

        public static void UpdateSE()
        {
            foreach (KeyValuePair<SoundEffect, List<SoundEffectInstance>> pair in se_list)
            {
                pair.Value.RemoveAll(se => se.State == SoundState.Stopped);
            }
        }

        public static void PlaySE(SoundEffect se, float vol)
        {
            if (!se_list.ContainsKey(se))
                se_list[se] = new List<SoundEffectInstance>();
            if (se_list[se].Count < max_se)
            {
                SoundEffectInstance instance = se.CreateInstance();
                instance.Volume = seVolume * vol;
                se_list[se].Add(instance);
                instance.Play();
            }
        }

        public static void PlayNextSE()
        {
            SoundEffect se = Cache.SE("se/next", 0);
            PlaySE(se, 1);
        }

        public static void PlayStartButtonSE()
        {
            SoundEffect se = Cache.SE("se/start_button", 0);
            PlaySE(se, 1);
        }

        public static void PlayCursorSE()
        {
            SoundEffect se = Cache.SE("se/cursor", 0);
            PlaySE(se, 1);
        }

        public static void PlayReadySE()
        {
            SoundEffect se = Cache.SE("se/ready", 0);
            PlaySE(se, 1);
        }

        public static void PlayTimeoutSE()
        {
            SoundEffect se = Cache.SE("se/timeout", 0);
            PlaySE(se, 1);
        }

        public static void PlayShutterCloseSE()
        {
            SoundEffect se = Cache.SE("se/shutter_close", 0);
            PlaySE(se, 1);
        }

        public static void PlayShutterOpenSE()
        {
            SoundEffect se = Cache.SE("se/shutter_open", 0);
            PlaySE(se, 1);
        }

        public static void PlayBeepSE()
        {
            SoundEffect se = Cache.SE("se/beep", 0);
            PlaySE(se, 1);
        }

        public static void PlayBossSirenSE()
        {
            SoundEffect se = Cache.SE("se/boss_siren", 0);
            PlaySE(se, 1);
        }

        public static void PlayBoss1EntranceSE()
        {
            SoundEffect se = Cache.SE("se/boss1_entrance", 0);
            PlaySE(se, 1);
        }

        public static void PlayBoss1CoreExplodeSE()
        {
            SoundEffect se = Cache.SE("se/boss1_core_explode", 0);
            PlaySE(se, 1);
        }

        public static void PlayBoss1ExplodeSE()
        {
            SoundEffect se = Cache.SE("se/boss1_explode", 0);
            PlaySE(se, 1);
        }

        public static void PlayFighterExplodeSE()
        {
            SoundEffect se = Cache.SE("se/fighter_explode", 0);
            PlaySE(se, 1);
        }

        public static void PlayEnemyExplodeSSE()
        {
            SoundEffect se = Cache.SE("se/enemy_explode_s", 0);
            PlaySE(se, 0.5f);
        }

        public static void PlayEnemyExplodeMSE()
        {
            SoundEffect se = Cache.SE("se/enemy_explode_m", 0);
            PlaySE(se, 1f);
        }

        public static void PlayTypeAShotSE()
        {
            SoundEffect se = Cache.SE("se/type_a_shot", 0);
            PlaySE(se, 0.6f);
        }

        public static void PlayTypeBShotSE()
        {
            SoundEffect se = Cache.SE("se/type_b_shot", 0);
            PlaySE(se, 0.6f);
        }

        public static void PlayBurstChargeSE()
        {
            SoundEffect se = Cache.SE("se/burst_charge", 0);
            PlaySE(se, 0.75f);
        }

        public static void PlayBulletAbsorbSE()
        {
            SoundEffect se = Cache.SE("se/bullet_absorb", 0);
            PlaySE(se, 0.4f);
        }

        public static void PlayBurstExplosionSmallSE()
        {
            SoundEffect se = Cache.SE("se/burst_explosion_small", 0);
            PlaySE(se, 0.3f);
        }

        public static void PlayBurstExplosionBigSE()
        {
            SoundEffect se = Cache.SE("se/burst_explosion_big", 0);
            PlaySE(se, 0.8f);
        }

        public static void PlayBurstBarrierSE()
        {
            SoundEffect se = Cache.SE("se/burst_barrier", 0);
            PlaySE(se, 0.6f);
        }

        public static void PlayBurstBarrierExpireSE()
        {
            SoundEffect se = Cache.SE("se/burst_barrier_expire", 0);
            PlaySE(se, 0.6f);
        }

        public static void PlayBurstFieldSE()
        {
            SoundEffect se = Cache.SE("se/burst_field", 0);
            PlaySE(se, 0.6f);
        }

        public static void PlayBurstFireSE()
        {
            SoundEffect se = Cache.SE("se/burst_fire", 0);
            PlaySE(se, 0.6f);
        }

        public static void PlayBulletHitSE()
        {
            SoundEffect se = Cache.SE("se/bullet_hit", 0);
            PlaySE(se, 0.2f);
        }

        public static void PlayBurstLaserSE()
        {
            SoundEffect se = Cache.SE("se/burst_laser", 0);
            PlaySE(se, 1f);
        }

        public static void PlayBurstSwordSE()
        {
            SoundEffect se = Cache.SE("se/burst_sword", 0);
            PlaySE(se, 1f);
        }

        public static SoundEffectInstance type_a_focus_instance = Cache.SE("se/type_a_focus").CreateInstance();
        public static void PlayTypeAFocusSE()
        {
            type_a_focus_instance.Volume = seVolume * 0.3f;
            if (type_a_focus_instance.State == SoundState.Stopped)
                type_a_focus_instance.Play();
        }

        public static SoundEffectInstance type_b_focus_instance = Cache.SE("se/type_b_focus").CreateInstance();
        public static void PlayTypeBFocusSE()
        {
            type_b_focus_instance.Volume = seVolume * 0.3f;
            if (type_b_focus_instance.State == SoundState.Stopped)
                type_b_focus_instance.Play();
        }

        public static SoundEffectInstance laser_hit_instance1 = Cache.SE("se/laser_hit").CreateInstance();
        public static SoundEffectInstance laser_hit_instance2 = Cache.SE("se/laser_hit").CreateInstance();
        public static void PlayLaserHit(int index)
        {
            if (index == 0)
            {
                laser_hit_instance1.Volume = seVolume * 0.8f;
                if (laser_hit_instance1.State == SoundState.Stopped)
                    laser_hit_instance1.Play();
            }
            else
            {
                laser_hit_instance2.Volume = seVolume * 0.8f;
                if (laser_hit_instance2.State == SoundState.Stopped)
                    laser_hit_instance2.Play();
            }
        }

        #endregion

    }
}
