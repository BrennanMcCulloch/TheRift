using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueTree {
    public class Page : MonoBehaviour
    {
        public string title;

        [TextArea(3,8)]
        public string text;

        [TextArea(3,8)]
        public string lockedText;

        private List<Choice> choices;
        private List<Prerequisite> prerequisites;

        void Awake() {
            choices = new List<Choice>();
            // get choices from first level children only
            foreach (Transform t in gameObject.transform) {
                Choice choice = t.GetComponent<Choice>();
                if (choice != null) choices.Add(choice);
            }

            // get prerequisites from this object's components only
            prerequisites = new List<Prerequisite>();
            foreach (Prerequisite p in gameObject.GetComponents<Prerequisite>()) {
                prerequisites.Add(p);
            }
        }

        // show different text if we're locked, aka we have unmet prerequisites
        public string Text() {
            if (Locked()) return lockedText;
            return text;
        }

        public Choice[] Choices() {
            return choices.ToArray();
        }

        // Return true if even one prerequisite isn't met
        public bool Locked() {
            // bail if there's nothing to check
            if (prerequisites == null || prerequisites.Count <= 0) return false;

            foreach (Prerequisite prerequisite in prerequisites) {
                if (!prerequisite.Met()) return true;
            }
            return false;
        }
    }

}