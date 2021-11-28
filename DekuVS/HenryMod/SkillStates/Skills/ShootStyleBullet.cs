using System;
using System.Collections.Generic;
using EntityStates;
using EntityStates.Bandit2.Weapon;
using EntityStates.Huntress;
using EntityStates.LemurianBruiserMonster;
using EntityStates.Merc;
using R2API.Utils;
using RoR2;
using RoR2.Audio;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{
    // Token: 0x02000003 RID: 3
    [R2APISubmoduleDependency(new string[]
    {
        "NetworkingAPI"
    })]
    public class ShootStyleBullet : BaseSkillState
    {

        public float previousMass;
        private Vector3 dashDirection;
        private string muzzleString;

        public static float duration = 0.2f;
        public static float initialSpeedCoefficient = 8f;
        public static float finalSpeedCoefficient = 0f;
        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;
        public static float procCoefficient = 1f;
        private Animator animator;

        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/tracersmokeline/TracerLaserTurbine");
        private Transform modelTransform;
        private CharacterModel characterModel;
        private BulletAttack afterattack;
        private Ray aimRay;
        private float rollSpeed;
        private Vector3 forwardDirection;
        private Vector3 previousPosition;


        public override void OnEnter()
        {

            base.OnEnter();

 
            AkSoundEngine.PostEvent(3842300745, this.gameObject);
            AkSoundEngine.PostEvent(573664262, this.gameObject);
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
            }
            base.PlayAnimation("FullBody, Override", "ShootStyleDash", "Attack.playbackRate", 0.1f);
            //EffectManager.SimpleMuzzleFlash(BlinkState.blinkPrefab, base.gameObject, this.muzzleString, false);
            //EffectManager.SimpleMuzzleFlash(Bandit2FireShiv.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
            //EffectManager.SimpleMuzzleFlash(Bandit2FireShiv.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
            //EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, this.muzzleString, true);

            //hasteleported = false;

            bool isAuthority = base.isAuthority;
            bool active = NetworkServer.active;


            base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, duration + 0.1f);


            // ray used to shoot position after teleporting
            uint bulletamount = (uint)(1U * this.attackSpeedStat);
            if (bulletamount > 20)
            {
                bulletamount = 20;
            }
            aimRay = base.GetAimRay();
            afterattack = new BulletAttack
            {
                bulletCount = bulletamount,
                aimVector = aimRay.direction,
                origin = aimRay.origin,
                damage = Modules.StaticValues.shootbulletDamageCoefficient * this.damageStat,
                damageColorIndex = DamageColorIndex.Default,
                damageType = (DamageType.Generic),
                falloffModel = BulletAttack.FalloffModel.None,
                maxDistance = initialSpeedCoefficient * duration * this.moveSpeedStat,
                force = 55f,
                procCoefficient = procCoefficient,
                minSpread = 0f,
                maxSpread = 0f,
                isCrit = base.RollCrit(),
                owner = base.gameObject,
                hitMask = LayerIndex.CommonMasks.bullet,
                muzzleName = this.muzzleString,
                smartCollision = true,
                procChainMask = default(ProcChainMask),
                radius = 3f,
                sniper = false,
                stopperMask = LayerIndex.noCollision.mask,
                tracerEffectPrefab = ShootStyleBullet.tracerEffectPrefab,
                spreadPitchScale = 0f,
                spreadYawScale = 0f,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                hitEffectPrefab = Evis.hitEffectPrefab

            };
            EffectManager.SimpleMuzzleFlash(Evis.hitEffectPrefab, base.gameObject, this.muzzleString, false);
            EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, this.muzzleString, true);
            EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, this.muzzleString, false);
            EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, this.muzzleString, false);
            this.muzzleString = "LFoot";

            base.characterMotor.useGravity = false;
            this.previousMass = base.characterMotor.mass;
            base.characterMotor.mass = 0f;


            this.RecalculateRollSpeed();

            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity = this.aimRay.direction * this.rollSpeed;
            }

            Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
            this.previousPosition = base.transform.position - b;



        }
        private void RecalculateRollSpeed()
        {
            this.rollSpeed = this.moveSpeedStat * ShootStyleBullet.initialSpeedCoefficient;
        }
        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(this.dashDirection);
            effectData.origin = origin;
            EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
        }

        public override void OnExit()
        {

            if(afterattack != null)
            {
                afterattack.Fire();
            }
            base.PlayAnimation("FullBody, Override", "ShootStyleDashExit", "Attack.playbackRate", 0.2f);
            Util.PlaySound(EvisDash.endSoundString, base.gameObject);
            base.characterMotor.mass = this.previousMass;
            base.characterMotor.useGravity = true;
            base.characterMotor.velocity = Vector3.zero;
            EffectManager.SimpleMuzzleFlash(Bandit2FireShiv.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
            EffectManager.SimpleMuzzleFlash(Bandit2FireShiv.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
            if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
            base.characterMotor.disableAirControlUntilCollision = false;
            base.characterMotor.velocity.y = 0;

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.RecalculateRollSpeed();
            this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));
            //if (!hasteleported)
            //{
            //    base.characterMotor.rootMotion += afterattack.maxDistance * aimRay.direction.normalized;
            //    hasteleported = true;
            //}

            if (base.characterDirection) base.characterDirection.forward = this.forwardDirection;
            if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = Mathf.Lerp(ShootStyleBullet.dodgeFOV, 60f, base.fixedAge / ShootStyleBullet.duration);

            Vector3 normalized = (base.transform.position - this.previousPosition).normalized;
            if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
            {
                Vector3 vector = normalized * this.rollSpeed;
                float d = Mathf.Max(Vector3.Dot(vector, this.aimRay.direction), 0f);
                vector = this.aimRay.direction * d;

                base.characterMotor.velocity = vector;
            }
            this.previousPosition = base.transform.position;

            if (this.modelTransform)
            {
                TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = 0.6f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
                temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay2.duration = 0.7f;
                temporaryOverlay2.animateShaderAlpha = true;
                temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay2.destroyComponentOnEnd = true;
                temporaryOverlay2.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashExpanded");
                temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
            }

            if (base.isAuthority && base.fixedAge >= ShootStyleBullet.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.forwardDirection = reader.ReadVector3();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }


    }
}
