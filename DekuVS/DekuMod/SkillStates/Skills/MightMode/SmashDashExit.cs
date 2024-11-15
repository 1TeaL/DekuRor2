using EntityStates;
using DekuMod.Modules.Survivors;
using DekuMod.SkillStates.BaseStates;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using DekuMod.Modules;

namespace DekuMod.SkillStates.Might
{
    public class SmashDashExit : BaseMeleeAttack
    {
        public HurtBox Target;
        public override void OnEnter()
        {

            this.hitboxName = "SmashRushHitbox";

            this.damageType = DamageType.Generic;
            this.damageCoefficient = Modules.StaticValues.smashRushDamageCoefficient;
            this.procCoefficient = 1f;
            this.pushForce = 500f;
            this.bonusForce = Vector3.zero;
            this.damageType = DamageType.Generic;            
            this.baseDuration = 0.5f;
            this.attackStartTime = 0.35f;
            this.attackEndTime = 0.6f;
            this.baseEarlyExitTime = 0.8f;
            this.hitStopDuration = 0.2f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 5f;

            this.swingSoundString = "shootstyedashcombosfx";
            this.hitSoundString = "";
            this.muzzleString = "mightSwingR";
            this.swingEffectPrefab = Modules.DekuAssets.dekuPunchEffect;
            this.hitEffectPrefab = Modules.DekuAssets.dekuHitImpactEffect;

            this.impactSound = Modules.DekuAssets.kickHitSoundEvent.index;
            
            switch (level)
            {
                case 0:
                    damageCoefficient *= 1.5f;
                    break;
                case 1:
                    damageCoefficient *= 2f;
                    break;
                case 2:
                    damageCoefficient *= 3f;
                    break;
            }
            dekucon = base.GetComponent<DekuController>();
            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

            base.OnEnter();
        }


        protected override void PlayAttackAnimation()
        {

            base.GetModelAnimator().SetBool("smashRushDashEnd", true);
            base.PlayAnimation("FullBody, Override", "SmashRushDashEnd", "Slash.playbackRate", 0.01f);
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
            int index = this.swingIndex;
            if (index == 0) index = 1;
            else index = 0;

            this.outer.SetNextState(new SmashRushCombo
            {
                swingIndex = index
            });
            //if (dekucon && base.isAuthority)
            //{
            //    Target = dekucon.GetTrackingTarget();
            //}

            //if (Target)
            //{
            //    float num2 = Vector3.Distance(base.transform.position, Target.transform.position);
            //    if (num2 >= StaticValues.smashRushDistance)
            //    {
            //        this.outer.SetNextState(new SmashDash
            //        {

            //        });
            //    }
            //    else
            //    {
            //        this.outer.SetNextState(new SmashRushCombo
            //        {
            //            swingIndex = index
            //        });

            //    }
            //}
            //else if (!Target)
            //{
            //    this.outer.SetNextState(new SmashRushCombo
            //    {
            //        swingIndex = index
            //    });
            //}
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