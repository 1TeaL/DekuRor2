﻿//using RoR2;
//using UnityEngine;
//using EntityStates;

//namespace DekuMod.SkillStates
//{
//    public class BlackwhipGrabController : MonoBehaviour
//    {
//        public Transform pivotTransform;
//        private CharacterBody body;
//        private CharacterDirection direction;
//        private ModelLocator modelLocator;
//        private Transform modelTransform;
//        private CharacterMotor motor;
//        private Quaternion originalRotation;

//        public void Release()
//        {
//            if (this.modelLocator)
//            {
//                this.modelLocator.enabled = true;
//            }
//            if (this.modelTransform)
//            {
//                this.modelTransform.rotation = this.originalRotation;
//            }
//            if (this.direction)
//            {
//                this.direction.enabled = true;
//            }
//            Object.Destroy(this);
//        }

//        private void Awake()
//        {
//            this.body = base.GetComponent<CharacterBody>();
//            this.motor = base.GetComponent<CharacterMotor>();
//            this.direction = base.GetComponent<CharacterDirection>();
//            this.modelLocator = base.GetComponent<ModelLocator>();
//            if (this.direction)
//            {
//                this.direction.enabled = false;
//            }
//            if (this.modelLocator && this.modelLocator.modelTransform)
//            {
//                this.modelTransform = this.modelLocator.modelTransform;
//                this.originalRotation = this.modelTransform.rotation;
//                this.modelLocator.enabled = false;
//            }
//        }

//        private void FixedUpdate()
//        {
//            if (this.motor)
//            {
//                this.motor.disableAirControlUntilCollision = true;
//                this.motor.velocity = Vector3.zero;
//                this.motor.rootMotion = Vector3.zero;
//                this.motor.Motor.SetPosition(this.pivotTransform.position, true);
//            }
//            if (this.pivotTransform)
//            {
//                base.transform.position = this.pivotTransform.position;
//            }
//            else
//            {
//                this.Release();
//            }
//            if (this.modelTransform)
//            {
//                this.modelTransform.position = this.pivotTransform.position;
//                this.modelTransform.rotation = this.pivotTransform.rotation;
//            }
//        }
//    }
//}