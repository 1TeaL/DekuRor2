﻿//using RoR2;
//using UnityEngine;
//using UnityEngine.Networking;
//using EntityStates;
//using System.Collections.Generic;
//using System.Linq;
//using DekuMod.Modules.Survivors;
//using static RoR2.BlastAttack;
//using DekuMod.Modules.Networking;
//using R2API.Networking.Interfaces;
//using R2API.Networking;
//using System;

//namespace DekuMod.SkillStates
//{
//    public class StLouis45 : BaseDekuSkillState
//    {
//        public float baseDuration = 1f;
//        public static float blastRadius = 10f;
//        public static float succForce = 4.5f;
//        //private GameObject effectPrefab = Modules.Asset.sEffect;

//        public float range = 10f;
//        public float rangeaddition = 15f;
//        public float force = 200f;
//        private float duration;
//        private float maxWeight;
//        private BlastAttack blastAttack;
//        //private bool hasFired;
//        public Vector3 theSpot;
//        public float whipage;
//        public float speedattack;

//        protected DamageType damageType = DamageType.Stun1s;
//        private BlastAttack blastAttack2;

//        public override void OnEnter()
//        {
//            base.OnEnter();
//            Ray aimRay = base.GetAimRay();
//            this.duration = this.baseDuration / attackSpeedStat;
//            speedattack = attackSpeedStat / 2;
//            if (speedattack < 1)
//            {
//                speedattack = 1;
//            }

//            float num = this.moveSpeedStat;
//            bool isSprinting = base.characterBody.isSprinting;
//            if (isSprinting)
//            {
//                num /= base.characterBody.sprintingSpeedMultiplier;
//            }
//            float num2 = (num / base.characterBody.baseMoveSpeed - 1f) * 0.67f;
//            float num3 = num2 + 1f;


//            //hasFired = false;
//            theSpot = aimRay.origin + range * aimRay.direction;
//            if (base.isAuthority)
//            {
//                AkSoundEngine.PostEvent("stlouisvoice", this.gameObject);
//            }
//            AkSoundEngine.PostEvent("stlouissfx", this.gameObject);
//            base.StartAimMode(duration, true);

//            base.characterMotor.disableAirControlUntilCollision = false;


//            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
//            PlayCrossfade("FullBody, Override", "StLouis45", "Attack.playbackRate", duration/2, 0.01f);
//            EffectManager.SimpleMuzzleFlash(Modules.Asset.dekuKickEffect, base.gameObject, "Swing1", true);


//            blastAttack = new BlastAttack();
//            blastAttack.radius = blastRadius * num3;
//            blastAttack.procCoefficient = 0.2f;
//            blastAttack.position = theSpot;
//            blastAttack.damageType = DamageType.Stun1s;
//            blastAttack.attacker = base.gameObject;
//            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
//            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.stlouis45DamageCoefficient * num3;
//            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
//            blastAttack.baseForce = force;
//            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
//            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

//        }

//        protected virtual void OnHitEnemyAuthority()
//        {
//            //base.healthComponent.Heal(((healthComponent.fullCombinedHealth / 20) * speedattack), default(ProcChainMask), true);

//        }

//        public override void OnExit()
//        {
//            base.OnExit();
//        }

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();
//            Ray aimRay = base.GetAimRay();
//            theSpot = aimRay.origin + range * aimRay.direction;

//            if ((base.fixedAge >= this.duration / 10) && base.isAuthority && whipage >= this.duration / 10)
//            {
//                blastAttack.position = theSpot;
//                range += rangeaddition;
//                whipage = 0f;
//                if (blastAttack.Fire().hitCount > 0)
//                {
//                    this.OnHitEnemyAuthority();
//                }
//                EffectManager.SpawnEffect(Modules.Asset.lightningNovaEffectPrefab, new EffectData
//                {
//                    origin = theSpot,
//                    scale = blastRadius,
//                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction)

//                }, true);
//                EffectManager.SpawnEffect(Modules.Asset.sonicboomEffectPrefab, new EffectData
//                {
//                    origin = theSpot,
//                    scale = blastRadius,
//                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction)

//                }, true);
//                //for (int i = 0; i <= 5; i++)
//                //{
//                //    float num = 60f;
//                //    Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
//                //    float num2 = 0.01f;
//                //    rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
//                //    rotation.y += UnityEngine.Random.Range(-num2, num2) * num;
//                //    EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
//                //    {
//                //        origin = base.transform.position,
//                //        scale = blastRadius,
//                //        rotation = rotation
//                //    }, false);

//                //}
//            }
//            else this.whipage += Time.fixedDeltaTime;


//            if ((base.fixedAge >= this.duration && base.isAuthority))
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
