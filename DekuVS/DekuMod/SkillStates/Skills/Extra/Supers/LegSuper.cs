using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using EntityStates.Merc;
using System.Collections.Generic;
using System.Linq;
using DekuMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;

namespace DekuMod.SkillStates
{

	public class LegSuper : BaseSpecial
	{
		public static float baseDuration = 4.5f;
        public static float exitDuration;
        public static float baseBlastRadius = Modules.StaticValues.finalsmashRange;
        public static float blastRadius;

        public float timer;
        public float fireTime = 0.5f;
        public float baseFireInterval = 0.1f;
        public float fireInterval;
        private float FOV = 120f;
        private float duration;
        public float previousMass;

        private Transform modelTransform;
        private GameObject muzzlePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        private Vector3 forwardDirection;
        private Vector3 previousPosition;
        private float basespeedCoefficient = 4f;
        private float speedCoefficient;
        private string muzzleString;

        private BlastAttack blastAttack;
        private float maxWeight;
        public bool animChange;

        public override void OnEnter()
		{
			base.OnEnter();
			
        }

        protected override void DoSkill()
        {
            this.duration = baseDuration;
            exitDuration = duration - fireTime;
            timer = 0f;
            blastRadius = baseBlastRadius * attackSpeedStat;
            fireInterval = baseFireInterval / attackSpeedStat;
            speedCoefficient = basespeedCoefficient * moveSpeedStat;
            animChange = false;
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(0.5f + this.duration, false);

            base.characterMotor.useGravity = false;
            this.previousMass = base.characterMotor.mass;
            base.characterMotor.mass = 0f;
            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.disableAirControlUntilCollision = true;

            if (base.isAuthority)
            {
                this.forwardDirection = aimRay.direction;
            }
            //if (base.characterMotor && base.characterDirection)
            //{
            //    base.characterMotor.velocity = this.forwardDirection * this.speedCoefficient * moveSpeedStat;
            //}
            this.previousPosition = -aimRay.direction * 2f;

            base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, baseDuration);

            this.muzzleString = "LFoot";
            EffectManager.SimpleMuzzleFlash(EvisDash.blinkPrefab, base.gameObject, this.muzzleString, false);
            EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, this.muzzleString, false);


            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            }

            if (base.isAuthority)
            {
                new PerformFinalSmashNetworkRequest(base.characterBody.masterObjectId,
                    base.transform.position,
                    base.GetAimRay().direction,
                    Modules.StaticValues.finalsmashDamageCoefficient).Send(NetworkDestination.Clients);
            }

            blastAttack = new BlastAttack();
            blastAttack.radius = blastRadius;
            blastAttack.procCoefficient = 1f;
            blastAttack.position = base.transform.position;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.finalsmashDamageCoefficient * (duration / fireInterval);
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = maxWeight * 20f;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.IgniteOnHit;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;


            PlayCrossfade("FullBody, Override", "FinalSmashDash", "Attack.playbackRate", duration - exitDuration, 0.01f);

        }
        public void GetMaxWeight()
        {
            Ray aimRay = base.GetAimRay();
            BullseyeSearch search = new BullseyeSearch
            {

                teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam()),
                filterByLoS = false,
                searchOrigin = base.transform.position,
                searchDirection = UnityEngine.Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = blastRadius,
                maxAngleFilter = 360f
            };

            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);



            List<HurtBox> target = search.GetResults().ToList<HurtBox>();
            foreach (HurtBox singularTarget in target)
            {
                if (singularTarget)
                {
                    if (singularTarget.healthComponent && singularTarget.healthComponent.body)
                    {
                        if (singularTarget.healthComponent.body.characterMotor)
                        {
                            if (singularTarget.healthComponent.body.characterMotor.mass > maxWeight)
                            {
                                maxWeight = singularTarget.healthComponent.body.characterMotor.mass;
                            }
                        }
                        else if (singularTarget.healthComponent.body.rigidbody)
                        {
                            if (singularTarget.healthComponent.body.rigidbody.mass > maxWeight)
                            {
                                maxWeight = singularTarget.healthComponent.body.rigidbody.mass;
                            }
                        }
                    }
                }
            }
        }
        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(this.forwardDirection);
            effectData.origin = origin;
            EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
        }

        public override void FixedUpdate()
		{
			base.FixedUpdate();
            GetMaxWeight();

            if (base.fixedAge >= fireTime && base.fixedAge < exitDuration)
            {
                this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));

                Ray aimRay = base.GetAimRay();
                aimRay.direction = this.forwardDirection;

                if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = Mathf.Lerp(FOV, 60f, base.fixedAge / duration);

                Vector3 normalized = (aimRay.origin - this.previousPosition).normalized;
                if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
                {
                    Vector3 vector = normalized * this.speedCoefficient;
                    float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
                    vector = this.forwardDirection * d;
                    //vector.y = 0f;

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

                if (timer > (fireInterval))
                {
                    timer = 0;
                    if (base.isAuthority)
                    {
                        new PerformFinalSmashNetworkRequest(base.characterBody.masterObjectId,
                            base.transform.position,
                            base.GetAimRay().direction,
                            Modules.StaticValues.finalsmashDamageCoefficient).Send(NetworkDestination.Clients);
                    }
                }
                else
                {
                    timer += Time.fixedDeltaTime;
                }
            }           
            else if(base.fixedAge > exitDuration)
            {
                if (!animChange)
                {
                    PlayAnimation("FullBody, Override", "FinalSmashSmash", "Attack.playbackRate", exitDuration);
                    animChange = true;
                }
                if (base.isAuthority)
                {
                    new PerformFinalSmashNetworkRequest(base.characterBody.masterObjectId,
                        base.transform.position,
                        base.GetAimRay().direction,
                        0f).Send(NetworkDestination.Clients);
                }
            }
            else
            {
                PlayAnimation("FullBody, Override", "FinalSmashDash", "Attack.playbackRate", fireTime);
            }
            
            if (base.fixedAge > baseDuration)
			{
                blastAttack.position = base.transform.position;
                blastAttack.Fire();
                EffectManager.SpawnEffect(Modules.Assets.elderlemurianexplosionEffect, new EffectData
                {
                    origin = base.transform.position,
                    scale = blastRadius,
                    rotation = Quaternion.LookRotation(Vector3.up)
                }, true);
                this.outer.SetNextStateToMain();
			}

		}

        public override void OnExit()
        {
            base.OnExit();

            base.characterMotor.disableAirControlUntilCollision = false;
            base.characterMotor.mass = this.previousMass;
            base.characterMotor.useGravity = true;
            base.characterMotor.velocity = Vector3.zero;

            if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
            base.characterMotor.disableAirControlUntilCollision = false;
            base.characterMotor.velocity.y = 0;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}