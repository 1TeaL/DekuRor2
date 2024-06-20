using EntityStates;
using RoR2;
using UnityEngine;
using DekuMod.Modules.Survivors;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using DekuMod.SkillStates.BaseStates;
using R2API;
using System.Reflection;
using R2API.Networking;
using DekuMod.Modules;

namespace DekuMod.SkillStates
{
    public class OklahomaSmash : BaseDekuSkillState
    {
        public HurtBox Target;
        private Animator animator;
        private CharacterModel characterModel;
        private Ray aimRay;
        private float rollSpeed;
        private float SpeedCoefficient;
        public static float initialSpeedCoefficient = Modules.StaticValues.oklahomaSpeedCoefficient;
        private float finalSpeedCoefficient = 0.1f;

        private float dashDuration = 1f;
        private float maxDuration = 2f;
        private float maxTravelTime = 0.5f;
        private float travelTimer;

        private Vector3 dashDirection;
        private Vector3 targetPosition;
        private Vector3 direction;
        private Transform modelTransform;
        private OverlapAttack attack;

        public List<HurtBox> HitResults { get; private set; }

        public override void OnEnter()
        {

            base.OnEnter();


            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();

                TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = 1f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay2.duration = 1f;
                temporaryOverlay2.animateShaderAlpha = true;
                temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay2.destroyComponentOnEnd = true;
                temporaryOverlay2.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());

            }

            this.animator = base.GetModelAnimator();
            aimRay = base.GetAimRay();
            //noctisCon.WeaponAppearR(0f, NoctisController.WeaponTypeR.NONE);


            direction = base.inputBank.moveVector;

            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            SpeedCoefficient = initialSpeedCoefficient * attackSpeedStat;

            PlayAnimation();

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
            this.attack.damage = StaticValues.oklahomaDamageCoefficient * this.damageStat;
            this.attack.procCoefficient = 1f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
            this.attack.pushAwayForce = 1000f;
        }

        private void PlayAnimation()
        {
            //AkSoundEngine.PostEvent("Dodge", base.gameObject);
            base.PlayAnimation("FullBody, Override", "Dash", "Attack.playbackRate", this.dashDuration);
            
        }

        private void RecalculateRollSpeed()
        {
            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num /= base.characterBody.sprintingSpeedMultiplier;
            }
            this.rollSpeed = num * Mathf.Lerp(SpeedCoefficient, finalSpeedCoefficient, base.fixedAge / dashDuration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            RecalculateRollSpeed();
            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

            if (Target)
            {
                targetPosition = Target.transform.position;
            }

            //dash/slide in direction, after that release the button for a dashing attack towards the target
            
            if (base.fixedAge <= dashDuration)
            {
                dashDirection = direction.normalized * rollSpeed;
            }
            if(base.fixedAge > dashDuration)
            {
                if (!base.IsKeyDownAuthority())
                {
                    if (Target)
                    {

                        if (base.isAuthority && Target)
                        {
                            dashDirection = (targetPosition).normalized * initialSpeedCoefficient;
                            base.characterDirection.forward = base.characterMotor.velocity.normalized;

                            travelTimer += Time.fixedDeltaTime;
                            if (travelTimer > maxTravelTime)
                            {
                                this.outer.SetNextStateToMain();
                            }
                            else
                            {
                                //travel until you hit something
                                this.attack.forceVector = base.characterMotor.velocity.normalized * 1000f;
                                if (this.attack.Fire(this.HitResults))
                                {
                                    if (this.HitResults.Count > 0)
                                    {
                                        foreach (HurtBox hurtBox in this.HitResults)
                                        {
                                            bool flag6 = hurtBox.healthComponent && hurtBox.healthComponent.health > 0f;
                                            if (flag6)
                                            {
                                            }
                                        }
                                        this.outer.SetNextStateToMain();
                                        return;
                                    }
                                    else
                                    {
                                        this.outer.SetNextStateToMain();
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.outer.SetNextStateToMain();
                        }


                        
                    }
                    else
                    {
                        dashDirection = direction.normalized * rollSpeed;
                    }
                } 
                else if (base.IsKeyDownAuthority())
                {
                    if (base.fixedAge > maxDuration)
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                }
            }

            base.characterMotor.velocity = dashDirection;
            base.characterDirection.forward = base.characterMotor.velocity.normalized;


        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}



