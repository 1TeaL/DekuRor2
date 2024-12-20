﻿using System;
using EntityStates;
using DekuMod.Modules;
using DekuMod.Modules.Survivors;
using UnityEngine;
using RoR2;

namespace DekuMod.SkillStates.BaseStates
{

	public class BaseDekuSkillState : BaseSkillState
	{
		public int level;
        public DekuController dekucon;
        public EnergySystem energySystem;
        public Animator animator;
        public Transform modelTransform;
        public CharacterModel characterModel;
        public HurtBoxGroup hurtboxGroup;

        public float walkCancelTime;

        public override void OnEnter()
		{
			base.OnEnter();
            animator = base.GetModelAnimator();
            dekucon = base.GetComponent<DekuController>();
            energySystem = base.GetComponent<EnergySystem>();
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                this.hurtboxGroup = this.modelTransform.GetComponent<HurtBoxGroup>();
            }
            if (characterBody.level >= 20)
            {
                level = 2;
            }
            else if (characterBody.level >= 10)
            {
                level = 1;
            }
            else
            {
                level = 0;
            }

            //for easy copy paste
            switch (level)
            {
                case 0:
                    Level1();
                    break;
                case 1:
                    Level2();
                    break;
                case 2:
                    Level3();
                    break;
            }

            walkCancelTime = 2f;
        }

        public virtual void Level1()
        {

        }
        public virtual void Level2()
        {

        }
        public virtual void Level3()
        {

        }

        //public override void Update()
        //{
        //    base.Update();

        //    if (base.age > walkCancelTime &&
        //        (base.inputBank.moveVector != Vector3.zero || base.inputBank.jump.down))
        //    {
        //        base.PlayCrossfade("Fullbody, Override", "BufferEmpty", 0.1f);
        //        this.outer.SetNextStateToMain();
        //        return;
        //    }
        //}

        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }

    }
}
