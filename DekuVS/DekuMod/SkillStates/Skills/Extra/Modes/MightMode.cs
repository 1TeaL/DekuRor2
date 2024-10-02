using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using ExtraSkillSlots;
using RoR2.Skills;
using AK.Wwise;
using DekuMod.Modules;
using R2API.Networking;
using UnityEngine;
using DekuMod.SkillStates.BaseStates;

namespace DekuMod.SkillStates
{

	public class MightMode : BaseDekuSkillState
	{

        public SkillDef skilldef1;
        public SkillDef skilldef2;
        public SkillDef skilldef3;
        public SkillDef skilldef4;

        private bool resetSwappedSkill2;
        private bool resetSwappedSkill3;

        private bool isSwitch;
        private float duration;
        private BlastAttack blastAttack;

        public override void OnEnter()
		{
			base.OnEnter();

            skilldef1 = characterBody.skillLocator.primary.skillDef;
            skilldef2 = characterBody.skillLocator.secondary.skillDef;
            skilldef3 = characterBody.skillLocator.utility.skillDef;
            skilldef4 = characterBody.skillLocator.special.skillDef;

            if (dekucon.resetSkill2)
            {
                resetSwappedSkill2 = true;
            }
            if (dekucon.resetSkill3)
            {
                resetSwappedSkill3 = true;
            }

            if (base.skillLocator.secondary.stock >= 1)
            {
                dekucon.resetSkill2 = true;
            }
            else if (base.skillLocator.secondary.cooldownRemaining > 0)
            {
                dekucon.resetSkill2 = false;
            }

            if (skilldef3 == Deku.mightUtilitySkillDef && base.skillLocator.utility.stock >= base.skillLocator.utility.GetBaseMaxStock())
            {
                dekucon.resetSkill3 = true;
            }
            else if (skilldef3 == Deku.mightUtilitySkillDef && base.skillLocator.utility.stock < base.skillLocator.utility.maxStock)
            {
                dekucon.resetSkill3 = false;
            }
            else if (base.skillLocator.utility.cooldownRemaining > 0)
            {
                dekucon.resetSkill3 = false;
            }


            if (skilldef1 != Deku.mightPrimarySkillDef)
            {
                base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, skilldef1, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Deku.mightPrimarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.secondary.UnsetSkillOverride(base.skillLocator.secondary, skilldef2, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.secondary.SetSkillOverride(base.skillLocator.secondary, Deku.mightSecondarySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, skilldef3, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Deku.mightUtilitySkillDef, GenericSkill.SkillOverridePriority.Contextual);

                base.skillLocator.special.UnsetSkillOverride(base.skillLocator.utility, skilldef4, GenericSkill.SkillOverridePriority.Contextual);
                base.skillLocator.special.SetSkillOverride(base.skillLocator.utility, Deku.mightSpecialSkillDef, GenericSkill.SkillOverridePriority.Contextual);

                if (resetSwappedSkill2)
                {
                    base.skillLocator.secondary.AddOneStock();
                }

                //make sure all stocks get given back
                if (resetSwappedSkill3)
                {
                    for (int i = 0; i < base.skillLocator.utility.GetBaseMaxStock(); i++)
                    {
                        base.skillLocator.utility.AddOneStock();
                    }
                }

                if(!resetSwappedSkill2 || !resetSwappedSkill3)
                {
                    skillLocator.DeductCooldownFromAllSkillsAuthority(dekucon.skillCDTimer);
                    //make sure number of stocks = time taken
                    for (int i = 0; i < (int)dekucon.skillCDTimer; i++)
                    {
                        base.skillLocator.utility.AddOneStock();
                    }
                }
                dekucon.skillCDTimer = 0f;

                if (energySystem.currentPlusUltra > Modules.StaticValues.super1Cost)
                {
                    energySystem.SpendPlusUltra(Modules.StaticValues.super1Cost);
                    SwitchAttack();
                }
            }


        }

        protected virtual void SwitchAttack()
        {
            isSwitch = true;
            base.skillLocator.ResetSkills();
            for (int i = 0; i < base.skillLocator.utility.maxStock; i++)
            {
                base.skillLocator.utility.AddOneStock();
            }

            duration = 0.5f;
            characterBody.ApplyBuff(Buffs.mightBuff.buffIndex, 1, characterBody.GetBuffCount(Buffs.mightBuff) + StaticValues.mightBuffDuration);

            if (dekucon.GetTrackingTarget())
            {
                characterBody.characterMotor.Motor.SetPositionAndRotation(dekucon.GetTrackingTarget().transform.position, Quaternion.LookRotation(base.GetAimRay().direction));
            }
            else
            {
                base.characterMotor.velocity = StaticValues.mightSwitchRadius * (base.GetAimRay().direction) * moveSpeedStat;
            }
        }

        public override void FixedUpdate()
		{
			base.FixedUpdate();

            if(isSwitch)
            {
                if(base.fixedAge > duration)
                {
                    base.characterMotor.velocity *= 0.1f;
                    //blast attack
                    blastAttack = new BlastAttack();
                    blastAttack.procCoefficient = 1f;
                    blastAttack.attacker = base.gameObject;
                    blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                    blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                    blastAttack.damageType = DamageType.Generic;
                    blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                    blastAttack.radius = StaticValues.mightSwitchRadius * attackSpeedStat;
                    blastAttack.baseDamage = damageStat * StaticValues.mightSwitchDamage * attackSpeedStat;
                    blastAttack.position = characterBody.corePosition + base.GetAimRay().direction * 3f;
                    blastAttack.baseForce = 10000f;

                    blastAttack.Fire();

                    this.outer.SetNextStateToMain();
                    return;
                }
            }
            else
            {
                this.outer.SetNextStateToMain();
                return;
            }
			
		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}