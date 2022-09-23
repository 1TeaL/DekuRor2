using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace DekuMod.Modules.Networking
{
    internal class PerformStLouisSmashNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;
        Vector3 position;
        
        //Don't network these.
        GameObject bodyObj;
        GameObject enemybodyObj;
        private BullseyeSearch search;
        private List<HurtBox> trackingTargets;
        private GameObject blastEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/SonicBoomEffect");

        public PerformStLouisSmashNetworkRequest()
        {

        }

        public PerformStLouisSmashNetworkRequest(NetworkInstanceId netID, Vector3 position)
        {
            this.netID = netID;
            this.position = position;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
            position = reader.ReadVector3();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
            writer.Write(position);
        }

        public void OnReceived()
        {

            if (NetworkServer.active)
            {
                GameObject masterobject = Util.FindNetworkObject(netID);
                CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                CharacterBody charBody = charMaster.GetBody();
                bodyObj = charBody.gameObject;

                //Check targets in range
                SearchForTarget(charBody, position);
                //Smash targets and stun
                SmashTargets(charBody);
            }
        }


        private void SearchForTarget(CharacterBody charBody, Vector3 position)
        {
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(charBody.teamComponent.teamIndex);
            this.search.filterByLoS = true;
            this.search.searchOrigin = position;
            this.search.searchDirection = Vector3.up;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = Modules.StaticValues.detroitRange;
            this.search.maxAngleFilter = 360;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(charBody.gameObject);
            this.trackingTargets = this.search.GetResults().ToList<HurtBox>();
        }


        private void SmashTargets(CharacterBody dekucharBody)
        {
            if (trackingTargets.Count > 0)
            {
                foreach (HurtBox singularTarget in trackingTargets)
                {
                    float Weight = 1f;
                    if (singularTarget.healthComponent.body.characterMotor)
                    {
                        Weight = singularTarget.healthComponent.body.characterMotor.mass;
                    }
                    else if (singularTarget.healthComponent.body.rigidbody)
                    {
                        Weight = singularTarget.healthComponent.body.rigidbody.mass;
                    }



                    DamageInfo damageInfo = new DamageInfo
                    {
                        attacker = bodyObj,
                        damage = dekucharBody.damage * Modules.StaticValues.stlouisDamageCoefficient,
                        position = singularTarget.transform.position,
                        procCoefficient = 1f,
                        damageType = DamageType.Generic,
                        crit = dekucharBody.RollCrit(),

                    };

                    Vector3 direction = dekucharBody.characterDirection.forward;
                    if (singularTarget.healthComponent.body.characterMotor)
                    {
                            direction = Vector3.up;

                            EffectManager.SpawnEffect(Modules.Assets.detroitweakEffect, new EffectData
                            {
                                origin = singularTarget.transform.position,
                                scale = 1f,
                                rotation = Quaternion.LookRotation(direction).normalized,

                            }, true);
                            EffectManager.SpawnEffect(blastEffectPrefab, new EffectData
                            {
                                origin = singularTarget.transform.position,
                                scale = 1f,
                                rotation = Quaternion.LookRotation(direction).normalized,

                            }, true);
                        
                        
                    }

                    singularTarget.healthComponent.TakeDamageForce(direction * 40f * (Weight), true, true);
                    singularTarget.healthComponent.TakeDamage(damageInfo);
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, singularTarget.healthComponent.gameObject);

                }

                

            }
        }

    }
}
