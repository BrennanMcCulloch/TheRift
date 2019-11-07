using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDead : MonoBehaviour
{

	public GameObject deadStuff;
	public GameObject deadMesh;
	public GameObject aliveMesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space")) 
        {
        	deadStuff.SetActive(!deadStuff.activeSelf);
        	deadMesh.SetActive(!deadMesh.activeSelf);
        	aliveMesh.SetActive(!aliveMesh.activeSelf);
        }
    }
}
