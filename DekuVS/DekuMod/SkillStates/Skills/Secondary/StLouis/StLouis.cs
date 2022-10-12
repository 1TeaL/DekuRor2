using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.Generic;
using System.Linq;
using DekuMod.Modules.Survivors;
using R2API.Networking;
using R2API.Networking.Interfaces;
using DekuMod.Modules.Networking;

namespace DekuMod.SkillStates
{
    public class StLouis : BaseSkillState
    {
        public DekuController dekucon;
        public EnergySystem energySystem;
        public bool hasTeleported;
        public bool hasFired;
        public float baseDuration = 1f;
        public float duration;
        public float fireTime;

        private float blastRadius = Modules.StaticValues.stlouisRange;

        private BlastAttack blastAttack;

        //Indicator
        public HurtBox Target;
        private float num;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / base.attackSpeedStat;
            this.fireTime = this.duration / 3f;
            hasFired = false;
            hasTeleported = false;

            dekucon = base.GetComponent<DekuController>();
            energySystem = base.GetComponent<EnergySystem>();
            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

            if (!Target)
            {
                return;
            }
            num = this.moveSpeedStat;
			bool isSprinting = base.characterBody.isSprinting;
			if (isSprinting)
			{
				num /= base.characterBody.sprintingSpeedMultiplier;
			}
            blastRadius *= num;
        }


        public override void OnExit()
        {
            base.OnExit();
            this.PlayAnimation("FullBody, Override", "BufferEmpty");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (Target)
            {
                if (!hasTeleported)
                {
                    hasTeleported = true;
                    base.characterMotor.velocity = Vector3.zero;
                    base.characterMotor.Motor.SetPositionAndRotation(Target.healthComponent.body.transform.position + Vector3.up, Target.healthComponent.body.transform.rotation, true);
                    //new PerformDetroitTeleportNetworkRequest(base.characterBody.masterObjectId, Target.gameObject).Send(NetworkDestination.Clients);

                    if (base.isAuthority)
                    {
                        AkSoundEngine.PostEvent("stlouisvoice", this.gameObject);
                    }
                    AkSoundEngine.PostEvent("stlouissfx", this.gameObject);
                }

                if (base.fixedAge > this.fireTime && !hasFired && base.isAuthority)
                {
                    energySystem.currentPlusUltra += Modules.StaticValues.skillPlusUltraGain;
                    hasFired = true;
                    EffectManager.SimpleMuzzleFlash(Modules.Assets.dekuKickEffect, base.gameObject, "Swing1", true);
                    PlayCrossfade("FullBody, Override", "StLouis", "Attack.playbackRate", duration - fireTime, 0.01f);
                    new PerformStLouisSmashNetworkRequest(base.characterBody.masterObjectId, Target.healthComponent.body.masterObjectId).Send(NetworkDestination.Clients);

                    blastAttack = new BlastAttack();
                    blastAttack.radius = blastRadius;
                    blastAttack.procCoefficient = 1f;
                    blastAttack.position = base.transform.position;
                    blastAttack.damageType = DamageType.Stun1s;
                    blastAttack.attacker = base.gameObject;
                    blastAttack.crit = base.RollCrit();
                    blastAttack.baseDamage = base.damageStat * Modules.StaticValues.stlouisDamageCoefficient * num;
                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                    blastAttack.baseForce = 500f;
                    blastAttack.bonusForce = GetAimRay().direction * 100f;
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;

                    EffectManager.SpawnEffect(Modules.Assets.lightningNovaEffectPrefab, new EffectData
                    {
                        origin = base.transform.position,
                        scale = blastRadius,

                    }, true);
                    EffectManager.SpawnEffect(Modules.Assets.sonicboomEffectPrefab, new EffectData
                    {
                        origin = base.transform.position,
                        scale = blastRadius,

                    }, true);

                    blastAttack.Fire();
                }

                if ((base.fixedAge >= this.duration && base.isAuthority))
                {
                    this.outer.SetNextStateToMain();
                    return;
                }

            }
            else
            {
                base.skillLocator.utility.AddOneStock();
                this.outer.SetNextStateToMain();
                return;

            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
