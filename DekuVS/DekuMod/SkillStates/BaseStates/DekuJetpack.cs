//using System;
//using EntityStates;
//using UnityEngine;

//namespace DekuMod.SkillStates
//{
//    internal class DekuJetpack : BaseState
//    {
//        public override void OnEnter()
//        {
//            base.OnEnter();
//            //this.jetEffect = base.FindModelChild("JetHolder");
//            //bool flag = this.jetEffect;
//            //if (flag)
//            //{
//            //    this.jetEffect.gameObject.SetActive(true);
//            //}
//        }

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();
//            bool isAuthority = base.isAuthority;
//            if (isAuthority)
//            {
//                float velocityY = Mathf.MoveTowards(base.characterMotor.velocity.y, -4f, 60f * Time.fixedDeltaTime);
//                base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, velocityY, base.characterMotor.velocity.z);
//            }
//        }

//        public override void OnExit()
//        {
//            //bool flag = this.jetEffect;
//            //if (flag)
//            //{
//            //    this.jetEffect.gameObject.SetActive(false);
//            //}
//            base.OnExit();
//        }

//        //private Transform jetEffect;
//    }
//}
