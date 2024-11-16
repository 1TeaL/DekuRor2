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
        private float rollSpeed;
        public static float initialSpeedCoefficient = StaticValues.stlouis3InitialSpeed;
        public static float finalSpeedCoefficient = 0f;
        public static float SpeedCoefficient;
        private Vector3 forwardDirection;
        private Vector3 previousPosition;
        public Vector3 origin;
        public Vector3 final;
        private readonly BullseyeSearch search = new BullseyeSearch();
        private readonly BullseyeSearch blackwhipSearch = new BullseyeSearch();
        public int numberOfHits;

        private GameObject areaIndicator;

        private bool animChange;
		private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        private EffectData effectData;
        private EffectData effectData2;
        private Animator animator;
        private float duration;
        private float firetime;
        private int totalHits1;
        private float timer1;
        private float previousMass;
        private float angle;

        private GameObject aimSphere;

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
            totalHits1 = StaticValues.stlouisTotalHits;
            //play animation of kick

            PlayCrossfade("FullBody, Override", "StLouis1", "Attack.playbackRate", duration, 0.1f);

        }
        protected override void BackwardSuper()
        {
			base.BackwardSuper();
            characterDirection.forward = base.GetAimRay().direction;

            //characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, 1);

            //indicator
            //this.areaIndicator = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            //this.areaIndicator.SetActive(true);
            duration = StaticValues.stlouisDuration2;
            firetime = duration * 0.5f;
            blastRadius = StaticValues.stlouisRadius2 * attackSpeedStat;
            blastPosition = GetAimRay().direction * blastRadius;

            
            //play animation of blackwhip lift up and slam
            PlayCrossfade("FullBody, Override", "BlackwhipSmash", "Attack.playbackRate", duration, 0.1f);


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

            if (base.isAuthority)
            {
                this.aimSphere.transform.localScale = new Vector3(4f, 4f, 4f);
            }
            RaycastHit raycastHit;
            bool ray = Physics.Raycast(blastPosition, Vector3.down, out raycastHit, 200f, LayerIndex.world.mask | LayerIndex.entityPrecise.mask) ;
            
            if (ray)
            {
                this.aimSphere.transform.position = raycastHit.point + Vector3.up;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = Vector3.up;
            }
            else
            {
                Vector3 position = blastPosition + 200f * Vector3.down;
                this.aimSphere.transform.position = position;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = Vector3.up;
            }
        }
        protected override void ForwardSuper()
        {
            base.ForwardSuper();
            numberOfHits = Mathf.RoundToInt(StaticValues.stlouisTotalHits3 * attackSpeedStat);

            base.characterMotor.disableAirControlUntilCollision = false;


            if (base.isAuthority && base.inputBank && base.characterDirection)
            {
                this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            }

            Vector3 rhs = base.characterDirection ? base.characterDirection.forward : this.forwardDirection;
            Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);

            float num = Vector3.Dot(this.forwardDirection, rhs);
            float num2 = Vector3.Dot(this.forwardDirection, rhs2);

            this.RecalculateRollSpeed();

            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity.y = 0f;
                base.characterMotor.velocity = this.forwardDirection * this.rollSpeed;
            }

            EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, "LFoot", false);
            EffectManager.SimpleMuzzleFlash(Modules.DekuAssets.muzzleflashMageLightningLargePrefab, base.gameObject, "LFoot", false);

            base.characterMotor.useGravity = false;
            this.previousMass = base.characterMotor.mass;
            base.characterMotor.mass = 0f;

            characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, 1);

            duration = StaticValues.stlouisDuration3;
            totalHits1 = StaticValues.stlouisTotalHits;


            //play animation of beginning to jump and fly
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


                    this.RecalculateRollSpeed();
                    this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));


                    if (base.characterDirection) base.characterDirection.forward = this.forwardDirection;
                    if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = Mathf.Lerp(EntityStates.Commando.DodgeState.dodgeFOV, 60f, base.fixedAge / duration);

                    Vector3 normalized = (base.transform.position - this.previousPosition).normalized;
                    if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
                    {
                        Vector3 vector = normalized * this.rollSpeed;
                        float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
                        vector = this.forwardDirection * d;
                        vector.y = 0f;

                        base.characterMotor.velocity = vector;
                    }
                    this.previousPosition = base.transform.position;

                    if (this.modelTransform)
                    {
                        TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(new GameObject());
                        temporaryOverlay.duration = duration;
                        temporaryOverlay.animateShaderAlpha = true;
                        temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                        temporaryOverlay.destroyComponentOnEnd = true;
                        temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                        TemporaryOverlayInstance temporaryOverlay2 = TemporaryOverlayManager.AddOverlay(new GameObject());
                        temporaryOverlay2.duration = duration;
                        temporaryOverlay2.animateShaderAlpha = true;
                        temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                        temporaryOverlay2.destroyComponentOnEnd = true;
                        temporaryOverlay2.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                    }

                    if (base.isAuthority && base.fixedAge >= duration)
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }

                    break;
			}

			
		}


        private void RecalculateRollSpeed()
        {
            this.rollSpeed = this.moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, base.fixedAge / duration);
        }

        public void BlackWhipSmashSearch(bool initial)
        {
            blackwhipSearch.teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam());
            blackwhipSearch.filterByLoS = false;
            blackwhipSearch.searchOrigin = base.GetAimRay().origin;
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


                        if (blackwhipComponent)
                        {

                        }
                        if (!blackwhipComponent)
                        {
                            blackwhipComponent = singularTarget.healthComponent.body.gameObject.AddComponent<BlackwhipComponent>();
                            blackwhipComponent.totalDuration = duration;
                            blackwhipComponent.dekucharbody = characterBody;
                            blackwhipComponent.charbody = singularTarget.healthComponent.body;

                            new BlackwhipImmobilizeRequest(singularTarget.healthComponent.body.masterObjectId, StaticValues.blackwhipOverdriveDamage * damageStat, characterBody.masterObjectId).Send(NetworkDestination.Clients);
                            new TakeDamageForceRequest(singularTarget.healthComponent.body.masterObjectId, blastPosition + Vector3.up * blastRadius, 3f, StaticValues.stlouisDamageCoefficient2 * damageStat, characterBody.masterObjectId).Send(NetworkDestination.Clients);

                        }
                    }
                    else
                    {
                        BlackwhipComponent blackwhipComponent = singularTarget.healthComponent.body.gameObject.GetComponent<BlackwhipComponent>();
                        if (blackwhipComponent)
                        {
                            new TakeDamageForceRequest(singularTarget.healthComponent.body.masterObjectId, aimSphere.transform.position, 3f, StaticValues.stlouisDamageCoefficient2 * damageStat, characterBody.masterObjectId).Send(NetworkDestination.Clients);
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

            switch (state)
            {
                case superState.SUPER1:
                    break;
                case superState.SUPER2:
                    EntityState.Destroy(this.aimSphere.gameObject);
                    break;
                case superState.SUPER3:
                    Util.PlaySound(EvisDash.endSoundString, base.gameObject);

                    base.characterMotor.mass = this.previousMass;
                    base.characterMotor.useGravity = true;
                    base.characterMotor.velocity = Vector3.zero;

                    if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
                    base.characterMotor.disableAirControlUntilCollision = false;
                    base.characterMotor.velocity.y = 0;

                    final = base.transform.position;
                    ApplyComponent();
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