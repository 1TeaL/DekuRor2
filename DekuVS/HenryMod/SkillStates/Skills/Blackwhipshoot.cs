using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using UnityEngine.Networking;
using System;
using DekuMod.Modules.Survivors;
using RoR2.Audio;

namespace DekuMod.SkillStates
{
    public class BlackwhipShoot : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        public static float throwForce = 150f;
        public static float radius = 5f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private Animator animator;
        //private ProjectileImpactEventCaller impactEventCaller;

        //private Vector3 moveVec;

        private string muzzleString;
        public float fajin;
        public DekuController dekucon;
        private float rollSpeed;
        public static float baseSpeedCoefficient = 20f;
        public static float SpeedCoefficient;
        private float waitReturnTimer;
        public static float waitReturnDuration = 0.3f;
        public static float holdTime = 0.3f;
        private float previousMass;
        private BlastAttack blastAttack;
        private bool hasActivated;

        protected NetworkSoundEventIndex impactSound;
        private OverlapAttack attack;
        protected string hitboxName;
        public DamageType damageType;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = BlackwhipShoot.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.35f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            dekucon = base.GetComponent<DekuController>();
            base.characterMotor.useGravity = false;
            this.previousMass = base.characterMotor.mass;
            base.characterMotor.mass = 0f;
            hasActivated = false;

            Ray aimRay = base.GetAimRay();

            if (dekucon.isMaxPower)
            {
                hitboxName = "BigModelHitbox";
                fajin = 2f;
                SpeedCoefficient = baseSpeedCoefficient * 2;
                damageType = DamageType.Stun1s | DamageType.BypassArmor;
            }
            else
            {
                hitboxName = "BodyHitbox";
                fajin = 1f;
                SpeedCoefficient = baseSpeedCoefficient;
                damageType = DamageType.SlowOnHit;
            }
            base.PlayAnimation("RightArm, Override", "Blackwhip", "attack.playbackRate", duration);

            this.muzzleString = "RHand";
            if (dekucon.isMaxPower)
            {
                EffectManager.SpawnEffect(Modules.Assets.impactEffect, new EffectData
                {
                    origin = FindModelChild(this.muzzleString).position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(aimRay.direction)
                }, false);
            }
            //if (NetworkServer.active)
            //{                
            //    impactEventCaller = Modules.Projectiles.blackwhipPrefab.GetComponent<ProjectileImpactEventCaller>();
            //    if ((bool)impactEventCaller)
            //    {
            //        Debug.Log("listen");
            //        impactEventCaller.impactEvent.AddListener(OnImpact);
            //    }
            //}

            //this.RecalculateRollSpeed();


            HitBoxGroup hitBoxGroup = Array.Find<HitBoxGroup>(base.GetModelTransform().GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
            this.attack = this.CreateAttack(hitBoxGroup);
        }

        private void RecalculateRollSpeed()
        {
            this.rollSpeed = this.moveSpeedStat * SpeedCoefficient * (this.attackSpeedStat/2);
        }


        //public void OnImpact(ProjectileImpactInfo impactInfo)
        //{
        //    Debug.Log("impact");
        //    Ray aimRay = base.GetAimRay();
        //    Vector3 direction = aimRay.direction;
        //    Vector3 impact = impactInfo.estimatedPointOfImpact;
        //    base.characterMotor.velocity = Vector3.zero;
        //    this.moveVec = 30f * impact.normalized;
        //    base.characterMotor.rootMotion += this.moveVec;
        //    base.characterMotor.velocity += this.moveVec * 2;
        //}



