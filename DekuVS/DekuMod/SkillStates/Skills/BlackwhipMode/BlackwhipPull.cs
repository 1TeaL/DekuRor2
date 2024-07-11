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
        private float initialDistance;
        private float accelTime;

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
            idealLocalCameraPos = new Vector3(0f, 0f, -30f),
            wallCushion = 0.1f,
            fov = 90f,
        };

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
                initialDistance = Vector3.Distance(enemyBody.corePosition, characterBody.corePosition);

                this.aimSphere = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
                this.aimSphere.transform.localScale = new Vector3(this.radius, this.radius, this.radius);

                blackwhipLineEffect = UnityEngine.Object.Instantiate(Modules.Assets.blackwhipLineRenderer, child.FindChild("RHand").transform);
                blackwhipLineRenderer = blackwhipLineEffect.GetComponent<LineRenderer>();
                fireTime = (Vector2.Distance(enemyBody.corePosition, characterBody.corePosition) / maxDistance) * StaticValues.blackwhipPullDuration;
                this.aimSphere.transform.position = enemyBody.corePosition;
                this.aimSphere.transform.up = Vector3.up;
                this.aimSphere.transform.forward = this.aimRay.direction;
                Chat.AddMessage("enemybody position" + enemyBody.corePosition);
                if(isGrounded)
                {
                    SmallHop(characterMotor, movespeed * StaticValues.blackwhipPullHop);
                    initiallyGrounded = true;
                }


                CameraParamsOverrideRequest request = new CameraParamsOverrideRequest
                {
                    cameraParamsData = cameraParams,
                    priority = 0,
                };

                camOverrideHandle = base.cameraTargetParams.AddParamsOverride(request, StaticValues.blackwhipPullDuration);
            }
            else if (raycast)
            {
                isRayCast = true;

                isWorld = true;
                endPoint = raycastHit.point;
                initialDistance = Vector3.Distance(endPoint, characterBody.corePosition);


                this.aimSphere = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
                this.aimSphere.transform.localScale = new Vector3(this.radius, this.radius, this.radius);
                blackwhipLineEffect = UnityEngine.Object.Instantiate(Modules.Assets.blackwhipLineRenderer, child.FindChild("RHand").transform);
                blackwhipLineRenderer = blackwhipLineEffect.GetComponent<LineRenderer>();

                fireTime = (Vector2.Distance(endPoint, characterBody.corePosition) / maxDistance) * StaticValues.blackwhipPullDuration;
                this.aimSphere.transform.position = raycastHit.point;
                this.aimSphere.transform.up = raycastHit.normal;
                this.aimSphere.transform.forward = this.aimRay.direction;
                Chat.AddMessage("endPoint" + endPoint);
                //if (StaticValues.Includes(LayerIndex.entityPrecise.mask, raycastHit.collider.gameObject.layer))
                //{
                //    isEnemy = true;
                //    this.aimSphere.transform.position = enemyBody.corePosition;
                //    this.aimSphere.transform.up = raycastHit.normal;
                //    this.aimSphere.transform.forward = this.aimRay.direction;
                //    Chat.AddMessage("enemybody position" + enemyBody.corePosition);
                //}
                //else if (StaticValues.Includes(LayerIndex.world.mask, raycastHit.collider.gameObject.layer))
                //{
                //    isWorld = true;
                //    endPoint = raycastHit.point;
                //    this.aimSphere.transform.position = raycastHit.point;
                //    this.aimSphere.transform.up = raycastHit.normal;
                //    this.aimSphere.transform.forward = this.aimRay.direction;
                //    Chat.AddMessage("endPoint" + endPoint);
                //}
                if (isGrounded)
                {
                    SmallHop(characterMotor, movespeed * StaticValues.blackwhipPullHop);
                    initiallyGrounded = true;
                }


                CameraParamsOverrideRequest request = new CameraParamsOverrideRequest
                {
                    cameraParamsData = cameraParams,
                    priority = 0,
                };

                camOverrideHandle = base.cameraTargetParams.AddParamsOverride(request, StaticValues.blackwhipPullDuration);
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

                accelTime += Time.deltaTime;
                Chat.AddMessage(accelTime + "accel time seconds");

                if (!base.IsKeyDownAuthority())
                {
                    if (base.fixedAge > fireTime)
                    {
                        SmallHop(characterMotor, movespeed * StaticValues.blackwhipPullHop);
                    }
                    this.outer.SetNextStateToMain();
                    return;
                }
                else
                {
                    if (base.fixedAge > fireTime)
                    {
                        characterBody.characterMotor.Motor.RotateCharacter(Quaternion.LookRotation(base.GetAimRay().direction));

                        //travel until get close enough, then slow down and also play animation change if needed
                        if (isEnemy)
                        {
                            if (Vector2.Distance(enemyBody.corePosition, characterBody.corePosition) > 1f)
                            {
                                Vector3 directionToGrapple = (enemyBody.corePosition - characterBody.corePosition).normalized;
                                Vector3 moveInput = characterBody.inputBank.moveVector.normalized;

                                float currentDistance = Vector3.Distance(enemyBody.corePosition, characterBody.corePosition);

                                // Calculate the target position to maintain the desired distance from the grapple point
                                Vector3 targetPosition = enemyBody.corePosition - directionToGrapple * initialDistance;
                                // Calculate the vector to move Deku towards the target position
                                Vector3 moveToTarget = targetPosition - transform.position;
                                // Lerp towards the target position to maintain distance smoothly


                                if (base.inputBank.jump.down)
                                {
                                    Vector3 targetVelocity = directionToGrapple * movespeed * StaticValues.blackwhipPullSpeed;
                                    if (initiallyGrounded && base.characterMotor.velocity.y <= 0f)
                                    {
                                        targetVelocity += Vector3.up * movespeed;
                                    }
                                    base.characterMotor.velocity = targetVelocity;
                                    //play travel animation/ set animation bool 

                                }
                                else
                                if (!base.inputBank.jump.down)
                                {
                                    if (currentDistance < initialDistance)
                                    {
                                        initialDistance = currentDistance;
                                    }
                                    Vector3 aimDirection = base.inputBank.aimDirection;
                                    Vector3 normalized = new Vector3(aimDirection.x, 0f, aimDirection.z).normalized;
                                    float forwardMultiplier = Vector3.Dot(base.inputBank.moveVector, normalized);
                                    if (forwardMultiplier < 0.3f)
                                    {
                                        forwardMultiplier = 0.3f;
                                    }

                                    // Adjust movement vector with player input
                                    Vector3 airControl = Vector3.zero;

                                    if (base.inputBank.moveVector != Vector3.zero)
                                    {
                                        airControl = forwardMultiplier * moveInput * movespeed * StaticValues.blackwhipPullSpeedControl;
                                    }


                                    // Lerp towards the target position to maintain distance smoothly
                                    targetVelocity = Vector3.Lerp(characterBody.characterMotor.velocity, moveToTarget * movespeed * StaticValues.blackwhipPullSpeed, Time.deltaTime);
                                    // Combine air control with target velocity
                                    targetVelocity += airControl;


                                    base.characterMotor.velocity = targetVelocity;
                                    //adjust animation based on ascend/descend?
                                }
                                //else
                                //{
                                //    Vector3 targetVelocity = directionToGrapple * movespeed * StaticValues.blackwhipPullSpeed;
                                //    base.characterMotor.velocity = targetVelocity;

                                //}
                                //if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
                                //{
                                //    Vector3 vector = (normalized * movespeed * StaticValues.blackwhipPullSpeed) + moveInput * movespeed;

                                //    base.characterMotor.velocity = vector;
                                //}
                            }
                            else
                            {
                                base.characterMotor.velocity = Vector3.zero;
                                //Vector3 normalized = (enemyBody.corePosition - characterBody.corePosition).normalized;
                                //if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
                                //{
                                //    Vector3 vector = normalized * StaticValues.blackwhipPullSpeed;

                                //    base.characterMotor.velocity = vector;
                                //}
                            }
                        }
                        else if (isWorld)
                        {
                            if (Vector2.Distance(endPoint, characterBody.corePosition) > 1f)
                            {
                                Vector3 directionToGrapple = (endPoint - characterBody.corePosition).normalized;
                                Vector3 moveInput = characterBody.inputBank.moveVector.normalized;

                                float currentDistance = Vector3.Distance(endPoint, characterBody.corePosition);

                                // Calculate the target position to maintain the desired distance from the grapple point
                                Vector3 targetPosition = endPoint - directionToGrapple * initialDistance;
                                // Calculate the vector to move Deku towards the target position
                                Vector3 moveToTarget = targetPosition - transform.position;


                                if (base.inputBank.jump.down)
                                {
                                    Vector3 targetVelocity = directionToGrapple * movespeed * StaticValues.blackwhipPullSpeed;
                                    if (initiallyGrounded && base.characterMotor.velocity.y <= 0f)
                                    {
                                        targetVelocity += Vector3.up * movespeed;
                                    }
                                    base.characterMotor.velocity = targetVelocity;
                                    //play travel animation/ set animation bool 

                                }
                                else
                                if (!base.inputBank.jump.down)
                                {
                                    if (currentDistance < initialDistance)
                                    {
                                        initialDistance = currentDistance;
                                    }
                                    Vector3 aimDirection = base.inputBank.aimDirection;
                                    Vector3 normalized = new Vector3(aimDirection.x, 0f, aimDirection.z).normalized;
                                    float forwardMultiplier = Vector3.Dot(base.inputBank.moveVector, normalized);
                                    if (forwardMultiplier < 0.3f)
                                    {
                                        forwardMultiplier = 0.3f;
                                    }

                                    // Adjust movement vector with player input
                                    Vector3 airControl = Vector3.zero;

                                    if (base.inputBank.moveVector != Vector3.zero)
                                    {
                                        airControl = forwardMultiplier * moveInput * movespeed * StaticValues.blackwhipPullSpeedControl;
                                    }

                                    // Lerp towards the target position to maintain distance smoothly
                                    targetVelocity = Vector3.Lerp(characterBody.characterMotor.velocity, moveToTarget * movespeed * StaticValues.blackwhipPullSpeed, Time.deltaTime);
                                    // Combine air control with target velocity
                                    targetVelocity += airControl;


                                    base.characterMotor.velocity = targetVelocity;
                                    //adjust animation based on ascend/descend?
                                }

                                //else
                                //{
                                //    Vector3 targetVelocity = directionToGrapple * movespeed * StaticValues.blackwhipPullSpeed;
                                //    base.characterMotor.velocity = targetVelocity;

                                //}


                                //    if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
                                //    {
                                //        Vector3 vector = (normalized * movespeed * StaticValues.blackwhipPullSpeed) + moveInput * movespeed;

                                //        base.characterMotor.velocity = vector;

                                //    }
                            }
                            else
                            {
                                base.characterMotor.velocity = Vector3.zero;
                            }
                        }

                    }

                }
                //else if(!extrainputBankTest.extraSkill4.down)
                //{
                //    if (base.fixedAge > fireTime)
                //    {
                //        SmallHop(characterMotor, movespeed * StaticValues.blackwhipPullHop);
                //    }
                //    this.outer.SetNextStateToMain();
                //    return;
                //}
                
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