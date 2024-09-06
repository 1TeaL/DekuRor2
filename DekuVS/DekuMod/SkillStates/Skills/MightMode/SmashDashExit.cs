using EntityStates;
using DekuMod.Modules.Survivors;
using DekuMod.SkillStates.BaseStates;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DekuMod.SkillStates.Might
{
    public class SmashDashExit : BaseMeleeAttack
    {
        public override void OnEnter()
        {

            this.hitboxName = "SmashRushHitbox";

            this.damageType = DamageType.Generic;
            this.damageCoefficient = Modules.StaticValues.smashRushDamageCoefficient;
            this.procCoefficient = 1f;
            this.pushForce = 500f;
            this.bonusForce = Vector3.zero;
            this.damageType = DamageType.Generic;            
            this.baseDuration = 1f;
            this.attackStartTime = 0.2f;
            this.attackEndTime = 0.4f;
            this.baseEarlyExitTime = 0.4f;
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 10f;

            this.swingSoundString = "shootstyedashcombosfx";
            this.hitSoundString = "";
            this.muzzleString = ChooseAnimationString();
            this.swingEffectPrefab = Modules.Asset.dekuKickEffect;
            this.hitEffectPrefab = Modules.Asset.dekuHitImpactEffect;

            this.impactSound = Modules.Asset.kickHitSoundEvent.index;
            
            switch (level)
            {
                case 0:
                    break;
                case 1:
                    damageCoefficient *= 2f;
                    break;
                case 2:
                    damageCoefficient *= 3f;
                    break;
            }
            base.OnEnter();
        }

        private string ChooseAnimationString()
        {
            string returnVal = "DashAttack";
            switch (level)
            {
                case 0:
                    returnVal = "DashAttack";
                    break;
                case 1:
                    returnVal = "DashAttack";
                    break;
                case 2:
                    returnVal = "TeleportAttack";
                    break;
            }

            return returnVal;
        }

        protected override void PlayAttackAnimation()
        {
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
        }

        protected override void SetNextState()
        {
            //int index = this.swingIndex;
            //index += 1;
            //if (index > 2)
            //{
            //    index = 0;
            //}

            this.outer.SetNextState(new SmashRushCombo
            {
                //swingIndex = index
            });
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (this.stopwatch >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}