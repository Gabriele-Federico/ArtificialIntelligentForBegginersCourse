using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - transform.position;
        agent.SetDestination(transform.position - fleeVector);
    }

    void Evade()
    {
        Vector3 targetDir = target.transform.position - transform.position;

        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - transform.position;

        float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));

        if((toTarget > 90 && relativeHeading < 20) || target.GetComponent<Drive>().currentSpeed < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1f, 1f) * wanderJitter, 0, Random.Range(-1f, 1f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = gameObject.transform.InverseTransformVector(targetLocal);
        Seek(targetWorld);
    }

    void Hide()
    {
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for(int i = 0; i < World.Istance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Istance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePos = World.Istance.GetHidingSpots()[i].transform.position + hideDir.normalized * 5;
            if(Vector3.Distance(transform.position, hidePos) < distance)
            {
                chosenSpot = hidePos;
                distance = Vector3.Distance(transform.position, hidePos);
            }
        }

        Seek(chosenSpot);
    }

    void CleverHide()
    {
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Istance.GetHidingSpots()[0];

        for (int i = 0; i < World.Istance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Istance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePos = World.Istance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10;
            if (Vector3.Distance(transform.position, hidePos) < distance)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = World.Istance.GetHidingSpots()[i];
                distance = Vector3.Distance(transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float dist = 100;
        hideCol.Raycast(backRay, out info, dist);


        Seek(info.point + chosenDir.normalized * 2);
    }

    bool CanSeeTarget()
    {
        RaycastHit info;
        Vector3 rayToTarget = target.transform.position - transform.position;

        if(Physics.Raycast(transform.position, rayToTarget, out info))
        {
            if(info.transform.gameObject.tag == "cop")
            {
                return true;
            }
        }
        return false;

    }

    bool TargetCanSeeMe()
    {
        Vector3 toAgent = transform.position - target.transform.position;
        float lookAngle = Vector3.Angle(target.transform.forward, toAgent);

        if (lookAngle < 60)
            return true;
        return false;
    }

    bool coolDown = false;

    void BehaviourCoolDown()
    {
        coolDown = false;
    }

    int range = 10;

    bool TargetInRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) < range;
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetInRange())
        {
            if (!coolDown)
            {
                if (CanSeeTarget() && TargetCanSeeMe())
                {
                    CleverHide();
                    coolDown = true;
                    Invoke("BehaviourCoolDown", 5);
                }
                else
                    Pursue();
            }
        }
        else
            Wander();
    }
}
