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

        private GameObject muzzlePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/tracersmokeline/TracerLaserTurbineReturn");
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

            aimRayDir = aimRay.direction;
            duration = baseDuration / attackSpeedStat;
            if (duration < 0.2f)
            {
                duration = 0.2f;
            }
            speedattack = attackSpeedStat / 3;
            if (speedattack < 1)
            {
                speedattack = 1;
            }
            StartAimMode(duration, true);

            SpeedCoefficient = initialSpeedCoefficient;

            AkSoundEngine.PostEvent(3842300745, gameObject);
            AkSoundEngine.PostEvent(573664262, gameObject);
            modelTransform = GetModelTransform();
            if (modelTransform)
            {
                animator = modelTransform.GetComponent<Animator>();
                characterModel = modelTransform.GetComponent<CharacterModel>();
            }
            //base.PlayAnimation("FullBody, Override", "ShootStyleDash", "Attack.playbackRate", 0.1f);
            PlayAnimation("FullBody, Override", "ShootStyleKick", "Attack.playbackRate", 0.1f);

            //hasteleported = false;

            bool isAuthority = base.isAuthority;
            bool active = NetworkServer.active;


            characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, duration / 2);

            if (NetworkServer.active && healthComponent)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = healthComponent.fullCombinedHealth * 0.01f;
                damageInfo.position = transform.position;
                damageInfo.force = Vector3.zero;
                damageInfo.damageColorIndex = DamageColorIndex.Default;
                damageInfo.crit = false;
                damageInfo.attacker = null;
                damageInfo.inflictor = null;
                damageInfo.damageType = DamageType.NonLethal | DamageType.BypassArmor;
                damageInfo.procCoefficient = 0f;
                damageInfo.procChainMask = default;
                healthComponent.TakeDamage(damageInfo);
            }

            // ray used to shoot position after teleporting
            uint bulletamount = (uint)(1U * attackSpeedStat);
            if (bulletamount > 20)
            {
                bulletamount = 20;
            }
            aimRay = GetAimRay();
            afterattack = new BulletAttack
            {
                bulletCount = bulletamount,
                aimVector = aimRay.direction,
                origin = aimRay.origin,
                damage = Modules.StaticValues.shootbulletDamageCoefficient * damageStat,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.None,
                maxDistance = SpeedCoefficient * duration * moveSpeedStat * speedattack,
                force = 55f,
                procCoefficient = procCoefficient,
                minSpread = 0f,
                maxSpread = 0f,
                isCrit = RollCrit(),
                owner = gameObject,
                hitMask = LayerIndex.CommonMasks.bullet,
                muzzleName = muzzleString,
                smartCollision = true,
                procChainMask = default,
                radius = 3f,
                sniper = false,
                stopperMask = LayerIndex.noCollision.mask,
                tracerEffectPrefab = tracerEffectPrefab,
                spreadPitchScale = 0f,
                spreadYawScale = 0f,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                hitEffectPrefab = Evis.hitEffectPrefab

            };
            muzzleString = "LFoot";
            EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, gameObject, muzzleString, false);
            EffectManager.SimpleMuzzleFlash(muzzlePrefab, gameObject, muzzleString, false);

            characterMotor.useGravity = false;
            previousMass = characterMotor.mass;
            characterMotor.mass = 0f;


            RecalculateRollSpeed();

            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = aimRay.direction * rollSpeed;
            }

            Vector3 b = characterMotor ? characterMotor.velocity : Vector3.zero;
            previousPosition = transform.position - b;



        }
        private void RecalculateRollSpeed()
        {
            rollSpeed = moveSpeedStat * SpeedCoefficient * speedattack;
        }
        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(aimRayDir);
            effectData.origin = origin;
            EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
        }

        public override void OnExit()
        {

            if (afterattack != null)
            {
                afterattack.Fire();
            }
            //base.PlayAnimation("FullBody, Override", "ShootStyleDashExit", "Attack.playbackRate", 0.2f);
            PlayCrossfade("FullBody, Override", "ShootStyleDashExit", 0.2f);
            Util.PlaySound(EvisDash.endSoundString, gameObject);
            characterMotor.mass = previousMass;
            characterMotor.useGravity = true;
            characterMotor.velocity = Vector3.zero;
            if (cameraTargetParams) cameraTargetParams.fovOverride = -1f;
            characterMotor.disableAirControlUntilCollision = false;
            characterMotor.velocity.y = 0;

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            RecalculateRollSpeed();
            CreateBlinkEffect(Util.GetCorePosition(gameObject));


            if (characterDirection) characterDirection.forward = aimRayDir;
            if (cameraTargetParams) cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);

            Vector3 normalized = (transform.position - previousPosition).normalized;
            if (characterMotor && characterDirection && normalized != Vector3.zero)
            {
                Vector3 vector = normalized * rollSpeed;
                float d = Mathf.Max(Vector3.Dot(vector, aimRay.direction), 0f);
                vector = aimRay.direction * d;

                characterMotor.velocity = vector;
            }
            previousPosition = transform.position;

            if (modelTransform)
            {
                //TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                //temporaryOverlay.duration = 0.6f;
                //temporaryOverlay.animateShaderAlpha = true;
                //temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                //temporaryOverlay.destroyComponentOnEnd = true;
                //temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                //temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
                //TemporaryOverlay temporaryOverlay2 = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                //temporaryOverlay2.duration = 0.7f;
                //temporaryOverlay2.animateShaderAlpha = true;
                //temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                //temporaryOverlay2.destroyComponentOnEnd = true;
                //temporaryOverlay2.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                //temporaryOverlay2.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
            }

            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            forwardDirection = reader.ReadVector3();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }


    }
}
