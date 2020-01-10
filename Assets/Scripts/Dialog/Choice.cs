using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueTree {
    public enum ChoiceResult { unanswered, succeeded, failed }

    public class Choice : MonoBehaviour
    {
        [TextArea(3,5)]
        public string text;

        // The difficulty of passing this choice
        public StatCheck statCheck;

        // hide this choice unless the prerequisites are met
        private List<Prerequisite> prerequisites;

        // Record the success or failure of the roll check
        public ChoiceResult result;

        // If true, do not remove this choice when the roll check fails
        public bool repeatable;

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
            return !PrerequisitesMet();
        }

        // used by Hidden() to conditionally show or hide this choice
        private bool PrerequisitesMet() {
            foreach (Prerequisite prerequisite in prerequisites) {
                if (!prerequisite.Met()) return false;
            }
            return true;
        }

        // The page to show after the choice is made
        public Page ResultsPage() {
            switch (result) {
                case ChoiceResult.succeeded:
                    return successPage;
                case ChoiceResult.failed:
                    return failurePage;
            }

            // Todo (matt) - we should never get to this point
            return null;
        }

        // Called by DialogueManager when a choice is made in the UI
        public ChoiceResult CheckResult(int statRoll) {
            result = statRoll > statCheck.statRequirement ? ChoiceResult.succeeded : ChoiceResult.failed;
            return result;
        }
    }
}
