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
        public static float baseDuration = 0.7f;
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
        public static float baseSpeedCoefficient = 15f;
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
        private BulletAttack bulletAttack;
        public float pullForce;
        public float hopUpFraction;
        public float blackwhipage;

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
            this.hasFired = false;
            dekucon.canPull = true;

            Ray aimRay = base.GetAimRay();

            if (dekucon.isMaxPower)
            {
                hitboxName = "BigModelHitbox";
                fajin = 2f;
                SpeedCoefficient = baseSpeedCoefficient * 1.5f;
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
                }, true);
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
            if (dekucon.isMaxPower)
            {
                base.characterMotor.velocity *= 0.1f;
            }            
            dekucon.RemoveBuffCount(50);
            base.characterMotor.mass = this.previousMass;
            base.characterMotor.useGravity = true;
            //base.characterMotor.velocity = Vector3.zero;
            base.PlayCrossfade("RightArm, Override", "BufferEmpty", 0f);
            dekucon.canPull = false;
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

        private void Fire()
        {
            //if (!this.hasFired)
            //{
            //    this.hasFired = true;
            //}
            Ray aimRay = base.GetAimRay();
            bulletAttack = new BulletAttack();

            bulletAttack.bulletCount = 1;
            bulletAttack.aimVector = aimRay.direction;
            bulletAttack.origin = aimRay.origin;
            bulletAttack.damage = 0f;
            bulletAttack.damageColorIndex = DamageColorIndex.Default;
            bulletAttack.damageType = damageType;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
            bulletAttack.maxDistance = 200f;
            bulletAttack.force = -2000f;
            bulletAttack.hitMask = LayerIndex.CommonMasks.bullet;
            bulletAttack.minSpread = 0f;
            bulletAttack.maxSpread = 0f;
            bulletAttack.isCrit = base.RollCrit();
            bulletAttack.owner = base.gameObject;
            bulletAttack.muzzleName = muzzleString;
            bulletAttack.smartCollision = false;
            bulletAttack.procChainMask = default(ProcChainMask);
            bulletAttack.procCoefficient = procCoefficient;
            bulletAttack.radius = 2f * fajin;
            bulletAttack.sniper = false;
            bulletAttack.stopperMask = LayerIndex.noCollision.mask;
            bulletAttack.weapon = null;
            bulletAttack.tracerEffectPrefab = Modules.Projectiles.blackwhipTracer;
            bulletAttack.spreadPitchScale = 0f;
            bulletAttack.spreadYawScale = 0f;
            bulletAttack.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
            bulletAttack.hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab;
            bulletAttack.hitCallback = ApplyPullOnHit;

            bulletAttack.Fire();
        }


        private bool ApplyPullOnHit(ref BulletAttack.BulletHit hitInfo)
        {
            bool result = bulletAttack.hitCallback(ref hitInfo);
            if (hitInfo.entityObject)
            {
                CharacterMotor motor = hitInfo.entityObject.GetComponent<CharacterMotor>();
                if (motor)
                {
                    float forceCoefficient = 5f;
                    Vector3 direction = base.transform.position - hitInfo.entityObject.transform.position;
                    direction.Normalize();
                    Vector3 force = direction * this.pullForce * motor.mass * forceCoefficient;
                    hitInfo.entityObject.GetComponent<HealthComponent>().TakeDamageForce(force, true, false);
                }
                Rigidbody rigidbody = hitInfo.entityObject.GetComponent<Rigidbody>();
                if (rigidbody)
                {
                    float forceCoefficient = 5f;
                    Vector3 direction = base.transform.position - hitInfo.entityObject.transform.position;
                    direction.Normalize();
                    Vector3 force = direction * this.pullForce * rigidbody.mass * forceCoefficient;
                    hitInfo.entityObject.GetComponent<HealthComponent>().TakeDamageForce(force, true, false);
                }
            }
            return result;
        }

        //    }
        //    return result;
        //    //var hurtbox = hitInfo.hitHurtBox;
        //    //if (hurtbox)
        //    //{
        //    //    var healthComponent = hurtbox.healthComponent;
        //    //    if (healthComponent)
        //    //    {
        //    //        var body = healthComponent.body;
        //    //        if (body)
        //    //        {
        //    //            CharacterMotor motor = hitInfo.entityObject.GetComponent<>
        //    //        }
        //    //    }
        //    //}
        //    //return false;
        //}

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.attack.Fire();
            //RecalculateRollSpeed();       
            bool flag = base.fixedAge >= this.duration && base.isAuthority;
            if (flag)
            {
                this.outer.SetNextStateToMain();
            }
            else
            {
                bool flag2 = base.fixedAge >= this.duration / 3 && !this.hasActivated;

                if (flag2)
                {
                    if (dekucon.isMaxPower)
                    {
                        if(blackwhipage >= this.duration / 3)
                        {
                            this.hasActivated = false;
                            blackwhipage = 0f;
                        }
                        else
                        {
                            this.blackwhipage += Time.fixedDeltaTime;
                        }


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
                                Fire();
                                Ray aimRay = base.GetAimRay();
                                ProjectileManager.instance.FireProjectile(Modules.Projectiles.blackwhipPrefab,
                                aimRay.origin,
                                Quaternion.LookRotation(aimRay.direction),
                                base.gameObject,
                                Modules.StaticValues.blackwhipshootDamageCoefficient * this.damageStat,
                                -1000f,
                                base.RollCrit(),
                                DamageColorIndex.Default,
                                null,
                                -1f);
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