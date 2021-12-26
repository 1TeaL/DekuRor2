using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using UnityEngine.Networking;
using System;
using DekuMod.Modules.Survivors;

namespace DekuMod.SkillStates
{
    public class BlackwhipShoot : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.65f;
        public static float throwForce = 150f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private Animator animator;
        private ProjectileImpactEventCaller impactEventCaller;

        private Vector3 moveVec;

        private string muzzleString;
        public float fajin;
        protected DamageType damageType;
        public DekuController dekucon;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = BlackwhipShoot.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.35f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            dekucon = base.GetComponent<DekuController>();

            Ray aimRay = base.GetAimRay();
            Vector3 direction = aimRay.direction;
            base.characterMotor.velocity = Vector3.zero;
            this.moveVec = 100f * direction.normalized;
            base.characterMotor.velocity += this.moveVec;

            if (dekucon.isMaxPower)
            {
                fajin = 2f;
            }
            else
            {
                fajin = 1f;
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
            if (NetworkServer.active)
            {                
                impactEventCaller = Modules.Projectiles.blackwhipPrefab.GetComponent<ProjectileImpactEventCaller>();
                if ((bool)impactEventCaller)
                {
                    Debug.Log("listen");
                    impactEventCaller.impactEvent.AddListener(OnImpact);
                }
            }
                        
        }


        public void OnImpact(ProjectileImpactInfo impactInfo)
        {
            Debug.Log("impact");
            Ray aimRay = base.GetAimRay();
            Vector3 direction = aimRay.direction;
            Vector3 impact = impactInfo.estimatedPointOfImpact;
            base.characterMotor.velocity = Vector3.zero;
            this.moveVec = 30f * impact.normalized;
            base.characterMotor.rootMotion += this.moveVec;
            //base.characterMotor.velocity += this.moveVec * 2;
        }

        public void Onhit()
        {
            Debug.Log("impact");
            Ray aimRay = base.GetAimRay();
            Vector3 direction = aimRay.direction;
            base.characterMotor.velocity = Vector3.zero;
            this.moveVec = 30f * direction.normalized;
            base.characterMotor.velocity += this.moveVec;
            //base.characterMotor.velocity += this.moveVec * 2;
        }

        public override void OnExit()
        {
            dekucon.RemoveBuffCount(50);
            base.PlayCrossfade("RightArm, Override", "BufferEmpty", 0f);
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                if (dekucon.isMaxPower)
                {
                    this.hasFired = false;
                }
                else
                {
                    this.hasFired = true;
                }

                //Util.PlaySound("HenryBombThrow", base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.blackwhipPrefab,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(aimRay.direction),
                        base.gameObject,
                        Modules.StaticValues.blackwhipshootDamageCoefficient * this.damageStat,
                        -3000f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        BlackwhipShoot.throwForce);

                    if (!Modules.Projectiles.blackwhipPrefab.GetComponent<ProjectileImpactExplosion>().alive)
                    {
                        Onhit();
                        
                    }
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
            return InterruptPriority.PrioritySkill;
        }
    }
}