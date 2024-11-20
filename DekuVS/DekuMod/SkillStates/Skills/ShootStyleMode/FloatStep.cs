using EntityStates;
using RoR2;
using UnityEngine;
using DekuMod.Modules;
using DekuMod.SkillStates.BaseStates;
using EntityStates.Merc;
using static UnityEngine.ParticleSystem.PlaybackState;
using R2API.Networking;

namespace DekuMod.SkillStates.ShootStyle
{
    public class FloatStep : BaseDekuSkillState
    {
        private string muzzleName = "LFinger";

        public Animator anim;
        public bool consumeStock = false;

        private float duration;
        private float fireTime;
        private float rollSpeed;
        private float initialSpeedCoefficient;
        private float finalSpeedCoefficient;

        private Vector3 dashDirection;
        private BlastAttack blastAttack;
        private float blastRadius;
        private float blastDamage;
        private Vector3 blastPosition;
        private float blastForce;
        private DamageType blastType = DamageType.Generic;

        public enum floatState {GROUND, AIR};
        public floatState state;

        private Transform modelTransform;
        private CharacterModel characterModel;

        public override void OnEnter()
        {
            base.OnEnter();
            anim = base.GetModelAnimator();
            GetModelAnimator().SetFloat("Attack.playbackRate", 1f);

            blastPosition = characterBody.corePosition;
            blastDamage = StaticValues.blastDashDamageCoefficient * damageStat;
            blastForce = StaticValues.blastDashForce;
            blastRadius = StaticValues.blastDashRadius;
            duration = StaticValues.blastDashDuration;
            fireTime = duration / 4f;
            blastType = DamageType.Generic;
            dashDirection = base.inputBank.moveVector;

            if(base.inputBank.moveVector == Vector3.zero)
            {
                dashDirection = characterDirection.forward;
            }


            //blast attack
            blastAttack = new BlastAttack();
            blastAttack.procCoefficient = 1f;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = 1000f;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = blastType;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            blastAttack.radius = blastRadius;
            blastAttack.baseDamage = blastDamage;
            blastAttack.position = blastPosition;
            blastAttack.baseForce = blastForce;

            AkSoundEngine.PostEvent("shootstyledashsfx", this.gameObject);

            this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));

            if(dekucon.WINDTRAIL.isStopped)
            {
                dekucon.WINDTRAIL.Play();
            }

