//using System;
//using EntityStates;
//using RoR2;
//using UnityEngine;

//namespace DekuMod.SkillStates.BaseStates
//{
//	// Token: 0x02000017 RID: 23
//	public class DekuMain : GenericCharacterMain
//	{
//		// Token: 0x0600003E RID: 62 RVA: 0x00003400 File Offset: 0x00001600
//		public override void OnEnter()
//		{
//			base.OnEnter();
//			this.dekuController = base.characterBody.GetComponent<DekuMod.Modules.DekuController>();
//			this.jetpackMachine = EntityStateMachine.FindByCustomName(base.gameObject, "Jet");
//			//this.animator = base.GetModelAnimator();
//			this.childLocator = base.GetModelChildLocator();
//			bool flag = this.childLocator;
//			//if (flag)
//			//{
//			//	this.tailBone = this.childLocator.FindChild("Tail").GetComponent<DynamicBone>();
//			//}
//		}

//		// Token: 0x0600003F RID: 63 RVA: 0x00003480 File Offset: 0x00001680
//		public override void ProcessJump()
//		{
//			base.ProcessJump();
//			bool flag = this.hasCharacterMotor && this.hasInputBank && base.isAuthority;
//			if (flag)
//			{
//				bool flag2 = base.inputBank.jump.down && base.characterMotor.velocity.y < 0f && !base.characterMotor.isGrounded && !this.dekuController.endJet;
//				if (flag2)
//				{
//					bool flag3 = !(this.jetpackMachine.state.GetType() == typeof(DekuJetpack));
//					if (flag3)
//					{
//						this.jetpackMachine.SetState(new DekuJetpack());
//						base.characterBody.AddBuff(Modules.Buffs.floatBuff);
//						this.dekuController.hasfloatBuff = true;
//					}
//				}
//				else
//				{
//					bool flag4 = this.jetpackMachine.state.GetType() == typeof(DekuJetpack);
//					if (flag4)
//					{
//						this.jetpackMachine.SetNextState(new Idle());
//						base.characterBody.RemoveBuff(Modules.Buffs.floatBuff);
//						this.dekuController.hasfloatBuff = false;
//					}
//				}
//			}
//			bool isGrounded = base.characterMotor.isGrounded;
//			if (isGrounded)
//			{
//				this.dekuController.endJet = false;
//			}
//		}

//		// Token: 0x06000040 RID: 64 RVA: 0x000035D0 File Offset: 0x000017D0
//		public override void FixedUpdate()
//		{
//			base.FixedUpdate();
//			bool flag = this.animator;
//			if (flag)
//			{
//				float inAir = 0f;
//				bool flag2 = !this.animator.GetBool("isGrounded");
//				if (flag2)
//				{
//					inAir = 1f;
//				}
//				this.animator.SetFloat("inAir", inAir);
//				this.animator.SetBool("inCombat", !base.characterBody.outOfCombat || !base.characterBody.outOfDanger);
//				this.animator.SetBool("useAdditive", !this.animator.GetBool("isSprinting"));
//				this.animator.SetBool("isHovering", inAir == 1f && base.characterBody.HasBuff(Modules.Buffs.floatBuff));
//				bool flag3 = this.tailBone;
//				if (flag3)
//				{
//					bool flag4 = this.animator.GetBool("isGrounded") && !this.animator.GetBool("isMoving");
//					if (flag4)
//					{
//						this.tailBone.enabled = false;
//					}
//					else
//					{
//						this.tailBone.enabled = true;
//					}
//				}
//			}
//		}

//		// Token: 0x04000027 RID: 39
//		private Animator animator;

//		// Token: 0x04000028 RID: 40
//		private Modules.DekuController dekuController;

//		// Token: 0x04000029 RID: 41
//		private EntityStateMachine jetpackMachine;

//		// Token: 0x0400002A RID: 42
//		private ChildLocator childLocator;

//		// Token: 0x0400002B RID: 43
//		private DynamicBone tailBone;
//	}
//}
