using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace DekuMod.Modules.Networking
{
    internal class PerformFinalSmashNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;
        NetworkInstanceId enemynetID;

        //Don't network these.
        GameObject bodyObj;
        GameObject enemybodyObj;
        public PerformFinalSmashNetworkRequest()
        {

        }

        public PerformFinalSmashNetworkRequest(NetworkInstanceId netID, NetworkInstanceId enemynetID)
        {
            this.netID = netID;
            this.enemynetID = enemynetID;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            enemynetID = reader.ReadNetworkId();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(enemynetID);
        }

        public void OnReceived()
        {

            if (NetworkServer.active)
            {
                GameObject masterobject = Util.FindNetworkObject(netID);
                CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                CharacterBody charBody = charMaster.GetBody();
                bodyObj = charBody.gameObject;

                GameObject enemymasterobject = Util.FindNetworkObject(enemynetID);
                CharacterMaster enemycharMaster = enemymasterobject.GetComponent<CharacterMaster>();
                CharacterBody enemycharBody = enemycharMaster.GetBody();
                enemybodyObj = enemycharBody.gameObject;

                //Pull targets and stun
                PullTargets(charBody, enemycharBody);
            }
        }


        private void PullTargets(CharacterBody charBody, CharacterBody enemycharBody)
        {

            if (enemycharBody.healthComponent && enemycharBody)
            {
                enemycharBody.characterMotor.Motor.SetPositionAndRotation(charBody.gameObject.transform.position + charBody.characterDirection.forward * 5f,
                                        Util.QuaternionSafeLookRotation(charBody.characterDirection.forward), true);

                DamageInfo damageInfo = new DamageInfo
                {
                    attacker = charBody.gameObject,
                    damage = charBody.damage * Modules.StaticValues.finalsmashDamageCoefficient,
                    position = enemycharBody.gameObject.transform.position,
                    procCoefficient = 1f,
                    damageType = DamageType.Stun1s,
                    crit = charBody.RollCrit(),

                };

                enemycharBody.healthComponent.TakeDamage(damageInfo);
                GlobalEventManager.instance.OnHitEnemy(damageInfo, enemycharBody.healthComponent.gameObject);


                EffectManager.SpawnEffect(Modules.Assets.dekuHitImpactEffect, new EffectData
                {
                    origin = enemycharBody.gameObject.transform.position,
                    scale = 1f,

                }, true);

            }


        }

    }
}
