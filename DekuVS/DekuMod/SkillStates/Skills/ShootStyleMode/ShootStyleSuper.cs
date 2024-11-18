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
using DekuMod.SkillStates.BlackWhip;
using UnityEngine.UIElements;
using static RoR2.CameraTargetParams;
using HG;

namespace DekuMod.SkillStates.ShootStyle
{

	public class ShootStyleSuper : BaseSpecial
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
        public static float initialSpeedCoefficient = StaticValues.stlouis3InitialSpeed;
        public static float SpeedCoefficient;
        private Vector3 forwardDirection;
        private Vector3 previousPosition;
        public Vector3 origin;
        public Vector3 final;
        private readonly BullseyeSearch search = new BullseyeSearch();
        private readonly BullseyeSearch blackwhipSearch = new BullseyeSearch();
        public int numberOfHits;


        private bool animChange;
		private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        private EffectData effectData;
        private EffectData effectData2;
        private Animator animator;
        private float attackTimer;
        private float duration;
        private bool hasDashed;
        private float firetime;
        private int totalHits1;
        private float timer1;
        private float previousMass;
        private float angle;

        private GameObject aimSphere;
        private HurtBoxGroup hurtboxGroup;

        private CharacterCameraParamsData super1CameraParams = new CharacterCameraParamsData()
        {
            maxPitch = 70,
            minPitch = -70,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = new Vector3(0, 0.0f, -40f),
            wallCushion = 0.1f,
        };
        private CharacterCameraParamsData super2CameraParams = new CharacterCameraParamsData()
        {
            maxPitch = 70,
            minPitch = -70,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = new Vector3(0, 50f, -100f),
            wallCushion = 0.1f,
        };
        private CharacterCameraParamsData super3CameraParams = new CharacterCameraParamsData()
        {
            maxPitch = 70,
            minPitch = -70,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = new Vector3(0, 0.0f, -100f),
            wallCushion = 0.1f,
        };

        private CameraParamsOverrideHandle camOverrideHandle;
        public enum stlouis3State { STARTUP, DASH, ATTACK};
        public stlouis3State stlouis3;

