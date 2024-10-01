using DekuMod.Modules;
using DekuMod.Modules.Survivors;
using DekuMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace DekuMod.SkillStates.Might
{
    public class SmashRushCombo : BaseMeleeAttack
    {
        public HurtBox Target;
        private bool keepMoving;
        private float rollSpeed;
        private float SpeedCoefficient;
        public static float initialSpeedCoefficient = 10f;
        private float finalSpeedCoefficient = 0f;

        public override void OnEnter()
        {
            keepMoving = true;

            this.hitboxName = "SmashRushHitbox";

            this.damageCoefficient = Modules.StaticValues.smashRushDamageCoefficient;
            this.procCoefficient = 1f;
            this.pushForce = 500f;
            this.bonusForce = Vector3.zero;
            this.damageType = DamageType.Generic;
            this.baseDuration = 0.5f;
            this.attackStartTime = 0.4f;
            this.attackEndTime = 0.8f;
            this.baseEarlyExitTime = 0.8f;
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 7f;

            this.swingSoundString = "shootstyedashcombosfx";
            this.hitSoundString = "";
            this.muzzleString = ChooseMuzzleString();
            this.swingEffectPrefab = Modules.DekuAssets.dekuPunchEffect;
            this.hitEffectPrefab = Modules.DekuAssets.dekuHitImpactEffect;

            this.impactSound = Modules.DekuAssets.kickHitSoundEvent.index;


            dekucon = base.GetComponent<DekuController>();
            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

            base.OnEnter();
        }

        public override void Level1()
        {

        }
        public override void Level2()
        {

        }
        public override void Level3()
        {

        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (swingIndex == 2 && this.stopwatch <= (this.baseDuration * this.attackStartTime) && keepMoving)
            {
                RecalculateRollSpeed();
                Vector3 velocity = base.GetAimRay().direction.normalized * rollSpeed;
                velocity.y = base.characterMotor.velocity.y;
                base.characterMotor.velocity = velocity;
                //base.characterDirection.forward = base.characterMotor.velocity.normalized;                


            }

        }

        private void RecalculateRollSpeed()
        {
            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num /= base.characterBody.sprintingSpeedMultiplier;
            }
            float num2 = (num / base.characterBody.baseMoveSpeed) * 0.67f;
            float num3 = num2 + 1f;
            this.rollSpeed = num3 * Mathf.Lerp(SpeedCoefficient, finalSpeedCoefficient, base.fixedAge / (base.baseDuration * this.attackEndTime));
        }

        private string ChooseMuzzleString()
        {
            string returnVal = "mightSwingR";
            switch (this.swingIndex)
            {
                case 0:
                    returnVal = "mightSwingR";
                    break;
                case 1:
                    returnVal = "mightSwingL";
                    break;
            }

            return returnVal;
        }

        protected override void PlayAttackAnimation()
        {
            switch (this.swingIndex)
            {
                case 0:
                    base.PlayCrossfade("FullBody, Override", "SmashRushR", "Slash.playbackRate", this.duration, 0.01f);
                    break;
                case 1:
                    base.PlayCrossfade("FullBody, Override", "SmashRushL", "Slash.playbackRate", this.duration, 0.01f);
                    break;
            }
            switch (level)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
            keepMoving = false;

        }

        protected override void SetNextState()
        {
            int index = this.swingIndex;
            if (index == 0) index = 1;
            else index = 0;

            if (Target)
            {
                float num2 = Vector3.Distance(base.transform.position, Target.transform.position);
                if (num2 >= StaticValues.smashRushDistance)
                {
                    //this.outer.SetNextState(new DashAttack
                    //{

                    //});
                }
                else
                {
                    this.outer.SetNextState(new SmashRushCombo
                    {
                        swingIndex = index
                    });

                }
            }
            else if (!Target)
            {
                this.outer.SetNextState(new SmashRushCombo
                {
                    swingIndex = index
                });
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}