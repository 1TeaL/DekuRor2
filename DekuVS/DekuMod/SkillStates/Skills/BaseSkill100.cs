using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace DekuMod.SkillStates
{

	public class BaseSkill100 : BaseSkillState
	{
		public DekuController dekucon;

        public override void OnEnter()
		{
			base.OnEnter();
			dekucon = base.GetComponent<DekuController>();


		}

		public void SpendHealth(float healthPercentage)
        {
			if (NetworkServer.active && base.healthComponent)
			{
				DamageInfo damageInfo = new DamageInfo();
				damageInfo.damage = base.healthComponent.fullCombinedHealth * healthPercentage;
				damageInfo.position = base.transform.position;
				damageInfo.force = Vector3.zero;
				damageInfo.damageColorIndex = DamageColorIndex.Default;
				damageInfo.crit = false;
				damageInfo.attacker = null;
				damageInfo.inflictor = null;
				damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
				damageInfo.procCoefficient = 0f;
				damageInfo.procChainMask = default(ProcChainMask);
				base.healthComponent.TakeDamage(damageInfo);
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

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