        public override void OnEnter()
		{
			base.OnEnter();
            this.animator = base.GetModelAnimator();
            base.GetModelAnimator().SetFloat("Attack.playbackRate", 1f);

			Ray aimRay = base.GetAimRay();
			characterBody.SetAimTimer(2f);


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

		protected override void NeutralSuper()
		{
			base.NeutralSuper();
            characterDirection.forward = base.GetAimRay().direction;

            blastRadius = StaticValues.stlouisRadius * attackSpeedStat;
            blastDamage = StaticValues.stlouisDamageCoefficient * damageStat * attackSpeedStat;
			//set in front of deku exactly
			blastPosition = characterBody.corePosition + (base.GetAimRay().direction* (StaticValues.stlouisRadius * attackSpeedStat * 0.5f));
            blastForce = 0f;
            duration = StaticValues.stlouisDuration;
            //play animation of kick

            GetModelAnimator().SetFloat("Attack.playbackRate", 1f);
            PlayCrossfade("FullBody, Override", "StLouis1", "Attack.playbackRate", duration, 0.1f);
            if (base.isAuthority && Config.allowVoice.Value)
            {
                AkSoundEngine.PostEvent("stlouisvoice", this.gameObject);
            }
            AkSoundEngine.PostEvent("stlouissfx", this.gameObject);

            EffectManager.SpawnEffect(Modules.DekuAssets.dekuLeftKickEffect, new EffectData
            {
                origin = characterBody.corePosition,
                scale = 1f,
                rotation = Util.QuaternionSafeLookRotation(base.GetAimRay().direction)

            }, true);
            dekucon.RLEG.Play();

            switch (level)
            {
                case 0:
                    totalHits1 = Mathf.RoundToInt(StaticValues.stlouisTotalHits * attackSpeedStat);
                    break;
                case 1:
                    initialSpeedCoefficient *= StaticValues.stlouis3Level2Multiplier;
                    totalHits1 = Mathf.RoundToInt(StaticValues.stlouisTotalHits * attackSpeedStat * StaticValues.stlouis3Level2Multiplier);
                    break;
                case 2:
                    initialSpeedCoefficient *= StaticValues.stlouis3Level3Multiplier;
                    totalHits1 = Mathf.RoundToInt(StaticValues.stlouisTotalHits * attackSpeedStat * StaticValues.stlouis3Level3Multiplier);
                    break;
            }

        }
        protected override void BackwardSuper()
        {
			base.BackwardSuper();
            characterDirection.forward = base.GetAimRay().direction;

            //characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, 1);

            //indicator
            this.aimSphere = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.aimSphere.SetActive(true);
            duration = StaticValues.stlouisDuration2;
            firetime = duration * 0.5f;
            blastRadius = StaticValues.stlouisRadius2 * attackSpeedStat;
            blastPosition = base.GetAimRay().origin + base.GetAimRay().direction * blastRadius * 2f;


            //play animation of blackwhip lift up and slam
            GetModelAnimator().SetFloat("Attack.playbackRate", 1f);
            PlayCrossfade("FullBody, Override", "BlackwhipSmash", "Attack.playbackRate", duration, 0.1f);

            dekucon.RARM.Play();
            dekucon.LARM.Play();
            //if (base.isAuthority && Config.allowVoice.Value)
            //{
            //    AkSoundEngine.PostEvent("stlouisvoice", this.gameObject);
            //}
            AkSoundEngine.PostEvent("blackwhipsfx", this.gameObject);

            switch (level)
            {
                case 0:
                    angle = 60f;
                    break;
                case 1:
                    angle = 180f;
                    break;
                case 2:
                    angle = 360f;
                    break;
            }

            BlackWhipSmashSearch(true);
            this.UpdateAreaIndicator();
        }

        private void UpdateAreaIndicator()
        {
            bool isAuthority = base.isAuthority;
            bool flag = isAuthority;
            if (flag)
            {
                this.aimSphere.transform.localScale = new Vector3(3f, 3f, 3f);
            }
            RaycastHit raycastHit;
            bool flag2 = Physics.Raycast(base.GetAimRay().origin, base.GetAimRay().direction, out raycastHit, blastRadius*2f, LayerIndex.world.mask | LayerIndex.entityPrecise.mask);
            //bool flag2 = Physics.Raycast(blastPosition, Vector3.down, out raycastHit, 200f, LayerIndex.world.mask | LayerIndex.entityPrecise.mask);
            bool flag3 = flag2;
            if (flag3)
            {
                this.aimSphere.transform.position = raycastHit.point + Vector3.up;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = Vector3.up;
            }
            else
            {
                Vector3 position = base.GetAimRay().origin + base.GetAimRay().direction * blastRadius * 2f;
                this.aimSphere.transform.position = position;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = Vector3.up;
            }
        }
        protected override void ForwardSuper()
        {
            base.ForwardSuper();

            base.characterMotor.disableAirControlUntilCollision = false;

            forwardDirection = base.GetAimRay().direction;
            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity.y = 0f;
                base.characterMotor.velocity = this.forwardDirection * SpeedCoefficient;
            }


            origin = base.characterBody.corePosition;
            base.characterMotor.useGravity = false;
            this.previousMass = base.characterMotor.mass;
            base.characterMotor.mass = 0f;

            characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1);

            duration = 1f;
            stlouis3 = stlouis3State.STARTUP;

