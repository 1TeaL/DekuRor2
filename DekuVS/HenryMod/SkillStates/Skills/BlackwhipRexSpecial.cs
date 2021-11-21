//using RoR2;
//using UnityEngine;
//using System.Linq;
//using UnityEngine.Networking;
//using EntityStates;
//using EntityStates.Huntress;
//using EntityStates.LemurianBruiserMonster;
//using EntityStates.Treebot;

//namespace DekuMod.SkillStates
//{ 
//	public class BlackwhipSpawn : BaseSkillState
//	{
//		public static float duration = 0.2f;
//		public static string enterSoundString;
//		public override void OnEnter()
//		{
//			base.OnEnter();
//			//Util.PlaySound(SpawnState.enterSoundString, base.gameObject);
//			base.PlayAnimation("RightArm, Override", "Blackwhip");
//		}
//		public override void FixedUpdate()
//		{
//			base.FixedUpdate();
//			if (base.fixedAge >= BlackwhipSpawn.duration)
//			{
//				this.outer.SetNextState(new BlackwhipProjectile());
//			}
//		}
//	}
//}