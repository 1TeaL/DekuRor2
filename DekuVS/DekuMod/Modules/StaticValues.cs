using System;

namespace DekuMod.Modules
{
    internal static class StaticValues
    {
        internal static string descriptionText = "Deku is high risk survivor that hurts himself to power up his skills.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Airforce shoots a single enemy, when powered up turns into Delaware Smash, having AOE and launching you backwards." + Environment.NewLine + Environment.NewLine
             + "< ! > Detroit smash is a chargeable punch that lunges you forward; when powered up turns it into St. Louis Smash, granting larger AOE." + Environment.NewLine + Environment.NewLine
             + "< ! > Blackwhip allows him to move through the environment and grapple on to enemies; when powered up lets him pull multiple enemies towards him." + Environment.NewLine + Environment.NewLine
             + "< ! > One for All 100% enables Deku to use one powered up skill at cost of a percentage of his health." + Environment.NewLine + Environment.NewLine;
        //Passive
        internal static float healMarkCoefficient = 0.1f;
        internal static float barrierMarkCoefficient = 0.1f;
        //dangersense
        internal const float dangersenseDamageCoefficient = 2f;
        internal const int dangersenseBuffTimer = 10;
        internal static float dangersensePlusUltraSpend = 5f;
        //float
        internal const float floatForceEnergyFraction = 0.01f;
        internal const float floatSpeed = 3f;


        //Energy
        internal static float maxPlusUltra = 100f;
        internal static float regenPlusUltraRate = 8f;
        internal static float basePlusUltraGain = 1f;
        internal static float skillPlusUltraGain = 2f;
        internal static float skillPlusUltraSpend = 10f;
        internal static float skill45PlusUltraSpend = 10f;
        internal static float skill100PlusUltraSpend = 5f;
        internal static float specialPlusUltraSpend = 50f;
        internal static float modePlusUltraSpend = 5f;
        internal static float goBeyondBuffGain = 1f;
        internal static int goBeyondBuffDuration = 60;
        internal static float goBeyondThreshold = 95f;

        //Primary
        //airforce
        internal const float airforceDamageCoefficient = 1f;
        internal const int airforceMaxRicochet = 5;
        internal const float airforce45DamageCoefficient = 2f;
        internal const float airforce100DamageCoefficient = 4f;
        internal const float airforce100HealthCostFraction = 0.005f;
        //shootstylekick
        internal const float shootkickDamageCoefficient = 1.5f;
        internal const float shootkick45DamageCoefficient = 2f;
        internal const float shootkick100DamageCoefficient = 1f;
        internal const float shootkick100HealthCostFraction = 0.1f;
        //blackwhip
        internal const float blackwhipDamageCoefficient = 0.1f;
        internal const float blackwhipRange = 20f;
        internal const int blackwhipDebuffDuration = 10;
        internal const int blackwhipTargets = 10;
        internal const float blackwhip45DamageCoefficient = 6f;
        internal const float blackwhip100DamageCoefficient = 6f;
        internal const float blackwhip100PullRange = 70f;
        internal const float blackwhip100HealthCostFraction = 0.10f;


        internal const float shootbulletDamageCoefficient = 2f;
        internal const float fajinDamageCoefficient = 0.5f;

        //Secondary
        //detroit
        internal const float detroitDamageCoefficient = 4f;
        internal const float detroitRange = 5f;
        internal const float detroit45DamageCoefficient = 6f;
        internal const float detroit100DamageCoefficient = 8f;
        internal const float detroit100HealthCostFraction = 0.15f;
        //stlouis
        internal const float stlouisDamageCoefficient = 3f;
        internal const float stlouisRange = 5f;
        internal const float stlouis45DamageCoefficient = 2f;
        internal const float stlouis100DamageCoefficient = 6f;
        internal const float stlouis100HealthCostFraction = 0.15f;
        //gearshift
        internal const int gearshiftBuffTimer = 10;
        internal const float gearshiftForceBoost = 10f;
        internal const float gearshiftMovespeedBoost = 1.2f;
        internal const int gearshift45BuffTimer = 10;
        internal const float gearshift45DamageCoefficient = 0.5f;
        internal const int gearshift100BuffAttacks = 5;
        internal const float gearshift100Threshold = 15f;
        internal const float gearshift100HealthCostFraction = 0.25f;
        //Secondary damage
        internal const float blackwhipshootDamageCoefficient = 4f;

        //Utility
        //delaware
        internal const float delawareDamageCoefficient = 6f;
        internal const float delaware45DamageCoefficient = 9f;
        internal const float delaware100DamageCoefficient = 12f;
        internal const float delaware100HealthCostFraction = 0.15f;
        //manchester
        internal const float manchesterArmor = 200f;
        internal const float manchesterDamageCoefficient = 3f;
        internal const float manchester45DamageCoefficient = 4f;
        internal const int manchester45BuffDuration = 4;
        internal const float manchester100DamageCoefficient = 6f;
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
        internal const float oklahomaDamageCoefficient = 1f;
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
