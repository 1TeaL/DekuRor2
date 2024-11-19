using System;
using EntityStates;
using RoR2;
using UnityEngine;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
using DekuMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using DekuMod.Modules;

namespace DekuMod.SkillStates
{
    public class ShootStyleKickComponent : MonoBehaviour
	{
		public CharacterBody charbody;
		public CharacterBody dekucharbody;
		public float numberOfHits;
		public float currentNumber;
        private Transform modelTransform;
        private Animator animator;
        public float timer;
		public float damage;
		private Vector3 pos;

		public void Start()
		{
			charbody = this.gameObject.GetComponent<CharacterBody>();
			//effectObj = UnityEngine.Object.Instantiate<GameObject>(Modules.Asset.detroitEffect, charbody.footPosition, Quaternion.LookRotation(Vector3.up));
			//effectObj.transform.parent = charbody.gameObject.transform;
			pos = charbody.corePosition;

			currentNumber = 0f;

            this.modelTransform = gameObject.GetComponent<ModelLocator>().modelTransform;
			if( modelTransform != null )
			{
                this.animator = this.modelTransform.GetComponent<Animator>();
                TemporaryOverlayInstance temporaryOverlay = TemporaryOverlayManager.AddOverlay(new GameObject());
                temporaryOverlay.duration = 1f;
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.originalMaterial = DekuAssets.whiteblinkingMaterial;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.inspectorCharacterModel = this.animator.gameObject.GetComponent<CharacterModel>();
            }

        }
	


		public void FixedUpdate()
		{

            if (charbody.healthComponent.alive)
			{
				timer += Time.fixedDeltaTime;
				if (timer > 0.5f)
				{
					if (currentNumber < numberOfHits)
					{
						currentNumber += 1;
						timer -= 0.05f;
						new PeformShootStyleKickAttackNetworkRequest(charbody.masterObjectId, Vector3.up, 2f, damage, dekucharbody.masterObjectId).Send(NetworkDestination.Server);

                        EffectManager.SpawnEffect(Modules.DekuAssets.impactEffect, new EffectData
                        {
                            origin = pos,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(Vector3.up)

                        }, false);
                    }
					else if (currentNumber == numberOfHits)
					{
						AkSoundEngine.PostEvent("impactsfx", charbody.gameObject);
						currentNumber += 1;
						new PeformShootStyleKickAttackNetworkRequest(charbody.masterObjectId, Vector3.down, 20f, damage, dekucharbody.masterObjectId).Send(NetworkDestination.Server);
                        EffectManager.SpawnEffect(Modules.DekuAssets.impactEffect, new EffectData
                        {
                            origin = pos,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(charbody.characterDirection.forward)

                        }, true);
                        EffectManager.SpawnEffect(Modules.DekuAssets.impactShaderEffect, new EffectData
                        {
                            origin = pos,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(charbody.characterDirection.forward)

                        }, false);
                    }
					else if (currentNumber > numberOfHits)
					{

						Destroy(this);
					}

				}

			}
			else if (!charbody)
			{
				Destroy(this);
			}
		}


	}
}
