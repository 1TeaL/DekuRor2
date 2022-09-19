using DekuMod.Modules.Networking;
using DekuMod.Modules.Survivors;
using EntityStates;
using EntityStates.Merc;
using R2API.Networking;
using R2API.Networking.Interfaces;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{
    [R2APISubmoduleDependency(new string[]
    {
        "NetworkingAPI"
    })]
    public class ShootStyleKick100 : BaseSkill100
    {

        public float previousMass;
        private Vector3 dashDirection;
        private string muzzleString;

        public static float duration;
        public float numberOfHits; 
        public static float baseDuration = 0.5f;
        public static float initialSpeedCoefficient = 50f;
        public static float SpeedCoefficient;
        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;
        public static float procCoefficient = 1f;
        private Animator animator;

        private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/tracersmokeline/TracerMageIceLaser");
        private Transform modelTransform;
        private CharacterModel characterModel;
        private BulletAttack afterattack;
        private Ray aimRay;
        private float rollSpeed;
        private Vector3 forwardDirection;
        private Vector3 previousPosition;

        //checking location for networking
        public Vector3 origin;
        public Vector3 final;
        private Vector3 theSpot;
        public float dashSearchRadius;
        private BullseyeSearch search;

        public override void OnEnter()
        {

            base.OnEnter();

            duration = baseDuration / this.attackSpeedStat;
            numberOfHits = 5f * attackSpeedStat ;
    
            SpeedCoefficient = initialSpeedCoefficient;
            base.StartAimMode(duration, true);

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

            base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, baseDuration);

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

            search = new BullseyeSearch();
            origin = base.transform.position;
            new SpendHealthNetworkRequest(characterBody.masterObjectId, 0.1f).Send(NetworkDestination.Clients);


        }
        private void RecalculateRollSpeed()
        {
            this.rollSpeed = this.moveSpeedStat * SpeedCoefficient;
        }
        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(this.dashDirection);
            effectData.origin = origin;
            EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
        }

        public void ApplyComponent()
        {
            Ray aimRay = base.GetAimRay();
            theSpot = Vector3.Lerp(origin,final, 0.5f);
            BullseyeSearch search = new BullseyeSearch
            {

                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = theSpot,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = dashSearchRadius,
                maxAngleFilter = 360f
            };

            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);



            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget)
                {

                    ShootStyleKickComponent shootStyleKickComponent = characterBody.gameObject.GetComponent<ShootStyleKickComponent>();
                    
                    shootStyleKickComponent = characterBody.gameObject.AddComponent<ShootStyleKickComponent>();
                    shootStyleKickComponent.charbody = characterBody;
                    shootStyleKickComponent.numberOfHits = numberOfHits;
                    
                }
            }
        }

        public override void OnExit()
        {
            Ray aimRay = base.GetAimRay();
            base.PlayCrossfade("FullBody, Override", "ShootStyleDashExit", 0.2f);
            Util.PlaySound(EvisDash.endSoundString, base.gameObject);
            base.characterMotor.mass = this.previousMass;
            base.characterMotor.useGravity = true;
            base.characterMotor.velocity = Vector3.zero;
            if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
            base.characterMotor.disableAirControlUntilCollision = false;
            base.characterMotor.velocity.y = 0;

            final = base.transform.position;
            ApplyComponent();

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.RecalculateRollSpeed();
            this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));


            if (base.characterDirection) base.characterDirection.forward = this.forwardDirection;
            if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, base.fixedAge / duration);

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

            if (base.isAuthority && base.fixedAge >= duration)
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
