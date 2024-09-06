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
        public static float initialSpeedCoefficient = 8f;
        public static float finalSpeedCoefficient = 1f;
        public static float SpeedCoefficient;
        private Vector3 forwardDirection;
        private Vector3 previousPosition;
        public Vector3 origin;
        public Vector3 final;
        private Vector3 theSpot;
        private readonly BullseyeSearch search = new BullseyeSearch();
        public int numberOfHits;

        private GameObject areaIndicator;

        private bool animChange;
		private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        public GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");
        private EffectData effectData;
        private EffectData effectData2;
        private Animator animator;
        private float duration;
        private int totalHits1;
        private float timer1;
        private float previousMass;

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
            blastRadius = StaticValues.stlouisRadius * attackSpeedStat;
            blastDamage = StaticValues.stlouisDamageCoefficient * damageStat * attackSpeedStat;
			//set in front of deku exactly
			blastPosition = characterBody.corePosition + (characterDirection.forward * (StaticValues.stlouisRadius * attackSpeedStat * 0.5f));
            blastForce = 0f;
            duration = StaticValues.stlouisDuration;
            totalHits1 = StaticValues.stlouisTotalHits;
            //play animation of quick punch

        }
        protected override void BackwardSuper()
        {
			base.BackwardSuper();

			characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, 1);

            blastRadius = StaticValues.detroitRadius2 * attackSpeedStat;
            blastDamage = StaticValues.detroitDamageCoefficient2 * damageStat * attackSpeedStat;
            //set around deku
            blastPosition = characterBody.corePosition;
			blastType = DamageType.Stun1s;
			blastForce = StaticValues.detroitForce2;
            //indicator
            this.areaIndicator = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);
            duration = 1f;
            //play animation of slam punch
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
            EffectManager.SimpleMuzzleFlash(Modules.Asset.muzzleflashMageLightningLargePrefab, base.gameObject, "LFoot", false);

            base.characterMotor.useGravity = false;
            this.previousMass = base.characterMotor.mass;
            base.characterMotor.mass = 0f;

            characterBody.ApplyBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex, 1, 1);

            duration = StaticValues.stlouisDuration;
            totalHits1 = StaticValues.stlouisTotalHits;
            duration = 1f;


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
                        EffectManager.SpawnEffect(Modules.Asset.lightningNovaEffectPrefab, new EffectData
                        {
                            origin = blastPosition,
                            scale = blastRadius,
                            rotation = Util.QuaternionSafeLookRotation(aimRay.direction)

                        }, true);
                        EffectManager.SpawnEffect(Modules.Asset.sonicboomEffectPrefab, new EffectData
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
                    if (!hasFired)
                    {
                        hasFired = true;
                        blastAttack.Fire();
                        EffectManager.SpawnEffect(Modules.Asset.mageLightningBombEffectPrefab, effectData, true);
                        EffectManager.SpawnEffect(Modules.Asset.detroitEffect, effectData2, true);

                        AkSoundEngine.PostEvent("impactsfx", this.gameObject);

                        SmallHop(characterMotor, StaticValues.stlouisDistance2);
                        base.characterMotor.velocity = -StaticValues.stlouisDistance2 * base.GetAimRay().direction * moveSpeedStat;
                    }
                    if (base.fixedAge > 1f)
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

        public void ApplyComponent()
        {
            blastPosition = Vector3.Lerp(origin, final, 0.5f);

            search.teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam());
            search.filterByLoS = false;
            search.searchOrigin = theSpot;
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