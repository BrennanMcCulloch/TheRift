//got this from a tutorial

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DialogueTree;

public class DialogueManager : MonoBehaviour
{
    // Direct access to the UI elements we want to control
    public GameObject canvas; //THIS NEEDS TO BE THE DIALOGUE BOX
    public Text nameText;
    public Text dialogueText;
    public GameObject navButtonsPanel;
    public Button rollButton;
    public Button continueButton;
    public Button[] choiceButtons;
    public GameObject choiceButtonsPanel;

    // Access to data from scene objects
    public Dialogue dialogue;
    public Animator animator;

    // Sound effects
    public AudioClip[] textSoundClips;
    public float audioStutter = 0f;
    public float basePitch = 1f;
    public float pitchVariance = 0f;
    private float pitchLowerBound;
    private float pitchUpperBound;
    private int currentSource = 0;
    private AudioSource[] sources;

    // Allows us to stop the coroutine on the fly, and prevent simultaneous execution
    private Coroutine typingPage;
    private string currentText;

    // Plays once for initialization
    void Start()
    {
        sources = GetComponents<AudioSource>();
        pitchLowerBound = basePitch - pitchVariance;
        pitchUpperBound = basePitch + pitchVariance;
    }

    public void StartDialogue(Dialogue newDialogue)
    {
        dialogue = newDialogue;
        animator.SetBool("isOpen", true);
        nameText.text = dialogue.title;
        ShowPage();
        // Stops us from moving around when dialoguing
        StateManager.setState((int)StateManager.StateEnum.Talking);
        //play narration if any exists
        dialogue.playStartNarration();
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        //play narration if any exists
        dialogue.PlayExitNarration();
        //reset dialogue, etc.
        dialogue.Reset();
        dialogue = null;
        choiceButtonsPanel.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
        if (typingPage != null) {
            StopCoroutine(typingPage);
            typingPage = null;
        }
        // Lets us start moving again
        StateManager.returnToPreviousState();
    }

    public void NextPage() {
        if (typingPage != null) {
            FinishTyping();
            DisplayChoicesIfPresent();
        }
        else if (dialogue.CurrentPage().EndsDialogue() ) {
            EndDialogue();
        }
        else if (dialogue.CurrentPage().resolutionType == PageResolutionType.nextChapter) {
            dialogue.NextChapter();
            ShowPage();
        }
        else {
            dialogue.NextPage();
            ShowPage();
        }
    }

     public void Back() {
        FinishTyping();
        choiceButtonsPanel.gameObject.SetActive(false);
        dialogue.LastPage();
        if (dialogue.CurrentPage() == null) EndDialogue();
        else ShowPage();
    }


    public void ShowPage() {
        currentText = dialogue.CurrentPage().Text();
        typingPage = StartCoroutine("TypeSentence", currentText);
        continueButton.gameObject.SetActive(true);
        dialogue.CurrentPage().Visited();
    }

    // Immediately types out the current page and clears the coroutine
    private void FinishTyping() {
        if (typingPage != null) StopCoroutine(typingPage);
        typingPage = null;
        dialogueText.text = currentText;
    }

    // Creates a typewriter effect for the current page text
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            float jitter = Random.Range(0.01f, 0.075f);
            PlayRandomTextSound();
            yield return new WaitForSeconds(jitter);
        }
        DisplayChoicesIfPresent();
        FinishTyping();
    }

    private void DisplayChoicesIfPresent() {
        // Reasons why we dont want to show the choices:
        // 1. there are no choices to show
        if (dialogue.CurrentPage().Choices().Length == 0) return;
        // 2. the page is locked per unmet prerequisites
        if (dialogue.CurrentPage().Locked()) return;

        ClearChoicePanel();
        // hide continue button, show choices panel
        choiceButtonsPanel.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);

        for (int i = 0; i < dialogue.CurrentPage().Choices().Length; i++) {
            Button button = choiceButtons[i];
            Choice choice = dialogue.CurrentPage().Choices()[i];

            // Prepare data for button text
            string buttonName = "Roll";
            string buttonText = "(" + ((1200 - (choice.rollRequirement * 100)) / 12) + "%) " + choice.text;

            // Populate button with text
            button.name = buttonName;
            button.GetComponentInChildren<Text>().text = buttonText;
            button.gameObject.SetActive(true);

            // Remove old click listeners and act upon the current choice when we click
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=>Choose(choice));
        }
    }

    private void ClearChoicePanel() {
        foreach (Button choiceButton in choiceButtons) {
            choiceButton.gameObject.SetActive(false);
            choiceButton.GetComponentInChildren<Text>().text = null;
        }
    }

    public void Choose(Choice choice) {
        choiceButtonsPanel.gameObject.SetActive(false);

        int roll = Player.instance.StatRoll();
        rollButton.GetComponentInChildren<Text>().text = "You Rolled: " + roll;
        dialogue.MakeChoice(choice, roll);
        ShowPage();
    }

    // Plays a random textSoundClip
    private void PlayRandomTextSound()
    {
        if (Random.Range(0f, audioStutter) < 1f)
        {
            if (currentSource == 0) currentSource = 1;
            else currentSource = 0;
            sources[currentSource].pitch = Random.Range(pitchLowerBound, pitchUpperBound);
            sources[currentSource].clip = textSoundClips[Random.Range(0, textSoundClips.Length)];
            sources[currentSource].Play();
        }
    }
}
