//using RoR2;
//using UnityEngine;
//using EntityStates;
//using EntityStates.VagrantMonster;
//using EntityStates.SiphonItem;

//namespace DekuMod.SkillStates
//{

//	public class BlackwhipCharge: BaseSkillState
//	{

//		public static float baseDuration = 3f;
//		public static string chargeSound;
//		private float duration;
//		private GameObject chargeVfxInstance;
//		private GameObject areaIndicatorVfxInstance;

//		public override void OnEnter()
//		{
//			base.OnEnter();
//			this.duration = BlackwhipCharge.baseDuration / (base.attachedBody ? base.attachedBody.attackSpeed : 1f);
//			base.TurnOffHealingFX();
//			if (base.attachedBody)
//			{
//				Vector3 position = base.transform.position;
//				Quaternion rotation = base.transform.rotation;
//				this.chargeVfxInstance = UnityEngine.Object.Instantiate<GameObject>(ChargeMegaNova.chargingEffectPrefab, position, rotation);
//				this.chargeVfxInstance.transform.localScale = Vector3.one * 0.25f;
//				Util.PlaySound(ChargeState.chargeSound, base.gameObject);
//				this.areaIndicatorVfxInstance = UnityEngine.Object.Instantiate<GameObject>(ChargeMegaNova.areaIndicatorPrefab, position, rotation);
//				ObjectScaleCurve component = this.areaIndicatorVfxInstance.GetComponent<ObjectScaleCurve>();
//				component.timeMax = this.duration;
//				component.baseScale = Vector3.one * DetonateState.baseSiphonRange * 2f;
//				this.areaIndicatorVfxInstance.GetComponent<AnimateShaderAlpha>().timeMax = this.duration;
//			}
//			RoR2Application.onLateUpdate += this.OnLateUpdate;
//		}

//		// Token: 0x06003CAD RID: 15533 RVA: 0x000EED58 File Offset: 0x000ECF58
//		public override void OnExit()
//		{
//			RoR2Application.onLateUpdate -= this.OnLateUpdate;
//			if (this.chargeVfxInstance != null)
//			{
//				EntityState.Destroy(this.chargeVfxInstance);
//				this.chargeVfxInstance = null;
//			}
//			if (this.areaIndicatorVfxInstance != null)
//			{
//				EntityState.Destroy(this.areaIndicatorVfxInstance);
//				this.areaIndicatorVfxInstance = null;
//			}
//			base.OnExit();
//		}

//		// Token: 0x06003CAE RID: 15534 RVA: 0x000EEDB0 File Offset: 0x000ECFB0
//		private void OnLateUpdate()
//		{
//			if (this.chargeVfxInstance)
//			{
//				this.chargeVfxInstance.transform.position = base.transform.position;
//			}
//			if (this.areaIndicatorVfxInstance)
//			{
//				this.areaIndicatorVfxInstance.transform.position = base.transform.position;
//			}
//		}
//		public override void FixedUpdate()
//		{
//			base.FixedUpdate();
//			if (base.isAuthority && base.fixedAge >= this.duration)
//			{
//				this.outer.SetNextState(new DetonateState());
//			}
//		}


//	}

//}
//}
