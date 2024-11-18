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
        //public Vector3 moveDirection;
        public bool hasFired;
        private float segments = 20;

        public int heightDifference;
        //public bool pushDamage;

        public void Start()
		{
            //effectObj = UnityEngine.Object.Instantiate<GameObject>(Modules.Asset.detroitEffect, charbody.footPosition, Quaternion.LookRotation(Vector3.up));
            //effectObj.transform.parent = charbody.gameObject.transform;

            charbody = GetComponent<CharacterBody>();

            child = dekucharbody.gameObject.GetComponent<ModelLocator>().modelTransform.GetComponent<ChildLocator>();
            //blackwhipLineEffect = UnityEngine.Object.Instantiate(Modules.DekuAssets.blackwhipLineRenderer, child.FindChild("RHand").transform);
            blackwhipLineEffect = UnityEngine.Object.Instantiate(Modules.DekuAssets.blackwhipLineRenderer, dekucharbody.transform);
            blackwhipLineRenderer = blackwhipLineEffect.GetComponent<LineRenderer>();
            duration = totalDuration * (0.2f);

            heightDifference = UnityEngine.Random.Range(-5, 5);
        }

        public void Update()
        {
            if (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime; // Increment the elapsed time
            }
            float t = Mathf.Clamp01(elapsedTime / duration); // Calculate the interpolation factor (0 to 1)
            int currentSegments = Mathf.FloorToInt(t * segments); // Calculate the current number of segments

            //Vector3[] segmentPoints = new Vector3[currentSegments];
            Vector3[] vector3s = new Vector3[2];
            Vector3 startPoint = child.FindChild("RHand").transform.position;


            // Lerp from the start point to the end point
            Vector3 lerpedPosition = Vector3.Lerp(startPoint, charbody.corePosition, t);

            vector3s[0] = startPoint;
            vector3s[1] = lerpedPosition;
            blackwhipLineRenderer.positionCount = vector3s.Length;
            blackwhipLineRenderer.SetPositions(vector3s);
            blackwhipLineRenderer.startWidth = 0.3f;
            blackwhipLineRenderer.endWidth = 0.3f;

            // Lerp from the start point to the end point
            //Vector3 lerpedPosition = Vector3.Lerp(startPoint, charbody.corePosition, t);

            //blackwhipLineRenderer.startWidth = 0.3f;
            //blackwhipLineRenderer.endWidth = 0.3f;

            //vector3s[0] = startPoint;
            //vector3s[1] = lerpedPosition;
            //blackwhipLineRenderer.positionCount = vector3s.Length;
            //blackwhipLineRenderer.SetPositions(vector3s);

            //for (int i = 0; i < currentSegments; i++)
            //{
            //    //Vector3 startPoint = child.FindChild("RHand").transform.position;
            //    Vector3 endPoint = charbody.corePosition;
            //    float segmentT = (float)i / segments; // Calculate the interpolation factor for this segment
            //    Vector3 pointOnCurve = CalculateBezierPoint(segmentT, startPoint, (startPoint + endPoint) / 2 + Vector3.up * heightDifference, endPoint); // Calculate Bezier point

            //    segmentPoints[i] = pointOnCurve;
            //}

            //blackwhipLineRenderer.startWidth = 0.3f;
            //blackwhipLineRenderer.endWidth = 0.3f;

            //blackwhipLineRenderer.positionCount = currentSegments;
            //blackwhipLineRenderer.SetPositions(segmentPoints); // Set the position of the segment
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
            timer += Time.fixedDeltaTime;

            if(timer > totalDuration)
            {
                EntityState.Destroy(this);
            }
            if (!charbody | !charbody.healthComponent.alive)
            {
                EntityState.Destroy(this);
            }
            //if (timer > duration && !hasFired)
            //{
            //    hasFired = true;
            //    if (pushDamage)
            //    {
            //        Chat.AddMessage("push happened");
            //        new TakeDamageForceRequest(charbody.masterObjectId, moveDirection, StaticValues.blackwhipStrikeForce, dekucharbody.damage * StaticValues.blackwhipStrikeDamage, dekucharbody.masterObjectId);
            //    }
            //    else
            //    {
            //        Chat.AddMessage("immobilize happened");
            //        new BlackwhipImmobilizeRequest(charbody.masterObjectId, StaticValues.blackwhipOverdriveDamage * dekucharbody.damage, dekucharbody.masterObjectId);
            //    }
            //}
		}


	}
}
