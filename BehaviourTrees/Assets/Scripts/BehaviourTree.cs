using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class isn't necessary, it felt kinda wrong when the teacher said to create it
public class BehaviourTree : Node
{
    public BehaviourTree()
    {
        name = "Tree";
    }

    public BehaviourTree(string n)
    {
        name = n;
    }

    public struct NodeLevel
    {
        int level;
        public Node node;
    }

    public override Status Process()
    {
        return children[currentChild].Process();
    }

}
