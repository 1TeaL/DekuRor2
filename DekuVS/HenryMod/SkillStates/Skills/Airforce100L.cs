using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using DekuMod.SkillStates.Orbs;
using System.Collections.Generic;
using RoR2.Orbs;

namespace DekuMod.SkillStates
{
    public class Airforce100L : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        public static float force = 300f;
        public static float recoil = 0.5f;
        public static float range = 200f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        protected DamageType damageType = DamageType.Stun1s;
        private BulletAttack bulletAttack;
        private BlastAttack blastAttack;
        public float blastRadius = 5f;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Airforce100L.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.5f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "LFinger";

            base.PlayCrossfade("Gesture, Override", "DekurapidpunchL", "Attack.playbackRate",this.fireTime, 0.1f);


        }

        private bool ApplyBlastAttackOnHit(ref BulletAttack.BulletHit hitInfo)
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
                        EffectManager.SpawnEffect(Modules.Assets.airforce100impactEffect, new EffectData
                        {
                            origin = healthComponent.body.corePosition,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(aimRay.direction)

                        }, true);

                        blastAttack = new BlastAttack();
                        blastAttack.radius = blastRadius;
                        blastAttack.procCoefficient = 0.2f;
                        blastAttack.position = healthComponent.body.corePosition;
                        blastAttack.attacker = base.gameObject;
                        blastAttack.crit = base.RollCrit();
                        blastAttack.baseDamage = Modules.StaticValues.airforce100DamageCoefficient * this.damageStat;
                        blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                        blastAttack.baseForce = Airforce100L.force;
                        blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                        blastAttack.damageType = damageType;
                        blastAttack.attackerFiltering = AttackerFiltering.Default;

                        blastAttack.Fire();
                    }
                }
            }
            return false;
        }

        public override void OnExit()
        {

            base.PlayCrossfade("Gesture, Override", "BufferEmpty", "Attack.playbackRate", this.duration, 0.1f);
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
                    base.AddRecoil(-1f * Airforce100L.recoil, -2f * Airforce100L.recoil, -0.5f * Airforce100L.recoil, 0.5f * Airforce100L.recoil);

                    EffectManager.SpawnEffect(Modules.Projectiles.airforce100Tracer, new EffectData
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
                    bulletAttack.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
                    bulletAttack.maxDistance = Airforce100L.range;
                    bulletAttack.force = Airforce100L.force;
                    bulletAttack.hitMask = LayerIndex.CommonMasks.bullet;
                    bulletAttack.minSpread = 0f;
                    bulletAttack.maxSpread = 0f;
                    bulletAttack.isCrit = base.RollCrit();
                    bulletAttack.owner = base.gameObject;
                    bulletAttack.muzzleName = muzzleString;
                    bulletAttack.smartCollision = false;
                    bulletAttack.procChainMask = default(ProcChainMask);
                    bulletAttack.procCoefficient = procCoefficient;
                    bulletAttack.radius = 0.5f;
                    bulletAttack.sniper = false;
                    bulletAttack.stopperMask = LayerIndex.CommonMasks.bullet;
                    bulletAttack.weapon = null;
                    //tracerEffectPrefab = Modules.Projectiles.bulletTracer,
                    bulletAttack.spreadPitchScale = 0f;
                    bulletAttack.spreadYawScale = 0f;
                    bulletAttack.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
                    bulletAttack.hitEffectPrefab = Modules.Assets.airforce100impactEffect;
                    bulletAttack.hitCallback = ApplyBlastAttackOnHit;

                    bulletAttack.Fire();


                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                Fire();
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