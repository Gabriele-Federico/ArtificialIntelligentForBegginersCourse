
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class World
{
    private static readonly World istance = new World();
    private static GameObject[] hidingSpots;

    static World()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }

    private World() { }

    public static World Istance
    {
        get
        {
            return istance;
        }
    }

    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }
}
