using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnim : MonoBehaviourPunCallbacks
{
    Animator _anim;
    float _timer;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void Move()
    {
        _anim.SetBool("isMoving", true);
    }

    public void Idle()
    {
        _anim.SetBool("isMoving", false);
    }

    public void Attack()
    {
        _anim.SetTrigger("attack");
        _anim.SetBool("isMoving", false);
    }

    public void ResetAttack()
    {
        _anim.ResetTrigger("attack");
    }
}