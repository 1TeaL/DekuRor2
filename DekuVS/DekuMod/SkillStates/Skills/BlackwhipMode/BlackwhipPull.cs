using DekuMod.SkillStates.BaseStates;
using EntityStates;
using R2API.Networking;
using RoR2;
using UnityEngine;
using System;
using EntityStates.Huntress;
using DekuMod.Modules;
using System.Net;
using Rewired.UI.ControlMapper;
using ExtraSkillSlots;
using static RoR2.CameraTargetParams;
using Unity.Audio;

namespace DekuMod.SkillStates.BlackWhip
{
    public class BlackwhipPull : BaseDekuSkillState
    {
        private ExtraInputBankTest extrainputBankTest;
        public Vector3 moveDirection;
        public float fireTime;
        public BullseyeSearch search;
        public bool hasFired;

        //teleporting up
        private GameObject aimSphere;
        public float radius = 1f;
        private Ray aimRay;
        private float baseDistance = StaticValues.blackwhipPullDistance;
        private float movespeed;
        private float maxDistance;

        private bool initiallyGrounded;
        private bool isRayCast;
        private bool isWorld;
        private bool isEnemy;
        private CharacterBody enemyBody;
        public ChildLocator child;
        public GameObject blackwhipLineEffect;
        public LineRenderer blackwhipLineRenderer;
        private float elapsedTime;
        private Vector3 endPoint;
        private Vector3 targetVelocity;



        private CameraParamsOverrideHandle camOverrideHandle;
        private CharacterCameraParamsData cameraParams = new CharacterCameraParamsData()
        {
            maxPitch = 70,
            minPitch = -70,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = new Vector3(0f, 0f, -20f),
            wallCushion = 0.1f,
            fov = 90f,
        };
        private float lastDistance;
        private float currentDistance;

        public override void OnEnter()
        {
            base.OnEnter();
            extrainputBankTest = gameObject.GetComponent<ExtraInputBankTest>();


            base.StartAimMode(1f, true);

            float num = this.moveSpeedStat;
            bool isSprinting = base.characterBody.isSprinting;
            if (isSprinting)
            {
                num /= base.characterBody.sprintingSpeedMultiplier;
            }
            float num2 = (num / base.characterBody.baseMoveSpeed) * 0.67f;
            movespeed = num2 + 1f;

            maxDistance = baseDistance * movespeed;
            child = base.GetModelChildLocator();

            switch (level)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }

            this.aimRay = base.GetAimRay();

            RaycastHit raycastHit;
            bool raycast = Physics.Raycast(aimRay.origin, aimRay.direction, out raycastHit, this.maxDistance, LayerIndex.world.mask);




