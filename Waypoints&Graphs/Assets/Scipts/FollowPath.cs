using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{

    public GameObject wpManager;
    GameObject[] wps;
    UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void GoToHeli()
    {
        agent.SetDestination(wps[1].transform.position);
        //g.AStar(currentNode, wps[1]);
        //currentWP = 0;

    }

    public void GoToRuin()
    {
        agent.SetDestination(wps[6].transform.position);
        //g.AStar(currentNode, wps[6]);
        //currentWP = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
      
    }
}
