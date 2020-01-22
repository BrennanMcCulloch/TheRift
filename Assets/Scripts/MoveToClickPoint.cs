using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour
{
    //Making this a singleton because I assume we'll only ever control the pc this way
    public static MoveToClickPoint instance;

    public NavMeshAgent agent;

    private Footsteps footstepGenerator;

    // Awake is called once before start
    void Awake()
    {
        instance = this;
        footstepGenerator = instance.GetComponent<Footsteps>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.destination = hit.point;
                footstepGenerator.enabled = true;
            }
        }
    }
}

