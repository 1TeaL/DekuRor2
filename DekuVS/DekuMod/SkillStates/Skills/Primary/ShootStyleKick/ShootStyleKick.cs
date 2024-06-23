//using System;
//using System.Collections.Generic;
//using EntityStates;
//using RoR2.Skills;
//using EntityStates.Merc;
//using EntityStates.Treebot.Weapon;
//using RoR2;
//using UnityEngine;
//using UnityEngine.Networking;
//using DekuMod.Modules.Survivors;

//namespace DekuMod.SkillStates
//{

//    public class ShootStyleKick : BaseSkill
//    {
//        public HurtBox Target;
//        public override void OnEnter()
//        {

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
