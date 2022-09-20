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

            this.damageType = DamageType.Generic;
            this.damageCoefficient = Modules.StaticValues.shootkickDamageCoefficient;
            this.procCoefficient = 1f;
            this.pushForce = 300f;
            if (swingIndex == 2)
            {
                this.bonusForce = new Vector3(0f, 1000f, 0f);
            }
            else if(swingIndex < 2)
            {
                this.bonusForce = new Vector3(0f, -300f, 0f);
            }
            this.baseDuration = 1f;
            this.attackStartTime = 0.2f;
            this.attackEndTime = 0.4f;
            this.baseEarlyExitTime = 0.4f;
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 10f;

            this.swingSoundString = "";
            this.hitSoundString = "";
            this.muzzleString = ChooseAnimationString();
            this.swingEffectPrefab = null;
            this.hitEffectPrefab = null;

            this.impactSound = Modules.Assets.swordHitSoundEvent.index;

            dekucon = base.GetComponent<DekuController>();
            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

            base.OnEnter();
        }


        private string ChooseAnimationString()
        {
            string returnVal = "RHand";
            //string returnVal = "SwingLeft";
            //switch (this.swingIndex)
            //{
            //    case 0:
            //        returnVal = "SwingLeft";
            //        break;
            //    case 1:
            //        returnVal = "SwingRight";
            //        break;
            //    case 2:
            //        returnVal = "SwingCenter";
            //        break;
            //}

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
            Chat.AddMessage("set next state");
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