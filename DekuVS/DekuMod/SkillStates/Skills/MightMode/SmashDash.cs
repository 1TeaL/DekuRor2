﻿using EntityStates;
using DekuMod.Modules.Survivors;
using DekuMod.SkillStates.BaseStates;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DekuMod.SkillStates.Might
{
    public class SmashDash : BaseDekuSkillState
    {
        public HurtBox Target;
        private bool targetIsValid;
        private OverlapAttack attack;
        private List<HurtBox> HitResults = new List<HurtBox>();

        private float procCoefficient = 1f;
        private float pushForce = 0f;
        private Vector3 storedPosition;
        public float dashSpeed = 50f;       
        public static float hopForce = 10f;
        public static float damageCoefficient = 0f;
        public float duration = 0.8f;

        public override void OnEnter()
        {
            base.OnEnter();

            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

            if (!Target)
            {
                return;
            }

            //if (base.characterBody)
            //{
            //    base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            //}
            bool flag2 = this.Target && this.Target.healthComponent && this.Target.healthComponent.alive;
            if (flag2)
            {
                this.targetIsValid = true;
            }
            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();
            bool flag3 = modelTransform;
            if (flag3)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "SmashRushHitbox");
            }
            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Generic;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = damageCoefficient * this.damageStat;
            this.attack.procCoefficient = procCoefficient;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
            this.attack.pushAwayForce = pushForce * 0.2f;
            //Util.PlaySound("Misc_StartDash", base.gameObject);
            base.GetModelAnimator().SetBool("smashRushDashEnd", false);
            base.GetModelAnimator().SetFloat("Slash.playbackRate", base.attackSpeedStat);
            base.PlayAnimation("FullBody, Override", "SmashRushDash", "Slash.playbackRate", 0.01f);


            AkSoundEngine.PostEvent("shootstyledashsfx", this.gameObject);


        }

        public override void Level2()
        {
            base.Level2();
            dashSpeed = 100f;
        }
        public override void Level3()
        {
            base.Level3();
            duration = 0.2f;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //base.PlayAnimation("FullBody, Override", "ShootStyleComboDash", "Slash.playbackRate", 1f);
            if (Target)
            {
                this.storedPosition = Target.transform.position;
            }

            switch (level)
            {
                case 0:
                    if (base.isAuthority && this.targetIsValid)
                    {
                        Vector3 velocity = (this.storedPosition - base.transform.position).normalized * dashSpeed;
                        base.characterMotor.velocity = velocity;
                        base.characterDirection.forward = base.characterMotor.velocity.normalized;
                        bool flag3 = base.fixedAge >= duration;
                        if (flag3)
                        {
                            this.outer.SetNextStateToMain();
                        }
                        else
                        {
                            this.attack.forceVector = base.characterMotor.velocity.normalized * pushForce;
                            if (this.attack.Fire(this.HitResults))
                            {
                                if (this.HitResults.Count > 0)
                                {
                                    foreach (HurtBox hurtBox in this.HitResults)
                                    {
                                        if (hurtBox.healthComponent && hurtBox.healthComponent.health > 0f)
                                        {
                                        }
                                    }
                                    base.GetModelAnimator().SetBool("smashRushDashEnd", true);
                                    this.outer.SetNextState(new SmashDashExit
                                    {
                                    });
                                }
                                else
                                {
                                    base.GetModelAnimator().SetBool("smashRushDashEnd", true);
                                    this.outer.SetNextState(new SmashDashExit
                                    {
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    break;
                case 1:
                    if (base.isAuthority && this.targetIsValid)
                    {
                        Vector3 velocity = (this.storedPosition - base.transform.position).normalized * dashSpeed;
                        base.characterMotor.velocity = velocity;
                        base.characterDirection.forward = base.characterMotor.velocity.normalized;
                        bool flag3 = base.fixedAge >= duration;
                        if (flag3)
                        {
                            this.outer.SetNextStateToMain();
                        }
                        else
                        {
                            this.attack.forceVector = base.characterMotor.velocity.normalized * pushForce;
                            if (this.attack.Fire(this.HitResults))
                            {
                                if (this.HitResults.Count > 0)
                                {
                                    foreach (HurtBox hurtBox in this.HitResults)
                                    {
                                        if (hurtBox.healthComponent && hurtBox.healthComponent.health > 0f)
                                        {
                                        }
                                    }
                                    base.GetModelAnimator().SetBool("smashRushDashEnd", true);
                                    this.outer.SetNextState(new SmashDashExit
                                    {
                                    });
                                }
                                else
                                {
                                    base.GetModelAnimator().SetBool("smashRushDashEnd", true);
                                    this.outer.SetNextState(new SmashDashExit
                                    {
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    break;
                case 2:
                    if (base.fixedAge >= duration)
                    {
                        base.characterMotor.Motor.SetPositionAndRotation(storedPosition - base.GetAimRay().direction.normalized * 2f, Quaternion.LookRotation(base.GetAimRay().direction));

                        base.GetModelAnimator().SetBool("smashRushDashEnd", true);
                        this.outer.SetNextState(new SmashDashExit
                        {
                        });

                    }
                    break;
            }


        }
        public override void OnExit()
        {
            base.OnExit();
            //base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
            base.characterMotor.velocity *= 0.1f;
            base.GetModelAnimator().SetBool("attacking", false);
            base.PlayAnimation("Fullbody, Override", "BufferEmpty");
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}