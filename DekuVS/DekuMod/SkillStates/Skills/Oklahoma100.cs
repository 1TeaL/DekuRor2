using DekuMod.Modules.Survivors;
using EntityStates;
using EntityStates.Huntress;
using EntityStates.VagrantMonster;
using RoR2;
using RoR2.Audio;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace DekuMod.SkillStates
{
    public class Oklahoma100 : BaseSkillState
    {

        public float spinage;
        public float baseduration = 1f;
        public float duration;
        public float speedattack;
        public static float blastRadius = 3f;
        public float force = 5000f;

        protected Animator animator;
        private GameObject areaIndicator;
        private RaycastHit raycastHit;
        private GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/LightningStakeNova");

        public static float healthCostFraction;
        public float fajin;
        protected DamageType damageType;
        public DekuController dekucon;

        private BlastAttack blastAttack;
        private float maxDistance = 0f;
        private float radius2;
        private float baseRadius = 1f;
        private float hitDis;
        private float damageintake;
        private BlastAttack blastAttack2;

        public override void OnEnter()
        {
            base.OnEnter();

            dekucon = base.GetComponent<DekuController>();
            if (dekucon.isMaxPower)
            {
                fajin = 2f;
            }
            else
            {
                fajin = 1f;
            }

            duration = baseduration / (this.attackSpeedStat);

            blastAttack = new BlastAttack();
            blastAttack.radius = Oklahoma100.blastRadius * fajin;
            blastAttack.procCoefficient = 0.25f;
            blastAttack.position = base.characterBody.corePosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = base.characterBody.damage * Modules.StaticValues.oklahoma100DamageCoefficient * fajin;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseForce = force;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.Default;

            //base.PlayCrossfade("RightArm, Override", "SmashCharge", 0.2f);
            base.PlayAnimation("Fullbody, Override", "Oklahoma", "Attack.playbackRate", duration);
            //Util.PlaySound(ChargeTrackingBomb.chargingSoundString, base.gameObject);
            AkSoundEngine.PostEvent(3940341776, this.gameObject);

            if (NetworkServer.active && base.healthComponent)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.1f;
                damageInfo.position = base.characterBody.corePosition;
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

            base.characterMotor.walkSpeedPenaltyCoefficient = 0.2f;
            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.AddBuff(Modules.Buffs.oklahomaBuff);
            }
            dekucon.OKLAHOMA.Play();
            this.areaIndicator = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (self.body.HasBuff(Modules.Buffs.oklahomaBuff.buffIndex))
            {
                damageintake = damageInfo.damage;

                var dekucon = self.body.gameObject.GetComponent<DekuController>();
                dekucon.oklahomacount += (damageintake / self.body.healthComponent.fullCombinedHealth) * 100;

            }
            orig.Invoke(self, damageInfo);
        }

        public void IndicatorUpdator()
        {
            Ray aimRay = base.GetAimRay();
            Vector3 direction = aimRay.direction;
            aimRay.origin = base.characterBody.corePosition;
            Physics.Raycast(aimRay.origin, aimRay.direction, out this.raycastHit, this.maxDistance);
            this.hitDis = this.raycastHit.distance;
            bool flag = this.hitDis < this.maxDistance && this.hitDis > 0f;
            if (flag)
            {
                this.maxDistance = this.hitDis;
            }
            this.radius2 = fajin * (this.baseRadius + (dekucon.oklahomacount));
            this.areaIndicator.transform.localScale = Vector3.one * this.radius2;
            this.areaIndicator.transform.localPosition = aimRay.origin;
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
        public override void OnExit()
        {

            blastAttack2 = new BlastAttack();
            blastAttack2.radius = radius2;
            blastAttack2.procCoefficient = 1f;
            blastAttack2.position = base.characterBody.corePosition;
            blastAttack2.attacker = base.gameObject;
            blastAttack2.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack2.baseDamage = base.characterBody.damage * Modules.StaticValues.oklahoma100DamageCoefficient * (dekucon.oklahomacount/10);
            blastAttack2.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack2.baseForce = force;
            blastAttack2.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack2.damageType = DamageType.Stun1s;
            blastAttack2.attackerFiltering = AttackerFiltering.Default;

            blastAttack2.Fire();

            Ray aimRay = base.GetAimRay();
            EffectManager.SpawnEffect(effectPrefab, new EffectData
            {
                origin = base.characterBody.corePosition,
                scale = radius2,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction)

            }, true);

            dekucon.oklahomacount = 0;
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            dekucon.OKLAHOMA.Stop();
            base.characterMotor.walkSpeedPenaltyCoefficient = 1f;
            if (NetworkServer.active)
            {
                base.characterBody.RemoveBuff(Modules.Buffs.oklahomaBuff);
            }
            bool flag = this.areaIndicator;
            if (flag)
            {
                this.areaIndicator.SetActive(false);
                EntityState.Destroy(this.areaIndicator);
            }
            base.OnExit();
        }

        public override void FixedUpdate()
        {


            base.FixedUpdate();
            bool flag = base.IsKeyDownAuthority();
            if (flag)
            {
                base.characterMotor.walkSpeedPenaltyCoefficient = 0.2f * fajin + dekucon.oklahomacount/100;
                IndicatorUpdator();
                Ray aimRay = base.GetAimRay();

                if (base.isAuthority && spinage >= this.duration / 4)
                {
                    blastAttack.position = base.characterBody.corePosition;
                    //hasFired = true;
                    if (dekucon.isMaxPower)
                    {

                        blastAttack.damageType = DamageType.BypassArmor | DamageType.Stun1s;
                    }
                    else
                    {
                        blastAttack.damageType = DamageType.Generic;
                    }
                    spinage = 0f;
                    blastAttack.Fire();

                    //base.PlayAnimation("Fullbody, Override", "Oklahoma", "Attack.playbackRate", duration/4);
                    base.PlayCrossfade("Fullbody, Override", "Oklahoma", duration / 4);

                }
                else this.spinage += Time.fixedDeltaTime;

            }

            else
            {
                bool isAuthority = base.isAuthority;
                if (isAuthority)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }
        }
    }
}
