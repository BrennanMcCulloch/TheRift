//got this from a tutorial

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public Button button;
    public GameObject canvas; //THIS NEEDS TO BE THE DIALOGUE BOX
    public GameObject player; 

    private Queue<Talkeys> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<Talkeys>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();
        dialogue.thingToDoOnEntry.Invoke();

        foreach (Talkeys sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        Talkeys sentence = sentences.Dequeue();
        dialogueText.text = sentence.whatToSay;

        //if the next option is a choice, display the rest of the queue
        if(sentences.Peek().isChoice)
        {
            int stagger = 200;
            while(sentences.Count > 0)
            {
                Button newButton = Instantiate(button) as Button;
                Vector3 temp = newButton.transform.position;
                temp.x += 25;
                temp.y += stagger;
                newButton.transform.position = temp;
                newButton.transform.SetParent(canvas.transform, false);

                Talkeys sentenceAgain = sentences.Dequeue();
                newButton.GetComponentInChildren<Text>().text = sentenceAgain.whatToSay;
                if (sentenceAgain.Body > 0)
                {
                    newButton.name = "Body Roll";
                    newButton.GetComponentInChildren<Text>().text += " Body " + sentenceAgain.Body;
                    newButton.onClick.AddListener(()=>bodyResult(sentenceAgain));
                }
                else if (sentenceAgain.Mind > 0)
                {
                    newButton.name = "Mind Roll";
                    newButton.GetComponentInChildren<Text>().text += " Mind " + sentenceAgain.Mind;
                    newButton.onClick.AddListener(() => mindResult(sentenceAgain));
                }
                else if (sentenceAgain.Soul > 0)
                {
                    newButton.name = "Soul Roll";
                    newButton.GetComponentInChildren<Text>().text += " Soul " + sentenceAgain.Soul;
                    newButton.onClick.AddListener(() => soulResult(sentenceAgain));
                }
                else //it's neutral, but a choice. Maybe do something?
                {
                    newButton.name = "Neutral";
                    newButton.onClick.AddListener(() => neutralResult(sentenceAgain));
                }

                stagger -= 100;
            }
        }
    }

    //DOESN'T WORK, NEED TO FIX
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; //makes it skip a frame
        }
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        destroyButtons();
    }

    void destroyButtons()
    {
        GameObject buttonAgain;
        buttonAgain = GameObject.Find("Body Roll");
        Destroy(buttonAgain);
        buttonAgain = GameObject.Find("Mind Roll");
        Destroy(buttonAgain);
        buttonAgain = GameObject.Find("Soul Roll");
        Destroy(buttonAgain);
        buttonAgain = GameObject.Find("Neutral");
        Destroy(buttonAgain);
    }

    void bodyResult(Talkeys sentence)
    {
        destroyButtons();
        Debug.Log("Needed Value: " + sentence.Body);
        int roll = player.GetComponent<PlayerMethods>().bodyRoll();
        if (roll < sentence.Body)
        {
            StartDialogue(sentence.nextDialogueFail);
        }
        else
        {
            StartDialogue(sentence.nextDialogueSuccess);
        }
    }

    void mindResult(Talkeys sentence)
    {
        destroyButtons();
        Debug.Log("Needed Value: " + sentence.Mind);
        int roll = player.GetComponent<PlayerMethods>().mindRoll();
        if (roll < sentence.Mind)
        {
            StartDialogue(sentence.nextDialogueFail);
        }
        else
        {
            StartDialogue(sentence.nextDialogueSuccess);
        }
    }

    void soulResult(Talkeys sentence)
    {
        destroyButtons();
        Debug.Log("Needed Value: " + sentence.Soul);
        int roll = player.GetComponent<PlayerMethods>().soulRoll();
        if (roll < sentence.Soul)
        {
            StartDialogue(sentence.nextDialogueFail);
        }
        else
        {
            StartDialogue(sentence.nextDialogueSuccess);
        }
    }

    void neutralResult(Talkeys sentence)
    {
        destroyButtons();
        //If there's a conditional that is required for entry
        if (sentence.nextDialogueSuccess.neutral != null)
        {
            Debug.Log("There's a condition for entry");
            bool result = sentence.nextDialogueSuccess.neutral.Invoke();

            if (result)
            {
                StartDialogue(sentence.nextDialogueSuccess);
            }
            else
            {
                StartDialogue(sentence.nextDialogueFail);
            }
        }
        //no conditional for entry
        else
        {
            Debug.Log("There's no condition for entry");
            StartDialogue(sentence.nextDialogueSuccess);
        }
    }
}
