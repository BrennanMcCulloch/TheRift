using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueTree;

public class Player : Singleton<Player>
{
    const int DICE_MIN_VALUE = 1;
    const int DICE_MAX_VALUE = 12;

    // Returns the roll of a D12, modified by the given stat type.
    public int StatRoll()
    {
        return Random.Range(DICE_MIN_VALUE,DICE_MAX_VALUE);
    }

}
