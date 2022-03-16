using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using UnityEngine.Networking;
using System;
using DekuMod.Modules.Survivors;
using RoR2.Audio;
using System.Collections.Generic;

namespace DekuMod.SkillStates
{
    public class BlackwhipShoot : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.7f;
        public static float throwForce = 150f;
        public static float radius = 5f;

        private float duration;
        private Animator animator;
        //private ProjectileImpactEventCaller impactEventCaller;

        //private Vector3 moveVec;

        private string muzzleString;
        public float fajin;
        public DekuController dekucon;
        private float rollSpeed;
        public static float baseSpeedCoefficient = 15f;
        public static float SpeedCoefficient;
        public static float waitReturnDuration = 0.3f;
        public static float holdTime = 0.3f;
        private float previousMass;
        private BlastAttack blastAttack;
        private bool hasActivated;

        protected NetworkSoundEventIndex impactSound;
        private OverlapAttack attack;
        private OverlapAttack attack2;
        protected string hitboxName;
        public DamageType damageType;
        private BulletAttack bulletAttack;
        public float pullForce;
        public float hopUpFraction;
        public float blackwhipage;

        private List<HealthComponent> hitHealthComponents;
        private Vector3 pullPoint;
        private enum SubState
        {
            Skewer,
            SkewerHit,
            Pull,
            Exit
        }

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = BlackwhipShoot.baseDuration / this.attackSpeedStat;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            dekucon = base.GetComponent<DekuController>();
            base.characterMotor.useGravity = false;
            this.previousMass = base.characterMotor.mass;
            base.characterMotor.mass = 0f;
            hasActivated = false;
            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            Ray aimRay = base.GetAimRay();
            base.StartAimMode(duration, true);
            if (dekucon.isMaxPower)
            {
                hitboxName = "BigModelHitbox";
                fajin = 2f;
                SpeedCoefficient = baseSpeedCoefficient * 1.5f;
                damageType = DamageType.Stun1s | DamageType.BypassArmor;
                dekucon.RemoveBuffCount(50);
            }
            else
            {
                dekucon.AddToBuffCount(10);
                hitboxName = "BodyHitbox";
                fajin = 1f;
                SpeedCoefficient = baseSpeedCoefficient;
                damageType = DamageType.SlowOnHit;
            }
            //base.PlayAnimation("RightArm, Override", "Blackwhip", "attack.playbackRate", duration);

