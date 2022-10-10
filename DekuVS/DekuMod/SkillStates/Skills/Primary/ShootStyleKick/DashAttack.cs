using EntityStates;
using DekuMod.Modules.Survivors;
using DekuMod.SkillStates.BaseStates;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DekuMod.SkillStates
{
    public class DashAttack : BaseSkill
    {
        public HurtBox Target;
        private bool targetIsValid;
        private OverlapAttack attack;
        private List<HurtBox> HitResults = new List<HurtBox>();

        private float procCoefficient = 1f;
        private float pushForce = 0f;
        private Vector3 storedPosition;
        public static float dashSpeed = 80f;       
        public static float hopForce = 10f;
        public static float damageCoefficient = 0f;
        public override void OnEnter()
        {
            dekucon = base.GetComponent<DekuController>();
            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

            if (!Target)
            {
                return;
            }

            if (base.characterBody)
            {
                base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            }
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
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "BigBodyHitbox");
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
            base.GetModelAnimator().SetBool("attacking", true);
            base.GetModelAnimator().SetFloat("Slash.playbackRate", base.attackSpeedStat);
            base.PlayAnimation("FullBody, Override", "ShootStyleComboDash", "Slash.playbackRate", 1f);


            AkSoundEngine.PostEvent("shootstyedashsfx", this.gameObject);
            base.OnEnter();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.PlayAnimation("FullBody, Override", "ShootStyleComboDash", "Slash.playbackRate", 1f);
            if (Target)
            {
                this.storedPosition = Target.transform.position;
            }
            bool flag2 = base.isAuthority && this.targetIsValid;
            if (flag2)
            {
                Vector3 velocity = (this.storedPosition - base.transform.position).normalized * dashSpeed;
                base.characterMotor.velocity = velocity;
                base.characterDirection.forward = base.characterMotor.velocity.normalized;
                bool flag3 = base.fixedAge >= 0.8f;
                if (flag3)
                {
                    this.outer.SetNextStateToMain();
                }
                else
                {
                    this.attack.forceVector = base.characterMotor.velocity.normalized * pushForce;
                    bool flag4 = this.attack.Fire(this.HitResults);
                    if (flag4)
                    {
                        bool flag5 = this.HitResults.Count > 0;
                        if (flag5)
                        {
                            foreach (HurtBox hurtBox in this.HitResults)
                            {
                                bool flag6 = hurtBox.healthComponent && hurtBox.healthComponent.health > 0f;
                                if (flag6)
                                {
                                }
                            }
                            this.outer.SetNextState(new DashAttackExit
                            {
                            });
                        }
                        else
                        {
                            this.outer.SetNextState(new DashAttackExit
                            {
                            });
                        }
                    }
                }
            }
            else
            {
                this.outer.SetNextStateToMain();
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
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