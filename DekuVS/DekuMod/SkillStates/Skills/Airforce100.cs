using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using DekuMod.SkillStates.Orbs;
using System.Collections.Generic;
using RoR2.Orbs;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{
    public class Airforce100 : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.4f;
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
        public int punchIndex;
        public int actualshotsFired;
        public int shotsFired = 1;

        public override void OnEnter()
        {
            base.OnEnter();
            if(shotsFired > 10)
            {
                shotsFired = 10;
            }
            this.duration = Airforce100.baseDuration / (this.attackSpeedStat * ((float)shotsFired/5));
            this.fireTime = 0.5f * this.duration;
            base.characterBody.SetAimTimer(duration);
            this.muzzleString = punchIndex % 2 == 0 ? "LFinger" : "RFinger";

            //base.PlayCrossfade("LeftArm, Override", punchIndex % 2 == 0 ? "DekurapidpunchL" : "DekurapidpunchR", this.duration);
            //base.PlayCrossfade("RightArm, Override", punchIndex % 2 == 0 ? "DekurapidpunchL" : "DekurapidpunchR", this.duration);

            //base.PlayCrossfade("Gesture, Override", punchIndex % 2 == 0 ? "DekurapidpunchL" : "DekurapidpunchR", "Attack.playbackRate", this.duration, this.fireTime);

            base.PlayCrossfade("FullBody, Override", punchIndex % 2 == 0 ? "BlackwhipLeft" : "Blackwhip", "Attack.playbackRate", duration, this.fireTime/3);
            //base.PlayCrossfade("LeftArm, Override", punchIndex % 2 == 0 ? "BlackwipLeft" : "BufferEmpty", "Attack.playbackRate", this.duration, this.fireTime / 3);
            //base.PlayCrossfade("RightArm, Override", punchIndex % 2 == 0 ? "BufferEmpty" : "Blackwhip", "Attack.playbackRate", this.duration, this.fireTime / 3);


            if (NetworkServer.active && base.healthComponent)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.01f;
                damageInfo.position = base.transform.position;
                damageInfo.force = Vector3.zero;
                damageInfo.damageColorIndex = DamageColorIndex.Default;
                damageInfo.crit = false;
                damageInfo.attacker = null;
                damageInfo.inflictor = null;
                damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
                damageInfo.procCoefficient = 0f;
                damageInfo.procChainMask = default(ProcChainMask);
                base.healthComponent.TakeDamage(damageInfo);
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
                        blastAttack.baseForce = Airforce100.force;
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
            base.PlayCrossfade("RightArm, Override", "BufferEmpty", this.fireTime/3);
            base.PlayCrossfade("LeftArm, Override", "BufferEmpty", this.fireTime / 3);
            //base.PlayCrossfade("Gesture, Override", "BufferEmpty", this.fireTime / 3);
            //base.PlayCrossfade("LeftArm, Override", "AirforceReset", 0.1f);
            //base.PlayCrossfade("RightArm, Override", "AirforceReset", 0.1f);
            //base.PlayCrossfade("Fullbody, Override", "AirforceReset", 0.1f);
            //base.PlayCrossfade("Fullbody, Override", "BufferEmpty", 0.1f);
            //base.PlayAnimation("Fullbody, Override", "Armature_AIdle", "Attack.playbackRate", 0.1f);
            //base.PlayAnimation("Body", "IdleIn");
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
                    base.AddRecoil(-1f * Airforce100.recoil, -2f * Airforce100.recoil, -0.5f * Airforce100.recoil, 0.5f * Airforce100.recoil);

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
                    bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
                    bulletAttack.maxDistance = Airforce100.range;
                    bulletAttack.force = Airforce100.force;
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

        protected void SetNextState()
        {
            int index = this.punchIndex;
            if (index == 0) index = 1;
            else index = 0;
            int actualshotsFired = shotsFired + 1;

            this.outer.SetNextState(new Airforce100
            {
                punchIndex = index,
                shotsFired = actualshotsFired,
            });

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
                if (inputBank.skill1.down)
                {
                    this.SetNextState();
                    return;
                }
                else
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}