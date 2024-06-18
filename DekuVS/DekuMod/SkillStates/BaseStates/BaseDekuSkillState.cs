using System;
using EntityStates;
using DekuMod.Modules;
using DekuMod.Modules.Survivors;

namespace DekuMod.SkillStates.BaseStates
{

	public class BaseDekuSkillState : BaseSkillState
	{
		public int level;
        public DekuController dekucon;
        public EnergySystem energySystem;

        public override void OnEnter()
		{
			base.OnEnter();
            dekucon = base.GetComponent<DekuController>();
            energySystem = base.GetComponent<EnergySystem>();
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
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }

        }

	}
}
