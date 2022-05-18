using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWP = 0;

    GameObject tracker;

    public float speed = 10f;
    public float rotSpeed = 1f;
    public float lookAhead = 10f;

    // Start is called before the first frame update
    void Start()
    {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;
    }

    void ProgressTracker()
    {

        if (Vector3.Distance(tracker.transform.position, transform.position) > lookAhead) return;

        if (Vector3.Distance(tracker.transform.position, waypoints[currentWP].transform.position) < 1)
            currentWP++;

        if (currentWP >= waypoints.Length)
            currentWP = 0;

        tracker.transform.LookAt(waypoints[currentWP].transform);
        tracker.transform.Translate(0, 0, (speed + 2) * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        ProgressTracker();

        //transform.LookAt(waypoints[currentWP].transform);
        Quaternion lookAt = Quaternion.LookRotation(tracker.transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, rotSpeed * Time.deltaTime);

        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