            if (characterBody.characterMotor.isGrounded)
            {
                state = floatState.GROUND;

                characterDirection.forward = base.GetAimRay().direction;
                PlayAnimation("FullBody, Override", "Dodge", "Attack.playbackRate", duration, 0.01f);

                this.modelTransform = base.GetModelTransform();
                if (this.modelTransform)
                {
                    this.animator = this.modelTransform.GetComponent<Animator>();
                    this.characterModel = this.modelTransform.GetComponent<CharacterModel>();

                    //TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(new GameObject());
                    //temporaryOverlay.duration = 0.3f;
                    //temporaryOverlay.animateShaderAlpha = true;
                    //temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    //temporaryOverlay.destroyComponentOnEnd = true;
                    //temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");

                    //TemporaryOverlayInstance temporaryOverlay2 = TemporaryOverlayManager.AddOverlay(new GameObject());
                    //temporaryOverlay2.duration = 0.3f;
                    //temporaryOverlay2.animateShaderAlpha = true;
                    //temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    //temporaryOverlay2.destroyComponentOnEnd = true;
                    //temporaryOverlay2.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                    TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(new GameObject());
                    temporaryOverlay.duration = duration;
                    temporaryOverlay.animateShaderAlpha = true;
                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay.destroyComponentOnEnd = true;
                    temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matMercEvisTarget");

                }

                switch (level)
                {
                    case 0:
                        initialSpeedCoefficient = StaticValues.dodgeSpeed;
                        finalSpeedCoefficient = 0.1f;
                        characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, duration/4f);

                        break;
                    case 1:
                        initialSpeedCoefficient = StaticValues.dodgeSpeed * attackSpeedStat;
                        characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, duration / 3f);
                        finalSpeedCoefficient = 0.1f;
                        break;
                    case 2:
                        initialSpeedCoefficient = StaticValues.dodgeSpeed * attackSpeedStat;
                        characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, duration / 2f);
                        finalSpeedCoefficient = 0.1f;
                        break;
                }
            }
            else
            {
                state = floatState.AIR;

                characterBody.characterMotor.useGravity = false;

                characterDirection.forward = dashDirection;
                PlayCrossfade("FullBody, Override", "BlastDashF", "Attack.playbackRate", duration, 0.01f);

                blastAttack.Fire();

                EffectManager.SpawnEffect(Modules.DekuAssets.blastdashWindEffect, new EffectData
                {
                    origin = FindModelChild("RHand").position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(-dashDirection)


                }, true);

                EffectManager.SpawnEffect(Modules.DekuAssets.blastdashWindEffect, new EffectData
                {
                    origin = FindModelChild("LHand").position,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(-dashDirection)

                }, true);

                switch (level)
                {
                    case 0:
                        initialSpeedCoefficient = StaticValues.blastDashSpeed;
                        finalSpeedCoefficient = 0.1f;
                        break;
                    case 1:
                        initialSpeedCoefficient = StaticValues.blastDashSpeed * 2f;
                        finalSpeedCoefficient = 0.1f;
                        break;
                    case 2:
                        initialSpeedCoefficient = StaticValues.blastDashSpeed * 3f;
                        finalSpeedCoefficient = 0.1f;
                        break;
                }
            }



            //PlayAnimation("RightArm, Override", "RightArmOut", "Attack.playbackRate", 1f);

        }

        public override void OnExit()
        {
            base.OnExit();
            base.characterBody.characterMotor.useGravity = true;
            if (dekucon.WINDTRAIL.isPlaying)
            {
                dekucon.WINDTRAIL.Stop();
            }
        }

        private void RecalculateRollSpeed()
        {
            this.rollSpeed = this.moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, base.fixedAge / duration);
        }
        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(characterBody.characterDirection.forward);
            effectData.origin = origin;
            EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //PlayCrossfade("FullBody, Override", "DelawareSmashBaseCharge", "Attack.playbackRate", 1f, 0.01f);
            //PlayAnimation("RightArm, Override", "RightArmOut", "Attack.playbackRate", duration);

            switch (state)
            {
                case floatState.GROUND:
                    {
                        base.characterDirection.forward = base.GetAimRay().direction;
                        if (fixedAge  <= duration)
                        {
                            this.RecalculateRollSpeed();
                            //this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));


                            base.characterMotor.rootMotion += dashDirection * this.rollSpeed * Time.fixedDeltaTime;
                        }

                        if (fixedAge > duration)
                        {
                            this.outer.SetNextStateToMain();
                            return;
                        }
                    }
                break;
                case floatState.AIR:
                    {
                        base.characterDirection.forward = this.dashDirection;
                        if (fixedAge < fireTime)
                        {
                            characterBody.characterMotor.velocity.y = 0f;
                        }

                        if (fixedAge > fireTime && fixedAge <= duration)
                        {
                            Ray aimRay = base.GetAimRay();
                            this.RecalculateRollSpeed();
                            this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));

                            base.characterMotor.rootMotion += dashDirection * this.rollSpeed * Time.fixedDeltaTime;


                            //Vector3 normalized = dashDirection.normalized;
                            //if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
                            //{
                            //    Vector3 vector = normalized * this.rollSpeed * attackSpeedStat;

                            //    base.characterMotor.velocity = vector;
                            //}
                        }

                        if (fixedAge > duration)
                        {
                            this.outer.SetNextStateToMain();
                            return;
                        }
                    }
                break;

            }

            
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}