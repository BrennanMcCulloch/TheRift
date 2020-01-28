using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //delegate representing input events
    public delegate void InputDelegate();
    //triggers on the initial press
    public static InputDelegate OnPress;
    //triggers if pressed last check and this one
    public static InputDelegate OnHold;
    //triggers on relase of mouse button/touchscreen
    public static InputDelegate OnRelease;

    // Update is called once per frame
    void Update()
    {
        //handle first frame of input
        if(InputStarts && OnPress != null) OnPress();
        //mouse held down
        else if(InputContinues && OnHold != null) OnHold();
        //On mouse up. Bound to need to use this.
        else if(InputEnds && OnRelease != null) OnRelease();
    }

    // When finger or mouse have just been pushed down
    bool InputStarts
    {
        get {return Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began); }
    }

    // When finger or mouse are already down and moving
    bool InputContinues
    {
        get { return Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved); }
    }

    // When finger or mousebutton is lifted
    bool InputEnds
    {
        get { return Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended); }
    }
}
