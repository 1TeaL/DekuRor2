using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using R2API.Networking.Interfaces;
using UnityEngine;
using UnityEngine.Networking;
using DekuMod.SkillStates;

namespace DekuMod.Modules.Networking
{
    public class ForceCounterState : INetMessage
    {
        NetworkInstanceId IDNet;
        int level;
        Vector3 enemyPos;                   

        public ForceCounterState()
        {

        }

        public ForceCounterState(NetworkInstanceId IDNet, int level, Vector3 enemyPos)
        {
            this.IDNet = IDNet;
            this.level = level;
            this.enemyPos = enemyPos;
        }

        public void Deserialize(NetworkReader reader)
        {
            enemyPos = reader.ReadVector3();
            level = reader.ReadInt32();
            IDNet = reader.ReadNetworkId();
        }

        public void OnReceived()
        {
            if (NetworkServer.active)
            {
                GameObject masterobj = Util.FindNetworkObject(IDNet);
                if (!masterobj)
                {
                    Debug.Log("masterobj not found");
                    return;
                }
                CharacterMaster charmast = masterobj.GetComponent<CharacterMaster>();
                if (!charmast)
                {
                    Debug.Log("charmast not found");
                    return;
                }
                GameObject charbodyobj = charmast.GetBodyObject();
                if (!charbodyobj)
                {
                    Debug.Log("charbodyobj not found");
                    return;
                }
                EntityStateMachine[] statemachines = charbodyobj.GetComponents<EntityStateMachine>();
                foreach (EntityStateMachine statemachine in statemachines)
                {
                    if (statemachine.customName == "Body")
                    {
                        statemachine.SetState(new SkillStates.Might.CounterFollowUp
                        {
                            level = level,
                            enemyPos = enemyPos,
                        });
                    }
                }

            }
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(level);
            writer.Write(IDNet);
        }
    }
}
