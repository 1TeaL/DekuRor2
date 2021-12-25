using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using UnityEngine.Networking;
using System;

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

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = BlackwhipShoot.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.35f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            base.PlayAnimation("RightArm, Override", "Blackwhip", "attack.playbackRate", duration);

            if (NetworkServer.active)
            {
                impactEventCaller = GetComponent<ProjectileImpactEventCaller>();
                if ((bool)impactEventCaller)
                {
                    impactEventCaller.impactEvent.AddListener(OnImpact);
                }
            }
        }

        public void OnImpact(ProjectileImpactInfo impactInfo)
        {

            Ray aimRay = base.GetAimRay();
            Vector3 direction = aimRay.direction;
            base.characterMotor.velocity = Vector3.zero;
            this.moveVec = 30f * direction;
            base.characterMotor.rootMotion += this.moveVec;
            //base.characterMotor.velocity += this.moveVec * 2;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                //Util.PlaySound("HenryBombThrow", base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.blackwhipPrefab,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(aimRay.direction),
                        base.gameObject,
                        Modules.StaticValues.blackwhipshootDamageCoefficient * this.damageStat,
                        4000f,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        BlackwhipShoot.throwForce);

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