            if (dekucon.GetTrackingTarget())
            {
                isRayCast = true;

                isEnemy = true;
                enemyBody = dekucon.trackingTarget.healthComponent.body;

                this.aimSphere = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
                this.aimSphere.transform.localScale = new Vector3(this.radius, this.radius, this.radius);

                blackwhipLineEffect = UnityEngine.Object.Instantiate(Modules.Assets.blackwhipLineRenderer, child.FindChild("LHand").transform);
                blackwhipLineRenderer = blackwhipLineEffect.GetComponent<LineRenderer>();
                fireTime = (Vector2.Distance(enemyBody.corePosition, characterBody.corePosition) / maxDistance) * StaticValues.blackwhipPullDuration;
                this.aimSphere.transform.position = enemyBody.corePosition;
                this.aimSphere.transform.up = Vector3.up;
                this.aimSphere.transform.forward = this.aimRay.direction;
                Chat.AddMessage("enemybody position" + enemyBody.corePosition);
                


                CameraParamsOverrideRequest request = new CameraParamsOverrideRequest
                {
                    cameraParamsData = cameraParams,
                    priority = 0,
                };

                camOverrideHandle = base.cameraTargetParams.AddParamsOverride(request, StaticValues.blackwhipPullDuration);


                this.lastDistance = Vector3.Distance(aimRay.origin, enemyBody.corePosition);
                if (base.characterMotor)
                {
                    Vector3 direction = aimRay.direction;
                    Vector3 vector = base.characterMotor.velocity;
                    vector = ((Vector3.Dot(vector, direction) < 0f) ? Vector3.zero : Vector3.Project(vector, direction));
                    vector += direction * StaticValues.blackwhipPullLookImpulse;
                    vector += base.characterMotor.moveDirection * StaticValues.blackwhipPullMoveImpulse;
                    base.characterMotor.velocity = vector;
                }

            }
            else if (raycast)
            {
                isRayCast = true;

                isWorld = true;
                endPoint = raycastHit.point;


                this.aimSphere = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
                this.aimSphere.transform.localScale = new Vector3(this.radius, this.radius, this.radius);
                blackwhipLineEffect = UnityEngine.Object.Instantiate(Modules.Assets.blackwhipLineRenderer, child.FindChild("LHand").transform);
                blackwhipLineRenderer = blackwhipLineEffect.GetComponent<LineRenderer>();

                fireTime = (Vector2.Distance(endPoint, characterBody.corePosition) / maxDistance) * StaticValues.blackwhipPullDuration;
                this.aimSphere.transform.position = raycastHit.point;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = this.aimRay.direction;
                Chat.AddMessage("endPoint" + endPoint);
                

                lastDistance = Vector3.Distance(transform.position, endPoint);

                CameraParamsOverrideRequest request = new CameraParamsOverrideRequest
                {
                    cameraParamsData = cameraParams,
                    priority = 0,
                };

                camOverrideHandle = base.cameraTargetParams.AddParamsOverride(request, StaticValues.blackwhipPullDuration);

                if (base.characterMotor)
                {
                    Vector3 direction = aimRay.direction;
                    Vector3 vector = base.characterMotor.velocity;
                    vector = ((Vector3.Dot(vector, direction) < 0f) ? Vector3.zero : Vector3.Project(vector, direction));
                    vector += direction * StaticValues.blackwhipPullLookImpulse;
                    vector += base.characterMotor.moveDirection * StaticValues.blackwhipPullMoveImpulse;
                    base.characterMotor.velocity = vector;
                }
            }
            else
            {
                this.outer.SetNextStateToMain();
                return;
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }

