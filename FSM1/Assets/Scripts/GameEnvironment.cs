using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameEnvironment : MonoBehaviour
{

    private static GameEnvironment istance;
    private List<GameObject> checkpoints = new List<GameObject>();
    public List<GameObject> Checkpoints { get { return checkpoints; } }

    public static GameEnvironment Singleton
    {
        get
        {
            if (istance == null)
            {
                istance = new GameEnvironment();
                istance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));

                istance.checkpoints = istance.checkpoints.OrderBy(waypoint => waypoint.name).ToList();
            }
            return istance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
