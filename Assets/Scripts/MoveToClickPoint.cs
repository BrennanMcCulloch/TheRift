using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : Singleton<MoveToClickPoint>
{
    public NavMeshAgent agent;
    public Footsteps footstepGenerator;

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
            footstepGenerator.enabled = true;
        }
    }
}

