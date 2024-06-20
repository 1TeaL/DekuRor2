using System;

namespace DekuMod.Modules
{
    internal static class StaticValues
    {
        //Passive
        internal static float healMarkCoefficient = 0.1f;
        internal static float barrierMarkCoefficient = 0.01f;
        //dangersense
        internal const float dangersenseDamageCoefficient = 2f;
        internal const int dangersenseBuffTimer = 10;
        internal static float dangersensePlusUltraSpend = 5f;
        //float
        internal const float floatForceEnergyFraction = 0.01f;
        internal const float floatSpeed = 7f;


        //Energy
        internal static float super1Cost = 33f;
        internal static float super2Cost = 66f;
        internal static float super3Cost = 100f;
        internal static float plusUltraHealthCost = 0.1f;
        internal static float plusUltraHealthCost2 = 0.3f;
        internal static float plusUltraHealthCost3 = 0.6f;
        internal static float bonusPlusUltraRate = 2f;
        internal static float baseRegenPlusUltra = 0.05f;
        internal static float maxPlusUltra = 100f;
        internal static float regenPlusUltraRate = 8f;
        internal static float basePlusUltraGain = 1f;
        internal static float skillPlusUltraGain = 2f;
        internal static float skillPlusUltraSpend = 5f;
        internal static float skill45PlusUltraSpend = 5f;
        internal static float skill100PlusUltraSpend = 5f;
        internal static float specialPlusUltraSpend = 50f;
        internal static float modePlusUltraSpend = 5f;
        internal static float goBeyondBuffGain = 1f;
        internal static int goBeyondBuffDuration = 60;
        internal static float goBeyondThreshold = 95f;

        //Might Mode
        //Primary
        internal const float smashRushDistance = 3f;
        internal const float smashRushDamageCoefficient = 3f;
        //Secondary
        internal const float delawareChargeThreshold = 2f;
        internal const float delawareChargeMultiplier = 3f;
        internal const float delawareDamageCoefficient = 3f;
        internal const float delawareDamageCoefficient1 = 2f;
        internal const float delawareDamageCoefficient2 = 10f;
        internal const float delawareCharge1 = 0.7f;
        internal const float delawareCharge2 = 0.4f;
        internal const float delawareDistance = 20f;
        //Utility
        internal const float counterDamageReduction = 0.5f;
        internal const float counterDuration = 5f;
        internal const int counterBuffDuration = 5;
        internal const float counterBuffAttackspeed = 3f;
        internal const float counterRadius = 3f;
        internal const float counterBuffArmor = 200f;
        internal const float counterDamageCoefficient = 6f;
        //Special
        internal const float detroitForce = 10000f;
        internal const float detroitForce2 = 20000f;
        internal const float detroitForce3 = 10000f;
        internal const float detroitRadius = 20f;
        internal const float detroitRadius2 = 30f;
        internal const float detroitRadius3 = 10f;
        internal const float detroitDamageCoefficient = 5f;
        internal const float detroitDamageCoefficient2 = 5f;
        internal const float detroitDamageCoefficient3 = 5f;

        //Shoot Style Mode
        //Primary
        internal const float airforceDamageCoefficient = 1f;
        internal const int airforceMaxRicochet = 5;
        //Secondary
        internal const float stlouis45DamageCoefficient = 2f;
        //Utility
        internal const float oklahomaSpeedCoefficient = 30f;
        internal const float oklahomaDamageCoefficient = 3f;
        //Special

        //Blackwhip Mode
        //Primary
        //Secondary
        //Utility
        //Special

        //Primary
        //airforce
        internal const float airforce45DamageCoefficient = 2f;
        internal const float airforce100DamageCoefficient = 4f;
        internal const float airforce100HealthCostFraction = 0.005f;
        //shootstylekick
        internal const float shootkickDamageCoefficient = 1.5f;
        internal const float shootkick45DamageCoefficient = 2f;
        internal const float shootkick100DamageCoefficient = 1f;
        internal const float shootkick100NumberOFHits = 4;
        internal const float shootkick100HealthCostFraction = 0.1f;
        //blackwhip
        internal const float blackwhipDamageCoefficient = 0.1f;
        internal const float blackwhipLineMaxHeight = 0.2f;
        internal const float blackwhipRange = 20f;
        internal const int blackwhipDebuffDuration = 10;
        internal const int blackwhipTargets = 10;
        internal const float blackwhip45DamageCoefficient = 6f;
        internal const float blackwhip100DamageCoefficient = 6f;
        internal const int blackwhipAttachDuration = 2;
        internal const float blackwhip100AttachRange = 30f;
        internal const float blackwhip100PullRange = 70f;
        internal const float blackwhip100HealthCostFraction = 0.10f;
        internal const float blackwhip100EnergyFraction = 0.02f;


