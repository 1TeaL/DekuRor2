using EntityStates;
using EntityStates.VagrantMonster;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using DekuMod.Modules.Survivors;
using RoR2.UI;
using DekuMod.Modules;
using static UnityEngine.ParticleSystem.PlaybackState;
using DekuMod.SkillStates.BaseStates;

namespace DekuMod.SkillStates.Might
{
    public class DelawareSmash : BaseDekuSkillState
    {
        private GameObject effectPrefab = Modules.Asset.banditmuzzleEffect;
        public float baseChargeThreshold = StaticValues.delawareChargeThreshold;
        private string muzzleName = "LFinger";

        public float chargeThreshold;
        public bool hasFired;
        public Animator anim;
        public bool consumeStock = false;

        private float recoilAmplitude = 4f;
        private float force;
        private float radius;
        private float damage;
        private float range;
        private DamageType damageType;

        public override void OnEnter()
        {
            base.OnEnter();
            anim = base.GetModelAnimator();
            anim.SetBool("delawareCharged", false);
            hasFired = false;
            chargeThreshold = baseChargeThreshold / attackSpeedStat;
            Ray aimRay = GetAimRay();
            characterBody.SetAimTimer(1f);
            GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            PlayCrossfade("FullBody, Override", "DelawareSmashBaseCharge", "Attack.playbackRate", 1f, 0.01f);

            damage = StaticValues.delawareDamageCoefficient * damageStat;
            force = 500f;
            radius = 1f;
            range = 20f;
            damageType = DamageType.Generic;

            switch (level)
            {
                case 0:
                    break;
                case 1:
                    chargeThreshold *= StaticValues.delawareCharge1;
                    break;
                case 2:
                    chargeThreshold *= StaticValues.delawareCharge2;
                    break;
            }

            //PlayAnimation("RightArm, Override", "RightArmOut", "Attack.playbackRate", 1f);

        }

        public override void OnExit()
        {
            base.OnExit();
            anim.SetBool("delawareCharged", true);
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsKeyDownAuthority())
            {
                //PlayCrossfade("FullBody, Override", "DelawareSmashBaseCharge", "Attack.playbackRate", 1f, 0.01f);
                //PlayAnimation("RightArm, Override", "RightArmOut", "Attack.playbackRate", duration);

                switch (level)
                {
                    case 0:

                        if (fixedAge >= chargeThreshold && base.skillLocator.secondary.stock >= 1)
                        {      
                            if (!consumeStock)
                            {
                                //take extra stock 
                                base.skillLocator.secondary.DeductStock(1);
                                consumeStock = true;
                            }
                        }
                        break;
                    case 1:

                        if (fixedAge >= chargeThreshold && base.skillLocator.secondary.stock >= 1)
                        {
                            if (!consumeStock)
                            {
                                //take extra stock 
                                base.skillLocator.secondary.DeductStock(1);
                                consumeStock = true;
                            }
                        }
                        break;
                    case 2:

                        if (fixedAge >= chargeThreshold)
                        {
                            //no need stock consumption
                            
                        }
                        break;
                }
            }
            else
            {
                if(!IsKeyDownAuthority())
                {
                    if(!hasFired)
                    {
                        anim.SetBool("delawareCharged", true);
                        hasFired = true;
                        if (fixedAge <= chargeThreshold && isAuthority)
                        {
                            switch (level)
                            {
                                case 0:

                                    Fire();
                                    break;
                                case 1:
                                    damage = damageStat * StaticValues.delawareDamageCoefficient1;
                                    Fire();
                                    Fire();
                                    break;
                                case 2:
                                    damageType = DamageType.Stun1s;
                                    damage = damageStat * StaticValues.delawareDamageCoefficient2;
                                    radius = 10f;
                                    Fire();
                                    break;
                            }
                        }
                        else if (fixedAge >= chargeThreshold && isAuthority)
                        {
                            float angle = Vector3.Angle(new Vector3(0, -1, 0), base.GetAimRay().direction);
                            if (angle < 60)
                            {
                                base.PlayAnimation("FullBody, Override", "DelawareSmashUp");
                            }
                            else if (angle > 120)
                            {
                                base.PlayAnimation("FullBody, Override", "DelawareSmashDown");
                            }
                            else
                            {
                                base.PlayAnimation("FullBody, Override", "DelawareSmash");
                            }
                            force = 4000f;
                            range = 40f;
                            damageType = DamageType.Stun1s;

                            if (isAuthority)
                            {
                                AkSoundEngine.PostEvent("delawarevoice", gameObject);
                            }

                            AkSoundEngine.PostEvent("delawaresfx", gameObject);

                            switch (level)
                            {
                                case 0:
                                    damage *= StaticValues.delawareChargeMultiplier;
                                    Fire();
                                    break;
                                case 1:
                                    Fire();
                                    Fire();
                                    Fire();
                                    Fire();
                                    Fire();
                                    Fire();
                                    break;
                                case 2:
                                    radius = 20f;
                                    damage *= StaticValues.delawareChargeMultiplier;
                                    Fire();
                                    break;
                            }
                            base.characterMotor.velocity = StaticValues.stlouisDistance2 * (-base.GetAimRay().direction)* moveSpeedStat;
                        }
                        this.outer.SetNextStateToMain();
                    }

                }
            }
            if (characterBody)
            {
                characterBody.SetAimTimer(1f);
            }
        }

        public void Fire()
        {
            //AddRecoil(-3f * recoilAmplitude, -4f * recoilAmplitude, -0.5f * recoilAmplitude, 0.5f * recoilAmplitude);
                      


            if (effectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(effectPrefab, gameObject, muzzleName, false);
            }

            if (isAuthority)
            {

                BulletAttack bulletAttack = new BulletAttack
                {

                    bulletCount = 1,
                    owner = gameObject,
                    weapon = gameObject,
                    origin = base.GetAimRay().origin,
                    aimVector = base.GetAimRay().direction,
                    minSpread = 0f,
                    maxSpread = 0f,
                    force = force,
                    falloffModel = BulletAttack.FalloffModel.None,
                    //tracerEffectPrefab = Modules.Asset.bandittracerEffectPrefab,
                    muzzleName = muzzleName,
                    hitEffectPrefab = Modules.Asset.banditimpactEffect,
                    isCrit = RollCrit(),
                    HitEffectNormal = true,
                    radius = radius,
                    maxDistance = range,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    stopperMask = LayerIndex.world.mask,
                    procCoefficient = 1f,
                    damage = damage,
                    damageType = damageType,
                    smartCollision = true,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                };

                switch (level)
                {
                    case 0:
                        EffectManager.SpawnEffect(Modules.Asset.delawareBullet, new EffectData
                        {
                            origin = FindModelChild(muzzleName).position,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                        }, true);
                        break;
                    case 1:
                        EffectManager.SpawnEffect(Modules.Asset.delawareBullet, new EffectData
                        {
                            origin = FindModelChild(muzzleName).position,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                        }, true);
                        break;
                    case 2:
                        //bulletAttack.tracerEffectPrefab = Assets.delawareBullet;

                        EffectManager.SpawnEffect(Modules.Asset.delawareEffect, new EffectData
                        {
                            origin = FindModelChild(muzzleName).position,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(base.GetAimRay().direction)

                        }, true);
                        break;
                }

                bulletAttack.Fire();

            }
        }

        private void AddRecoil(object value1, object value2, object value3, object value4)
        {
            throw new NotImplementedException();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}