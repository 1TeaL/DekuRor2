using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace DekuMod.Modules.Networking
{
    internal class SpendHealthNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;
        float healthPercentage;

        //Don't network these.
        GameObject bodyObj;

        public SpendHealthNetworkRequest()
        {

        }

        public SpendHealthNetworkRequest(NetworkInstanceId netID, float healthPercentage)
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
            //Teleport to enemy

            GameObject masterobject = Util.FindNetworkObject(netID);
            CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
            CharacterBody charBody = charMaster.GetBody();
            bodyObj = charBody.gameObject;

            if (NetworkServer.active && charBody.healthComponent)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = charBody.healthComponent.fullCombinedHealth * healthPercentage;
                damageInfo.position = charBody.transform.position;
                damageInfo.force = Vector3.zero;
                damageInfo.damageColorIndex = DamageColorIndex.WeakPoint;
                damageInfo.crit = false;
                damageInfo.attacker = null;
                damageInfo.inflictor = null;
                damageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
                damageInfo.procCoefficient = 0f;
                damageInfo.procChainMask = default(ProcChainMask);
                charBody.healthComponent.TakeDamage(damageInfo);
            }

        }
    }
}
