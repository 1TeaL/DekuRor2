//using System.Diagnostics;
//using RoR2;
//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;

//namespace DekuMod.Modules
//{

//	public class DekuController : MonoBehaviour
//	{

//		private void Awake()
//		{
//			this.characterBody = base.gameObject.GetComponent<CharacterBody>();
//			this.model = base.gameObject.GetComponentInChildren<CharacterModel>();
//			this.modelSkinController = base.gameObject.GetComponentInChildren<ModelSkinController>();
//			this.childLocator = base.gameObject.GetComponentInChildren<ChildLocator>();
//			this.skillLocator = base.gameObject.GetComponentInChildren<SkillLocator>();
//			this.modelLocator = base.gameObject.GetComponent<ModelLocator>();
//			this.jetStopwatch = new Stopwatch();
//		}

//		// Token: 0x060000DD RID: 221 RVA: 0x0000DE27 File Offset: 0x0000C027
//		//public void Start()
//		//{
//		//	base.StartCoroutine(this.SetupManipulator());
//		//}

//		// Token: 0x060000DE RID: 222 RVA: 0x0000DE37 File Offset: 0x0000C037
//		//private IEnumerator SetupManipulator()
//		//{
//		//	yield return new WaitForEndOfFrame();
//		//	this.FindVariantMaterials();
//		//	this.elementalSkills = this.GetElementalSkills();
//		//	this.currentElement = this.GetStartingElement();
//		//	this.UpdateElement(this.currentElement);
//		//	this.SetMaterialEmissive(this.currentElement);
//		//	this.hasSwapped = false;
//		//	yield break;
//		//}

//		// Token: 0x060000DF RID: 223 RVA: 0x0000DE48 File Offset: 0x0000C048
//		//private void FindVariantMaterials()
//		//{
//		//	int skinIndex = this.modelSkinController.currentSkinIndex;
//		//	Material tempMat2 = this.modelSkinController.skins[skinIndex].rendererInfos[0].defaultMaterial;
//		//	string tempName = tempMat2.name;
//		//	string searchname = tempName.Substring(0, tempName.IndexOf("-"));
//		//	this.variantFireMaterial = Shaders.GetMaterialFromStorage(searchname + "-Fire");
//		//	this.variantLightningMaterial = Shaders.GetMaterialFromStorage(searchname + "-Lightning");
//		//	this.variantIceMaterial = Shaders.GetMaterialFromStorage(searchname + "-Ice");
//		//}

//		// Token: 0x060000E0 RID: 224 RVA: 0x0000DEDC File Offset: 0x0000C0DC
//		//internal void UpdateElement(DekuController.Element element)
//		//{
//		//	this.currentElement = element;
//		//	foreach (KeyValuePair<ElementalSkillDef, GenericSkill> item in this.elementalDict)
//		//	{
//		//		item.Key.SwitchElement(item.Value, element);
//		//	}
//		//}

//		// Token: 0x060000E1 RID: 225 RVA: 0x0000DF4C File Offset: 0x0000C14C
//		public void FixedUpdate()
//		{
//			bool flag = this.hasfloatBuff;
//			if (flag)
//			{
//				this.jetStopwatch.Start();
//				bool flag2 = this.jetStopwatch.Elapsed.TotalSeconds >= (double)jetpackDuration;
//				if (flag2)
//				{
//					this.endJet = true;
//				}
//			}
//			else
//			{
//				bool isGrounded = this.characterBody.characterMotor.isGrounded;
//				if (isGrounded)
//				{
//					this.jetStopwatch.Reset();
//				}
//			}
//		}

//		// Token: 0x060000E2 RID: 226 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
//		//private ElementalSkillDef[] GetElementalSkills()
//		//{
//		//	List<ElementalSkillDef> elementalSkills = new List<ElementalSkillDef>();
//		//	foreach (GenericSkill skill in this.skillLocator.allSkills)
//		//	{
//		//		bool flag = skill.skillDef.GetType().IsAssignableFrom(typeof(ElementalSkillDef));
//		//		if (flag)
//		//		{
//		//			bool flag2 = skill.skillDef as ElementalSkillDef != null;
//		//			if (flag2)
//		//			{
//		//				elementalSkills.Add(skill.skillDef as ElementalSkillDef);
//		//				this.elementalDict.Add(skill.skillDef as ElementalSkillDef, skill);
//		//			}
//		//		}
//		//	}
//		//	return elementalSkills.ToArray();
//		//}

//		// Token: 0x060000E3 RID: 227 RVA: 0x0000E068 File Offset: 0x0000C268
//		//private DekuController.Element GetStartingElement()
//		//{
//		//	bool flag = this.elementSkill;
//		//	DekuController.Element result;
//		//	if (flag)
//		//	{
//		//		string skillName = this.elementSkill.skillDef.skillName;
//		//		if (!(skillName == "Manipulator.Start Fire"))
//		//		{
//		//			if (!(skillName == "Manipulator.Start Lightning"))
//		//			{
//		//				if (!(skillName == "Manipulator.Start Ice"))
//		//				{
//		//					Debug.LogWarning("ManipulatorMod: GetStartingElement warning: Returned default!");
//		//					result = DekuController.Element.Fire;
//		//				}
//		//				else
//		//				{
//		//					result = DekuController.Element.Ice;
//		//				}
//		//			}
//		//			else
//		//			{
//		//				result = DekuController.Element.Lightning;
//		//			}
//		//		}
//		//		else
//		//		{
//		//			result = DekuController.Element.Fire;
//		//		}
//		//	}
//		//	else
//		//	{
//		//		result = DekuController.Element.Fire;
//		//	}
//		//	return result;
//		//}

