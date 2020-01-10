using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueTree;

namespace DialogueTree {
    [System.Serializable]
    public class StatPrerequisite : Prerequisite
    {
        public enum StatOperator { equals, greater_than, less_than }

        public StatOperator statOperator;
        public StatCheck statCheck;
        public override bool Met() {
            int playerStat = Player.Instance.stats[statCheck.statType];

            switch (statOperator) {
                case StatOperator.equals:
                    return playerStat == statCheck.statRequirement;
                case StatOperator.greater_than:
                    return playerStat > statCheck.statRequirement;
                case StatOperator.less_than:
                    return playerStat < statCheck.statRequirement;
                default:
                    throw new PrerequisiteError("Invalid stat operator!");
            }
        }
    }
}
