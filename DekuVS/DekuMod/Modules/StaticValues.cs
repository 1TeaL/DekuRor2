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
        
        //Energy
        internal static float maxPlusUltra = 100f;
        internal static float regenPlusUltraRate = 8f;
        internal static float basePlusUltraGain = 1f;
        internal static float skillPlusUltraGain = 2f;
        internal static float skillPlusUltraSpend = 5f;
        internal static float skill45PlusUltraSpend = 15f;
        internal static float skill100PlusUltraSpend = 25f;
        internal static float specialPlusUltraSpend = 50f;
        internal static float goBeyondBuffGain = 1f;

        //Primary
        //airforce
        internal const float airforceDamageCoefficient = 1f;
        internal const float airforce45DamageCoefficient = 1.25f;
        internal const float airforce100DamageCoefficient = 2f;
        //shootstylekick
        internal const float shootkickDamageCoefficient = 2f;
        internal const float shootkick45DamageCoefficient = 3f;
        internal const float shootkick100DamageCoefficient = 1f;
        //blackwhip
        internal const float blackwhipPullRange = 50f;
        internal const float blackwhipDamageCoefficient = 1f;
        internal const float blackwhip45DamageCoefficient = 1f;
        internal const float blackwhip100DamageCoefficient = 2f;


        internal const float shootbulletDamageCoefficient = 2f;
        internal const float fajinDamageCoefficient = 0.5f;

        //Secondary
        //detroit
        internal const float detroitDamageCoefficient = 6f;
        internal const float detroitRange = 5f;
        internal const float detroit45DamageCoefficient = 12f;
        internal const float detroit100DamageCoefficient = 18f;
        //stlouis
        internal const float stlouisDamageCoefficient = 4f;
        internal const float stlouis45DamageCoefficient = 6f;
        internal const float stlouis100DamageCoefficient = 4f;
        //dangersense
        internal const float dangersense45DamageReduction = 0.5f;
        internal const float dangersenseDamageCoefficient = 1f;
        internal const float dangersense45DamageCoefficient = 2f;
        internal const float dangersense100DamageCoefficient = 2f;
        internal const int dangersenseBuffTimer = 6;
        internal const int dangersense45BuffTimer = 6;
        internal const int dangersense100BuffTimer = 8;
        //Secondary damage
        internal const float blackwhipshootDamageCoefficient = 4f;

        //Utility
        //delaware
        internal const float delawareDamageCoefficient = 6f;
        internal const float delaware45DamageCoefficient = 6f;
        internal const float delaware100DamageCoefficient = 6f;
        //manchester
        internal const float manchesterArmor = 300f;
        internal const float manchesterDamageCoefficient = 4f;
        internal const float manchester45DamageCoefficient = 4f;
        internal const float manchester100DamageCoefficient = 8f;
        //smokescreen
        internal const float smokescreenDuration = 4f;
        internal const float smokescreen45Duration = 8f;
        internal const float smokescreen100Duration = 8f;
        internal const float smokescreenDamageCoefficient = 2f;
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
        internal const float detroitdelawareDamageCoefficient = 20f;
        internal const float detroitdelawareRange = 50f;
        //leg
        internal const float finalsmashDamageCoefficient = 2f;
        internal const float finalsmashRange = 20f;
        //quirk
        internal const float fajinDuration = 10f;

        //Extra quirks
        internal const float floatForceEnergyFraction = 0.01f;
        internal const float floatSpeed = 1f;
        internal const float blackwhipPull = 8f;

        internal const int fajinMaxStack = 200;
        internal const int kickMaxStack= 4;
        internal const int fajinMaxPower = 50;
        internal const float fajinMaxMultiplier = 2f;
        internal const float floatDuration = 10f;



    }
}