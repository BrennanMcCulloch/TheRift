//Stole this online
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    //When something is drawn, we make planes. Trail render. 
    public GameObject drawPrefab;
   

    private GameObject theTrail;
    private Plane planeObj;
    private Vector3 startPos;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        planeObj = new Plane(Camera.main.transform.forward * -1, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {

            theTrail = (GameObject)Instantiate(drawPrefab, this.transform.position, Quaternion.identity);

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(planeObj.Raycast(mouseRay, out distance))
            {
                startPos = mouseRay.GetPoint(distance);
            }
        }
        else if(Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(distance) != startPos && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (planeObj.Raycast(mouseRay, out distance))
            {
                theTrail.transform.position = mouseRay.GetPoint(distance);
            }
        }

        //On mouse up. Bound to need to use this.
        else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
        {
            Destroy(theTrail, 6);//Destroys the game object after 6 second so it's not hogging memory
        }
    }
}
