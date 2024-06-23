//using DekuMod.Modules.Survivors;
//using EntityStates;
//using RoR2.Skills;
//using RoR2;
//using UnityEngine.Networking;
//using UnityEngine;

//namespace DekuMod.SkillStates
//{

//	public class QuirkSuper : BaseSpecial
//	{
//		public static float baseDuration = 0.5f;

//		private float duration;
//		public override void OnEnter()
//		{
//			base.OnEnter();

//			if (energySystem.currentPlusUltra > Modules.StaticValues.specialPlusUltraSpend)
//			{
//				energySystem.SpendPlusUltra(Modules.StaticValues.specialPlusUltraSpend);

//				bool active = NetworkServer.active;
//				if (active)
//				{
//					base.characterBody.AddTimedBuffAuthority(Modules.Buffs.fajinBuff.buffIndex, Modules.StaticValues.fajinDuration);
//				}
//			}
//			else
//			{
//				if (base.isAuthority)
//				{
//					Chat.AddMessage($"You need {Modules.StaticValues.specialPlusUltraSpend} plus ultra.");
//					this.outer.SetNextStateToMain();
//					return;

//				}
//			}


//		}	

//		public override void FixedUpdate()
//		{
//			base.FixedUpdate();
//			if (base.fixedAge > baseDuration)
//			{
//				this.outer.SetNextStateToMain();
//			}

//		}
//		public override void OnExit()
//		{
//			base.OnExit();

//		}
//		public override InterruptPriority GetMinimumInterruptPriority()
//		{
//			return InterruptPriority.Frozen;
//		}
//	}
//}