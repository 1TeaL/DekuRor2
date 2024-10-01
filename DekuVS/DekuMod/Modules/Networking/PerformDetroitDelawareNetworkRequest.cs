using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.UI.Image;


namespace DekuMod.Modules.Networking
{
    internal class PerformDetroitDelawareNetworkRequest : INetMessage
    {
        //Network these ones.
        NetworkInstanceId netID;

        //Don't network these.
        GameObject bodyObj;
        private BullseyeSearch search;
        private List<HurtBox> trackingTargets;

        public PerformDetroitDelawareNetworkRequest()
        {

        }

        public PerformDetroitDelawareNetworkRequest(NetworkInstanceId netID)
        {
            this.netID = netID;
        }

        public void Deserialize(NetworkReader reader)
        {
            netID = reader.ReadNetworkId();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(netID);
        }

        public void OnReceived()
        {

            if (NetworkServer.active)
            {
                search = new BullseyeSearch();
                GameObject masterobject = Util.FindNetworkObject(netID);
                CharacterMaster charMaster = masterobject.GetComponent<CharacterMaster>();
                CharacterBody charBody = charMaster.GetBody();
                bodyObj = charBody.gameObject;


                //Check targets in range
                SearchForTarget(charBody);
                //Pull targets and stun
                PullTargets(charBody);
            }
        }

        private void SearchForTarget(CharacterBody charBody)
        {
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(charBody.teamComponent.teamIndex);
            this.search.filterByLoS = false;
            this.search.searchOrigin = charBody.transform.position + charBody.characterDirection.forward * 5f;
            this.search.searchDirection = Vector3.up;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = StaticValues.detroitdelawareBlastRadius;
            this.search.maxAngleFilter = 360f;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(charBody.gameObject);
            this.trackingTargets = this.search.GetResults().ToList<HurtBox>();
        }
        private void PullTargets(CharacterBody charBody)
        {
            if (trackingTargets.Count > 0)
            {
                foreach (HurtBox singularTarget in trackingTargets)
                {
                    if (singularTarget.healthComponent && singularTarget.healthComponent.body)
                    {
                        if (singularTarget.healthComponent.body.characterMotor)
                        {
                            singularTarget.healthComponent.body.characterMotor.Motor.SetPositionAndRotation(charBody.transform.position + charBody.characterDirection.forward * 5f,
                                                    Util.QuaternionSafeLookRotation(charBody.characterDirection.forward), true);
                        }
                        else if (singularTarget.healthComponent.body.rigidbody)
                        {
                            singularTarget.healthComponent.body.rigidbody.MovePosition(charBody.transform.position + charBody.characterDirection.forward * 5f);
                        }

                        DamageInfo damageInfo = new DamageInfo
                        {
                            attacker = bodyObj,
                            damage = charBody.damage * Modules.StaticValues.detroitdelawareDamageCoefficient * charBody.attackSpeed,
                            position = singularTarget.transform.position,
                            procCoefficient = 0.1f,
                            damageType = DamageType.Stun1s,
                            crit = charBody.RollCrit(),
                        };

                        singularTarget.healthComponent.TakeDamage(damageInfo);
                        GlobalEventManager.instance.OnHitEnemy(damageInfo, singularTarget.healthComponent.gameObject);


                        EffectManager.SpawnEffect(Modules.DekuAssets.dekuHitImpactEffect, new EffectData
                        {
                            origin = singularTarget.healthComponent.body.gameObject.transform.position,
                            scale = 1f,

                        }, true);

                    }
                }
            }

                    
            

        }

    }
}
