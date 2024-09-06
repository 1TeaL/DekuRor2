//using RoR2;
//using UnityEngine;
//using UnityEngine.Networking;
//using EntityStates;
//using System.Collections.Generic;
//using System.Linq;
//using RoR2.Audio;
//using System;
//using R2API.Networking;
//using DekuMod.Modules;
//using DekuMod.Modules.Survivors;

//namespace DekuMod.SkillStates
//{
//    public class Blackwhip45 : BaseQuirk45
//    {
//        private RaycastHit raycastHit;
//        private bool raycast;
//        public HurtBox Target;
//        private bool targetIsValid;
//        private OverlapAttack attack;
//        private List<HurtBox> HitResults = new List<HurtBox>();
//        private float fireTime = 0.1f;

//        private float procCoefficient = 1f;
//        private float pushForce = 600f;
//        private Vector3 storedPosition;
//        public static float dashSpeed = 100f;
//        public static float hopForce = 10f;
//        public static float damageCoefficient = Modules.StaticValues.blackwhip45DamageCoefficient;


//        public override void OnEnter()
//        {
//            base.OnEnter();
//        }

//        protected override void DoSkill()
//        {
//            base.DoSkill();

//            if (dekucon && base.isAuthority)
//            {
//                Target = dekucon.GetTrackingTarget();
//            }

            
//            raycast = Physics.Raycast(base.GetAimRay(), out raycastHit, 100f, LayerIndex.world.mask | LayerIndex.entityPrecise.mask);
//            if (!Target && !raycast)
//            {                
//                energySystem.currentPlusUltra += Modules.StaticValues.skill45PlusUltraSpend;
//                return;
//            }
//            if (base.characterBody)
//            {
//                base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
//            }
//            if (this.Target && this.Target.healthComponent && this.Target.healthComponent.alive)
//            {
//                this.targetIsValid = true;

//            }
//            else if(raycast)
//            {
//                this.targetIsValid = true;
//            }
//            HitBoxGroup hitBoxGroup = null;
//            Transform modelTransform = base.GetModelTransform();
//            bool flag3 = modelTransform;
//            if (flag3)
//            {
//                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "SmashRushHitbox");
//            }
//            this.attack = new OverlapAttack();
//            this.attack.damageType = DamageType.Stun1s;
//            this.attack.attacker = base.gameObject;
//            this.attack.inflictor = base.gameObject;
//            this.attack.teamIndex = base.GetTeam();
//            this.attack.damage = damageCoefficient * this.damageStat;
//            this.attack.procCoefficient = procCoefficient;
//            this.attack.hitBoxGroup = hitBoxGroup;
//            this.attack.isCrit = base.RollCrit();
//            this.attack.pushAwayForce = pushForce;
//            //Util.PlaySound("Misc_StartDash", base.gameObject);
//            base.GetModelAnimator().SetBool("attacking", true);
//            base.GetModelAnimator().SetFloat("Slash.playbackRate", base.attackSpeedStat);
//            base.PlayCrossfade("FullBody, Override", "ShootStyleComboDash", "Slash.playbackRate", 1f, 0.01f);


//            if (base.isAuthority)
//            {
//                AkSoundEngine.PostEvent("blackwhipvoice", this.gameObject);
//            }
//            AkSoundEngine.PostEvent("blackwhipsfx", this.gameObject);
//            AkSoundEngine.PostEvent("shootstyedashsfx", this.gameObject);
//        }

//        protected override void DontDoSkill()
//        {
//            base.DontDoSkill();
//        }
//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();
//            if(base.fixedAge > fireTime)
//            {
//                if (Target)
//                {
//                    this.storedPosition = Target.transform.position;
//                    dekucon.enemyBody = Target.healthComponent.body;
//                    dekucon.blackwhipTimer += 1f;
//                }
//                if (raycast)
//                {
//                    this.storedPosition = raycastHit.point;
//                    dekucon.storedPos = raycastHit.point;
//                    dekucon.blackwhipAttachWorld = true;
//                    dekucon.enemyBody = null;
//                    dekucon.blackwhipTimer += 1f;

//                }

//                if (base.isAuthority && this.targetIsValid)
//                {
//                    Vector3 velocity = (this.storedPosition - base.transform.position).normalized * dashSpeed;
//                    base.characterMotor.velocity = velocity;
//                    base.characterDirection.forward = base.characterMotor.velocity.normalized;
//                    bool flag3 = base.fixedAge >= 1f;
//                    if (flag3)
//                    {
//                        this.outer.SetNextStateToMain();
//                    }
//                    else
//                    {
//                        this.attack.forceVector = base.characterMotor.velocity.normalized * pushForce;
//                        bool flag4 = this.attack.Fire(this.HitResults);
//                        if (flag4)
//                        {
//                            base.PlayCrossfade("FullBody, Override", "ShootStyleCombo3", "Slash.playbackRate", 0.3f, 0.01f);
//                            EffectManager.SimpleMuzzleFlash(Modules.Asset.dekuKickEffect, base.gameObject, "Swing3", true);
//                            bool flag5 = this.HitResults.Count > 0;
//                            if (flag5)
//                            {
//                                foreach (HurtBox singularTarget in HitResults)
//                                {
//                                    if (singularTarget)
//                                    {
//                                        if (singularTarget.healthComponent && singularTarget.healthComponent.body)
//                                        {
//                                            singularTarget.healthComponent.body.ApplyBuff(Modules.Buffs.blackwhipDebuff.buffIndex, 1, 6);
//                                        }
//                                    }
//                                }
//                                this.outer.SetNextStateToMain();
//                                return;

//                            }
//                            else
//                            {
//                                this.outer.SetNextStateToMain();
//                                return;
//                            }
//                        }
//                    }
//                }
//                else 
//                {
//                    this.outer.SetNextStateToMain();
//                }

//            }

            
//        }



//        public override void OnExit()
//        {
//            base.OnExit();
//            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
//            base.characterMotor.velocity *= 0.1f;
//            base.GetModelAnimator().SetBool("attacking", false);
//            base.PlayAnimation("Fullbody, Override", "BufferEmpty");
            
//        }
//        public override InterruptPriority GetMinimumInterruptPriority()
//        {
//            return InterruptPriority.Frozen;
//        }
//    }
//}