        internal const float shootbulletDamageCoefficient = 2f;
        internal const float fajinDamageCoefficient = 0.5f;

        //Secondary
        //detroit
        internal const float detroitRange = 5f;
        internal const float detroit45DamageCoefficient = 6f;
        internal const float detroit100DamageCoefficient = 8f;
        internal const float detroit100HealthCostFraction = 0.15f;
        //stlouis
        internal const float stlouisDamageCoefficient = 3f;
        internal const float stlouisRange = 5f;
        internal const float stlouis100DamageCoefficient = 6f;
        internal const float stlouis100HealthCostFraction = 0.15f;
        //gearshift
        internal const int gearshiftBuffTimer = 2;
        internal const float gearshiftForceBoost = 10f;
        internal const float gearshiftMovespeedBoost = 1.2f;
        internal const int gearshift45BuffTimer = 2;
        internal const float gearshift45DamageCoefficient = 0.5f;
        internal const int gearshift100BuffAttacks = 5;
        internal const float gearshift100Threshold = 15f;
        internal const float gearshift100HealthCostFraction = 0.25f;
        internal const float gearshiftEnergyFraction = 0.015f;
        //Secondary damage
        internal const float blackwhipshootDamageCoefficient = 4f;

        //Utility
        //delaware
        //manchester
        internal const float manchesterArmor = 200f;
        internal const float manchesterDamageCoefficient = 3f;
        internal const float manchester45DamageCoefficient = 2.5f;
        internal const int manchester45BuffDuration = 4;
        internal const float manchester100DamageCoefficient = 4f;
        internal const float manchester100HealthCostFraction = 0.15f;
        //smokescreen
        internal const float smokescreenDuration = 6f;
        internal const float smokescreen45Duration = 10f;
        internal const float smokescreen100Duration = 6f;
        internal const float smokescreenDamageCoefficient = 6f;
        internal const float smokescreen100HealthCostFraction = 0.25f;
        //Utility damage
        internal const float shootattackDamageCoefficient = 1f;
        internal const float shootattack45DamageCoefficient = 1.5f;
        internal const float shootattack100DamageCoefficient = 1f;
        internal const float shootbulletstunDamageCoefficient = 1f;
        internal const float shootbulletstun45DamageCoefficient = 3f;
        internal const float shootbulletstun100DamageCoefficient = 1f;
        internal const float floatDamageCoefficient = 4f;
        //internal const float oklahomaDamageCoefficient = 1f;
        internal const float oklahoma45DamageCoefficient = 3f;
        internal const float oklahoma100DamageCoefficient = 2f;

        //Special
        //fist
        internal const float detroitdelawareDamageCoefficient = 0.1f;
        internal const float detroitdelawareSmashDamageCoefficient = 15f;
        internal const float detroitdelawareBlastRadius = 30f;
        //leg
        internal const float finalsmashDamageCoefficient = 0.3f;
        internal const float finalsmashSmashDamageCoefficient = 15f;
        internal const float finalsmashBlastRadius = 15f;
        //quirk
        internal const float fajinDuration = 10f;
        internal const float fajinDamageMultiplier = 1.5f;
        //gobeyond
        internal const float gobeyondDamageCoefficient = 10f;
        internal const float gobeyondHealCoefficient = 0.05f;
        //ofa
        internal const float ofaHealthCost = 0.1f;

        //Extra quirks
        internal const float blackwhipPull = 8f;

        internal const int fajinMaxStack = 200;
        internal const int kickMaxStack= 4;
        internal const int fajinMaxPower = 50;
        internal const float floatDuration = 10f;



    }
}
