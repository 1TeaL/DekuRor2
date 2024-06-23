using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using DekuMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EntityStates.Merc;
using DekuMod.Modules;
using AK.Wwise;
using System;
using static UnityEngine.ParticleSystem.PlaybackState;
using EntityStates.Huntress;
using EntityStates.Loader;
using static UnityEngine.UI.Image;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;

namespace DekuMod.SkillStates.BlackWhip
{

	public class BlackwhipSuper : BaseSpecial
	{
		private bool hasFired;

		private BlastAttack blastAttack;
        private float blastRadius;
        private float blastDamage;
        private Vector3 blastPosition;
        private float blastForce;
        private DamageType blastType = DamageType.Generic;

        private Transform modelTransform;
        private CharacterModel characterModel;
        private Vector3 theSpot;
        private readonly BullseyeSearch search = new BullseyeSearch();
        private float duration;
        private float fireTime;

        private GameObject areaIndicator;

        private bool animChange;
		private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        private EffectData effectData;
        private EffectData effectData2;
        private Animator animator;
        private float previousMass;
        private float range;
        private float angle;
        private Vector3 moveDirection;
        private bool isMove;
        private Vector3 forwardDirection;

        public override void OnEnter()
		{
			base.OnEnter();
            this.animator = base.GetModelAnimator();
            base.GetModelAnimator().SetFloat("Attack.playbackRate", 1f);

			Ray aimRay = base.GetAimRay();
			characterBody.SetAimTimer(2f);
            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent("stlouisvoice", this.gameObject);
            }
            AkSoundEngine.PostEvent("stlouissfx", this.gameObject);


            this.animator = base.GetModelAnimator();


            //PlayCrossfade("FullBody, Override", "StLouis45", "Attack.playbackRate", 1f, 0.01f);
            //EffectManager.SimpleMuzzleFlash(Modules.Assets.dekuKickEffect, base.gameObject, "Swing1", true);



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

            effectData = new EffectData
            {
                scale = blastRadius,
                origin = blastPosition,
                rotation = Quaternion.LookRotation(new Vector3(base.GetAimRay().direction.x, base.GetAimRay().direction.y, base.GetAimRay().direction.z)),
            };
            effectData2 = new EffectData
            {
                scale = attackSpeedStat,
                origin = blastPosition,
                rotation = Quaternion.LookRotation(new Vector3(base.GetAimRay().direction.x, base.GetAimRay().direction.y, base.GetAimRay().direction.z)),
            };

        }

