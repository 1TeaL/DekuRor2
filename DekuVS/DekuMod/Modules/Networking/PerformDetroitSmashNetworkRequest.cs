﻿using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace DekuMod.Modules.Networking
{
    internal class PerformDetroitSmashNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;
        NetworkInstanceId enemyNetID;

        //Don't network these.
        GameObject bodyObj;
        GameObject enemybodyObj;
        private BullseyeSearch search;
        private List<HurtBox> trackingTargets;
        private GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");

        public PerformDetroitSmashNetworkRequest()
        {

        }

        public PerformDetroitSmashNetworkRequest(NetworkInstanceId netID, NetworkInstanceId enemyNetID)
        {
            this.netID = netID;
            this.enemyNetID = enemyNetID;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            enemyNetID = reader.ReadNetworkId();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(enemyNetID);
        }

        public void OnReceived()
        {

            if (NetworkServer.active)
            {
                GameObject masterobject = Util.FindNetworkObject(netID);
                CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                CharacterBody charBody = charMaster.GetBody();
                bodyObj = charBody.gameObject;

                GameObject enemymasterobject = Util.FindNetworkObject(enemyNetID);
                CharacterMaster enemycharMaster = enemymasterobject.GetComponent<CharacterMaster>();
                CharacterBody enemycharBody = enemycharMaster.GetBody();
                enemybodyObj = enemycharBody.gameObject;

                //Pull targets and stun
                SmashTarget(charBody, enemycharBody);
            }
        }

        //Don't do anything until we have a target.
        //Pull target: let's use a blast attack pointed towards the player with a slight upwards force.
        //directional vector: terminal point - initial point.
        //get mass, apply force. object mass * forcemultiplier for enemies.
        //Make direction point towards player.
        private void SmashTarget(CharacterBody dekucharBody, CharacterBody enemycharBody)
        {

            if (enemycharBody.healthComponent && enemycharBody.healthComponent.body)
            {
                float Weight = 1f;
                if (enemycharBody.healthComponent.body.characterMotor)
                {
                    Weight = enemycharBody.healthComponent.body.characterMotor.mass;
                }
                else if (enemycharBody.healthComponent.body.rigidbody)
                {
                    Weight = enemycharBody.healthComponent.body.rigidbody.mass;
                }

                

                DamageInfo damageInfo = new DamageInfo
                {
                    attacker = bodyObj,
                    damage = dekucharBody.damage * Modules.StaticValues.detroitDamageCoefficient,
                    position = enemycharBody.transform.position,
                    procCoefficient = 1f,
                    damageType = DamageType.Stun1s,
                    crit = dekucharBody.RollCrit(),

                };

                Vector3 direction = Vector3.down;
                if (enemycharBody.characterMotor)
                {
                    if (enemycharBody.characterMotor.isGrounded)
                    {
                        direction = Vector3.up;

                        EffectManager.SpawnEffect(Modules.Assets.detroitweakEffect, new EffectData
                        {
                            origin = enemycharBody.transform.position,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(Vector3.down).normalized,

                        }, true);
                        EffectManager.SpawnEffect(blastEffectPrefab, new EffectData
                        {
                            origin = enemycharBody.transform.position,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(Vector3.down).normalized,

                        }, true);
                    }
                    else
                    {

                        EffectManager.SpawnEffect(Modules.Assets.detroitweakEffect, new EffectData
                        {
                            origin = enemycharBody.transform.position,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(Vector3.up).normalized,

                        }, true);
                        EffectManager.SpawnEffect(blastEffectPrefab, new EffectData
                        {
                            origin = enemycharBody.transform.position,
                            scale = 1f,
                            rotation = Quaternion.LookRotation(Vector3.up).normalized,

                        }, true);
                    }
                }
                else
                {

                    EffectManager.SpawnEffect(Modules.Assets.detroitweakEffect, new EffectData
                    {
                        origin = enemycharBody.transform.position,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(Vector3.up).normalized,

                    }, true);
                    EffectManager.SpawnEffect(blastEffectPrefab, new EffectData
                    {
                        origin = enemycharBody.transform.position,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(Vector3.up).normalized,

                    }, true);
                }

                enemycharBody.healthComponent.TakeDamageForce(direction * 40f *(Weight), true, true);
                enemycharBody.healthComponent.TakeDamage(damageInfo);
                GlobalEventManager.instance.OnHitEnemy(damageInfo, enemycharBody.healthComponent.gameObject);

            }
        }

    }
}