        public override void Update()
        {
            this.UpdateAreaIndicator();
            
            if(isRayCast)
            {
                if (elapsedTime < fireTime)
                {
                    elapsedTime += Time.deltaTime; // Increment the elapsed time
                }
                float t = Mathf.Clamp01(elapsedTime / fireTime); // Calculate the interpolation factor (0 to 1)

                Vector3[] vector3s = new Vector3[2];
                Vector3 startPoint = child.FindChild("LHand").transform.position;
                if (isEnemy)
                {
                    endPoint = enemyBody.corePosition;
                }

                // Lerp from the start point to the end point
                Vector3 lerpedPosition = Vector3.Lerp(startPoint, endPoint, t);
                
                vector3s[0] = startPoint;
                vector3s[1] = lerpedPosition;
                blackwhipLineRenderer.positionCount = vector3s.Length;
                blackwhipLineRenderer.SetPositions(vector3s);
                blackwhipLineRenderer.startWidth = 0.3f;
                blackwhipLineRenderer.endWidth = 0.3f;


                if (!base.IsKeyDownAuthority())
                {
                    //if (base.fixedAge > fireTime)
                    //{
                    //    SmallHop(characterMotor, StaticValues.blackwhipPullHop);
                    //}
                    this.outer.SetNextStateToMain();
                    return;
                }
                else
                {
                    if (base.fixedAge > fireTime)
                    {
                        //characterBody.characterMotor.Motor.RotateCharacter(Quaternion.LookRotation(base.GetAimRay().direction));
                        Chat.AddMessage($"{characterBody.characterMotor.velocity}" + "velocity");


                        if (isEnemy)
                        {
                            endPoint = enemyBody.corePosition;
                        }

                        currentDistance = (endPoint - transform.position).magnitude;

                        float acceleration = StaticValues.blackwhipPullAcceleration;
                        if (this.currentDistance > this.lastDistance)
                        {
                            acceleration *= StaticValues.blackwhipPullEscapeForce;
                        }
                        this.lastDistance = this.currentDistance;
                        if (base.isAuthority && base.characterMotor && base.characterBody)
                        {
                            Ray ownerAimRay = base.GetAimRay();
                            Vector3 normalized = (endPoint - characterBody.aimOrigin).normalized;
                            Vector3 force = normalized * acceleration;
                            float time = Mathf.Clamp01(base.fixedAge / StaticValues.blackwhipPulllookAccelerationRampUpDuration);
                            float curveModifier = Mathf.Log10(time) + 1f;
                            float aimRayRemap = Util.Remap(Vector3.Dot(ownerAimRay.direction, normalized), -1f, 1f, 1f, 0f);
                            force += ownerAimRay.direction * (StaticValues.blackwhipPullLookAcceleration * curveModifier * aimRayRemap);
                            force += base.characterMotor.moveDirection * StaticValues.blackwhipPullMoveAcceleration;
                            base.characterMotor.ApplyForce(force * (base.characterMotor.mass * Time.deltaTime), true, true);
                        }

                        //Vector3 directionToGrapple = (endPoint - characterBody.corePosition).normalized;

                        //// Calculate the target position to maintain the desired distance from the grapple point
                        //Vector3 targetPosition = endPoint - directionToGrapple * initialDistance;
                        //// Calculate the vector to move Deku towards the target position
                        //Vector3 moveToTarget = targetPosition - transform.position;

                        //Vector3 aimDirection = base.inputBank.aimDirection.normalized;
                        //Vector3 moveInput = new Vector3(aimDirection.x, 0f, aimDirection.z);

                        //float lookAngle = Vector3.Angle(moveInput, directionToGrapple);

                        //if(lookAngle <= 90f)
                        //{
                        //    moveInput *= lookAngle / 90f;
                        //}
                        //else if (lookAngle > 90f)
                        //{
                        //    moveInput *= 1 - (lookAngle-90f / 90f);
                        //}

                        //float lerpValue = 0.5f;    
                        //lerpValue = Mathf.Clamp(Vector3.Distance(transform.position, endPoint) / initialDistance, 0f, 1f);
                        //float yDistance = Mathf.Lerp(1f, 0f, targetPosition.y - characterBody.corePosition.y);


                        ////Vector3 totalDirection = (directionToGrapple * StaticValues.blackwhipPullGrappleMultiplier 
                        ////    + airControl * StaticValues.blackwhipPullInputMultiplier).normalized;

                        //// Blend the directions
                        //Vector3 blendedDirection = Vector3.Lerp(moveInput, directionToGrapple, lerpValue).normalized;

                        //Vector3 gravity = Physics.gravity;
                        //float gravityStrength = gravity.magnitude;

                        //targetVelocity = blendedDirection * StaticValues.blackwhipPullSpeed + gravity;
                        //targetVelocity += yDistance * characterBody.characterMotor.velocity;

                        //if(Vector3.Distance(transform.position, endPoint) > initialDistance)
                        //{
                        //    targetVelocity = blendedDirection * StaticValues.blackwhipPullSpeed;
                        //}

                        //// Lerp towards the target position to maintain distance smoothly
                        ////targetVelocity = Vector3.Lerp(characterBody.characterMotor.velocity, moveToTarget  * StaticValues.blackwhipPullSpeed, Time.deltaTime);
                        //Vector3.SmoothDamp(characterBody.characterMotor.velocity, targetVelocity, ref characterBody.characterMotor.velocity, 1f, StaticValues.blackwhipPullSpeed * 3f,  Time.deltaTime);                       

                        ////base.characterBody.characterMotor.velocity = targetVelocity;
                        ////base.characterBody.characterMotor.velocity += airControl;
                        ////base.characterMotor.velocity = targetVelocity;
                        ////adjust animation based on ascend/descend?                          



                    }

                }
                
            }
            else
            {
                this.outer.SetNextStateToMain();
                return;
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

        private void UpdateAreaIndicator()
        {
            //if (isAuthority)
            //{
            //    this.aimSphere.transform.localScale = new Vector3(this.radius, this.radius, this.radius);
            //}

            RaycastHit raycastHit;
            bool raycast = Physics.Raycast(aimRay.origin, aimRay.direction, out raycastHit, this.maxDistance, LayerIndex.world.mask | LayerIndex.entityPrecise.mask);
            
            if (isEnemy)
            {
                this.aimSphere.transform.position = enemyBody.corePosition;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = this.aimRay.direction;
            }
            else if (isWorld)
            {
                this.aimSphere.transform.position = endPoint;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = this.aimRay.direction;
            }

            
        }
        

        public override void OnExit()
        {
            base.OnExit();
            if(blackwhipLineEffect) Destroy(blackwhipLineEffect);
            if(aimSphere != null) Destroy(aimSphere);
            cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 0.5f);
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}