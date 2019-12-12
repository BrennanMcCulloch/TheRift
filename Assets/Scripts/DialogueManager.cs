//got this from a tutorial

using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    //Attempts to begin dialogue, returns whether or not this was a success
    public bool StartDialogue(Dialogue dialogue)
    {
        if (dialogue.neutral.target != null)
        {
            bool result = dialogue.neutral.Invoke();
            if (result)
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
            else
            {
                //Dialogue failed
                return false;
            }
        }
        else
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
        //Dialogue successfully began
        return true;
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
        if (sentences.Peek().isChoice)
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
                if (sentenceAgain.Body > 0)
                {
                    newButton.GetComponentInChildren<Text>().text = sentenceAgain.whatToSay;
                    newButton.name = "Body Roll";
                    newButton.GetComponentInChildren<Text>().text += " (Body " + sentenceAgain.Body + ")";
                    newButton.onClick.AddListener(()=>bodyResult(sentenceAgain));
                }
                else if (sentenceAgain.Mind > 0)
                {
                    newButton.GetComponentInChildren<Text>().text = sentenceAgain.whatToSay;
                    newButton.name = "Mind Roll";
                    newButton.GetComponentInChildren<Text>().text += " (Mind " + sentenceAgain.Mind + ")";
                    newButton.onClick.AddListener(() => mindResult(sentenceAgain));
                }
                else if (sentenceAgain.Soul > 0)
                {
                    newButton.GetComponentInChildren<Text>().text = sentenceAgain.whatToSay;
                    newButton.name = "Soul Roll";
                    newButton.GetComponentInChildren<Text>().text += " (Soul " + sentenceAgain.Soul + ")";
                    newButton.onClick.AddListener(() => soulResult(sentenceAgain));
                }
                else //it's neutral, but a choice. Maybe do something?
                {
                    if (sentenceAgain.nextDialogueSuccess.neutral.target != null)
                    {
                        bool result = sentenceAgain.nextDialogueSuccess.neutral.Invoke();
                        if(result)
                        {
                            newButton.GetComponentInChildren<Text>().text = sentenceAgain.whatToSay;
                            newButton.name = "Neutral";
                            newButton.onClick.AddListener(() => neutralResult(sentenceAgain));
                        }
                        else
                        {
                            Destroy(newButton);
                        }
                    }
                    else
                    {
                        newButton.GetComponentInChildren<Text>().text = sentenceAgain.whatToSay;
                        newButton.name = "Neutral";
                        newButton.onClick.AddListener(() => neutralResult(sentenceAgain));
                    }
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
        StateManager.returnToPreviousState();
    }

    void destroyButtons()
    {
        foreach(Button gameObj in GameObject.FindObjectsOfType<Button>())
        {
            if(gameObj.name == "Body Roll" || gameObj.name == "Mind Roll" || gameObj.name == "Soul Roll" || gameObj.name == "Neutral" || gameObj.name == "Button(Clone)" || gameObj.name == "Roll Result")
            {
                Destroy(gameObj.gameObject);
            }
        }
    }

    void bodyResult(Talkeys sentence)
    {
        destroyButtons();
        Debug.Log("Needed Value: " + sentence.Body);
        int roll = player.GetComponent<PlayerMethods>().bodyRoll();
        Button newButton = Instantiate(button) as Button;
        Vector3 temp = newButton.transform.position;
        temp.y = -525;
        newButton.transform.position = temp;
        newButton.transform.SetParent(canvas.transform, false);
        newButton.name = "Roll Result";
        newButton.GetComponentInChildren<Text>().text = "You Rolled: " + roll;
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
        //Debug.Log("Needed Value: " + sentence.Mind);//test
        int roll = player.GetComponent<PlayerMethods>().mindRoll();
        Button newButton = Instantiate(button) as Button;
        Vector3 temp = newButton.transform.position;
        temp.y = -525;
        newButton.transform.position = temp;
        newButton.transform.SetParent(canvas.transform, false);
        newButton.name = "Roll Result";
        newButton.GetComponentInChildren<Text>().text = "You Rolled: " + roll;
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
        Button newButton = Instantiate(button) as Button;
        Vector3 temp = newButton.transform.position;
        temp.y = -525;
        newButton.transform.position = temp;
        newButton.transform.SetParent(canvas.transform, false);
        newButton.name = "Roll Result";
        newButton.GetComponentInChildren<Text>().text = "You Rolled: " + roll;
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
        StartDialogue(sentence.nextDialogueSuccess);
    }
}