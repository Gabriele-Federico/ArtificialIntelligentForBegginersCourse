using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPatients : MonoBehaviour
{

    public GameObject patientPrefab;
    public int numPatients;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnPatient", 5);
    }

    void SpawnPatient()
    {
        Instantiate(patientPrefab, transform.position, Quaternion.identity);
        Invoke("SpawnPatient", Random.Range(2, 10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
