using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Inventory;

namespace DialogueTree {
    public enum PageResolutionType { nextPage, previousPage, nextChapter, rootPage, endDialogue }
    public class Page : MonoBehaviour
    {
        [Tooltip("This text is shown at the top of the dialogue window.")]
        public string title;

        [TextArea(3,8), Tooltip("The text that's seen when a player first visits this page")]
        public string text;

        [TextArea(3,8), Tooltip("The text that's seen when a player has already seen this page. Leave blank and the normal text will always show.")]
        public string visitedText;

        [TextArea(3,8), Tooltip("The text that's seen when a player cant access a page due to unmet prerequisites. Needs to be set if any prerequisites are present.")]
        public string lockedText;

        [Tooltip("Used by the DialogueManager to determine which page to show when Continue is clicked\n - Next Page: Show the first page nested under this one\n - Previous Page: Navigate back up the dialogue tree to the first page that had choices\n - Root Page: Navigate all the way to the top-level page (chapter) for this page\n - End Dialogue: Closes the dialogue")]
        public PageResolutionType resolutionType;

        private List<Choice> choices;
        private List<Prerequisite> prerequisites;

        private Page nextPage, lastPage, lastChoicePage;
        private bool visited;
        private List<Item> items;

        void Awake() {
            LoadChoices();
            LoadPrerequisites();
            LoadPages();
        }

        private void LoadChoices() {
            choices = new List<Choice>();
            items = new List<Item>();
            // get choices from first level children only
            foreach (Transform t in gameObject.transform) {
                Choice choice = t.GetComponent<Choice>();
                if (choice != null) choices.Add(choice);
                else {
                    Item pageItem = t.GetComponent<Item>();
                    if (pageItem != null) items.Add(pageItem);
                }
            }
        }

        private void LoadPrerequisites() {
            // get prerequisites from this object's components only
            prerequisites = new List<Prerequisite>();
            foreach (Prerequisite prerequisite in gameObject.GetComponents<Prerequisite>()) {
                prerequisites.Add(prerequisite);
            }
        }

        private void LoadPages() {
            foreach (Transform t in gameObject.transform) {
                Page nPage = t.GetComponent<Page>();
                if (nPage != null) nextPage = nPage;
            }

            // Navigate up the tree until we find a choice. The parent page of that choice is our last meaningful navigation point.
            Transform parent = transform.parent;
            // While we have a parent and we dont have a last page
            while (parent != null && lastPage == null) {
                if (parent.GetComponent<Choice>() != null) {
                    lastPage = parent.parent.GetComponent<Page>();
                }
                parent = parent.parent;
            }
        }

        // show different text if we're locked, aka we have unmet prerequisites
        public string Text() {
            if (Locked()) return lockedText;
            else if (visited && visitedText.Length > 0) return visitedText;
            return text;
        }

        public Choice[] Choices() {
            return choices.Where(choice => !choice.Hidden() ).ToArray();
        }

        public Page NextPage() {
            return nextPage;
        }

        public Page LastPage() {
            return lastPage;
        }
        public void Visited() {
            visited = true;
            GiveItems();
        }

        private void GiveItems() {
            if (items !=  null && items.Count > 0) {
                InventoryManager.Instance.AddItems(items);
                items = null;
            }
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

        public bool EndsDialogue() {
             return (resolutionType == PageResolutionType.endDialogue) || Locked() || nextPage == null;
        }
    }

}