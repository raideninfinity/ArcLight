using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcLight
{

    public static partial class Status
    {
        //Chain

        public static float PlayerChainTime = 20.0f; //seconds

        //Weapon Damage

        public static float[] PBulletA1_Damage = new float[] { 20f, 24f, 28.8f, 34.56f};
        public static float[] PBulletA2_Damage = new float[] { 10f, 12f, 14.4f, 17.28f};
        public static float[] PBulletA3_Damage = new float[] { 16f, 19.2f, 23.04f, 27.648f};
        public static float[] PBulletA4_Damage = new float[] { 12f, 14.4f, 15.36f, 18.432f};
        public static float[] PBulletB1_Damage = new float[] { 20f, 24f, 28.8f, 34.56f};
        public static float[] PBulletB2_Damage = new float[] { 9f, 10.8f, 12.96f, 15.552f};
        public static float[] PBulletB3_Damage = new float[] { 15f, 18f, 21.6f, 25.92f};

        public static float[] PLaserA1_Damage = new float[] { 220f, 264f, 316.8f, 380.16f}; //per second
        public static float[] PLaserA1_AddDamage = new float[] { 75f, 90f, 108f, 129.6f}; // cd 1 second
        public static float[] PLaserA2_Damage = new float[] { 127.5f, 153f, 183.6f, 220.32f}; //per second
        public static float[] PLaserA2_AddDamage = new float[] { 45f, 54f, 64.8f, 77.76f}; // cd 1 second
        public static float[] PLaserB1_Damage = new float[] { 280f, 336f, 403.2f, 483.84f}; //per second
        public static float[] PLaserB1_AddDamage = new float[] { 60f, 72f, 86.4f, 103.68f}; // cd 1 second
        public static float[] PLSwordB1_Damage = new float[] { 490f, 588f, 705.6f, 846.72f}; //per second
        public static float[] PLSwordB1_AddDamage = new float[] { 195f, 234f, 280.8f, 336.96f}; // cd 1 second

        public static float PLaserB1_MultGainRate = 0.5f; // per second
        public static float PLaserB1_MultDecRate = 1.5f; // per second
        public static float PLaserB1_MultMax = 1.5f; // + 1.0f

        public static float PBarrierA1_Damage = 200;
        public static float PBarrierB1_Damage = 200;

        //Burst Damage

        public static float PBurstEffectA1_Damage = 500; //type-a spread 1 blast
        public static float PBurstEffectA2_Damage = 200; //type-a spread 2 blast
        public static float PBurstEffectB1_Damage = 250; //type-b spread 1 blast
        public static float PBurstBulletB2_Damage = 50; //type-b spread 2 counter shot
        public static float PBurstLaserA1_Damage = 300; //per second, type-a focus 1 laser
        public static float PBurstLaserA1_AddDamage = 150; //one time, type-a focus 1 laser
        public static float PBurstFieldA1_Damage = 250; //per second, type-a focus 2
        public static float PBurstEffectB2_Damage = 425; //type-b focus 1 counter blast
        public static float PBurstSwordB1_Damage = 375; //per second, type-b focus 2 sword
        public static float PBurstSwordB1_AddDamage = 250; //one time, type-b focus 2 sword

        //Burst Time
        public static int PBurstBarrierB1_Time = 180;
        public static int PBurstBarrierB2_Time = 180;
        public static int PBurstFieldA1Time = 240;
        public static int PBurstLaserA1Time = 120;
        public static int PBurstSwordB1Time = 120;
    }

    public static partial class Status
    {
        //Enemy Max HP
        public static float EnemyS1_MaxHP = 100;
        public static float EnemyS1B_MaxHP = 100;
        public static float EnemyS2_MaxHP = 150;
        public static float EnemyS3_MaxHP = 50;

        public static float EnemyM1_MaxHP = 1000;
        public static float EnemyM1B_MaxHP = 1200;
        public static float EnemyM2_MaxHP = 1000;

        public static float EnemyB1_SubCoreA_MaxHP = 4000;
        public static float EnemyB1_SubCoreB_MaxHP = 3500;
        public static float EnemyB1_SubCoreC_MaxHP = 3000;
        public static float EnemyB1_MainCore_MaxHP = 8000;

        //Kill Score
        public static float EnemyS1_KillScore = 50;
        public static float EnemyS1B_KillScore = 50;
        public static float EnemyS2_KillScore = 100;
        public static float EnemyS3_KillScore = 300;

        public static float EnemyM1_KillScore = 800;
        public static float EnemyM1B_KillScore = 800;
        public static float EnemyM2_KillScore = 1200;

        public static float EnemyB1_SubCore_KillScore = 2000;
        public static float EnemyB1_MainCore_KillScore = 5000;
    }

    public static partial class Status
    {
        //Energy Restore
        public static float EnergyGainPerSecond = 200.0f;
        public static float EnergyHitGainMultSpread = 1.0f; // 1.0f: 1 hp = 0.01%
        public static float EnergyHitGainMultFocus = 1.0f;
        public static float EnergyHitGainBonusSpread = 1.5f;
        public static float EnergyHitGainBonusFocus = 1.5f;

        //Score Gain
        public static float ScoreHitGainMult = 1.0f; // 1 hp = 10 score
        public static float RemainScore = 10000;
        public static float BurstChainScore = 5000;
        public static float NoMissScore = 25000;

        //Multiplier
        public static float AbsorbScoreMult = 1.0f;
        public static float GrazeScoreMult = 5.0f;
        public static float AbsorbEnergyBonusMult = 1.0f;
        public static float GrazeEnergyMult = 5.0f;

        //Timer
        public static float Boss1Timer = 120;
        public static float Boss1TimerScore = 240000;
        public static float Boss1DefeatScore = 50000;
        public static float[] StageClearScore = { 100000 };

        public static float BossDefeatScore(int stage)
        {
            if (stage == 1) { return Boss1DefeatScore; }
            return 0;
        }

        public static float MidbossDefeatScore(int stage)
        {
            return 0;
        }

    }
}
