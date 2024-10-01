using System;
using UnityEngine;

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
        internal static float baseRegenPlusUltra = 1/50f;
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

        //passive
        //internal static float blackwhipPullGrappleMultiplier = 3f;
        //internal static float blackwhipPullInputMultiplier = 2f;
        internal static float blackwhipPullDistance = 35f;
        internal static float blackwhipPullSpeed = 100f;
        internal static float blackwhipPullAcceleration = 50f;
        internal static float blackwhipPullLookAcceleration = 4f;
        internal static float blackwhipPullMoveAcceleration = 4f;
        internal static float blackwhipPullEscapeForce = 2f;
        internal static float blackwhipPullLookImpulse = 5f;
        internal static float blackwhipPullMoveImpulse = 5f;
        internal static float blackwhipPulllookAccelerationRampUpDuration = 0.25f;
        internal static float blackwhipPullSpeedControl = 2f;
        internal static float blackwhipPullHop = 5f;
        internal static float blackwhipPullDuration = 0.3f;

        //Might Mode
        internal const float mightBuffMultiplier = 2f;
        internal const int mightBuffDuration = 6;
        internal const float mightSwitchRadius = 20f;
        internal const float mightSwitchDamage = 2f;
        //Primary
        internal const float smashRushDistance = 5f;
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
        internal const float detroitDamageCoefficient = 6f;
        internal const float detroitDamageCoefficient2 = 6f;
        internal const float detroitDamageCoefficient3 = 6f;

        //Shoot Style Mode
        internal const float shootSwitchRadius = 5f;
        internal const float shootSwitchSlamForce = 1000f;
        internal const float shootSwitchDropForce = 100f;
        internal const float shootSwitchHeightStart = 40f;
        internal const float shootSwitchDamage = 2f;
        //Primary
        internal const float airforceDamageCoefficient = 1f;
        internal const int airforceMaxRicochet = 5;
        //Secondary
        internal const float stlouis45DamageCoefficient = 2f;
        internal const float blastDashDamageCoefficient = 2f;
        internal const float blastDashSpeed = 20f;
        internal const float blastDashForce = 2000f;
        internal const float blastDashRadius = 10f;
        internal const float blastDashDuration = 0.6f;
        //Utility
        internal const float oklahomaSpeedCoefficient = 10f;
        internal const float oklahomaDamageCoefficient = 3f;
        internal const float oklahomaDashDuration = 0.5f;
        internal const float oklahomaDashTotalDuration = 1f;
        //Special
        internal const int stlouisTotalHits = 5;
        internal const int stlouisTotalHits3 = 5;
        internal const float stlouisDistance2 = 10f;
        internal const float stlouisDuration = 1f;
        internal const float stlouisDuration2 = 1f;
        internal const float stlouisDuration3 = 1f;
        internal const float stlouisRadius = 5f;
        internal const float stlouisRadius2 = 5f;
        internal const float stlouisRadius3 = 5f;
        internal const float stlouisDamageCoefficient = 3f;
        internal const float stlouisDamageCoefficient2 = 3f;
        internal const float stlouisDamageCoefficient3 = 3f;
        internal const float stlouisDamageForce = 0f;
        internal const float stlouisDamageForce2 = 0f;
        internal const float stlouisDamageForce3 = 0f;

        //Blackwhip Mode
        internal const int blackwhipCDBuffDuration = 6;
        internal const float blackwhipCDMultiplier = 0.5f;
        internal const float blackwhipSwitchRadius = 20f;
        internal const float blackwhipSwitchDamage = 3f;
        //Primary
        internal const float blackwhipStrikeRange = 40f;
        internal const float blackwhipStrikeDamage = 2f;
        internal const float blackwhipStrikeForce = 20f;
        internal const float blackwhipProc = 0.5f;
        //Secondary
        internal const float pinpointRange = 50f;
        internal const float pinpointRadius = 5f;
        internal const float pinpointDamageCoefficient = 6f;
        internal const float pinpointDuration = 1f;
        internal const int blackwhipDebuffDuration = 2;
        internal const float blackwhipDebuffMultiplier = 0.1f;
        //Utility
        internal static int blackwhipOverlayDuration = 10;
        internal static float blackwhipDodgeDuration = 0.3f;
        internal static float blackwhipDodgeSpeed = 4f;
        //Special
        internal static float blackwhipOverdriveDuration = 0.6f;
        internal static float blackwhipOverdriveDamage = 4f;
        internal static float blackwhipOverdriveRange = 60f;
        internal static float blackwhipOverdriveAngle = 180f;
        internal static float blackwhipOverdriveDuration2 = 1f;
        internal static float blackwhipOverdriveDamage2 = 4f;
        internal static float blackwhipOverdriveRange2 = 20f;
        internal static float blackwhipOverdriveRadius2 = 30f;
        internal static float blackwhipOverdriveAngle2 = 360f;
        internal static float blackwhipOverdriveForce2 = 15000f;
        internal static float blackwhipOverdriveDuration3 = 1f;
        internal static float blackwhipOverdriveRadius3 = 3f;
        internal static float blackwhipOverdriveRange3 = 50f;
        internal static float blackwhipOverdriveAngle3 = 360f;
        internal static float blackwhipOverdriveDamage3 = 10f;
        internal static float blackwhipOverdriveForce3 = 10000f;
        internal static float blackwhipOverdriveSpeed3 = 10f;

        #region Mask Check
        public static bool Includes(
            LayerMask mask,
            int layer)
        {
            return (mask.value & 1 << layer) > 0;
        }
        #endregion
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
        //internal const float blackwhipRange = 20f;
        //internal const int blackwhipDebuffDuration = 10;
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
        //internal const float stlouisDamageCoefficient = 3f;
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
