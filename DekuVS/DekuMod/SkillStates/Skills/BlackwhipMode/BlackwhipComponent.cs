using System;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DekuMod.Modules.Networking;
using R2API.Networking;
using R2API.Networking.Interfaces;
using System.Net;
using DekuMod.Modules;

namespace DekuMod.SkillStates.BlackWhip
{
    public class BlackwhipComponent : MonoBehaviour
	{
		public ChildLocator child;
		public CharacterBody charbody;
		public CharacterBody dekucharbody;
        public GameObject blackwhipLineEffect;
        public LineRenderer blackwhipLineRenderer;

        public float timer;
        public float duration;
        public float totalDuration;
        public float elapsedTime;
        private float segments = 20;
        public Vector3 moveDirection;
        public bool hasFired;
        public bool pushDamage;

        public void Start()
		{
			charbody = this.gameObject.GetComponent<CharacterBody>();
            //effectObj = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.detroitEffect, charbody.footPosition, Quaternion.LookRotation(Vector3.up));
            //effectObj.transform.parent = charbody.gameObject.transform;
            child = charbody.gameObject.GetComponent<ChildLocator>();
            blackwhipLineEffect = UnityEngine.Object.Instantiate(Modules.Assets.blackwhipLineRenderer, child.FindChild("RHand").transform);
            blackwhipLineRenderer = blackwhipLineEffect.GetComponent<LineRenderer>();
            duration = totalDuration * (2/3f);
        }

        public void Update()
        {
            if (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime; // Increment the elapsed time
                float t = Mathf.Clamp01(elapsedTime / duration); // Calculate the interpolation factor (0 to 1)
                int currentSegments = Mathf.FloorToInt(t * segments); // Calculate the current number of segments

                for (int i = 0; i <= currentSegments; i++)
                {
                    Vector3 startPoint = child.FindChild("RHand").transform.position;
                    Vector3 endPoint = charbody.corePosition;
                    float segmentT = (float)i / segments; // Calculate the interpolation factor for this segment
                    Vector3 pointOnCurve = CalculateBezierPoint(segmentT, startPoint, (startPoint + endPoint) / 2, endPoint); // Calculate Bezier point
                    blackwhipLineRenderer.SetPosition(i, pointOnCurve); // Set the position of the segment
                }
            }
        }

        // Function to calculate a point on a quadratic Bezier curve
        Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 point = uu * p0; // (1 - t)^2 * p0
            point += 2 * u * t * p1; // 2 * (1 - t) * t * p1
            point += tt * p2;        // t^2 * p2

            return point;
        }



        public void FixedUpdate()
		{           
            if(timer > duration && !hasFired)
            {
                hasFired = true;
                if (pushDamage)
                {
                    new TakeDamageForceRequest(charbody.masterObjectId, moveDirection, StaticValues.blackwhipStrikeForce, dekucharbody.damage * StaticValues.blackwhipStrikeDamage, dekucharbody.masterObjectId);
                }
                else
                {
                    new BlackwhipImmobilizeRequest(charbody.masterObjectId, StaticValues.blackwhipOverdriveDamage * dekucharbody.damage, dekucharbody.masterObjectId);
                }
            }
            if(timer > totalDuration)
            {
                Destroy(this);
                Destroy(blackwhipLineEffect);
            }
            else
            {
                timer += Time.fixedDeltaTime;
            }
            if (charbody.healthComponent.alive)
			{

			}
			else if (!charbody)
			{
				Destroy(this);
                Destroy(blackwhipLineEffect);
            }
		}


	}
}
