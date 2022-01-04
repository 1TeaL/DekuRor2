//using System;
//using EntityStates;
//using RoR2;
//using UnityEngine;
//using DekuMod.Modules.Survivors;
//using EntityStates.Mage;

//namespace DekuMod.SkillStates.BaseStates
//{
//    public class DekuMain : GenericCharacterMain
//    {
//        private Animator animator;
//        public DekuController dekucon;
//        private EntityStateMachine jetpackMachine;
//        private ChildLocator childLocator;
//        private DynamicBone tailBone;

//        public override void OnEnter()
//        {
//            base.OnEnter();
//            dekucon = base.GetComponent<DekuController>();
//            //this.jetpackMachine = EntityStateMachine.FindByCustomName(base.gameObject, "Jet");
//            //this.animator = base.GetModelAnimator();
//            //this.childLocator = base.GetModelChildLocator();
//            //bool flag = this.childLocator;
//            //if (flag)
//            //{
//            //	this.tailBone = this.childLocator.FindChild("Tail").GetComponent<DynamicBone>();
//            //}
//        }


//        public override void ProcessJump()
//        {
//            base.ProcessJump();
//            bool flag = this.hasCharacterMotor && this.hasInputBank && base.isAuthority;
//            if (flag)
//            {
//                bool flag2 = base.inputBank.jump.down && base.characterMotor.velocity.y < 0f && !base.characterMotor.isGrounded && !dekucon.endFloat;
//                if (flag2)
//                {
//                    //bool flag3 = !(this.jetpackMachine.state.GetType() == typeof(DekuJetpack));
//                    //if (flag3)
//                    //if(!this.state.GetType() == typeof(DekuJetpack))
//                    //{
//                        this.outer.SetNextState(new DekuJetpack());
//                        this.jetpackMachine.SetState(new DekuJetpack());
//                        base.characterBody.AddBuff(Modules.Buffs.floatBuff);
//                        dekucon.hasFloatBuff = true;
//                    //}
//                    //}
//                    //else
//                    //{
//                    //    bool flag4 = this.jetpackMachine.state.GetType() == typeof(DekuJetpack);
//                    //    if (flag4)
//                    //    {
//                    //        this.jetpackMachine.SetNextState(new Idle());
//                    //        base.characterBody.RemoveBuff(Modules.Buffs.floatBuff);
//                    //        dekucon.hasFloatBuff = false;
//                    //    }
//                }
//            }
//            bool isGrounded = base.characterMotor.isGrounded;
//            if (isGrounded)
//            {
//                dekucon.endFloat = false;
//            }
//        }

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();
//            //bool flag = this.animator;
//            //if (flag)
//            //{
//            //    float inAir = 0f;
//            //    bool flag2 = !this.animator.GetBool("isGrounded");
//            //    if (flag2)
//            //    {
//            //        inAir = 1f;
//            //    }
//            //    this.animator.SetFloat("inAir", inAir);
//            //    this.animator.SetBool("inCombat", !base.characterBody.outOfCombat || !base.characterBody.outOfDanger);
//            //    this.animator.SetBool("useAdditive", !this.animator.GetBool("isSprinting"));
//            //    this.animator.SetBool("isHovering", inAir == 1f && base.characterBody.HasBuff(Modules.Buffs.floatBuff));
//            //    bool flag3 = this.tailBone;
//            //    if (flag3)
//            //    {
//            //        bool flag4 = this.animator.GetBool("isGrounded") && !this.animator.GetBool("isMoving");
//            //        if (flag4)
//            //        {
//            //            this.tailBone.enabled = false;
//            //        }
//            //        else
//            //        {
//            //            this.tailBone.enabled = true;
//            //        }
//            //    }
//            //}
//        }


//    }
//}
