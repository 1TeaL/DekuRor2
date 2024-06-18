using DekuMod.Modules;
using DekuMod.Modules.Survivors;
using DekuMod.SkillStates.BaseStates;
using EntityStates;
using RoR2;
using UnityEngine;

namespace DekuMod.SkillStates.Might
{
    public class SmashRushStart : BaseDekuSkillState
    {
        public HurtBox Target;

        public override void OnEnter()
        {
            dekucon = base.GetComponent<DekuController>();
            if (dekucon && base.isAuthority)
            {
                Target = dekucon.GetTrackingTarget();
            }

            if (Target)
            {
                float distance = Vector3.Distance(base.transform.position, Target.transform.position);
                if (distance >= StaticValues.smashRushDistance)
                {
                    this.outer.SetNextState(new SmashDash
                    {

                    });
                }
                else
                {
                    this.outer.SetNextState(new SmashRushCombo
                    {

                    });

                }
            }
            else if (!Target)
            {
                this.outer.SetNextState(new SmashRushCombo
                {

                });
            }

            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}