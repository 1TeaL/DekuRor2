using DekuMod.SkillStates.BaseStates;
using EntityStates;
using R2API.Networking;
using RoR2;
using UnityEngine;
using System;
using EntityStates.Huntress;
using DekuMod.Modules;
using System.Net;

namespace DekuMod.SkillStates.BlackWhip
{
    public class BlackwhipPull : BaseDekuSkillState
    {
        public Vector3 moveDirection;
        public float fireTime;
        public BullseyeSearch search;
        public bool hasFired;

        //teleporting up
        private GameObject aimSphere;
        public float radius = 1f;
        private Ray aimRay;
        private float baseDistance = StaticValues.blackwhipPullDistance;
        private float maxDistance;

        private bool isRayCast;
        private bool isWorld;
        private bool isEnemy;
        private CharacterBody enemyBody;
        public ChildLocator child;
        public GameObject blackwhipLineEffect;
        public LineRenderer blackwhipLineRenderer;
        private float elapsedTime;
        private Vector3 endPoint;

        public override void OnEnter()
        {
            base.OnEnter();

            base.StartAimMode(1f, true);

            maxDistance = baseDistance * moveSpeedStat;
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
            bool raycast = Physics.Raycast(aimRay.origin, aimRay.direction, out raycastHit, this.maxDistance, LayerIndex.world.mask | LayerIndex.entityPrecise.mask);

            if (raycast)
            {
                isRayCast = true;
                fireTime = (raycastHit.distance/maxDistance) * StaticValues.blackwhipPullDuration;
                this.aimSphere = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
                this.aimSphere.transform.localScale = new Vector3(this.radius, this.radius, this.radius);

                blackwhipLineEffect = UnityEngine.Object.Instantiate(Modules.Assets.blackwhipLineRenderer, child.FindChild("RHand").transform);
                blackwhipLineRenderer = blackwhipLineEffect.GetComponent<LineRenderer>();

                if (StaticValues.Includes(LayerIndex.entityPrecise.mask, raycastHit.collider.gameObject.layer))
                {
                    isEnemy = true;
                    enemyBody = raycastHit.collider.gameObject.GetComponent<CharacterBody>();
                    this.aimSphere.transform.position = enemyBody.corePosition;
                    this.aimSphere.transform.up = raycastHit.normal;
                    this.aimSphere.transform.forward = this.aimRay.direction;
                    Chat.AddMessage("enemybody position" + enemyBody.corePosition);
                }
                else if (StaticValues.Includes(LayerIndex.world.mask, raycastHit.collider.gameObject.layer))
                {
                    isWorld = true;
                    endPoint = raycastHit.point;
                    this.aimSphere.transform.position = raycastHit.point;
                    this.aimSphere.transform.up = raycastHit.normal;
                    this.aimSphere.transform.forward = this.aimRay.direction;
                    Chat.AddMessage("endPoint" + endPoint);
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

            if (isRayCast)
            {
                if (base.IsKeyDownAuthority())
                {
                    if (base.fixedAge > fireTime)
                    {
                        //travel until get close enough, then slow down and also play animation change if needed
                        if (isEnemy)
                        {
                            if (Vector2.Distance(enemyBody.corePosition, characterBody.corePosition) > 2f)
                            {
                                Vector3 normalized = (enemyBody.corePosition - characterBody.corePosition).normalized;
                                if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
                                {
                                    Vector3 vector = normalized * moveSpeedStat * attackSpeedStat * StaticValues.blackwhipPullSpeed;

                                    base.characterMotor.velocity = vector;
                                }
                            }
                            else
                            {
                                Vector3 normalized = (enemyBody.corePosition - characterBody.corePosition).normalized;
                                if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
                                {
                                    Vector3 vector = normalized * StaticValues.blackwhipPullSpeed;

                                    base.characterMotor.velocity = vector;
                                }
                            }
                        }
                        else if (isWorld)
                        {
                            if (Vector2.Distance(endPoint, characterBody.corePosition) > 2f)
                            {
                                Vector3 normalized = (endPoint - characterBody.corePosition).normalized;
                                if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
                                {
                                    Vector3 vector = normalized * moveSpeedStat * attackSpeedStat * StaticValues.blackwhipPullSpeed;

                                    base.characterMotor.velocity = vector;

                                }
                            }
                            else
                            {
                                base.characterMotor.velocity = Vector3.zero;
                            }
                        }

                    }

                }
                else
                {
                    SmallHop(characterMotor, StaticValues.blackwhipPullSpeed * (moveSpeedStat / 7f) * (fixedAge / 10f + 1));
                    this.outer.SetNextStateToMain();
                    return;
                }
            }
            else
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void Update()
        {
            base.Update();
            this.UpdateAreaIndicator();
            
            if(isRayCast)
            {
                if (elapsedTime < fireTime)
                {
                    elapsedTime += Time.deltaTime; // Increment the elapsed time
                }
                float t = Mathf.Clamp01(elapsedTime / fireTime); // Calculate the interpolation factor (0 to 1)

                Vector3[] vector3s = new Vector3[2];
                Vector3 startPoint = child.FindChild("RHand").transform.position;
                if (isEnemy)
                {
                    endPoint = enemyBody.corePosition;
                }

                // Lerp from the start point to the end point
                Vector3 lerpedPosition = Vector3.Lerp(startPoint, endPoint, t);

                vector3s[0] = startPoint;
                vector3s[1] = lerpedPosition;
                blackwhipLineRenderer.SetPositions(vector3s);
                
            }
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
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}