            float currentMovespeed = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                currentMovespeed /= base.characterBody.sprintingSpeedMultiplier;
            }
            float basemovespeedMultiplier = (currentMovespeed / base.characterBody.baseMoveSpeed - 1f) * 0.67f;
            float movespeedMultiplier = basemovespeedMultiplier + 1f;

            SpeedCoefficient = initialSpeedCoefficient * movespeedMultiplier;

            TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(new GameObject());
            temporaryOverlay.duration = 1f;
            temporaryOverlay.animateShaderAlpha = true;
            temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            temporaryOverlay.destroyComponentOnEnd = true;
            temporaryOverlay.originalMaterial = DekuAssets.mercDashMaterial;

            TemporaryOverlayInstance temporaryOverlay2 = TemporaryOverlayManager.AddOverlay(new GameObject());
            temporaryOverlay2.duration = 1f;
            temporaryOverlay2.animateShaderAlpha = true;
            temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            temporaryOverlay2.destroyComponentOnEnd = true;
            temporaryOverlay2.originalMaterial = DekuAssets.fullCowlingMaterial;

            switch (level)
            {
                case 0:
                    numberOfHits = Mathf.RoundToInt(StaticValues.stlouisTotalHits3 * movespeedMultiplier);
                    break;
                case 1:
                    SpeedCoefficient *= StaticValues.stlouis3Level2Multiplier;
                    numberOfHits = Mathf.RoundToInt(StaticValues.stlouisTotalHits3 * movespeedMultiplier * StaticValues.stlouis3Level2Multiplier);
                    break;
                case 2:
                    SpeedCoefficient *= StaticValues.stlouis3Level3Multiplier;
                    numberOfHits = Mathf.RoundToInt(StaticValues.stlouisTotalHits3 * movespeedMultiplier * StaticValues.stlouis3Level3Multiplier);
                    break;
            }

            GetModelAnimator().SetFloat("Attack.playbackRate", 1f);
            PlayCrossfade("FullBody, Override", "StLouis3", "Attack.playbackRate", duration, 0.1f);

            if (base.isAuthority && Config.allowVoice.Value)
            {
                AkSoundEngine.PostEvent("stlouisvoice", this.gameObject);
            }
            AkSoundEngine.PostEvent("stlouissfx", this.gameObject);
        }



        public override void FixedUpdate()
		{
			base.FixedUpdate();

			switch(state)
			{
				case superState.SUPER1:
                    Ray aimRay = base.GetAimRay();
                    timer1 += Time.fixedDeltaTime;
                    if (timer1 > this.duration / totalHits1)
                    {
                        timer1 = 0f;
                        blastAttack.position = blastPosition;
                        
                        blastAttack.Fire();
                        EffectManager.SpawnEffect(Modules.DekuAssets.lightningNovaEffectPrefab, new EffectData
                        {
                            origin = blastPosition,
                            scale = blastRadius,
                            rotation = Util.QuaternionSafeLookRotation(aimRay.direction)

                        }, true);
                        EffectManager.SpawnEffect(Modules.DekuAssets.sonicboomEffectPrefab, new EffectData
                        {
                            origin = blastPosition,
                            scale = blastRadius,
                            rotation = Util.QuaternionSafeLookRotation(aimRay.direction)

                        }, true);
                        blastPosition += characterDirection.forward * (StaticValues.stlouisRadius * attackSpeedStat * 0.5f);
                    }
                    else timer1 += Time.fixedDeltaTime;


                    if ((base.fixedAge >= this.duration && base.isAuthority))
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    break;
				case superState.SUPER2:
                    characterDirection.forward = base.GetAimRay().direction;
            blastPosition = base.GetAimRay().origin + base.GetAimRay().direction * blastRadius * 2f;
                    if (!hasFired && base.fixedAge > firetime)
                    {
                        hasFired = true;
                        blastRadius = 200f;
                        BlackWhipSmashSearch(false);

                    }
                    if (base.fixedAge >= duration)
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    break; 
				case superState.SUPER3:

                    switch (stlouis3)
                    {
                        case stlouis3State.STARTUP:
                            if(base.fixedAge > 0.4f)
                            {
                                AkSoundEngine.PostEvent("ofasfx", this.gameObject);
                                Util.PlaySound(EvisDash.endSoundString, base.gameObject);

                                if (base.isAuthority)
                                {
                                    if (this.characterModel)
                                    {
                                        this.characterModel.invisibilityCount++;
                                    }
                                    if (this.hurtboxGroup)
                                    {
                                        HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                                        int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
                                        hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
                                    }

                                }

                                dekucon.RLEG.Play();
                                stlouis3 = stlouis3State.DASH;
                            }
                            break;
                        case stlouis3State.DASH:

                            if (dekucon.WINDTRAIL.isStopped)
                            {
                                dekucon.WINDTRAIL.Play();
                            }

                            this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));


                            switch (level)
                            {
                                case 0:
                                    characterMotor.velocity.y = 0f;
                                    base.characterMotor.rootMotion += forwardDirection * SpeedCoefficient * Time.fixedDeltaTime;
                                    break;
                                case 1:
                                    characterMotor.velocity.y = 0f;
                                    base.characterMotor.rootMotion += forwardDirection * SpeedCoefficient * Time.fixedDeltaTime;
                                    break;
                                case 2:

                                    characterMotor.velocity = Vector3.zero;
                                    break;
                            }

                            if(base.fixedAge > 0.8f)
                            {

                                switch (level)
                                {
                                    case 0:
                                        break;
                                    case 1:
                                        break;
                                    case 2:
                                        base.characterMotor.rootMotion += forwardDirection * SpeedCoefficient;
                                        break;
                                }

                                PlayCrossfade("FullBody, Override", "StLouis3End", "Attack.playbackRate", duration, 0.01f);


                                EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, "RFoot", false);
                                EffectManager.SimpleMuzzleFlash(Modules.DekuAssets.muzzleflashMageLightningLargePrefab, base.gameObject, "RFoot", false);

                                if (this.modelTransform)
                                {
                                    TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(new GameObject());
                                    temporaryOverlay.duration = 0.4f;
                                    temporaryOverlay.destroyComponentOnEnd = true;
                                    temporaryOverlay.originalMaterial = DekuAssets.fullCowlingMaterial;
                                    temporaryOverlay.inspectorCharacterModel = this.animator.gameObject.GetComponent<CharacterModel>();
                                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                                    temporaryOverlay.animateShaderAlpha = true;

                                }
                                base.characterMotor.disableAirControlUntilCollision = false;
                                base.characterMotor.useGravity = true;
                                if (this.characterModel)
                                {
                                    this.characterModel.invisibilityCount--;
                                }
                                if (this.hurtboxGroup)
                                {
                                    HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                                    int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
                                    hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
                                }
                                if (base.characterMotor)
                                {
                                    base.characterMotor.enabled = true;
                                }
                                final = base.transform.position;
                                ApplyComponent();

                                stlouis3 = stlouis3State.ATTACK;
                            }
                            break;
                        case stlouis3State.ATTACK:

                            if(base.isAuthority)
                            {
                                if (this.characterModel)
                                {
                                    this.characterModel.invisibilityCount--;
                                }
                                if (this.hurtboxGroup)
                                {
                                    HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                                    int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
                                    hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
                                }
                            }
                            //attackTimer += Time.fixedDeltaTime;
                            characterMotor.velocity.y = 0f;
                            characterMotor.velocity *= 0.1f;
                            if (base.fixedAge > 1.3f)
                            {
                                EffectData effectData = new EffectData
                                {
                                    scale = 1f,
                                    origin = blastPosition,
                                    rotation = Quaternion.LookRotation(characterDirection.forward),
                                };

                                EffectManager.SpawnEffect(DekuAssets.slashEffect, effectData, true);

                                this.outer.SetNextStateToMain();
                                return;
                            }

                            break;
                    }


                    break;
			}

			
		}

        
        public void BlackWhipSmashSearch(bool initial)
        {
            blackwhipSearch.teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam());
            blackwhipSearch.filterByLoS = false;
            blackwhipSearch.searchOrigin = blastPosition;
            blackwhipSearch.searchDirection = base.GetAimRay().direction;
            blackwhipSearch.sortMode = BullseyeSearch.SortMode.Distance;
            blackwhipSearch.maxDistanceFilter = blastRadius;
            blackwhipSearch.maxAngleFilter = angle;


            blackwhipSearch.RefreshCandidates();
            blackwhipSearch.FilterOutGameObject(base.gameObject);



            List<HurtBox> target = blackwhipSearch.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget.healthComponent.body && singularTarget.healthComponent)
                {
                    if (initial)
                    {
                        BlackwhipComponent blackwhipComponent = singularTarget.healthComponent.body.gameObject.GetComponent<BlackwhipComponent>();

                        if (singularTarget.healthComponent.body.characterMotor)
                        {
                            singularTarget.healthComponent.body.characterMotor.Motor.SetPositionAndRotation(blastPosition + Vector3.up * 15f,
                                                    Util.QuaternionSafeLookRotation(characterBody.characterDirection.forward), true);
                        }
                        else if (singularTarget.healthComponent.body.rigidbody)
                        {
                            singularTarget.healthComponent.body.rigidbody.MovePosition(blastPosition + Vector3.up * 15f);
                        }


                        if (blackwhipComponent)
                        {
                            blackwhipComponent.elapsedTime = 0f;

                            new BlackwhipImmobilizeRequest(singularTarget.healthComponent.body.masterObjectId, StaticValues.stlouisDamageCoefficient2 * damageStat, characterBody.masterObjectId).Send(NetworkDestination.Clients);

                        }
                        if (!blackwhipComponent)
                        {
                            blackwhipComponent = singularTarget.healthComponent.body.gameObject.AddComponent<BlackwhipComponent>();
                            blackwhipComponent.totalDuration = duration;
                            blackwhipComponent.dekucharbody = characterBody;

                            new BlackwhipImmobilizeRequest(singularTarget.healthComponent.body.masterObjectId, StaticValues.stlouisDamageCoefficient2 * damageStat, characterBody.masterObjectId).Send(NetworkDestination.Clients);
                            //new TakeDamageForceRequest(singularTarget.healthComponent.body.masterObjectId, blastPosition + Vector3.up * blastRadius, 3f, StaticValues.stlouisDamageCoefficient2 * damageStat, characterBody.masterObjectId).Send(NetworkDestination.Clients);

                        }
                    }
                    else
                    {
                        if (singularTarget.healthComponent.body.HasBuff(Buffs.blackwhipDebuff))
                        {
                            new TakeDamageForceRequest(singularTarget.healthComponent.body.masterObjectId, aimSphere.transform.position- singularTarget.healthComponent.body.corePosition, 50f, StaticValues.stlouisDamageCoefficient2 * damageStat, characterBody.masterObjectId).Send(NetworkDestination.Clients);
                        }

                    }

                }
            }
        }
        public void ApplyComponent()
        {
            blastPosition = Vector3.Lerp(origin, final, 0.5f);

            search.teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam());
            search.filterByLoS = false;
            search.searchOrigin = blastPosition;
            search.searchDirection = UnityEngine.Random.onUnitSphere;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.maxDistanceFilter = (final - origin).magnitude / 2;
            search.maxAngleFilter = 360f;


            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);



            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget.healthComponent.body && singularTarget.healthComponent)
                {
                    int buffcount = singularTarget.healthComponent.body.GetBuffCount(Modules.Buffs.delayAttackDebuff.buffIndex);
                    if (NetworkServer.active)
                    {
                        singularTarget.healthComponent.body.ApplyBuff(Modules.Buffs.delayAttackDebuff.buffIndex, numberOfHits + buffcount);
                    }
                    ShootStyleKickComponent shootStyleKickComponent = singularTarget.healthComponent.body.gameObject.GetComponent<ShootStyleKickComponent>();

                    if (shootStyleKickComponent)
                    {
                        shootStyleKickComponent.numberOfHits += numberOfHits;
                        shootStyleKickComponent.timer = 0;
                    }
                    if (!shootStyleKickComponent)
                    {
                        shootStyleKickComponent = singularTarget.healthComponent.body.gameObject.AddComponent<ShootStyleKickComponent>();
                        shootStyleKickComponent.charbody = singularTarget.healthComponent.body;
                        shootStyleKickComponent.dekucharbody = characterBody;
                        shootStyleKickComponent.numberOfHits = numberOfHits;
                        shootStyleKickComponent.damage = base.damageStat * Modules.StaticValues.stlouisDamageCoefficient3 * attackSpeedStat;
                    }



                }
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
                    UpdateAreaIndicator();
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

            characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 0);
            if (aimSphere)
            {
                this.aimSphere.SetActive(false);
                EntityState.Destroy(this.aimSphere);

            }
            base.characterBody.characterMotor.useGravity = true;
            base.characterBody.inputBank.enabled = true;

            if (dekucon.WINDTRAIL.isPlaying)
            {
                dekucon.WINDTRAIL.Stop();
            }

            switch (state)
            {
                case superState.SUPER1:
                    break;
                case superState.SUPER2:
                    break;
                case superState.SUPER3:

                    base.characterMotor.mass = this.previousMass;
                    base.characterMotor.useGravity = true;
                    base.characterMotor.velocity = Vector3.zero;

                    if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
                    base.characterMotor.disableAirControlUntilCollision = false;
                    base.characterMotor.velocity.y = 0;

                    break;
            }

            //switch(level)
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