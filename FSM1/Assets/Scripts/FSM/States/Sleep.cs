using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sleep : State
{
    public Sleep(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.SLEEP;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        base.Exit();
    }
}
