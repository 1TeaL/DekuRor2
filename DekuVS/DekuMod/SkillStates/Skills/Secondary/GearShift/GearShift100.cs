//using DekuMod.Modules.Networking;
//using DekuMod.Modules.Survivors;
//using EntityStates;
//using ExtraSkillSlots;
//using R2API.Networking;
//using R2API.Networking.Interfaces;
//using RoR2;
//using System;
//using UnityEngine;
//using UnityEngine.Networking;
//using Random = UnityEngine.Random;

//namespace DekuMod.SkillStates
//{
//    public class GearShift100 : BaseQuirk100
//    {

//        public static float duration = 0.5f;

//        public override void OnEnter()
//        {
//            base.OnEnter();

//        }

//        protected override void DoSkill()
//        {
//            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
//            base.PlayCrossfade("FullBody, Override", "GearShift", "Attack.playbackRate", duration, 0.01f);
//            float num = this.moveSpeedStat;
//            bool isSprinting = base.characterBody.isSprinting;
//            if (isSprinting)
//            {
//                num /= base.characterBody.sprintingSpeedMultiplier;
//            }
//            int num2 = (int)Math.Round(1f + (num / (7f * 1.5f) - 1f));

//            bool active = NetworkServer.active;
//            if (active)
//            {
//                int buffstacks = Modules.StaticValues.gearshift100BuffAttacks * num2;
//                base.characterBody.ApplyBuff(Modules.Buffs.gearshift100Buff.buffIndex, buffstacks);

//            }

//            if (base.isAuthority)
//            {
//                new SpendHealthNetworkRequest(characterBody.masterObjectId, Modules.StaticValues.gearshift100HealthCostFraction * characterBody.healthComponent.fullHealth).Send(NetworkDestination.Clients);
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