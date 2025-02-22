﻿using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;
using DekuMod.Modules.Survivors;
using static RoR2.BlastAttack;
using DekuMod.SkillStates.BaseStates;

namespace DekuMod.SkillStates
{
    public class StLouis100old : BaseDekuSkillState
    {
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");
        private GameObject effectPrefab2 = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/MageLightningBombExplosion");
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        public float baseDuration = 1f;
        public static float blastRadius = 15f;
        public static float succForce = 4.5f;
        //private GameObject effectPrefab = Modules.Asset.sEffect;

        public float range = 10f;
        public float rangeaddition = 15f;
        public float force = 1000f;
        private float duration;
        private float maxWeight;
        private BlastAttack blastAttack;
        //private bool hasFired;
        public Vector3 theSpot;
        public float whipage;
        public float speedattack;

        protected DamageType damageType = DamageType.Stun1s;
        private BlastAttack blastAttack2;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration / attackSpeedStat;
            speedattack = attackSpeedStat / 2;
            if (speedattack < 1)
            {
                speedattack = 1;
            }

            //EffectManager.SpawnEffect(Modules.Asset.impactEffect, new EffectData
            //{
            //    origin = base.transform.position,
            //    scale = 1f,
            //    rotation = Quaternion.LookRotation(aimRay.direction)
            //}, false);
            


            //hasFired = false;
            theSpot = aimRay.origin + range * aimRay.direction;
            AkSoundEngine.PostEvent(3709822086, this.gameObject);
            AkSoundEngine.PostEvent(3062535197, this.gameObject);
            base.StartAimMode(duration, true);

            base.characterMotor.disableAirControlUntilCollision = false;


            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            base.PlayCrossfade("FullBody, Override", "LegSmash", "Attack.playbackate", duration / 2, 0.1f);

            //EffectManager.SpawnEffect(Modules.Asset.blackwhip, new EffectData
            //{
            //    origin = theSpot,
            //    scale = 1f,       

            //}, true);

            blastAttack = new BlastAttack();
            blastAttack.radius = blastRadius * speedattack;
            blastAttack.procCoefficient = 0.2f;
            blastAttack.position = theSpot;
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            //blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.stlouisDamageCoefficient * fajin;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = force;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = damageType;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;


            //EffectData effectData = new EffectData();
            //effectData.origin = theSpot2;
            //effectData.scale = (blastRadius / 5) * this.attackSpeedStat;
            //effectData.rotation = Quaternion.LookRotation(new Vector3(aimRay.direction.x, aimRay.direction.y, aimRay.direction.z));

            //EffectManager.SpawnEffect(this.effectPrefab, effectData, false);

        }

        public void HandleHits(HitPoint[] hitPoints)
        {
        }
        protected virtual void OnHitEnemyAuthority()
        {
            base.healthComponent.Heal(((healthComponent.fullCombinedHealth / 20) * speedattack), default(ProcChainMask), true);

            //var hurtbox = blastAttack.inflictor;
            //if (hurtbox)
            //{

            //    Ray aimRay = base.GetAimRay();
            //    EffectManager.SpawnEffect(this.effectPrefab2, new EffectData
            //    {
            //        origin = hurtbox.transform.position,
            //        scale = blastRadius * speedattack * fajin,
            //        rotation = Util.QuaternionSafeLookRotation(aimRay.direction)

            //    }, true);

            //    blastAttack2 = new BlastAttack();
            //    blastAttack2.radius = blastRadius * speedattack * fajin;
            //    blastAttack2.procCoefficient = 0.2f;
            //    blastAttack2.position = hurtbox.transform.position;
            //    blastAttack2.attacker = base.gameObject;
            //    blastAttack2.crit = base.RollCrit();
            //    blastAttack2.baseDamage = Modules.StaticValues.stlouis100DamageCoefficient * this.damageStat;
            //    blastAttack2.falloffModel = BlastAttack.FalloffModel.None;
            //    blastAttack2.baseForce = force;
            //    blastAttack2.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            //    blastAttack2.damageType = damageType;
            //    blastAttack2.attackerFiltering = AttackerFiltering.Default;

            //    blastAttack2.Fire();

            //}
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Ray aimRay = base.GetAimRay();
            theSpot = aimRay.origin + range * aimRay.direction;

            if ((base.fixedAge >= this.duration / 10) && base.isAuthority && whipage >= this.duration / 10)
            {
                blastAttack.position = theSpot;
                range += rangeaddition;
                whipage = 0f;
                if (blastAttack.Fire().hitCount > 0)
                {
                    this.OnHitEnemyAuthority();
                }
                EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
                {
                    origin = theSpot,
                    scale = blastRadius * speedattack,
                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction)

                }, true);
                EffectManager.SpawnEffect(effectPrefab, new EffectData
                {
                    origin = theSpot,
                    scale = blastRadius * speedattack,
                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction)

                }, true);
                //for (int i = 0; i <= 5; i++)
                //{
                //    float num = 60f;
                //    Quaternion rotation = Util.QuaternionSafeLookRotation(base.characterDirection.forward.normalized);
                //    float num2 = 0.01f;
                //    rotation.x += UnityEngine.Random.Range(-num2, num2) * num;
                //    rotation.y += UnityEngine.Random.Range(-num2, num2) * num;
                //    EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
                //    {
                //        origin = base.transform.position,
                //        scale = blastRadius,
                //        rotation = rotation
                //    }, false);

                //}
            }
            else this.whipage += Time.fixedDeltaTime;


            if ((base.fixedAge >= this.duration && base.isAuthority))
            {
                this.outer.SetNextStateToMain();
                return;
            }


        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
