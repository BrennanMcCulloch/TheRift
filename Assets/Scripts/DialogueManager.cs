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

    // Allows us to stop the coroutine on the fly, and prevent simultaneous execution
    private Coroutine typingPage;

    public void StartDialogue(Dialogue newDialogue)
    {
        dialogue = newDialogue;
        animator.SetBool("isOpen", true);
        nameText.text = dialogue.title;

        // start the coroutine that 'types' the text in the dialogue window
        typingPage = StartCoroutine("TypeSentence", dialogue.currentPage.Text());

        // Stops us from moving around when dialoguing
        StateManager.setState((int)StateManager.StateEnum.Talking);
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        dialogue = null;
        if (typingPage != null) {
            StopCoroutine(typingPage);
            typingPage = null;
        }

        // Lets us start moving again
        StateManager.returnToPreviousState();
    }

    // Triggered by the 'Continue' button in the UI.
    // The UI 'types out' the current page.
    // We dont want to go to the next page if it's still typing;
    // instead we want to 'fast forward' the current typing
    public void NextPage() {
        // if we're typing...
        if (typingPage != null) {
            FinishTyping();
            DisplayChoicesIfPresent();
        }
        else {
            if (dialogue.NextPage())
            {
                typingPage = StartCoroutine("TypeSentence", dialogue.currentPage.Text());
            }
            else EndDialogue();
        }
    }

    // Immediately types out the current page and clears the coroutine
    private void FinishTyping() {
        StopCoroutine(typingPage);
        typingPage = null;
        dialogueText.text = dialogue.currentPage.Text();
    }

    // Creates a typewriter effect for the current page text
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            float jitter = Random.Range(0.01f, 0.075f);
            yield return new WaitForSeconds(jitter);
        }
        DisplayChoicesIfPresent();
        typingPage = null;
    }

    private void DisplayChoicesIfPresent() {
        // Reasons why we dont want to show the choices:
        // 1. there are no choices to show
        if (dialogue.currentPage.Choices().Length == 0) return;
        // 2. the page is locked per unmet prerequisites
        if (dialogue.currentPage.Locked()) return;
        // Swap visibility on the choice buttons and the continue button
        navButtonsPanel.gameObject.SetActive(false);
        choiceButtonsPanel.gameObject.SetActive(true);

        for (int i = 0; i < dialogue.currentPage.Choices().Length; i++) {
            Button button = choiceButtons[i];
            Choice choice = dialogue.currentPage.Choices()[i];

            // Prepare data for button text
            string rollType = choice.statCheck.statType.ToString();
            string buttonName = rollType + " Roll";
            string buttonText = " (" + rollType + " " + choice.statCheck.statRequirement + ") " + choice.text;

            // Populate button with text
            button.name = buttonName;
            button.GetComponentInChildren<Text>().text = buttonText;

            // Remove old click listeners and act upon the current choice when we click
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=>DispalyResult(choice));
        }
    }

    // Performs a roll check,
    void DispalyResult(Choice choice)
    {
        // Once a choice has been made, show the continue button
        navButtonsPanel.gameObject.SetActive(true);
        choiceButtonsPanel.gameObject.SetActive(false);

        // Perform and display a roll.
        int roll = Player.Instance.StatRoll(choice.statCheck.statType);
        // Todo (matt) - we can probably do something with this result value
        ChoiceResult result = choice.CheckResult(roll);
        rollButton.GetComponentInChildren<Text>().text = "You Rolled: " + roll;

        // If there is a results page, insert it after the current dialogue page and then call nextpage.
        // Todo (matt) - this is a very hacky way to do this. there is an architecture problem somewhere.
        Page resultsPage = choice.ResultsPage();
        if (resultsPage != null) dialogue.InsertNext(resultsPage);
        NextPage();

        // Todo(matt) - after introducing 'repeatable' to pages and choices, rework this page direction.
        // actually, probably rework how this all works anyway. not in love with the insertnext logic.

    }
}
