using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBanditController : EnemyControllerBase
{

    private Transform _player;

    protected override void Start()
    {
        base.Start();
        _player = FindObjectOfType<Player_controller>().transform;
    }

    protected override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);
        switch (_currentState) {
            case EnemyState.Idle:
                _enemyRb.velocity = Vector2.zero;
                break;
            case EnemyState.Move:
                _startPoint = transform.position;
                break;
        
        }

    }

    //protected override void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //    if(Mathf.Abs(transform.position.x -_player.position.x) <= _range)
    //    {
    //        if (transform.position.x < _player.position.x && !_faceRight)
    //            Flip();
    //        else if (transform.position.x > _player.position.x && _faceRight)
    //            Flip();
    //    }
    //}
}
