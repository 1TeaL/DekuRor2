//using System;
//using EntityStates;
//using UnityEngine;

//namespace DekuMod.SkillStates
//{
//	// Token: 0x02000016 RID: 22
//	internal class DekuJetpack : BaseState
//	{
//		// Token: 0x0600003A RID: 58 RVA: 0x000032F8 File Offset: 0x000014F8
//		public override void OnEnter()
//		{
//			base.OnEnter();
//			this.jetEffect = base.FindModelChild("JetHolder");
//			bool flag = this.jetEffect;
//			if (flag)
//			{
//				this.jetEffect.gameObject.SetActive(true);
//			}
//		}

//		// Token: 0x0600003B RID: 59 RVA: 0x00003344 File Offset: 0x00001544
//		public override void FixedUpdate()
//		{
//			base.FixedUpdate();
//			bool isAuthority = base.isAuthority;
//			if (isAuthority)
//			{
//				float velocityY = Mathf.MoveTowards(base.characterMotor.velocity.y, -4f, 60f * Time.fixedDeltaTime);
//				base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, velocityY, base.characterMotor.velocity.z);
//			}
//		}

//		// Token: 0x0600003C RID: 60 RVA: 0x000033BC File Offset: 0x000015BC
//		public override void OnExit()
//		{
//			bool flag = this.jetEffect;
//			if (flag)
//			{
//				this.jetEffect.gameObject.SetActive(false);
//			}
//			base.OnExit();
//		}

//		// Token: 0x04000026 RID: 38
//		private Transform jetEffect;
//	}
//}
