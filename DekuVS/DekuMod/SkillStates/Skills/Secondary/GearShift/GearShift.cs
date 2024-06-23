//using DekuMod.Modules.Networking;
//using DekuMod.Modules.Survivors;
//using EntityStates;
//using R2API.Networking;
//using R2API.Networking.Interfaces;
//using RoR2;
//using UnityEngine;
//using UnityEngine.Networking;
//using Random = UnityEngine.Random;

//namespace DekuMod.SkillStates
//{
//    public class GearShift : BaseQuirk
//    {

//        public static float duration = 0.5f;

//        public override void OnEnter()
//        {
//            base.OnEnter();


//        }

//        protected override void DoSkill()
//        {
//            base.DoSkill();
//            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
//            base.PlayCrossfade("FullBody, Override", "GearShift", "Attack.playbackRate", duration, 0.01f);
//            bool active = NetworkServer.active;
//            if (active)
//            {
//                if(characterBody.HasBuff(Modules.Buffs.gearshiftBuff))
//                {
//                    base.characterBody.ApplyBuff(Modules.Buffs.gearshiftBuff.buffIndex, 0);
//                }
//                else if (!characterBody.HasBuff(Modules.Buffs.gearshiftBuff))
//                {
//                    base.characterBody.ApplyBuff(Modules.Buffs.gearshiftBuff.buffIndex, 1);
//                }

//            }
//        }
//        protected override void DontDoSkill()
//        {
//            base.DontDoSkill();
//            skillLocator.secondary.AddOneStock();
//        }

//        public override void OnExit()
//        {
//            base.OnExit();
//        }

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();

//            if (base.fixedAge >= duration && base.isAuthority)
//            {
//                this.outer.SetNextStateToMain();
//                return;
//            }
            
//        }

//        public override InterruptPriority GetMinimumInterruptPriority()
//        {
//            return InterruptPriority.PrioritySkill;
//        }
//    }
//}