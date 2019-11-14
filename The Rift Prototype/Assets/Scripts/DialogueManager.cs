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
    public GameObject button;
    public GameObject canvas; //THIS NEEDS TO BE THE DIALOGUE BOX

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
                GameObject newButton = Instantiate(button) as GameObject;
                Vector3 temp = newButton.transform.position;
                temp.y += stagger;
                newButton.transform.position = temp;
                newButton.transform.SetParent(canvas.transform, false);

                Talkeys sentenceAgain = sentences.Dequeue();
                newButton.GetComponentInChildren<Text>().text = sentenceAgain.whatToSay;
                if (sentenceAgain.Body > 0)
                {
                    newButton.name = "Body Roll";
                    newButton.GetComponentInChildren<Text>().text += " Body " + sentenceAgain.Body;
                }
                else if (sentenceAgain.Mind > 0)
                {
                    newButton.name = "Mind Roll";
                    newButton.GetComponentInChildren<Text>().text += " Mind " + sentenceAgain.Mind;
                }
                else if (sentenceAgain.Soul > 0)
                {
                    newButton.name = "Soul Roll";
                    newButton.GetComponentInChildren<Text>().text += " Soul " + sentenceAgain.Soul;
                }
                else //it's neutral, but a choice. Maybe do something?
                {
                    newButton.name = "Neutral";
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
}
