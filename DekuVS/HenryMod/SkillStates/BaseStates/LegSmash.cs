﻿using EntityStates;
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

namespace DekuMod.SkillStates.BaseStates
{
    public class LegSmash : BaseSkillState
    {

        //protected float baseDuration = 6f;


        ////private float earlyExitTime;
        public float smashage;
        public float duration = 1f;
        //private bool hasFired;
        //private float hitPauseTimer;
        //private OverlapAttack attack;
        //protected bool inHitPause;
        //private bool hasHopped;
        //protected float stopwatch;

        //private BaseState.HitStopCachedState hitStopCachedState;
        //private Vector3 storedVelocity;
        //private Transform smashIndicatorInstance;
        protected Animator animator;
        private GameObject areaIndicator;
        private float maxCharge;
        private int baseMaxCharge = 4;
        private float maxDistance;
        private float chargePercent;
        private float baseDistance = 4f;
        private RaycastHit raycastHit;
        private float hitDis;
        private float baseDamageMult = 6f;
        private float damageMult;
        private float radius;
        private float baseRadius = 1.5f;
        private Vector3 maxMoveVec;
        private Vector3 randRelPos;
        private int randFreq;
        private bool reducerFlipFlop;
        private GameObject effectPrefab = Resources.Load<GameObject>("Prefabs/effects/LightningStakeNova");

        public static float healthCostFraction;



        public override void OnEnter()
        {
            base.OnEnter();            
            float[] source = new float[]
            {
                this.attackSpeedStat,
                4f
            };
            this.maxCharge = (float)this.baseMaxCharge / source.Min();
            this.areaIndicator = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);
            //base.PlayCrossfade("RightArm, Override", "SmashCharge", 0.2f);
            base.PlayAnimation("Fullbody, Override", "LegSmashCharge", "Attack.playbackRate", 0.2f);
            Util.PlaySound(ChargeTrackingBomb.chargingSoundString, base.gameObject);

            //DamageInfo damageInfo = new DamageInfo();
            //damageInfo.damage = base.healthComponent.combinedHealth * 0.1f;
            //damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.10f;
            //damageInfo.position = base.characterBody.corePosition;
            //damageInfo.force = Vector3.zero;
            //damageInfo.damageColorIndex = DamageColorIndex.Default;
            //damageInfo.crit = false;
            //damageInfo.attacker = null;
            //damageInfo.inflictor = null;
            //damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
            //damageInfo.procCoefficient = 0f;
            //damageInfo.procChainMask = default(ProcChainMask);
            //base.healthComponent.TakeDamage(damageInfo);


            GetComponent<CharacterBody>().bodyFlags = CharacterBody.BodyFlags.SprintAnyDirection;
        }

        public void IndicatorUpdator()
        {
            Ray aimRay = base.GetAimRay();
            Vector3 direction = aimRay.direction;
            aimRay.origin = base.characterBody.corePosition;
            this.maxDistance = (1f + 4f * this.chargePercent) * this.baseDistance * (this.moveSpeedStat / 7f);
            Physics.Raycast(aimRay.origin, aimRay.direction, out this.raycastHit, this.maxDistance);
            this.hitDis = this.raycastHit.distance;
            bool flag = this.hitDis < this.maxDistance && this.hitDis > 0f;
            if (flag)
            {
                this.maxDistance = this.hitDis;
            }
            this.damageMult = this.baseDamageMult + 6f * (this.chargePercent * this.baseDamageMult);
            this.radius = (this.baseRadius * this.damageMult + 20f) / 4f;
            this.maxMoveVec = this.maxDistance * direction;
            this.areaIndicator.transform.localScale = Vector3.one * this.radius;
            this.areaIndicator.transform.localPosition = aimRay.origin + this.maxMoveVec;
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        public override void OnExit()
        {

            base.characterMotor.walkSpeedPenaltyCoefficient = 1f;
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
            bool flag = IsKeyDownAuthority();
            if (flag)
            {
                this.chargePercent = base.fixedAge / this.maxCharge;
                this.randRelPos = new Vector3((float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f, (float)Random.Range(-12, 12) / 4f);
                this.randFreq = Random.Range(1, this.baseMaxCharge * 100) / 100;
                bool flag2 = this.reducerFlipFlop;
                if (flag2)
                {
                    bool flag3 = (float)this.randFreq <= this.chargePercent;
                    if (flag3)
                    {
                        EffectData effectData = new EffectData
                        {
                            scale = 1f,
                            origin = base.characterBody.corePosition + this.randRelPos
                        };
                        EffectManager.SpawnEffect(this.effectPrefab, effectData, true);
                    }
                    this.reducerFlipFlop = false;
                }
                else
                {
                    this.reducerFlipFlop = true;
                }

                this.IndicatorUpdator();

                //if (NetworkServer.active && base.healthComponent && smashage >= duration)
                //{

                //    DamageInfo damageInfo = new DamageInfo();
                //    //damageInfo.damage = base.healthComponent.combinedHealth * 0.1f;
                //    damageInfo.damage = base.healthComponent.fullCombinedHealth * 0.1f;
                //    damageInfo.position = base.characterBody.corePosition;
                //    damageInfo.force = Vector3.zero;
                //    damageInfo.damageColorIndex = DamageColorIndex.Default;
                //    damageInfo.crit = false;
                //    damageInfo.attacker = null;
                //    damageInfo.inflictor = null;
                //    damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
                //    damageInfo.procCoefficient = 0f;
                //    damageInfo.procChainMask = default(ProcChainMask);
                //    base.healthComponent.TakeDamage(damageInfo);
                //    smashage = 0f;
                //}
                //else this.smashage = smashage + Time.fixedDeltaTime;

            }
            else
            {
                bool isAuthority = base.isAuthority;
                if (isAuthority)
                {
                    LegSmashRelease legsmashRelease = new LegSmashRelease();
                    legsmashRelease.damageMult = this.damageMult;
                    legsmashRelease.radius = this.radius;
                    legsmashRelease.moveVec = this.maxMoveVec;
                    this.outer.SetNextState(legsmashRelease);
                }
            }
        }
    }
}