            base.PlayCrossfade("RightArm, Override", "Blackwhip", "Attack.playbackRate", this.duration, this.duration / 2);

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
            if (dekucon.isMaxPower)
            {
                this.attack2 = this.CreateAttack2(hitBoxGroup);
            }

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
            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
            if (dekucon.isMaxPower)
            {
                base.characterMotor.velocity *= 0.5f;
            }            
            dekucon.RemoveBuffCount(50);
            base.characterMotor.mass = this.previousMass;
            base.characterMotor.useGravity = true;
            //base.characterMotor.velocity = Vector3.zero;
            //base.PlayCrossfade("RightArm, Override", "BufferEmpty", 0f);
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
                damage = Modules.StaticValues.blackwhipshootDamageCoefficient * this.damageStat * fajin,
                procCoefficient = 1f,
                hitEffectPrefab = Modules.Assets.detroitweakEffect,
                forceVector = Vector3.zero,
                pushAwayForce = 1000f,
                hitBoxGroup = hitBoxGroup,
                isCrit = base.RollCrit(),
                impactSound = this.impactSound,                
                
            };
        }

        protected OverlapAttack CreateAttack2(HitBoxGroup hitBoxGroup)
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
                impactSound = this.impactSound,

            };
        }

        private void Fire()
        {
            //if (!this.hasFired)
            //{
            //    this.hasFired = true;
            //}
            Ray aimRay = base.GetAimRay();
            //this.pullPoint = aimRay.GetPoint(3f);
            //this.pullPoint.y = base.transform.position.y + 1f;

            bool flag = Util.HasEffectiveAuthority(base.gameObject);
            if (flag)
            {
                BulletAttack bulletAttack = new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = 0f,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = damageType,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = 200f,
                    force = -5000f * fajin,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    muzzleName = muzzleString,
                    smartCollision = false,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 0f,
                    radius = 3f * fajin,
                    sniper = false,
                    stopperMask = LayerIndex.noCollision.mask,
                    weapon = null,
                    tracerEffectPrefab = Modules.Projectiles.blackwhipTracer,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,

                };bulletAttack.Fire();
            //    bulletAttack.hitCallback = delegate (ref BulletAttack.BulletHit hitInfo)
            //    {
            //        bool result = bulletAttack.DefaultHitCallback(ref hitInfo);
            //        bool flag13 = hitInfo.hitHurtBox;
            //        if (flag13)
            //        {
            //            this.OnHitEnemyAuthority();
            //        }
            //        return result;
            //    };
            //}
            //bool active = NetworkServer.active;
            //if (active)
            //{
            //    RaycastHit[] array = Physics.SphereCastAll(aimRay.origin, 2f, aimRay.direction, 100f, LayerIndex.CommonMasks.bullet, QueryTriggerInteraction.UseGlobal);
            //    for (int i = 0; i < array.Length; i++)
            //    {
            //        bool flag3 = array[i].collider;
            //        if (flag3)
            //        {
            //            Collider collider = array[i].collider;
            //            bool flag4 = collider;
            //            if (flag4)
            //            {
            //                HurtBox component = collider.GetComponent<HurtBox>();
            //                bool flag5 = component;
            //                if (flag5)
            //                {
            //                    HealthComponent healthComponent = component.healthComponent;
            //                    bool flag6 = healthComponent;
            //                    if (flag6)
            //                    {
            //                        TeamComponent component2 = healthComponent.GetComponent<TeamComponent>();
            //                        bool flag7 = component2.teamIndex != base.teamComponent.teamIndex;
            //                        bool flag8 = flag7;
            //                        if (flag8)
            //                        {
            //                            bool flag9 = !this.hitHealthComponents.Contains(healthComponent);
            //                            if (flag9)
            //                            {
            //                                this.hitHealthComponents.Add(healthComponent);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    foreach (HealthComponent healthComponent2 in this.hitHealthComponents)
            //    {
            //        bool flag10 = healthComponent2 && healthComponent2.body;
            //        if (flag10)
            //        {
            //            EntityStateMachine component3 = healthComponent2.body.GetComponent<EntityStateMachine>();
            //            bool flag11 = healthComponent2.body.GetComponent<SetStateOnHurt>() && healthComponent2.body.GetComponent<SetStateOnHurt>().canBeFrozen && component3;
            //            if (flag11)
            //            {
            //                SkeweredState newNextState = new SkeweredState
            //                {
            //                    skewerDuration = this.skewerTime,
            //                    pullDuration = this.pullTime,
            //                    destination = this.pullPoint
            //                };
            //                component3.SetInterruptState(newNextState, InterruptPriority.Death);
            //            }
            //        }
            //    }
            //    bool flag12 = this.hitHealthComponents.Count > 0;
            //    if (flag12)
            //    {
            //        this.subState = Skewer.SubState.SkewerHit;
            //        this.stopwatch = 0f;
            //    }
            //    else
            //    {
            //        Util.PlaySound("DSpecialSwing", base.gameObject);
            //        this.subState = Skewer.SubState.Exit;
            //        this.stopwatch = 0f;
            //    }
            }
        }

        //private void OnHitEnemyAuthority()
        //{
        //    base.PlayAnimation("FullBody, Override", "DownSpecialHit", "Slash.playbackrate", this.duration * 0.5f);
        //    Util.PlaySound("DSpecialHit", base.gameObject);
        //    this.subState = Skewer.SubState.SkewerHit;
        //    this.stopwatch = 0f;
        //    base.characterMotor.velocity = Vector3.zero;
        //    base.AddRecoil(-1f * this.attackRecoil / 2f, -2f * this.attackRecoil / 2f, -0.5f * this.attackRecoil / 2f, 0.5f * this.attackRecoil / 2f);
        //}

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && blackwhipage >= this.duration / 5)
            {
                this.attack.Fire();
                blackwhipage = 0;
                if (dekucon.isMaxPower)
                {
                    this.attack2.Fire();
                }
            }
            else this.blackwhipage += Time.fixedDeltaTime; 
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
                        if(blackwhipage >= this.duration / 6)
                        {
                            this.hasActivated = false;
                            blackwhipage = 0f;
                        }
                        else
                        {
                            this.blackwhipage += Time.fixedDeltaTime;
                            this.hasActivated = true;
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
                                BlackwhipShoot.throwForce);
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