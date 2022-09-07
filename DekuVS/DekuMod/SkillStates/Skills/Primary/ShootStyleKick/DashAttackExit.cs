using EntityStates;
using DekuMod.Modules.Survivors;
using DekuMod.SkillStates.BaseStates;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DekuMod.SkillStates
{
    public class DashAttackExit : BaseMeleeAttack
    {
        public override void OnEnter()
        {
            this.hitboxName = "BigModelHitbox";

            this.damageType = DamageType.Generic;
            this.damageCoefficient = Modules.StaticValues.shootkickDamageCoefficient;
            this.procCoefficient = 1f;
            this.pushForce = 300f;
            this.bonusForce = new Vector3(0f, -500f, 0f);
            this.baseDuration = 1f;
            this.attackStartTime = 0.2f;
            this.attackEndTime = 0.4f;
            this.baseEarlyExitTime = 0.4f;
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 10f;

            this.swingSoundString = "RimuruSwordSwing";
            this.hitSoundString = "";
            this.muzzleString = ChooseAnimationString();
            //this.swingEffectPrefab = Modules.Assets.swordSwingEffect;
            //this.hitEffectPrefab = Modules.Assets.swordHitImpactEffect;

            this.impactSound = Modules.Assets.swordHitSoundEvent.index;

            base.OnEnter();
        }

        private string ChooseAnimationString()
        {
            string returnVal = "SwingLeft";
            switch (this.swingIndex)
            {
                case 0:
                    returnVal = "SwingLeft";
                    break;
                case 1:
                    returnVal = "SwingRight";
                    break;
                case 2:
                    returnVal = "SwingCenter";
                    break;
            }

            return returnVal;
        }
        protected override void PlayAttackAnimation()
        {
            base.PlayAttackAnimation();
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
            int index = this.swingIndex;
            index += 1;
            if (index > 2)
            {
                index = 0;
            }

            this.outer.SetNextState(new ShootStyleCombo
            {
                swingIndex = index
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