    using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueTree {

    [System.Serializable]

    public class Dialogue : MonoBehaviour
    {
        public string title;
        public AudioClip startNarrationClip;
        public AudioClip exitNarrationClip;

        private Page currentPage;
        private Page currentChapter;
        private List<Page> chapters;
        private bool startNarrationReady = false;
        private bool exitNarrationReady = false;

        void Awake() {
            LoadPages();
            currentChapter = chapters[0];
            currentPage = currentChapter;
            if (startNarrationClip != null) startNarrationReady = true;
            if (exitNarrationClip != null) exitNarrationReady = true;
         }

        private void LoadPages() {
            chapters = new List<Page>();
            // get chapters(pages) from first level children only
            foreach (Transform t in gameObject.transform) {
                Page page = t.GetComponent<Page>();
                if (page != null) chapters.Add(page);
            }
        }

        public Page CurrentPage() {
            return currentPage;
        }

        // NextPage resolves in a few different ways.
        public void NextPage() {
            currentPage.Visited();
            switch (currentPage.resolutionType) {
                case PageResolutionType.nextPage:
                    currentPage = currentPage.NextPage();
                    break;
                case PageResolutionType.previousPage:
                    LastPage();
                    break;
                case PageResolutionType.rootPage:
                    currentPage = currentChapter;
                    break;
            }
        }

        public void LastPage() {
            currentPage = currentPage.LastPage();
        }

        public void NextChapter() {
            chapters.RemoveAt(0);
            currentChapter = chapters[0];
            currentPage = currentChapter;
        }

        public void MakeChoice(Choice choice, int roll) {
            choice.CheckResult(roll);
            currentPage = choice.NextPage();
        }

        public void Reset() {
            currentPage = currentChapter;
        }

        // Plays the start narration once, if possible
        public void playStartNarration()
        {
            if (startNarrationReady == true)
            {
                Narration.Narrate(startNarrationClip);
                startNarrationReady = false;
            }
        }

        // Plays the exit narration once, if possible
        public void PlayExitNarration()
        {
            if (exitNarrationReady == true)
            {
                Narration.Narrate(exitNarrationClip);
                exitNarrationReady = false;
            }
        }
    }
}