        public override void OnExit()
        {
            dekucon.RemoveBuffCount(50);
            base.characterMotor.mass = this.previousMass;
            base.characterMotor.useGravity = true;
            //base.characterMotor.velocity = Vector3.zero;
            base.PlayCrossfade("RightArm, Override", "BufferEmpty", 0f);
            base.OnExit();
        }
        protected OverlapAttack CreateAttack(HitBoxGroup hitBoxGroup)
        {
            return new OverlapAttack
            {
                damageType = damageType,
                attacker = base.gameObject,
                inflictor = base.gameObject,
                teamIndex = base.GetTeam(),
                damage = Modules.StaticValues.blackwhipshootDamageCoefficient * this.damageStat,
                procCoefficient = 1f,
                hitEffectPrefab = Modules.Assets.detroitweakEffect,
                forceVector = Vector3.zero,
                pushAwayForce = 1000f,
                hitBoxGroup = hitBoxGroup,
                isCrit = base.RollCrit(),
                impactSound = this.impactSound
            };
        }
        //private void Fire()
        //{
        //    if (!this.hasFired)
        //    {
        //        this.hasFired = true;

        //        if (base.isAuthority)
        //        {
        //            Ray aimRay = base.GetAimRay();
        //            if(base.age < holdTime)
        //            {
        //                ProjectileManager.instance.FireProjectile(Modules.Projectiles.blackwhipPrefab,
        //                aimRay.origin,
        //                Util.QuaternionSafeLookRotation(aimRay.direction),
        //                base.gameObject,
        //                Modules.StaticValues.blackwhipshootDamageCoefficient * this.damageStat,
        //                0f,
        //                base.RollCrit(),
        //                DamageColorIndex.Default,
        //                null,
        //                BlackwhipShoot.throwForce);
        //            }                    
        //            ProjectileManager.instance.FireProjectile(Modules.Projectiles.blackwhipPrefab,
        //            aimRay.origin,
        //            Util.QuaternionSafeLookRotation(aimRay.direction),
        //            base.gameObject,
        //            Modules.StaticValues.blackwhipshootDamageCoefficient * this.damageStat,
        //            -4000f,
        //            base.RollCrit(),
        //            DamageColorIndex.WeakPoint,
        //            null,
        //            BlackwhipShoot.throwForce);              

        //        }

        //    }
        //}

        public override void FixedUpdate()
        {
            //this.attack.Fire();
            base.FixedUpdate();
            //RecalculateRollSpeed();       
            bool flag = base.fixedAge >= this.duration && base.isAuthority;
            if (flag)
            {
                this.outer.SetNextStateToMain();
            }
            else
            {
                bool flag2 = base.fixedAge >= this.duration / 2 && !this.hasActivated;
                if (flag2)
                {
                    if (dekucon.isMaxPower)
                    {
                        this.hasActivated = false;
                    }
                    else
                    {
                        this.hasActivated = true;
                    }
                    bool isAuthority = base.isAuthority;
                    if (isAuthority)
                    {
                        bool down = base.inputBank.skill2.down;
                        if (down)
                        {
                            bool isAuthority2 = base.isAuthority;
                            if (isAuthority2)
                            {
                                Ray aimRay = base.GetAimRay();
                                ProjectileManager.instance.FireProjectile(Modules.Projectiles.blackwhipPrefab,
                                aimRay.origin,
                                Quaternion.LookRotation(aimRay.direction),
                                base.gameObject,
                                Modules.StaticValues.blackwhipshootDamageCoefficient * this.damageStat,
                                0f,
                                base.RollCrit(),
                                DamageColorIndex.Default,
                                null,                                
                                -1);
                            }
                        }
                        else
                        {

                            RecalculateRollSpeed();
                            Ray aimRay = base.GetAimRay();
                            if (base.characterMotor && base.characterDirection)
                            {
                                base.characterMotor.velocity = aimRay.direction * this.rollSpeed;
                            }
                            bool isAuthority3 = base.isAuthority;
                            if (isAuthority3)
                            {
                                ProjectileManager.instance.FireProjectile(Modules.Projectiles.blackwhipPrefab,
                                aimRay.origin,
                                Util.QuaternionSafeLookRotation(aimRay.direction),
                                base.gameObject,
                                Modules.StaticValues.blackwhipshootDamageCoefficient * this.damageStat,
                                0f,
                                base.RollCrit(),
                                DamageColorIndex.WeakPoint,
                                null,
                                BlackwhipShoot.throwForce);
                            }
                        }
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}