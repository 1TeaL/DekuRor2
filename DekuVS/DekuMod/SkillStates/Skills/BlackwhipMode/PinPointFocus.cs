using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using DekuMod.SkillStates.Orbs;
using System.Collections.Generic;
using RoR2.Orbs;
using static RoR2.BulletAttack;
using DekuMod.Modules;
using R2API;
using DekuMod.SkillStates.BaseStates;

namespace DekuMod.SkillStates.BlackWhip
{
    public class PinPointFocus : BaseDekuSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = StaticValues.pinpointDuration;
        public static float force = 0f;
        public static float recoil = 1f;
        public static float range = StaticValues.pinpointRange;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public DamageType damageType = DamageType.Generic;
        private BulletAttack bulletAttack;
        private BlastAttack blastAttack;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = 0.2f * duration;
            characterBody.SetAimTimer(duration);
            muzzleString = "RFinger";

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
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
            if (isAuthority)
            {
                AkSoundEngine.PostEvent("airforcesfx", gameObject);
            }

            if (isAuthority)
            {
                Ray aimRay = GetAimRay();


                EffectManager.SpawnEffect(Modules.DekuAssets.delawareBullet, new EffectData
                {
                    origin = FindModelChild(this.muzzleString).position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(aimRay.direction)

                }, true);


                bulletAttack = new BulletAttack();
                bulletAttack.bulletCount = (uint)(1U);
                bulletAttack.aimVector = aimRay.direction;
                bulletAttack.origin = aimRay.origin;
                bulletAttack.damage = Modules.StaticValues.airforce100DamageCoefficient * this.damageStat;
                bulletAttack.damageColorIndex = DamageColorIndex.Default;
                bulletAttack.damageType = damageType;
                bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
                bulletAttack.maxDistance = range;
                bulletAttack.force = force;
                bulletAttack.hitMask = LayerIndex.CommonMasks.bullet;
                bulletAttack.minSpread = 0f;
                bulletAttack.maxSpread = 0f;
                bulletAttack.isCrit = base.RollCrit();
                bulletAttack.owner = base.gameObject;
                bulletAttack.muzzleName = muzzleString;
                bulletAttack.smartCollision = true;
                bulletAttack.procChainMask = default(ProcChainMask);
                bulletAttack.procCoefficient = procCoefficient;
                bulletAttack.radius = 2f;
                bulletAttack.sniper = false;
                bulletAttack.stopperMask = 0;//pierce everything
                bulletAttack.weapon = null;
                //tracerEffectPrefab = Modules.Projectiles.bulletTracer,
                bulletAttack.spreadPitchScale = 0f;
                bulletAttack.spreadYawScale = 0f;
                bulletAttack.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
                bulletAttack.hitEffectPrefab = Modules.DekuAssets.airforce100impactEffect;
                bulletAttack.hitCallback = ApplyBlastAttackOnHit;

                DamageAPI.AddModdedDamageType(bulletAttack, Damage.blackwhipImmobilise);
                bulletAttack.Fire();

            }

        }

        private bool ApplyBlastAttackOnHit(BulletAttack bulletAttackRef, ref BulletAttack.BulletHit hitInfo)
        {

            var hurtbox = hitInfo.hitHurtBox;
            if (hurtbox)
            {
                var healthComponent = hurtbox.healthComponent;
                if (healthComponent)
                {
                    var body = healthComponent.body;
                    if (body)
                    {
                        Ray aimRay = base.GetAimRay();
                        EffectManager.SpawnEffect(Modules.DekuAssets.airforce100impactEffect, new EffectData
                        {
                            origin = healthComponent.body.corePosition,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(aimRay.direction)

                        }, true);

                        blastAttack = new BlastAttack();
                        blastAttack.radius = StaticValues.pinpointRadius;
                        blastAttack.procCoefficient = procCoefficient;
                        blastAttack.position = healthComponent.body.corePosition;
                        blastAttack.attacker = base.gameObject;
                        blastAttack.crit = base.RollCrit();
                        blastAttack.baseDamage = Modules.StaticValues.pinpointDamageCoefficient * this.damageStat;
                        blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                        blastAttack.baseForce = force;
                        blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                        blastAttack.damageType = damageType;
                        blastAttack.attackerFiltering = AttackerFiltering.Default;

                        DamageAPI.AddModdedDamageType(blastAttack, Damage.blackwhipImmobilise);

                        blastAttack.Fire();
                    }
                }
            }
            return false;
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