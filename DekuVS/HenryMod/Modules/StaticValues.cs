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




        internal const float airforceDamageCoefficient = 1f;
        internal const float shootkickDamageCoefficient = 3f;
        internal const float airforce45DamageCoefficient = 1.25f;
        internal const float shootbulletDamageCoefficient = 2f;

        internal const float blackwhipDamageCoefficient = 0.5f;
        internal const float manchesterDamageCoefficient = 3f;
        internal const float blackwhip45DamageCoefficient = 0.8f;
        internal const float blackwhipshootDamageCoefficient = 3f;

        internal const float detroit100DamageCoefficient = 6f;
        internal const float detroitDamageCoefficient = 4f;
        internal const float StLouis45DamageCoefficient = 6f;
        internal const float delawareDamageCoefficient = 6f;
        internal const float shootattackDamageCoefficient = 2f;
        internal const float shootbulletstunDamageCoefficient = 1f;



        internal const float blackwhipPull = 8f;

        internal const int fajinMaxStack = 100;
        internal const float fajinMaxMultiplier = 2f;




    }
}