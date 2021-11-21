using System.Collections.Generic;
using System.Linq;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates.ClayBoss;
using EntityStates;
using EntityStates.Huntress;

namespace DekuMod.SkillStates
{

	public class BlackwhipPull : BaseSkillState
	{
		private GameObject areaIndicator;

		public static float duration = 10f;
		public static float maxTetherDistance = 40f;
		public static float tetherMulchDistance = 5f;
		public static float tetherMulchDamageScale = 2f;
		public static float tetherMulchTickIntervalScale = 0.5f;
		public static float damagePerSecond = 2f;
		public static float damageTickFrequency = 3f;
		public static float entryDuration = 1f;
		public static GameObject mulchEffectPrefab;
		public static string enterSoundString;
		public static string beginMulchSoundString;
		public static string stopMulchSoundString;
		private GameObject mulchEffect;
		private Transform muzzleTransform;
		private List<Skills.TarTetherController> tetherControllers;
		private float stopwatch;
		private uint soundID;

		private BlackwhipPull.SubState subState;
		private enum SubState
		{
			Entry,
			Tethers
		}
		public override void OnEnter()
		{
			base.OnEnter();
			this.stopwatch = 0f;
			//if (NetworkServer.active && base.characterBody)
			////{
			//	base.characterBody.AddBuff(RoR2Content.Buffs.ArmorBoost);
			//}
            if (base.modelLocator)
            {
                
				ChildLocator component = base.modelLocator.modelTransform.GetComponent<ChildLocator>();
                if (component)
                {
                    this.muzzleTransform = component.FindChild("RHand");
                }
            }
            this.subState = BlackwhipPull.SubState.Entry;
            //base.PlayCrossfade("Body", "PrepSiphon", "PrepSiphon.playbackRate", BlackwhipPull.entryDuration, 0.1f);
            base.PlayAnimation("RightArm, Override", "Blackwhip", "Attack.playbackRate", 0.1f);
			//this.soundID = Util.PlayAttackSpeedSound(Recover.enterSoundString, base.gameObject, this.attackSpeedStat);
		}


		private void FireTethers()
		{
			Vector3 position = this.muzzleTransform.position;
			float breakDistanceSqr = BlackwhipPull.maxTetherDistance * BlackwhipPull.maxTetherDistance;
			List<GameObject> list = new List<GameObject>();
			this.tetherControllers = new List<Skills.TarTetherController>();
			BullseyeSearch bullseyeSearch = new BullseyeSearch();
			bullseyeSearch.searchOrigin = position;
			bullseyeSearch.maxDistanceFilter = BlackwhipPull.maxTetherDistance;
			bullseyeSearch.teamMaskFilter = TeamMask.AllExcept(TeamIndex.Player);
			bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
			bullseyeSearch.filterByLoS = true;
			bullseyeSearch.searchDirection = Vector3.up;
			bullseyeSearch.RefreshCandidates();
			bullseyeSearch.FilterOutGameObject(base.gameObject);
			List<HurtBox> list2 = bullseyeSearch.GetResults().ToList<HurtBox>();
			Debug.Log(list2);
			for (int i = 0; i < list2.Count; i++)
			{
				GameObject gameObject = list2[i].healthComponent.gameObject;
				if (gameObject)
				{
					list.Add(gameObject);
				}
			}
			float tickInterval = 1f / BlackwhipPull.damageTickFrequency;
			float damageCoefficientPerTick = BlackwhipPull.damagePerSecond / BlackwhipPull.damageTickFrequency;
			float mulchDistanceSqr = BlackwhipPull.tetherMulchDistance * BlackwhipPull.tetherMulchDistance;
			GameObject original = Resources.Load<GameObject>("Prefabs/NetworkedObjects/TarTether");
			for (int j = 0; j < list.Count; j++)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(original, position, Quaternion.identity);
				Skills.TarTetherController component = gameObject2.GetComponent<Skills.TarTetherController>();
				component.NetworkownerRoot = base.gameObject;
				component.NetworktargetRoot = list[j];
				component.breakDistanceSqr = breakDistanceSqr;
				component.damageCoefficientPerTick = damageCoefficientPerTick;
				component.tickInterval = tickInterval;
				component.tickTimer = (float)j * 0.1f;
				component.mulchDistanceSqr = mulchDistanceSqr;
				component.mulchDamageScale = BlackwhipPull.tetherMulchDamageScale;
				component.mulchTickIntervalScale = BlackwhipPull.tetherMulchTickIntervalScale;
				this.tetherControllers.Add(component);
				NetworkServer.Spawn(gameObject2);
			}
		}

		private void DestroyTethers()
		{
			if (this.tetherControllers != null)
			{
				for (int i = this.tetherControllers.Count - 1; i >= 0; i--)
				{
					if (this.tetherControllers[i])
					{
						EntityState.Destroy(this.tetherControllers[i].gameObject);
					}
				}
			}
		}

		public override void OnExit()
		{
			this.DestroyTethers();
			if (this.mulchEffect)
			{
				EntityState.Destroy(this.mulchEffect);
			}
			//AkSoundEngine.StopPlayingID(this.soundID);
			Util.PlaySound(Recover.stopMulchSoundString, base.gameObject);
			//if (NetworkServer.active && base.characterBody)
			//{
			//	base.characterBody.RemoveBuff(RoR2Content.Buffs.ArmorBoost);
			//}
			base.OnExit();
		}

		private static void RemoveDeadTethersFromList(List<Skills.TarTetherController> tethersList)
		{
			for (int i = tethersList.Count - 1; i >= 0; i--)
			{
				if (!tethersList[i])
				{
					tethersList.RemoveAt(i);
				}
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.stopwatch += Time.fixedDeltaTime;

			if (this.subState == BlackwhipPull.SubState.Entry)
			{
				if (this.stopwatch >= BlackwhipPull.entryDuration)
				{
					this.subState = BlackwhipPull.SubState.Tethers;
					this.stopwatch = 0f;
					//base.PlayAnimation("Body", "ChannelSiphon");
					Util.PlaySound(Recover.beginMulchSoundString, base.gameObject);
					if (NetworkServer.active)
					{
						this.FireTethers();
						this.mulchEffect = UnityEngine.Object.Instantiate<GameObject>(Recover.mulchEffectPrefab, this.muzzleTransform.position, Quaternion.identity);
						ChildLocator component = this.mulchEffect.gameObject.GetComponent<ChildLocator>();
						if (component)
						{
							//if (this.areaIndicator.transform)
							//{
							//	this.areaIndicator.transform.localScale = new Vector3(BlackwhipPull.maxTetherDistance * 2f, BlackwhipPull.maxTetherDistance * 2f, BlackwhipPull.maxTetherDistance * 2f);
							//}
						}
						this.mulchEffect.transform.parent = this.muzzleTransform;
						return;
					}
				}
			}
			else if (this.subState == BlackwhipPull.SubState.Tethers && NetworkServer.active)
			{

				BlackwhipPull.RemoveDeadTethersFromList(this.tetherControllers);
				if ((this.stopwatch >= BlackwhipPull.duration || this.tetherControllers.Count == 0) && base.isAuthority)
				{
					this.outer.SetNextState(new RecoverExit());
					return;
				}
			}
		}

	}
}
