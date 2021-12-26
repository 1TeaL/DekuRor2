using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;

namespace DekuMod.SkillStates
{
    public class Airforce : BaseSkillState
    {
        public static float procCoefficient = 0.5f;
        public static float baseDuration = 0.5f;
        public static float force = 300f;
        public static float recoil = 1f;
        public static float range = 200f;

        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerSmokeChase"); 
        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public float fajin;
        protected DamageType damageType;
        public DekuController dekucon;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Airforce.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "LFinger";

            base.PlayCrossfade("LeftArm, Override", "FingerFlick","Attack.playbackRate",this.duration, 0.3f);

            dekucon = base.GetComponent<DekuController>();
            if (dekucon.isMaxPower)
            {
                fajin = 2f;
            }
            else
            {
                fajin = 1f;
            }
        }

        public override void OnExit()
        {
            dekucon.RemoveBuffCount(50);
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(1f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                AkSoundEngine.PostEvent(1063047365, this.gameObject);



                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    base.AddRecoil(-1f * Airforce.recoil, -2f * Airforce.recoil, -0.5f * Airforce.recoil, 0.5f * Airforce.recoil);


                    if (dekucon.isMaxPower)
                    {
                        EffectManager.SpawnEffect(Modules.Assets.impactEffect, new EffectData
                        {
                            origin = FindModelChild(this.muzzleString).position,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(aimRay.direction)
                        }, false);
                        EffectManager.SpawnEffect(Modules.Projectiles.airforceTracer, new EffectData
                        {
                            origin = base.transform.position,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(aimRay.direction)

                        }, false);
                        damageType = DamageType.BypassArmor | DamageType.Stun1s;
                    }
                    else
                    {
                        damageType = DamageType.Generic;
                        EffectManager.SpawnEffect(Modules.Projectiles.airforceTracer, new EffectData
                        {
                            origin = FindModelChild(this.muzzleString).position,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(aimRay.direction)

                        }, false);
                    }

                    new BulletAttack
                    {
                        bulletCount = (uint)(2U * fajin),
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = Modules.StaticValues.airforceDamageCoefficient * this.damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = damageType,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        maxDistance = Airforce.range,
                        force = Airforce.force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = base.RollCrit(),
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = procCoefficient,
                        radius = 0.5f * fajin,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        //tracerEffectPrefab = Modules.Projectiles.bulletTracer,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,

                    }.Fire();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}