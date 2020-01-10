using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueTree;

namespace DialogueTree {
    [System.Serializable]
    public class ChoicePrerequisite : Prerequisite
    {
        public Choice choice;
        public ChoiceResult result;

        public override bool Met() {
            if (choice != null) return choice.result == result;
            else throw new PrerequisiteError("A choice is required for this prerequisite");
        }
    }
}