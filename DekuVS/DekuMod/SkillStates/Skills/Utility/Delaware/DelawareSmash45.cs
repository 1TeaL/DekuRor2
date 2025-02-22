﻿//using EntityStates;
//using EntityStates.VagrantMonster;
//using R2API.Networking;
//using RoR2;
//using System;
//using UnityEngine;
//using UnityEngine.Networking;

//namespace DekuMod.SkillStates
//{
//    public class DelawareSmash45: BaseDekuSkillState
//    {
//        public uint Distance = 50;
//        public static float damageCoefficient;
//        public float baseDuration = 1f;
//        private float duration;
//        public static event Action<int> Compacted;
//        //public static GameObject explosionPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/MageLightningBombExplosion");
//        public static GameObject explosionPrefab /*= Modules.Projectiles.delawareTracer*/;

//        public override void OnEnter() 
//        {
//            base.OnEnter();
//            Ray aimRay = base.GetAimRay();
//            this.duration = this.baseDuration/this.attackSpeedStat;
//            if (base.isAuthority)
//            {
//                AkSoundEngine.PostEvent("delawarevoice", this.gameObject);
//            }
//            AkSoundEngine.PostEvent("delawaresfx", this.gameObject);
//            base.StartAimMode(0.6f, true);

//            base.characterMotor.disableAirControlUntilCollision = false;


//            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);

//            base.PlayAnimation("FullBody, Override", "DelawareSmash45", "Attack.playbackRate", duration);

//            //if (NetworkServer.active) base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);

//            if (base.isAuthority)
//            {
//                Vector3 theSpot = aimRay.origin + 8 * aimRay.direction;
//                Vector3 theSpot2 = aimRay.origin + 2 * aimRay.direction;

//                BlastAttack blastAttack = new BlastAttack();
//                blastAttack.radius = 20f;
//                blastAttack.procCoefficient = 1f;
//                blastAttack.position = theSpot;
//                blastAttack.attacker = base.gameObject;
//                blastAttack.crit = base.RollCrit();
//                blastAttack.baseDamage = this.damageStat /** Modules.StaticValues.delaware45DamageCoefficient*/;
//                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
//                blastAttack.baseForce = 600f;
//                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
//                blastAttack.damageType = DamageType.Stun1s;
//                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

//                EffectData effectData = new EffectData();
//                {
//                effectData.scale = 15;
//                effectData.origin = theSpot2;
//                effectData.rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z));
//                };

//                EffectManager.SpawnEffect(explosionPrefab, effectData, true);

//                //base.characterMotor.velocity = -Distance * aimRay.direction * moveSpeedStat / 7;
//                BlastAttack.Result result = blastAttack.Fire();
//                if (result.hitCount > 0)
//                {
//                    this.OnHitEnemyAuthority(result);
//                }
//            }
//        }

//        protected virtual void OnHitEnemyAuthority(BlastAttack.Result result)
//        {
//            foreach (BlastAttack.HitPoint hitpoint in result.hitPoints)
//            {

//                if (!hitpoint.hurtBox.healthComponent.body.HasBuff(Modules.Buffs.barrierMark.buffIndex))
//                {
//                    hitpoint.hurtBox.healthComponent.body.ApplyBuff(Modules.Buffs.barrierMark.buffIndex, 1, -1);
//                }
//            }

//        }
//        public override void OnExit()
//        {

//            base.OnExit();
//        }

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();
//            if ((base.fixedAge >= this.duration && base.isAuthority) || (!base.IsKeyDownAuthority()))
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