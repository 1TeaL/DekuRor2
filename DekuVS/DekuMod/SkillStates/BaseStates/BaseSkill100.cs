using DekuMod.Modules.Networking;
using DekuMod.Modules.Survivors;
using EntityStates;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.CameraTargetParams;

namespace DekuMod.SkillStates
{

	public class BaseSkill100 : BaseSkillState
	{
		public DekuController dekucon;
		public CharacterBody body;

		private CharacterCameraParamsData emoteCameraParams = new CharacterCameraParamsData()
		{
			maxPitch = 70,
			minPitch = -70,
			pivotVerticalOffset = 1f,
			idealLocalCameraPos = new Vector3(0, 0.0f, -15f),
			wallCushion = 0.1f,
		};
		private CameraParamsOverrideHandle camOverrideHandle;
        private float duration;

        public override void OnEnter()
		{
			base.OnEnter();
			dekucon = base.GetComponent<DekuController>();

			CameraParamsOverrideRequest request = new CameraParamsOverrideRequest
			{
				cameraParamsData = emoteCameraParams,
				priority = 0,
			};

			camOverrideHandle = base.cameraTargetParams.AddParamsOverride(request, duration);

		}


		public override void FixedUpdate()
		{
			base.FixedUpdate();

		}

		public override void OnExit()
        {
            base.OnExit();
			base.characterBody.hideCrosshair = false;
			base.cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 0.5f);
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}