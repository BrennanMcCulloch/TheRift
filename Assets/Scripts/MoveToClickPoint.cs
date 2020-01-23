using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour
{
    //Making this a singleton because I assume we'll only ever control the pc this way
    public static MoveToClickPoint instance;

    public NavMeshAgent agent;

    // Awake is called once before start
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Set up event handlers
	void OnEnable()
    {
		InputManager.OnPress += OnPress;
	}

	// Remove event handlers
	void OnDisable()
    {
		InputManager.OnPress -= OnPress;
	}

    // Set destination to the clicked point upon click
    public void OnPress()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            agent.destination = hit.point;
        }
    }
}

