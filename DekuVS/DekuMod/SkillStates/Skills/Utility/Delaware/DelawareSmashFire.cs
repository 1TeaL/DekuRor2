﻿using EntityStates;
using EntityStates.VagrantMonster;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using DekuMod.Modules.Survivors;
using RoR2.UI;

namespace DekuMod.SkillStates
{
    public class DelawareSmashFire : BaseSkill
    {
        public float baseDuration = 0.5f;
        public float duration;
        private DamageType damageType;
        public HurtBox Target;
        private Animator animator;


        private float radius = 15f;
        private float damageCoefficient = Modules.StaticValues.detroitDamageCoefficient;
        private float procCoefficient = 1f;
        private float force = 1f;
        private float speedOverride = -1f;
        private float recoilAmplitude = 4f;
        private GameObject effectPrefab = Modules.Assets.banditmuzzleEffect;
        private string muzzleName = "RFinger";
        private float bulletCount = 1f;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            base.characterBody.SetAimTimer(this.duration);
            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            base.AddRecoil(-3f * recoilAmplitude, -4f * recoilAmplitude, -0.5f * recoilAmplitude, 0.5f * recoilAmplitude);

            //PlayCrossfade("RightArm, Override", "RightArmPunch", "Attack.playbackRate", duration, 0.1f);
            AkSoundEngine.PostEvent(3660048432, base.gameObject);
            damageType = DamageType.Shock5s;

            if (effectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(effectPrefab, base.gameObject, muzzleName, false);
            }
            if (base.isAuthority)
            {
                new BulletAttack
                {
                    bulletCount = (uint)bulletCount,
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    maxSpread = 0f,
                    force = force,
                    falloffModel = BulletAttack.FalloffModel.None,
                    tracerEffectPrefab = Modules.Assets.bandittracerEffectPrefab,
                    muzzleName = muzzleName,
                    hitEffectPrefab = Modules.Assets.banditimpactEffect,
                    isCrit = base.RollCrit(),
                    HitEffectNormal = true,
                    radius = 0.7f,
                    maxDistance = 500f,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    procCoefficient = 1f,
                    damage = damageCoefficient * this.damageStat,
                    damageType = damageType,
                    smartCollision = true
                }.Fire();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
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