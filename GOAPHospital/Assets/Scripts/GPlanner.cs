using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public Node(Node parent, float cost, Dictionary<string, int> allStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        state = new Dictionary<string, int>(allStates);
        this.action = action;
    }

    public Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefState, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        state = new Dictionary<string, int>(allStates);
        this.action = action;
        foreach(KeyValuePair<string, int> b in beliefState)
        {
            if (!state.ContainsKey(b.Key))
                state.Add(b.Key, b.Value);
        }

    }
}

public class GPlanner
{

    public Queue<GAction> plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach(GAction a in actions)
        {
            if (a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("No plan");
            return null;
        }

        Node cheapest = null;
        foreach(Node leaf in leaves)
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if(leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                } 
            }

        }

        List<GAction> resultingPlan = new List<GAction>();
        Node current = cheapest;
        while(current != null)
        {
            if(current.action != null)
            {
                resultingPlan.Insert(0, current.action);
            }
            current = current.parent;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach(GAction a in resultingPlan)
        {
            queue.Enqueue(a);
        }

        Debug.Log("The plan is: ");
        foreach(GAction a in queue)
        {
            Debug.Log("Q:" + a.actionName);
        }

        return queue;
    }


    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach(GAction action in usableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(KeyValuePair<string, int> eff in action.effects)
                {
                    if (!currentState.ContainsKey(eff.Key))
                        currentState.Add(eff.Key, eff.Value);
                }

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                        foundPath = true;
                }
            }
        }

        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach(GAction a in actions)
        {
            if (!a.Equals(removeMe))
            {
                subset.Add(a);
            }
        }
        return subset;
    }
}
