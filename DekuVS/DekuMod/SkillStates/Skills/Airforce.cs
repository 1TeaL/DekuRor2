using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using DekuMod.SkillStates.Orbs;
using System.Collections.Generic;
using RoR2.Orbs;
using static RoR2.BulletAttack;

namespace DekuMod.SkillStates
{
    public class Airforce : BaseSkillState
    {
        public static float procCoefficient = 0.5f;
        public static float baseDuration = 0.5f;
        public static float force = 300f;
        public static float recoil = 1f;
        public static float range = 200f;

        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/tracerhuntresssnipe"); 
        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public float fajin;
        protected DamageType damageType;
        public DekuController dekucon;
        public static int maxRicochetCount = 8;
        public static bool resetBouncedObjects = true;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Airforce.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(this.duration);
            this.muzzleString = "LFinger";

            hasFired = false;

            //base.PlayCrossfade("LeftArm, Override", "FingerFlick","Attack.playbackRate",this.duration, this.fireTime);
            base.PlayAnimation("LeftArm, Override", "FingerFlick", "Attack.playbackRate", this.duration);

            dekucon = base.GetComponent<DekuController>();
            if (dekucon.isMaxPower)
            {
                fajin = 2f;
                dekucon.RemoveBuffCount(50);
            }
            else
            {
                fajin = 1f;
            }

            dekucon.AddToBuffCount(10);

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
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
                    }, true);
                    EffectManager.SpawnEffect(Modules.Projectiles.airforceTracer, new EffectData
                    {
                        origin = base.transform.position,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(aimRay.direction)

                    }, true);
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

                    }, true);
                }

                bool hasHit = false;
                Vector3 hitPoint = Vector3.zero;
                float hitDistance = 0f;
                HealthComponent hitHealthComponent = null;

                var bulletAttack = new BulletAttack
                {
                    bulletCount = (uint)(2U),
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

                };
                if (maxRicochetCount > 0 && dekucon.isMaxPower)
                {
                    bulletAttack.hitCallback = delegate (BulletAttack bulletAttackRef, ref BulletHit hitInfo)
                    {
                        var result = BulletAttack.defaultHitCallback(bulletAttackRef, ref hitInfo);
                        if (hitInfo.hitHurtBox)
                        hasHit = true;
                        hitPoint = hitInfo.point;
                        hitDistance = hitInfo.distance;
                        {
                            hitHealthComponent = hitInfo.hitHurtBox.healthComponent;
                        }
                        return result;
                    };
                }
                bulletAttack.filterCallback = delegate (BulletAttack bulletAttackRef, ref BulletAttack.BulletHit info)
                {
                    return (!info.entityObject || info.entityObject != bulletAttack.owner) && BulletAttack.defaultFilterCallback(bulletAttackRef, ref info);
                };
                bulletAttack.Fire();
                if (hasHit)
                {
                    if (hitHealthComponent != null)
                    {
                        CritRicochetOrb critRicochetOrb = new CritRicochetOrb();
                        critRicochetOrb.bouncesRemaining = maxRicochetCount - 1;
                        critRicochetOrb.resetBouncedObjects = resetBouncedObjects;
                        critRicochetOrb.damageValue = bulletAttack.damage;
                        critRicochetOrb.isCrit = base.RollCrit();
                        critRicochetOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                        critRicochetOrb.damageType = bulletAttack.damageType;
                        critRicochetOrb.attacker = base.gameObject;
                        critRicochetOrb.attackerBody = base.characterBody;
                        critRicochetOrb.procCoefficient = bulletAttack.procCoefficient;
                        critRicochetOrb.duration = 0.2f;
                        critRicochetOrb.bouncedObjects = new List<HealthComponent>();
                        critRicochetOrb.range = Mathf.Max(30f, hitDistance);
                        critRicochetOrb.tracerEffectPrefab = tracerEffectPrefab;
                        critRicochetOrb.hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.hitEffectPrefab;
                        critRicochetOrb.origin = hitPoint;
                        critRicochetOrb.bouncedObjects.Add(hitHealthComponent);
                        var nextTarget = critRicochetOrb.PickNextTarget(hitPoint);
                        if (nextTarget)
                        {
                            critRicochetOrb.target = nextTarget;
                            OrbManager.instance.AddOrb(critRicochetOrb);
                        }
                    }
                }

            }
            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate(); 

            if (base.fixedAge >= this.fireTime && !this.hasFired)
            {
                if (dekucon.isMaxPower)
                {
                    hasFired = true;
                    this.Fire();
                    this.Fire();
                    this.Fire();

                }
                else
                {
                    hasFired = true;
                    this.Fire();
                }
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