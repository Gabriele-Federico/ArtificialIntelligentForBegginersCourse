using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State 
{
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK, SLEEP, RUNAWAY
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;

    private float visDist = 10.0f;
    private float visAngle = 30.0f;
    private float shootDist = 7.0f;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }
    
    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    public bool CanSeePlayer()
    {
        Vector3 directionFromPlayer = player.position - npc.transform.position;
        float angle = Vector3.Angle(directionFromPlayer, npc.transform.forward);

        return (directionFromPlayer.magnitude < visDist && angle < visAngle);
    }

    public bool CanAttackPlayer()
    {
        Vector3 directionFromPlayer = player.position - npc.transform.position;

        return (directionFromPlayer.magnitude < shootDist);
    }

    public bool IsPlayerBehind()
    {
        Vector3 directionFromPlayer = npc.transform.position - player.position;
        float angle = Vector3.Angle(directionFromPlayer, npc.transform.forward);

        return (directionFromPlayer.magnitude < 2 && angle < 30);
    }
}
