using DekuMod.SkillStates;
using DekuMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace DekuMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void RegisterStates()
        {
            entityStates.Add(typeof(Smash));
            entityStates.Add(typeof(SmashRelease));
            entityStates.Add(typeof(ShootStyleBullet));
            entityStates.Add(typeof(ShootStyleDash));
            entityStates.Add(typeof(ShootStyleDashAttack));
            entityStates.Add(typeof(DelawareSmash));

            entityStates.Add(typeof(Airforce));
            entityStates.Add(typeof(OFAstate));
            entityStates.Add(typeof(OFAstatescepter));
            entityStates.Add(typeof(BlackwhipFront));
            entityStates.Add(typeof(OFAdown));

        }
    }
}