﻿using System;
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
		public float numberOfHits;
		public float currentNumber;
		public float timer;
		public float damage;

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
			if (timer > 1f)
            {
				if(currentNumber < numberOfHits)
				{
					currentNumber += 1;
					timer -= 0.1f;
					new PeformShootStyleKickAttackNetworkRequest(charbody.masterObjectId, Vector3.up, 2f, damage).Send(NetworkDestination.Server);
				}
				else if (currentNumber == numberOfHits)
				{
					currentNumber += 1;
					new PeformShootStyleKickAttackNetworkRequest(charbody.masterObjectId, Vector3.down, 100f, damage).Send(NetworkDestination.Server);
				}
				else if (currentNumber > numberOfHits)
                {

					Destroy(this);
				}

            }


			if (!charbody)
			{
				Destroy(this);
			}
		}


	}
}