using DekuMod.SkillStates;
using System.Collections.Generic;
using System;
using DekuMod.SkillStates.BaseStates;

namespace DekuMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void RegisterStates()
        {
            //base skills
            entityStates.Add(typeof(BaseSkill));
            entityStates.Add(typeof(BaseSkill45));
            entityStates.Add(typeof(BaseSkill100));

            //go beyond
            entityStates.Add(typeof(GoBeyond));
            entityStates.Add(typeof(GoBeyondEmptySkill));

            //primary
            entityStates.Add(typeof(Airforce));
            entityStates.Add(typeof(Airforce45));
            entityStates.Add(typeof(Airforce100));

            entityStates.Add(typeof(BaseMeleeAttack));
            entityStates.Add(typeof(ShootStyleKick));
            entityStates.Add(typeof(DashAttack));
            entityStates.Add(typeof(DashAttackExit));
            entityStates.Add(typeof(ShootStyleCombo));
            entityStates.Add(typeof(ShootStylePrimary));

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
            entityStates.Add(typeof(DelawareSmash100));
            entityStates.Add(typeof(StLouis45));


            //secondary
            entityStates.Add(typeof(Blackwhip100));
            entityStates.Add(typeof(Detroit45));
            entityStates.Add(typeof(Manchester45));
            entityStates.Add(typeof(Manchester100));
            entityStates.Add(typeof(OFAcycle1));
            entityStates.Add(typeof(OFAcycle1scepter));
            entityStates.Add(typeof(OFAcycle2));
            entityStates.Add(typeof(OFAcycle2scepter));
            entityStates.Add(typeof(OFAcycledown));
            entityStates.Add(typeof(ShootStyleBulletStun45));
            entityStates.Add(typeof(ShootStyleBulletStun100));
            entityStates.Add(typeof(ShootStyleDash45));
            entityStates.Add(typeof(ShootStyleDashAttack45));
            entityStates.Add(typeof(ShootStyleDash100));
            entityStates.Add(typeof(ShootStyleDashAttack100));
            entityStates.Add(typeof(ShootStyleKick45));
            entityStates.Add(typeof(ShootStyleKick100));

            //utility
            entityStates.Add(typeof(DelawareSmash));
            entityStates.Add(typeof(DelawareSmash45));
            entityStates.Add(typeof(DangerSense));
            entityStates.Add(typeof(DangerSense45));
            entityStates.Add(typeof(DangerSense100));
            entityStates.Add(typeof(DangerSenseCounter));
            entityStates.Add(typeof(Float));
            entityStates.Add(typeof(Float45));
            entityStates.Add(typeof(Float100));
            entityStates.Add(typeof(FloatCancel));
            entityStates.Add(typeof(FloatCancel45));
            entityStates.Add(typeof(FloatCancel100));
            entityStates.Add(typeof(StLouis));
            entityStates.Add(typeof(StLouis100));
            entityStates.Add(typeof(Oklahoma));
            entityStates.Add(typeof(Oklahoma45));
            entityStates.Add(typeof(Oklahoma100));


            //ofa
            entityStates.Add(typeof(OFAstate));
            entityStates.Add(typeof(OFAstatescepter));
            entityStates.Add(typeof(OFAstate45));
            entityStates.Add(typeof(OFAstatescepter45));
            entityStates.Add(typeof(OFAdown));

            //extra
            entityStates.Add(typeof(Fajinstate));
            entityStates.Add(typeof(Fajinstatescepter));
            entityStates.Add(typeof(QuirkSuper));
            entityStates.Add(typeof(Fajinscepter));
            entityStates.Add(typeof(BlackwhipShoot));
            entityStates.Add(typeof(Smokescreen));
        }
    }
}