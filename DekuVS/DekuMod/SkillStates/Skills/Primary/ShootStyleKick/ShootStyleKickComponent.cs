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

namespace DekuMod.SkillStates
{
    public class ShootStyleKickComponent : MonoBehaviour
	{
		public CharacterBody charbody;
		private GameObject effectObj;
		public float numberOfHits;
		public float currentNumber;
		public float timer;

		public void Start()
		{
			charbody = this.gameObject.GetComponent<CharacterBody>();
			//effectObj = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.detroitEffect, charbody.footPosition, Quaternion.LookRotation(Vector3.up));
			//effectObj.transform.parent = charbody.gameObject.transform;

			currentNumber = 0f;
		}
	


		public void FixedUpdate()
		{

			timer += Time.fixedDeltaTime;
			if (timer > 0.1f)
            {
				if(currentNumber < numberOfHits)
				{ 
					new PeformShootStyleKickAttackNetworkRequest(charbody.masterObjectId, Vector3.up, 5f).Send(NetworkDestination.Clients);
				}
				else if (currentNumber == numberOfHits)
				{

					new PeformShootStyleKickAttackNetworkRequest(charbody.masterObjectId, Vector3.down, 100f).Send(NetworkDestination.Clients);
				}
				else if (currentNumber > numberOfHits)
                {

					Destroy(this);
				}

            }


			if (!charbody)
			{
				Destroy(this);
				Destroy(effectObj);
			}
		}


		public void OnDestroy()
		{
			Destroy(effectObj);
		}
	}
}
