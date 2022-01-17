using DekuMod.SkillStates;
using System.Collections.Generic;
using System;

namespace DekuMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void RegisterStates()
        {
            entityStates.Add(typeof(Airforce));
            entityStates.Add(typeof(ShootStyleKick));
            entityStates.Add(typeof(ShootStyleKick2));
            entityStates.Add(typeof(ShootStyleBullet));
            entityStates.Add(typeof(BlackwhipFront));
            entityStates.Add(typeof(Manchester));
            entityStates.Add(typeof(Blackwhip45));
            entityStates.Add(typeof(ShootStyleDash));
            entityStates.Add(typeof(ShootStyleDashAttack));
            entityStates.Add(typeof(ShootStyleBulletStun));
            entityStates.Add(typeof(Detroit));
            entityStates.Add(typeof(DetroitRelease));
            entityStates.Add(typeof(Detroit100));
            entityStates.Add(typeof(Detroit100Release));
            entityStates.Add(typeof(DelawareSmash));
            entityStates.Add(typeof(StLouis45));

            entityStates.Add(typeof(OFAstate));
            entityStates.Add(typeof(OFAstatescepter));
            entityStates.Add(typeof(OFAstate45));
            entityStates.Add(typeof(OFAstatescepter45));
            entityStates.Add(typeof(OFAdown));

            entityStates.Add(typeof(Fajinstate));
            entityStates.Add(typeof(Fajinstatescepter));
            entityStates.Add(typeof(Fajin));
            entityStates.Add(typeof(Fajinscepter));
            entityStates.Add(typeof(BlackwhipShoot));
            entityStates.Add(typeof(Smokescreen));
        }
    }
}