using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum StateEnum {Drawing, Talking};
    //private string[] StateEnumNames = System.Enum.GetNames(typeof(StateEnum));

    private static int stateEnumCount = System.Enum.GetNames(typeof(StateEnum)).Length;
    private static int currentState = (int)StateEnum.Drawing;
    private static int previousState = (int)StateEnum.Drawing;
    private static MonoBehaviour[][] stateScripts = new MonoBehaviour[stateEnumCount][];

    // Start is called before the first frame update
    void Start()
    {
        stateScripts[(int)StateEnum.Drawing] = new MonoBehaviour[] {DrawManager.instance, CreateMesh.instance};
        stateScripts[(int)StateEnum.Talking] = new MonoBehaviour[0]; //no scripts for this yet
    }

    //Exit current state and set new one
    public static void setState(int newState)
    {
        if(newState != currentState)
        {
            exitCurrentState();
            enterState(newState);
        }
    }

    //Exit current state and return to the previous
    public static void returnToPreviousState()
    {
        exitCurrentState();
        enterState(previousState);
    }

    //Disable scripts for the current state
    private static void exitCurrentState()
    {
        foreach(MonoBehaviour script in stateScripts[currentState])
        {
            script.enabled = false;
        }
    }

    //Changes the state and enables the necessary scripts
    private static void enterState(int newState)
    {
        currentState = newState;
        foreach(MonoBehaviour script in stateScripts[currentState])
        {
            script.enabled = true;
        }
    }

}
