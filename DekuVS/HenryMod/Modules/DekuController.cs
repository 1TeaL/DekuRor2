using System;
using RoR2;
using UnityEngine;

namespace DekuMod.Modules
{

	public class DekuController : MonoBehaviour
	{

		public bool hasBazookaReady;
		private CharacterBody characterBody;
		private CharacterModel model;
		private ChildLocator childLocator;
		//private DekuTracker tracker;
		private Animator modelAnimator;
		private bool inFrenzy;

		// Token: 0x04000291 RID: 657
		private ParticleSystem[] frenzyEffects;

		// Token: 0x04000292 RID: 658
		private ParticleSystem superSaiyanEffect;
		private void Awake()
		{
			this.characterBody = base.gameObject.GetComponent<CharacterBody>();
			this.childLocator = base.gameObject.GetComponentInChildren<ChildLocator>();
			this.model = base.gameObject.GetComponentInChildren<CharacterModel>();
			//this.tracker = base.gameObject.GetComponent<DekuTracker>();
			this.modelAnimator = base.gameObject.GetComponentInChildren<Animator>();
			this.hasBazookaReady = false;
			this.inFrenzy = false;
			this.frenzyEffects = new ParticleSystem[]
			{
				this.childLocator.FindChild("FrenzyEffect").gameObject.GetComponent<ParticleSystem>(),
				this.childLocator.FindChild("FrenzyFistEffectL").gameObject.GetComponent<ParticleSystem>(),
				this.childLocator.FindChild("FrenzyFistEffectR").gameObject.GetComponent<ParticleSystem>()
			};
			this.superSaiyanEffect = this.childLocator.FindChild("SuperSaiyanEffect").gameObject.GetComponent<ParticleSystem>();
			base.Invoke("CheckWeapon", 0.2f);
		}

		private void FixedUpdate()
		{
			//bool flag = this.inFrenzy;
			//if (flag)
			//{
			//	//bool flag2 = !this.characterBody.HasBuff(Buffs.frenzyBuff) && !this.characterBody.HasBuff(Buffs.frenzyScepterBuff);
			//	//if (flag2)
			//	//{
			//	//	this.ExitFrenzy();
			//	//}
			//}
		}

		//private void CheckWeapon()
		//{
		//	string skillNameToken = this.characterBody.skillLocator.primary.skillDef.skillNameToken;
		//	if (!(skillNameToken == "ROB_HENRY_BODY_PRIMARY_PUNCH_NAME"))
		//	{
		//		if (!(skillNameToken == "ROB_HENRY_BODY_PRIMARY_GUN_NAME"))
		//		{
		//			this.childLocator.FindChild("SwordModel").gameObject.SetActive(true);
		//			this.childLocator.FindChild("BoxingGloveL").gameObject.SetActive(false);
		//			this.childLocator.FindChild("BoxingGloveR").gameObject.SetActive(false);
		//			this.childLocator.FindChild("AltGun").gameObject.SetActive(false);
		//			this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("Body, Alt"), 0f);
		//		}
		//		else
		//		{
		//			this.childLocator.FindChild("SwordModel").gameObject.SetActive(false);
		//			this.childLocator.FindChild("BoxingGloveL").gameObject.SetActive(false);
		//			this.childLocator.FindChild("BoxingGloveR").gameObject.SetActive(false);
		//			this.childLocator.FindChild("AltGun").gameObject.SetActive(true);
		//			this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("Body, Alt"), 0f);
		//		}
		//	}
		//	else
		//	{
		//		this.childLocator.FindChild("SwordModel").gameObject.SetActive(false);
		//		this.childLocator.FindChild("BoxingGloveL").gameObject.SetActive(true);
		//		this.childLocator.FindChild("BoxingGloveR").gameObject.SetActive(true);
		//		this.childLocator.FindChild("AltGun").gameObject.SetActive(false);
		//		this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("Body, Alt"), 1f);
		//	}
		//	bool flag = false;
		//	bool flag2 = this.characterBody.skillLocator.secondary.skillDef.skillNameToken == "ROB_HENRY_BODY_SECONDARY_STINGER_NAME";
		//	if (flag2)
		//	{
		//		this.childLocator.FindChild("GunModel").gameObject.SetActive(false);
		//		this.childLocator.FindChild("Gun").gameObject.SetActive(false);
		//		this.characterBody.crosshairPrefab = Assets.LoadCrosshair("SimpleDot");
		//		flag = true;
		//	}
		//	else
		//	{
		//		bool flag3 = this.characterBody.skillLocator.secondary.skillDef.skillNameToken == "ROB_HENRY_BODY_SECONDARY_UZI_NAME";
		//		if (flag3)
		//		{
		//			this.childLocator.FindChild("GunModel").GetComponent<SkinnedMeshRenderer>().sharedMesh = Assets.mainAssetBundle.LoadAsset<Mesh>("meshUzi");
		//		}
		//	}
		//	bool flag4 = this.characterBody.skillLocator.special.skillDef.skillNameToken == "ROB_HENRY_BODY_SPECIAL_FRENZY_NAME";
		//	bool flag5 = this.characterBody.skillLocator.special.skillDef.skillNameToken == "ROB_HENRY_BODY_SPECIAL_SCEPFRENZY_NAME";
		//	if (flag5)
		//	{
		//		flag4 = true;
		//	}
		//	bool flag6 = !flag4;
		//	if (flag6)
		//	{
		//		HenryFuryComponent component = base.GetComponent<HenryFuryComponent>();
		//		bool flag7 = component;
		//		if (flag7)
		//		{
		//			Object.Destroy(component);
		//		}
		//	}
		//	bool flag8 = !flag && this.tracker;
		//	if (flag8)
		//	{
		//		Object.Destroy(this.tracker);
		//	}
		//}

