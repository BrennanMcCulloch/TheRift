using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueTree {

    [System.Serializable]

    // Used in various components to standardize roll checks
    public class StatCheck {
        public StatType statType;
        public int statRequirement;
    }

    public class Dialogue : MonoBehaviour
    {
        public string title;
        public Page currentPage;

        private List<Page> pages;
        private int currentPageIndex;


        void Awake() {
            pages = new List<Page>();
            // get pages from first level children only
            foreach (Transform t in gameObject.transform) {
                Page page = t.GetComponent<Page>();
                if (page != null) pages.Add(page);
            }
            // Todo (matt) - load the first page, or the current page from the save file
            currentPage = pages[0]; // something like -> || PlayerPrefs.getString("npc_name_dialogue");
        }

        public bool NextPage() {
            if (HasMorePages() && !currentPage.Locked()){
                currentPageIndex++;
                currentPage = pages[currentPageIndex];
                return true;
            }
            return false;
        }

        // Called by a choice object. Inserts its results page ahead of the current page.
        // So when we call nextpage, we see the results page first, before we see the previous nextpage.
        // Todo (matt) - as i said in choice.cs, this is a hacky way to do it and i dont like it.
        public void InsertNext(Page page) {
            pages.Insert(currentPageIndex + 1, page);
        }

        public bool HasMorePages() {
            return currentPageIndex < pages.Count - 1;
        }

    }
}
