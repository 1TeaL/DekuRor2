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
    public class Detroit45 : BaseSkill45
    {
        public float smashage;
        public float duration = 1f;

        protected Animator animator;
        private GameObject areaIndicator;
        private float maxCharge;
        private int baseMaxCharge = 3;
        private float maxDistance;
        private float chargePercent;
        private float baseDistance = 2f;
        private RaycastHit raycastHit;
        private float hitDis;
        private float damageMult;
        private float radius;
        private float baseRadius = 2f;
        private Vector3 maxMoveVec;



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
            //base.PlayAnimation("FullBody, Override", "SmashCharge", "Attack.playbackRate", 1f);
            base.GetModelAnimator().SetFloat("Attack.playbackRate", attackSpeedStat);
            PlayCrossfade("RightArm, Override", "DetroitCharge", "Attack.playbackRate", duration, 0.01f);
            //base.PlayAnimation("RightArm, Override", "SmashCharge");
            //base.PlayCrossfade("RightArm, Override", "SmashCharge", 0.2f);
            //base.PlayAnimation("RightArm, Override", "SmashCharge", "Attack.playbackRate", 0.2f);
            //Util.PlaySound(ChargeTrackingBomb.chargingSoundString, base.gameObject);
            if (base.isAuthority)
            {
                AkSoundEngine.PostEvent("detroitchargesfxvoice", this.gameObject);
            }

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
            this.damageMult = Modules.StaticValues.detroit100DamageCoefficient + 2f * (this.chargePercent * Modules.StaticValues.detroit100DamageCoefficient);
            this.radius = (this.baseRadius * this.damageMult + 10f) / 4f;
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
            if (base.fixedAge > duration)
            {
                PlayCrossfade("RightArm, Override", "DetroitCharging", "Attack.playbackRate", duration, 0.01f);
            }
            bool flag = base.fixedAge < this.maxCharge && base.IsKeyDownAuthority();
            if (flag)
            {
                this.chargePercent = base.fixedAge / this.maxCharge;

                base.characterMotor.walkSpeedPenaltyCoefficient = 1f - this.chargePercent / 3f;
                this.IndicatorUpdator();
            }

            
            else
            {
                bool isAuthority = base.isAuthority;
                if (isAuthority)
                {
                    Detroit45Release detroit45Release = new Detroit45Release();
                    detroit45Release.damageMult = this.damageMult;
                    detroit45Release.radius = this.radius;
                    detroit45Release.moveVec = this.maxMoveVec;
                    this.outer.SetNextState(detroit45Release);
                }
            }
        }
    }
}
