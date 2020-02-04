using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    public GameObject otherSide;
    public GameObject otherCamera;
    public GameObject thisCamera;

    private static NavMeshAgent playerAgent;
    private Material material;
    private Color originalColor;
    private bool interactable;

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        originalColor = material.color;
        playerAgent = Player.Instance.GetComponent<NavMeshAgent>();
    }

    // Make this interactible when in range of the player
    private void OnTriggerEnter(Collider other)
    {
        //only activate when the player is near
        if (other.gameObject.tag == "Player")
        {
            material.color = Color.green;
            interactable = true;
        }
    }

    // Deactivate when player leaves
    private void OnTriggerExit(Collider other)
    {
        //only deactivate when the player leaves
        if (other.gameObject.tag == "Player")
        {
            material.color = originalColor;
            interactable = false;
        }
    }

    // Warp player to linked door and switch cameras when clicked
    private void OnMouseUpAsButton()
    {
        if (interactable)
        {
            CamSwitcher.SwitchTo(otherCamera, thisCamera);
            Debug.Log(Player.Instance.gameObject.transform.position);//test
            //Player.Instance.gameObject.transform.position = otherSide.transform.position;
            //Debug.Log(Player.Instance.gameObject.transform.position);//test
            //Player.Instance.transform.Translate(otherSide.transform.forward);
            playerAgent.Warp(otherSide.transform.position);
            Debug.Log(Player.Instance.gameObject.transform.position);//test
        }
    }
}
