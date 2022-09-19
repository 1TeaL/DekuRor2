using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace DekuMod.Modules.Networking
{
    internal class HealNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;
        float healthPercentage;

        //Don't network these.
        GameObject bodyObj;

        public HealNetworkRequest()
        {

        }

        public HealNetworkRequest(NetworkInstanceId netID, float healthPercentage)
        {
            this.netID = netID;
            this.healthPercentage = healthPercentage;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            healthPercentage = reader.ReadSingle();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(healthPercentage);
        }

        public void OnReceived()
        {
            if (NetworkServer.active)
            {
                if (NetworkServer.active)
                {
                    GameObject masterobject = Util.FindNetworkObject(netID);
                    CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                    CharacterBody charBody = charMaster.GetBody();
                    bodyObj = charBody.gameObject;

                    charBody.healthComponent.Heal(healthPercentage, new ProcChainMask(), true);
                }

            }
            

        }
    }
}
