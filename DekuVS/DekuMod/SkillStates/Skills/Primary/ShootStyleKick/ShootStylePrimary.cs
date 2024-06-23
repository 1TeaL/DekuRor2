//using DekuMod.Modules.Survivors;
//using DekuMod.SkillStates.BaseStates;
//using EntityStates;
//using RoR2;
//using UnityEngine;

//namespace DekuMod.SkillStates
//{
//    public class ShootStylePrimary : BaseSkillState
//    {
//        public DekuController dekucon;
//        public HurtBox Target;
//        public override void OnEnter()
//        {
//            dekucon = base.GetComponent<DekuController>();
//            if (dekucon && base.isAuthority)
//            {
//                Target = dekucon.GetTrackingTarget();
//            }

//            if (Target)
//            {
//                float num = 10f;
//                if (!base.isGrounded)
//                {
//                    num = 7f;
//                }
//                float num2 = Vector3.Distance(base.transform.position, Target.transform.position);
//                if (num2 >= num)
//                {
//                    this.outer.SetNextState(new DashAttack
//                    {

//                    });
//                }
//                else
//                {
//                    this.outer.SetNextState(new ShootStyleCombo
//                    {

//                    });

//                }
//            }
//            else if (!Target)
//            {
//                this.outer.SetNextState(new ShootStyleCombo
//                {

//                });
//            }

//            base.OnEnter();
//        }

//        public override void OnExit()
//        {
//            base.OnExit();
//        }
//        public override InterruptPriority GetMinimumInterruptPriority()
//        {
//            return InterruptPriority.Skill;
//        }
//    }
//}