		// Token: 0x060002FB RID: 763 RVA: 0x0002ADB4 File Offset: 0x00028FB4
		//public void UpdateCrosshair()
		//{
		//	GameObject crosshairPrefab = Assets.LoadCrosshair("Standard");
		//	bool flag = this.characterBody.skillLocator.secondary.skillDef.skillNameToken == "ROB_HENRY_BODY_SECONDARY_STINGER_NAME";
		//	if (flag)
		//	{
		//		crosshairPrefab = Assets.LoadCrosshair("SimpleDot");
		//	}
		//	bool flag2 = this.hasBazookaReady;
		//	if (flag2)
		//	{
		//		crosshairPrefab = Assets.GlaiveCrosshair;
		//	}
		//	this.characterBody.crosshairPrefab = crosshairPrefab;
		//}

		// Token: 0x060002FC RID: 764 RVA: 0x0002AE20 File Offset: 0x00029020
		//public void EnterFrenzy()
		//{
		//	this.inFrenzy = true;
		//	bool value = Config.rampageEffects.Value;
		//	if (value)
		//	{
		//		for (int i = 0; i < this.frenzyEffects.Length; i++)
		//		{
		//			bool flag = this.frenzyEffects[i];
		//			if (flag)
		//			{
		//				this.frenzyEffects[i].Play();
		//			}
		//		}
		//	}
		//}

		// Token: 0x060002FD RID: 765 RVA: 0x0002AE80 File Offset: 0x00029080
		//public void EnterScepterFrenzy()
		//{
		//	this.inFrenzy = true;
		//	bool value = Config.rampageEffects.Value;
		//	if (value)
		//	{
		//		for (int i = 0; i < this.frenzyEffects.Length; i++)
		//		{
		//			bool flag = this.frenzyEffects[i];
		//			if (flag)
		//			{
		//				this.frenzyEffects[i].Play();
		//			}
		//		}
		//	}
		//	this.childLocator.FindChild("SaiyanHair").gameObject.SetActive(true);
		//}

		// Token: 0x060002FE RID: 766 RVA: 0x0002AEFC File Offset: 0x000290FC
		//private void ExitFrenzy()
		//{
		//	this.inFrenzy = false;
		//	bool value = Config.rampageEffects.Value;
		//	if (value)
		//	{
		//		for (int i = 0; i < this.frenzyEffects.Length; i++)
		//		{
		//			bool flag = this.frenzyEffects[i];
		//			if (flag)
		//			{
		//				this.frenzyEffects[i].Stop();
		//			}
		//		}
		//	}
		//	this.childLocator.FindChild("SaiyanHair").gameObject.SetActive(false);
		//}


	}
}
