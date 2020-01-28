using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;

public class WordScamble : MonoBehaviour
{
    private Text textComp;
    private char[] baseWord;
    private char[] newWord;

    // Start is called before the first frame update
    void Start()
    {
        textComp = GetComponent<Text>();
        baseWord = textComp.text.ToCharArray();
        newWord = new char[baseWord.Length];
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
}
