using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using DekuMod.SkillStates.Orbs;
using System.Collections.Generic;
using RoR2.Orbs;
using static RoR2.BulletAttack;
using DekuMod.SkillStates.BaseStates;

namespace DekuMod.SkillStates.ShootStyle
{
    public class Airforce : BaseDekuSkillState
    {
        public static float procCoefficient = 0.5f;
        public static float baseDuration = 0.5f;
        public static float force = 300f;
        public static float recoil = 1f;
        public static float range = 200f;

        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/tracerhuntresssnipe");
        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public DamageType damageType = DamageType.Generic;
        public static int maxRicochetCount = Modules.StaticValues.airforceMaxRicochet;
        public static bool resetBouncedObjects = true;


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = 0.2f * duration;
            characterBody.SetAimTimer(duration);
            muzzleString = "LFinger";

            hasFired = false;

            GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            //base.PlayCrossfade("LeftArm, Override", "FingerFlick","Attack.playbackRate",this.duration, this.fireTime);
            PlayAnimation("LeftArm, Override", "FingerFlick", "Attack.playbackRate", duration);
            //PlayAnimation("FullBody, Override", "GoBeyond", "Attack.playbackRate", duration);


        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            characterBody.AddSpreadBloom(1f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
            if (isAuthority)
            {
                AkSoundEngine.PostEvent("airforcesfx", gameObject);
            }

            if (isAuthority)
            {
                Ray aimRay = GetAimRay();
                AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);


                //EffectManager.SpawnEffect(Modules.Projectiles.airforceTracer, new EffectData
                //{
                //    origin = FindModelChild(this.muzzleString).position,
                //    scale = 1f,
                //    rotation = Quaternion.LookRotation(aimRay.direction)

                //}, true);


                bool hasHit = false;
                Vector3 hitPoint = Vector3.zero;
                float hitDistance = 0f;
                HealthComponent hitHealthComponent = null;

                var bulletAttack = new BulletAttack
                {
                    bulletCount = 2U,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = Modules.StaticValues.airforceDamageCoefficient * damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = damageType,
                    falloffModel = FalloffModel.DefaultBullet,
                    maxDistance = range,
                    force = force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = RollCrit(),
                    owner = gameObject,
                    muzzleName = muzzleString,
                    smartCollision = false,
                    procChainMask = default,
                    procCoefficient = procCoefficient,
                    radius = 0.5f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    //tracerEffectPrefab = Modules.Projectiles.bulletTracer,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,

                };
                if (maxRicochetCount > 0 && bulletAttack.isCrit)
                {
                    bulletAttack.hitCallback = delegate (BulletAttack bulletAttackRef, ref BulletHit hitInfo)
                    {
                        var result = defaultHitCallback(bulletAttackRef, ref hitInfo);
                        if (hitInfo.hitHurtBox)
                        {
                            hasHit = true;
                            hitPoint = hitInfo.point;
                            hitDistance = hitInfo.distance;

                            hitHealthComponent = hitInfo.hitHurtBox.healthComponent;
                            //hitHealthComponent.body.AddBuff();

                        }
                        return result;
                    };
                }
                bulletAttack.filterCallback = delegate (BulletAttack bulletAttackRef, ref BulletHit info)
                {
                    return (!info.entityObject || info.entityObject != bulletAttack.owner) && defaultFilterCallback(bulletAttackRef, ref info);
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
                        critRicochetOrb.isCrit = RollCrit();
                        critRicochetOrb.teamIndex = TeamComponent.GetObjectTeam(gameObject);
                        critRicochetOrb.damageType = bulletAttack.damageType;
                        critRicochetOrb.attacker = gameObject;
                        critRicochetOrb.attackerBody = characterBody;
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

            if (fixedAge >= fireTime && !hasFired)
            {
                hasFired = true;
                Fire();
            }


            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}