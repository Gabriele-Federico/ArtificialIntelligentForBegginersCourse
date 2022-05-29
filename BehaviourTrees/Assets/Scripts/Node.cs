using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum Status
    {
        SUCCESS, RUNNING, FAILURE 
    };

    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string name;
    
    public Node() { }

    public Node(string name)
    {
        this.name = name;
    }

    public void AddChild(Node n)
    {
        children.Add(n);
    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public void PrintTree()
    {
        Debug.Log(name);
        foreach (Node child in children)
        {
            child.PrintTree();
        }

    }
}