//		// Token: 0x060000E4 RID: 228 RVA: 0x0000E0E0 File Offset: 0x0000C2E0
//		//internal void SetMaterialEmissive(DekuController.Element newElement)
//		//{
//		//	switch (newElement)
//		//	{
//		//		case DekuController.Element.Fire:
//		//			this.UpdateMaterial(this.model, this.variantFireMaterial);
//		//			break;
//		//		case DekuController.Element.Lightning:
//		//			this.UpdateMaterial(this.model, this.variantLightningMaterial);
//		//			break;
//		//		case DekuController.Element.Ice:
//		//			this.UpdateMaterial(this.model, this.variantIceMaterial);
//		//			break;
//		//		default:
//		//			Debug.LogWarning("ManipulatorMod: SetMaterialEmissive warning: using default, may be recursive!");
//		//			this.SetMaterialEmissive(this.GetStartingElement());
//		//			break;
//		//	}
//		//}

//		// Token: 0x060000E5 RID: 229 RVA: 0x0000E160 File Offset: 0x0000C360
//		private void UpdateMaterial(CharacterModel model, Material newMat)
//		{
//			CharacterModel.RendererInfo[] rendererInfos = model.baseRendererInfos;
//			CharacterModel.RendererInfo bodyInfo = rendererInfos[model.baseRendererInfos.Length - 1];
//			CharacterModel.RendererInfo swordInfo = rendererInfos[0];
//			bodyInfo.defaultMaterial = newMat;
//			swordInfo.defaultMaterial = newMat;
//			rendererInfos.SetValue(bodyInfo, model.baseRendererInfos.Length - 1);
//			rendererInfos.SetValue(swordInfo, 0);
//			model.baseRendererInfos = rendererInfos;
//		}

//		// Token: 0x060000E6 RID: 230 RVA: 0x0000E1CC File Offset: 0x0000C3CC
//		//public void ElementalBonus(int hitCount, int targetSkill)
//		//{
//		//	GenericSkill genericTarget = null;
//		//	switch (targetSkill)
//		//	{
//		//		case 0:
//		//			genericTarget = this.skillLocator.primary;
//		//			break;
//		//		case 1:
//		//			genericTarget = this.skillLocator.secondary;
//		//			break;
//		//		case 2:
//		//			genericTarget = this.skillLocator.utility;
//		//			break;
//		//		case 3:
//		//			genericTarget = this.skillLocator.special;
//		//			break;
//		//	}
//		//	bool flag = this.characterBody.HasBuff(Buffs.fireBuff);
//		//	if (flag)
//		//	{
//		//		this.characterBody.RemoveBuff(Buffs.fireBuff);
//		//	}
//		//	bool flag2 = this.characterBody.HasBuff(Buffs.lightningBuff);
//		//	if (flag2)
//		//	{
//		//		for (int i = 0; i < hitCount; i++)
//		//		{
//		//			bool flag3 = genericTarget != null;
//		//			if (flag3)
//		//			{
//		//				genericTarget.rechargeStopwatch += 0.2f * (genericTarget.finalRechargeInterval - genericTarget.rechargeStopwatch);
//		//			}
//		//		}
//		//		this.characterBody.RemoveBuff(Buffs.lightningBuff);
//		//	}
//		//	bool flag4 = this.characterBody.HasBuff(Buffs.iceBuff);
//		//	if (flag4)
//		//	{
//		//		for (int j = 0; j < hitCount; j++)
//		//		{
//		//			this.characterBody.healthComponent.AddBarrier(this.characterBody.healthComponent.fullHealth * 0.05f);
//		//		}
//		//		this.characterBody.RemoveBuff(Buffs.iceBuff);
//		//	}
//		//}

//		// Token: 0x040000F8 RID: 248
//		public GenericSkill elementSkill;

//		// Token: 0x040000F9 RID: 249
//		internal bool hasfloatBuff;

//		// Token: 0x040000FA RID: 250
//		internal bool endJet;

//		// Token: 0x040000FB RID: 251
//		internal bool hasSwapped = false;

//		// Token: 0x040000FC RID: 252
//		//internal ElementalSkillDef[] elementalSkills;

//		// Token: 0x040000FD RID: 253
//		//internal Dictionary<ElementalSkillDef, GenericSkill> elementalDict = new Dictionary<ElementalSkillDef, GenericSkill>();

//		// Token: 0x040000FE RID: 254
//		public DekuController.Element currentElement;

//		// Token: 0x040000FF RID: 255
//		private CharacterBody characterBody;

//		// Token: 0x04000100 RID: 256
//		private CharacterModel model;

//		// Token: 0x04000101 RID: 257
//		private SkillLocator skillLocator;

//		// Token: 0x04000102 RID: 258
//		private Stopwatch jetStopwatch;

//		// Token: 0x04000103 RID: 259
//		private ModelSkinController modelSkinController;

//		// Token: 0x04000104 RID: 260
//		private ChildLocator childLocator;

//		// Token: 0x04000105 RID: 261
//		private ModelLocator modelLocator;

//		// Token: 0x04000106 RID: 262
//		private Material variantFireMaterial;

//		// Token: 0x04000107 RID: 263
//		private Material variantLightningMaterial;

//		// Token: 0x04000108 RID: 264
//		private Material variantIceMaterial;
//        private double jetpackDuration = 5f;

//        // Token: 0x02000053 RID: 83
//        public enum Element
//		{
//			// Token: 0x040001A0 RID: 416
//			None,
//			// Token: 0x040001A1 RID: 417
//			Fire,
//			// Token: 0x040001A2 RID: 418
//			Lightning,
//			// Token: 0x040001A3 RID: 419
//			Ice
//		}




//	}
//}
