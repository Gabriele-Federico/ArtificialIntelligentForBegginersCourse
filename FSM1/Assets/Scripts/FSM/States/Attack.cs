using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    private float rotationSpeed = 2f;
    private AudioSource shoot;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ATTACK;
        shoot = _npc.GetComponent<AudioSource>();
    }

    public override void Enter()
    {
        anim.SetTrigger("isShooting");
        agent.isStopped = true;
        shoot.Play();
        base.Enter();
    }

    public override void Update()
    {
        Vector3 directionFromPlayer = player.position - npc.transform.position;
        float angle = Vector3.Angle(directionFromPlayer, npc.transform.forward);
        directionFromPlayer.y = 0;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(directionFromPlayer), Time.deltaTime * rotationSpeed);

        if (!CanAttackPlayer())
        {
            nextState = new Patrol(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }


    }

    public override void Exit()
    {
        anim.ResetTrigger("isShooting");
        shoot.Stop();
        base.Exit();
    }
}
