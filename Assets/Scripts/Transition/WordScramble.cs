using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;

public class WordScramble : MonoBehaviour
{
    private Text textComp;
    private char[] baseWord;
    private char[] newWord;
    private char letter;

    // Start is called before the first frame update
    void Start()
    {
        textComp = GetComponent<Text>();
        baseWord = textComp.text.ToCharArray();
        newWord = new char[baseWord.Length];
        letter = baseWord[0];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < newWord.Length; i++)
        {
            newWord[i] = baseWord[Random.Range(0, baseWord.Length)];
        }
        textComp.text = new string(newWord);
    }

    // switch the text to its original letter
    void OnDisable()
    {
        textComp.text = letter.ToString();
    }
}
