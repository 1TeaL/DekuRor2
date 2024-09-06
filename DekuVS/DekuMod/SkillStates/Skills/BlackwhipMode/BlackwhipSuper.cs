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
using R2API;
using System.Net;

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
        private float duration;
        private float fireTime;
        public BullseyeSearch search;
        private CharacterBody enemyBody;

        private GameObject areaIndicator;

        private bool animChange;
		private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        private EffectData effectData;
        private EffectData effectData2;
        private Animator animator;
        private ChildLocator child;
        private float previousMass;
        private float range;
        private float angle;
        private float elapsedTime;
        private GameObject blackwhipLineEffect;
        private LineRenderer blackwhipLineRenderer;

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
            //EffectManager.SimpleMuzzleFlash(Modules.Asset.dekuKickEffect, base.gameObject, "Swing1", true);



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

            DamageAPI.AddModdedDamageType(blastAttack, Damage.blackwhipImmobilise);

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
            search = new BullseyeSearch();
            search.teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam());
            search.filterByLoS = false;
            search.searchOrigin = blastPosition;
            search.searchDirection = base.GetAimRay().direction;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.maxDistanceFilter = blastRadius;
            search.maxAngleFilter = angle;


            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);



            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget.healthComponent.body && singularTarget.healthComponent)
                {
                    BlackwhipComponent blackwhipComponent = singularTarget.healthComponent.body.gameObject.GetComponent<BlackwhipComponent>();


                    if (blackwhipComponent)
                    {
                        //switch (state)
                        //{
                        //    case superState.SUPER1:
                        //        break;
                        //    case superState.SUPER2:
                        //        break;
                        //    case superState.SUPER3:
                        //        break;
                        //}
                    }
                    if (!blackwhipComponent)
                    {
                        blackwhipComponent = singularTarget.healthComponent.body.gameObject.AddComponent<BlackwhipComponent>();
                        blackwhipComponent.totalDuration = duration;
                        blackwhipComponent.dekucharbody = characterBody;
                        blackwhipComponent.charbody = singularTarget.healthComponent.body;

                        switch (state)
                        {
                            case superState.SUPER1:
                                new BlackwhipImmobilizeRequest(singularTarget.healthComponent.body.masterObjectId, StaticValues.blackwhipOverdriveDamage * damageStat, characterBody.masterObjectId).Send(NetworkDestination.Clients);
                                break;
                            case superState.SUPER2:
                                break;
                            case superState.SUPER3:
                                new BlackwhipImmobilizeRequest(singularTarget.healthComponent.body.masterObjectId, 0f, characterBody.masterObjectId).Send(NetworkDestination.Clients);
                                //SetStateOnHurt component = singularTarget.healthComponent.GetComponent<SetStateOnHurt>();
                                //bool flag = component == null;
                                //if (!component)
                                //{
                                //    if (component.canBeFrozen && NetworkServer.active)
                                //    {
                                //        component.SetFrozen(3f);
                                //        bool flag2 = singularTarget.healthComponent.body.characterMotor;
                                //        if (flag2)
                                //        {
                                //            singularTarget.healthComponent.body.characterMotor.velocity = Vector3.zero;
                                //        }
                                //    }
                                //}
                            break;
                        }

                    }

                }
            }
        }

        protected override void NeutralSuper()
		{
			base.NeutralSuper();
            //play animation of quick blackwhip infront of him- immobilize enemies in front of him
            range = StaticValues.blackwhipOverdriveRange;
            blastRadius = StaticValues.blackwhipOverdriveRange * attackSpeedStat;
            duration = StaticValues.blackwhipOverdriveDuration;
            fireTime = duration / 2f;
            angle = StaticValues.blackwhipOverdriveAngle;
            blastPosition = characterBody.corePosition;
        }

        protected override void BackwardSuper()
        {
			base.BackwardSuper();

            angle = StaticValues.blackwhipOverdriveAngle2;
            range = StaticValues.blackwhipOverdriveRange2 * attackSpeedStat;
            duration = StaticValues.blackwhipOverdriveDuration2;
            blastRadius = StaticValues.blackwhipOverdriveRadius2 * attackSpeedStat;
            blastDamage = StaticValues.blackwhipOverdriveDamage2 * damageStat * attackSpeedStat;
            blastForce = StaticValues.blackwhipOverdriveForce2 * attackSpeedStat;
            blastType = DamageType.Stun1s;
            //set around deku
            blastPosition = characterBody.corePosition;
            //indicator
            this.areaIndicator = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);
            duration = StaticValues.blackwhipOverdriveDuration2;
            fireTime = duration / 2f;
            //play animation of arm reaching out and pulling everyone in to spot infront
        }
        protected override void ForwardSuper()
        {
            base.ForwardSuper();
            child = base.GetModelChildLocator();

            angle = StaticValues.blackwhipOverdriveAngle3;
            range = StaticValues.blackwhipOverdriveRange3 * attackSpeedStat;
            duration = StaticValues.blackwhipOverdriveDuration3;
            blastRadius = StaticValues.blackwhipOverdriveRadius3 * attackSpeedStat;
            blastDamage = StaticValues.blackwhipOverdriveDamage3 * damageStat * attackSpeedStat;
            blastForce = StaticValues.blackwhipOverdriveForce3 * attackSpeedStat;

            base.characterMotor.useGravity = false;
            this.previousMass = base.characterMotor.mass;
            base.characterMotor.mass = 0f;
            base.characterMotor.disableAirControlUntilCollision = false;

            RaycastHit raycastHit;
            bool raycast = Physics.Raycast(base.GetAimRay().origin, base.GetAimRay().direction, out raycastHit, range, LayerIndex.world.mask);

            if (dekucon.GetTrackingTarget())
            {
                blastPosition = dekucon.GetTrackingTarget().transform.position;
                enemyBody = dekucon.trackingTarget.healthComponent.body;
            }
            else if (raycast)
            {
                blastPosition = raycastHit.point;
            }
            else
            {
                blastPosition = base.GetAimRay().direction * range;
            }


            blackwhipLineEffect = UnityEngine.Object.Instantiate(Modules.Asset.blackwhipLineRenderer, child.FindChild("RHand").transform);
            blackwhipLineRenderer = blackwhipLineEffect.GetComponent<LineRenderer>();
            Chat.AddMessage("blastPosition" + blastPosition);


            //if (base.isAuthority && base.inputBank && base.characterDirection)
            //{
            //    this.forwardDirection = base.GetAimRay().direction;
            //}
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
                        blastAttack.Fire();
                    }



                    if ((base.fixedAge >= this.duration && base.isAuthority))
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    break;
                case superState.SUPER3:

                    //characterBody.characterMotor.velocity = Vector3.zero;
                    //if(base.isAuthority && base.fixedAge > fireTime && !hasFired)
                    //{
                    //    hasFired = true;
                    //    characterBody.characterMotor.Motor.SetPositionAndRotation(blastPosition, Quaternion.LookRotation(forwardDirection));
                    //    //play animation of pose
                    //}
                    if (enemyBody)
                    {
                        if (enemyBody.characterMotor)
                        {
                            enemyBody.characterMotor.Motor.SetPosition(blastPosition);
                        }
                        else if (enemyBody.rigidbody)
                        {
                            enemyBody.rigidbody.MovePosition(blastPosition);
                        }
                    }

                    if (Vector2.Distance(blastPosition, characterBody.corePosition) > 2f)
                    {
                        Vector3 normalized = (blastPosition - characterBody.corePosition).normalized;
                        if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
                        {
                            Vector3 vector = normalized * moveSpeedStat * attackSpeedStat * StaticValues.blackwhipOverdriveSpeed3;
                            characterBody.characterDirection.forward = normalized;
                            base.characterMotor.velocity = vector;
                        }
                    }
                    else if (Vector2.Distance(blastPosition, characterBody.corePosition) <= 2f)
                    {
                        blastAttack.Fire();
                        this.outer.SetNextStateToMain();
                        return;

                    }

                    if(base.fixedAge > duration)
                    {
                        blastAttack.Fire();
                        this.outer.SetNextStateToMain();
                        return;

                    }

                    this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));

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

                    //if (base.isAuthority && base.fixedAge >= duration)
                    //{

                    //    blastAttack.Fire();
                    //    this.outer.SetNextStateToMain();
                    //    return;
                    //}

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

                    if (elapsedTime < fireTime)
                    {
                        elapsedTime += Time.deltaTime; // Increment the elapsed time
                    }
                    float t = Mathf.Clamp01(elapsedTime / fireTime); // Calculate the interpolation factor (0 to 1)

                    Vector3[] vector3s = new Vector3[2];
                    Vector3 startPoint = child.FindChild("RHand").transform.position;
                        
                    // Lerp from the start point to the end point
                    Vector3 lerpedPosition = Vector3.Lerp(startPoint, blastPosition, t);

                    vector3s[0] = startPoint;
                    vector3s[1] = lerpedPosition;
                    blackwhipLineRenderer.SetPositions(vector3s);

                    
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

            }
            base.characterBody.characterMotor.useGravity = true;
            base.characterBody.inputBank.enabled = true;

            switch (state)
            {
                case superState.SUPER1:

                    break;
                case superState.SUPER2:
                    break;
                case superState.SUPER3:

                    Util.PlaySound(EvisDash.endSoundString, base.gameObject);

                    base.characterMotor.mass = this.previousMass;
                    base.characterMotor.useGravity = true;
                    base.characterMotor.velocity = Vector3.zero;

                    if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
                    base.characterMotor.disableAirControlUntilCollision = false;
                    base.characterMotor.velocity.y = 0;
                break;

            }
            //switch (level)
            //{
            //    case 0:
            //        break;
            //    case 1:
            //        break;
            //    case 2:

            //        break;
            //}
            //if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
        }
        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Death;
		}
	}
}