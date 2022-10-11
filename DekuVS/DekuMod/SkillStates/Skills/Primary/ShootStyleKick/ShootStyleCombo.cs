using DekuMod.Modules.Survivors;
using DekuMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace DekuMod.SkillStates
{
    public class ShootStyleCombo : BaseMeleeAttack
    {
        public HurtBox Target;
        public DekuController dekucon;
        public override void OnEnter()
        {
            this.hitboxName = "BigBodyHitbox";

            this.damageCoefficient = Modules.StaticValues.shootkickDamageCoefficient;
            this.procCoefficient = 1f;
            this.pushForce = 500f;
            if (swingIndex == 2)
            {
                this.bonusForce = new Vector3(10f, 1000f, 0f);
                this.damageType = DamageType.Shock5s;
            }
            else if(swingIndex < 2)
            {
                this.bonusForce = new Vector3(10f, -300f, 0f);
                this.damageType = DamageType.Generic;
            }
            this.baseDuration = 1f;
            this.attackStartTime = 0.2f;
            this.attackEndTime = 0.4f;
            this.baseEarlyExitTime = 0.4f;
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 9f;

            this.swingSoundString = "shootstyedashcombosfx";
            this.hitSoundString = "";
            this.muzzleString = ChooseAnimationString();
            this.swingEffectPrefab = Modules.Assets.dekuKickEffect;
            this.hitEffectPrefab = Modules.Assets.dekuHitImpactEffect;

            this.impactSound = Modules.Assets.kickHitSoundEvent.index;

            dekucon = base.GetComponent<DekuController>();
            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

            base.OnEnter();
        }


        private string ChooseAnimationString()
        {
            string returnVal = "Swing1";
            switch (this.swingIndex)
            {
                case 0:
                    returnVal = "Swing1";
                    break;
                case 1:
                    returnVal = "Swing2";
                    break;
                case 2:
                    returnVal = "Swing3";
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

            if (swingIndex == 2)
            {
                base.skillLocator.DeductCooldownFromAllSkillsServer(1f);
            }
        }

        protected override void SetNextState()
        {
            int index = this.swingIndex;
            index += 1;
            if (index > 2)
            {
                index = 0;
            }

            if (Target)
            {
                float num = 10f;
                if (!base.isGrounded)
                {
                    num = 7f;
                }
                float num2 = Vector3.Distance(base.transform.position, Target.transform.position);
                if (num2 >= num)
                {
                    this.outer.SetNextState(new DashAttack
                    {

                    });
                }
                else
                {
                    this.outer.SetNextState(new ShootStyleCombo
                    {
                        swingIndex = index
                    });

                }
            }
            else if (!Target)
            {
                this.outer.SetNextState(new ShootStyleCombo
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