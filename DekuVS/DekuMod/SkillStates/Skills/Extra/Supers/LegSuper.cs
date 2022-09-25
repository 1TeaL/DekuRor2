using DekuMod.Modules.Survivors;
using EntityStates;
using RoR2.Skills;
using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace DekuMod.SkillStates
{

	public class LegSuper : BaseSpecial
	{
		public static float baseDuration = 5f;
        public static float blastRadius = 20f;
        public static float succForce = 10f;

        private float duration;
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration;
			base.StartAimMode(0.5f + this.duration, false);

			bool active = NetworkServer.active;
			if (active)
			{
				base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
			}

            Pull();


        }

		public void Pull()
        {
            //pull
            if (NetworkServer.active)
            {
                Collider[] array = Physics.OverlapSphere(base.transform.position, blastRadius * attackSpeedStat, LayerIndex.defaultLayer.mask);
                for (int i = 0; i < array.Length; i++)
                {
                    HealthComponent healthComponent = array[i].GetComponent<HealthComponent>();
                    if (healthComponent)
                    {
                        TeamComponent component2 = healthComponent.GetComponent<TeamComponent>();
                        if (component2.teamIndex != TeamIndex.Player)
                        {
                            var charb = healthComponent.body;
                            if (charb)
                            {
                                Vector3 pushForce = (base.transform.position - charb.corePosition) * succForce;
                                var motor = charb.GetComponent<CharacterMotor>();
                                var rb = charb.GetComponent<Rigidbody>();

                                float mass = 1;
                                if (motor) mass = motor.mass;
                                else if (rb) mass = rb.mass;
                                if (mass < 100) mass = 100;

                                pushForce *= mass;

                                DamageInfo info = new DamageInfo
                                {
                                    attacker = base.gameObject,
                                    inflictor = base.gameObject,
                                    damage = Modules.StaticValues.finalsmashDamageCoefficient,
                                    damageColorIndex = DamageColorIndex.Default,
                                    damageType = DamageType.Stun1s,
                                    crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master),
                                    dotIndex = DotController.DotIndex.None,
                                    force = pushForce,
                                    position = base.transform.position,
                                    procChainMask = default(ProcChainMask),
                                    procCoefficient = 0.4f
                                };

                                charb.healthComponent.TakeDamageForce(info, true, true);
                            }
                        }
                    }
                }
            }
        }


		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.fixedAge > baseDuration)
			{
				this.outer.SetNextStateToMain();
			}

		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}