        public void ApplyComponent()
        {
            search.teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam());
            search.filterByLoS = false;
            search.searchOrigin = blastPosition;
            search.searchDirection = base.GetAimRay().direction;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.maxDistanceFilter = range;
            search.maxAngleFilter = angle;


            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);



            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget.healthComponent.body && singularTarget.healthComponent)
                {
                    //freeze enemies
                    BlackwhipComponent blackwhipComponent = singularTarget.healthComponent.body.gameObject.GetComponent<BlackwhipComponent>();

                    if (isMove)
                    {
                        moveDirection = (singularTarget.healthComponent.body.corePosition - blastPosition).normalized;
                    }

                    if (blackwhipComponent)
                    {

                    }
                    if (!blackwhipComponent)
                    {
                        blackwhipComponent = singularTarget.healthComponent.body.gameObject.AddComponent<BlackwhipComponent>();
                        blackwhipComponent.totalDuration = duration;
                        blackwhipComponent.moveDirection = moveDirection;
                        blackwhipComponent.pushDamage = isMove;

                    }

                }
            }
        }

        protected override void NeutralSuper()
		{
			base.NeutralSuper();
            //play animation of quick punch
            range = StaticValues.blackwhipOverdriveRange;
            duration = StaticValues.blackwhipOverdriveDuration;
            fireTime = duration / 2f;
            angle = StaticValues.blackwhipOverdriveAngle;
            isMove = false;
            moveDirection = Vector3.zero;
        }

        protected override void BackwardSuper()
        {
			base.BackwardSuper();

            isMove = true;
            angle = StaticValues.blackwhipOverdriveAngle2;
            range = StaticValues.blackwhipOverdriveRange2 * attackSpeedStat;
            duration = StaticValues.blackwhipOverdriveDuration2;
            blastRadius = StaticValues.blackwhipOverdriveRadius2 * attackSpeedStat;
            blastDamage = StaticValues.blackwhipOverdriveDamage2 * damageStat * attackSpeedStat;
            //set around deku
            blastPosition = characterBody.corePosition + base.GetAimRay().direction * StaticValues.blackwhipOverdriveRange2;
            //indicator
            this.areaIndicator = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);
            duration = StaticValues.blackwhipOverdriveDuration2;
            fireTime = duration / 2f;
            //play animation of expanding out and pulling everyone in to spot infront
        }
        protected override void ForwardSuper()
        {
            if (!dekucon.Target)
            {
                this.outer.SetNextStateToMain();
                return;
            }

            base.ForwardSuper();

            isMove = false;
            angle = StaticValues.blackwhipOverdriveAngle3;
            range = StaticValues.blackwhipOverdriveRange3 * attackSpeedStat;
            duration = StaticValues.blackwhipOverdriveDuration3;
            blastRadius = StaticValues.blackwhipOverdriveRange3 * attackSpeedStat;
            blastDamage = StaticValues.blackwhipOverdriveDamage3 * damageStat * attackSpeedStat;
            blastForce = StaticValues.blackwhipOverdriveForce3 * attackSpeedStat;
            blastPosition = dekucon.Target.transform.position;

            base.characterMotor.useGravity = false;
            this.previousMass = base.characterMotor.mass;
            base.characterMotor.mass = 0f;
            base.characterMotor.disableAirControlUntilCollision = false;


            if (base.isAuthority && base.inputBank && base.characterDirection)
            {
                this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            }
            ApplyComponent();

            //play animation of beginning to jump and fly
        }
        public override void FixedUpdate()
		{
			base.FixedUpdate();

			switch(state)
			{
				case superState.SUPER1:

                    if(base.fixedAge > fireTime && !hasFired)
                    {
                        hasFired= true;
                        ApplyComponent();
                    }
                    


                    if ((base.fixedAge >= this.duration && base.isAuthority))
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    break;
				case superState.SUPER2:
                    if (base.fixedAge > fireTime && !hasFired)
                    {
                        hasFired = true;
                        ApplyComponent();
                    }



                    if ((base.fixedAge >= this.duration && base.isAuthority))
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    break;
                case superState.SUPER3:

                    if(base.isAuthority && base.fixedAge > fireTime && !hasFired)
                    {
                        hasFired = true;
                        characterBody.characterMotor.Motor.SetPositionAndRotation(blastPosition + forwardDirection* blastRadius, Quaternion.LookRotation(forwardDirection));
                        //play animation of pose
                    }

                    this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));


                    if (base.characterDirection) base.characterDirection.forward = this.forwardDirection;
                    if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = Mathf.Lerp(EntityStates.Commando.DodgeState.dodgeFOV, 60f, base.fixedAge / duration);


                    if (this.modelTransform)
                    {
                        TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                        temporaryOverlay.duration = duration;
                        temporaryOverlay.animateShaderAlpha = true;
                        temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                        temporaryOverlay.destroyComponentOnEnd = true;
                        temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                        temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                        TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                        temporaryOverlay2.duration = duration;
                        temporaryOverlay2.animateShaderAlpha = true;
                        temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                        temporaryOverlay2.destroyComponentOnEnd = true;
                        temporaryOverlay2.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                        temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                    }

                    if (base.isAuthority && base.fixedAge >= duration)
                    {

                        blastAttack.Fire();
                        this.outer.SetNextStateToMain();
                        return;
                    }

                    break;
			}

			
		}




        public override void Update()
        {
            base.Update();
            //indicator update
            switch (state)
            {
                case superState.SUPER1:
                    
                    break;
                case superState.SUPER2:
                    if (this.areaIndicator)
                    {
                        this.areaIndicator.transform.localScale = Vector3.one * blastRadius * (attackSpeedStat);
                        this.areaIndicator.transform.localPosition = blastPosition;
                    }
                    break;
                case superState.SUPER3:
                    
                    break;

            }
        }


        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(characterBody.characterDirection.forward);
            effectData.origin = origin;
            EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (areaIndicator)
            {
                this.areaIndicator.SetActive(false);
                EntityState.Destroy(this.areaIndicator);
                base.characterBody.characterMotor.useGravity = true;
                base.characterBody.inputBank.enabled = true;

            }
            switch(level)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    Util.PlaySound(EvisDash.endSoundString, base.gameObject);

                    base.characterMotor.mass = this.previousMass;
                    base.characterMotor.useGravity = true;
                    base.characterMotor.velocity = Vector3.zero;

                    if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
                    base.characterMotor.disableAirControlUntilCollision = false;
                    base.characterMotor.velocity.y = 0;

                    ApplyComponent();
                    break;
            }
            //if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
        }
        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Death;
		}
	}
}