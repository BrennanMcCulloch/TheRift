using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueTree {
    public enum ChoiceResult { unanswered, succeeded, failed }

    public class Choice : MonoBehaviour
    {
        [TextArea(3,5)]
        public string text;

        [TextArea(3,5), Tooltip("The text that's shown when a player re-attempts a choice. Leave blank to show original text every time.")]
        public string visitedText;

        [Tooltip("The difficulty of achieveing a successful result for this choice.")]
        public StatCheck statCheck;

        // hide this choice unless the prerequisites are met
        private List<Prerequisite> prerequisites;

        // Record the success or failure of the roll check
        public ChoiceResult result;

        // Whether or not to remove this choice after an attempt
        [Tooltip("If true, the choice can only be tried once and will be hidden regardless of outcome.")]
        public bool removeAfterAttempt;

        // The page we see after the player performs the rollcheck
        public Page successPage;
        public Page failurePage;

        void Awake() {
            // Grab prerequisites from this objects components only
            prerequisites = new List<Prerequisite>();
            foreach (Prerequisite p in gameObject.GetComponents<Prerequisite>()) {
                prerequisites.Add(p);
            }
        }

        // whether or not to show this choice in the dialogue window
        public bool Hidden() {
            return !PrerequisitesMet() || (removeAfterAttempt && Visited());
        }

        // used by Hidden() to conditionally show or hide this choice
        private bool PrerequisitesMet() {
            foreach (Prerequisite prerequisite in prerequisites) {
                if (!prerequisite.Met()) return false;
            }
            return true;
        }

        // The page to show after the choice is made
        public Page NextPage() {
            Debug.Log(result);
            switch (result) {
                case ChoiceResult.succeeded:
                    return successPage;
                case ChoiceResult.failed:
                    return failurePage;
                default:
                    return null;
            }
        }

        public string Text() {
            if (Visited() && visitedText != null) return visitedText;
            return text;
        }

        public bool Visited() {
            return result != ChoiceResult.unanswered;
        }

        // Called by DialogueManager when a choice is made in the UI
        public ChoiceResult CheckResult(int statRoll) {
            result = statRoll > statCheck.statRequirement ? ChoiceResult.succeeded : ChoiceResult.failed;
            return result;
        }
    }
}
