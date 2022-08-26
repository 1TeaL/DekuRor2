using EntityStates;
using EntityStates.Merc;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{
    [R2APISubmoduleDependency(new string[]
    {
        "NetworkingAPI"
    })]
    public class ShootStyleBullet : BaseSkillState
    {

        public float previousMass;
        private Vector3 dashDirection;
        private string muzzleString;

        public static float speedattack;
        public static float duration;
        public static float baseDuration = 0.4f;
        public static float initialSpeedCoefficient = 3f;
        public static float SpeedCoefficient;
        public static float finalSpeedCoefficient = 0f;
        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;
        public static float procCoefficient = 1f;
        private Animator animator;

        private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/tracersmokeline/TracerLaserTurbineReturn");
        private Transform modelTransform;
        private CharacterModel characterModel;
        private BulletAttack afterattack;
        private Ray aimRay;
        private float rollSpeed;
        private Vector3 forwardDirection;
        private Vector3 previousPosition;

        private Vector3 aimRayDir;


        public override void OnEnter()
        {

            base.OnEnter();

            this.aimRayDir = aimRay.direction;
            duration = baseDuration / this.attackSpeedStat;
            if (duration < 0.2f)
            {
                duration = 0.2f;
            }
            speedattack = this.attackSpeedStat / 3;
            if (speedattack < 1)
            {
                speedattack = 1;
            }
            base.StartAimMode(duration, true);

            SpeedCoefficient = initialSpeedCoefficient;
 
            AkSoundEngine.PostEvent(3842300745, this.gameObject);
            AkSoundEngine.PostEvent(573664262, this.gameObject);
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
            }
            //base.PlayAnimation("FullBody, Override", "ShootStyleDash", "Attack.playbackRate", 0.1f);
            base.PlayAnimation("FullBody, Override", "ShootStyleKick", "Attack.playbackRate", 0.1f);

            //hasteleported = false;

            bool isAuthority = base.isAuthority;
            bool active = NetworkServer.active;


            base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, duration/2);

            if (NetworkServer.active && base.healthComponent)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.01f;
                damageInfo.position = base.transform.position;
                damageInfo.force = Vector3.zero;
                damageInfo.damageColorIndex = DamageColorIndex.Default;
                damageInfo.crit = false;
                damageInfo.attacker = null;
                damageInfo.inflictor = null;
                damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
                damageInfo.procCoefficient = 0f;
                damageInfo.procChainMask = default(ProcChainMask);
                base.healthComponent.TakeDamage(damageInfo);
            }

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
                maxDistance = SpeedCoefficient * duration * this.moveSpeedStat * speedattack,
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
            this.muzzleString = "LFoot";
            EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, this.muzzleString, false);
            EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, this.muzzleString, false);

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
            this.rollSpeed = this.moveSpeedStat * ShootStyleBullet.SpeedCoefficient * speedattack;
        }
        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(this.aimRayDir);
            effectData.origin = origin;
            EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
        }

        public override void OnExit()
        {

            if(afterattack != null)
            {
                afterattack.Fire();
            }
            //base.PlayAnimation("FullBody, Override", "ShootStyleDashExit", "Attack.playbackRate", 0.2f);
            base.PlayCrossfade("FullBody, Override", "ShootStyleDashExit", 0.2f);
            Util.PlaySound(EvisDash.endSoundString, base.gameObject);
            base.characterMotor.mass = this.previousMass;
            base.characterMotor.useGravity = true;
            base.characterMotor.velocity = Vector3.zero;
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


            if (base.characterDirection) base.characterDirection.forward = this.aimRayDir;
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
                temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay2.duration = 0.7f;
                temporaryOverlay2.animateShaderAlpha = true;
                temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay2.destroyComponentOnEnd = true;
                temporaryOverlay2.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
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
