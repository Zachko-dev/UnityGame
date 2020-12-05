using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcherController : EnemyControllerBase
{

    [Header("Shooting")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _arrowSpeed;
    [SerializeField] private float _angerRange;

    protected bool _attaking;
    private bool _isAngry;
    protected Player_controller _player;

    protected override void Start()
    {
        base.Start();
        _player = FindObjectOfType<Player_controller>();
        StartCoroutine(ScanForPlayer());
    }

    protected override void Update()
    {
        if (_isAngry)
        {
            return;
        }
        base.Update();
    }

    protected void Shoot()
    {
        GameObject arrow = Instantiate(_projectilePrefab, _shootPoint.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = transform.right * _arrowSpeed;
        arrow.GetComponent<SpriteRenderer>().flipX = !_faceRight;
        Destroy(arrow, 5f);
    }

    protected override void FixedUpdate()
    {
       // base.FixedUpdate();
    }

    protected IEnumerator ScanForPlayer()
    {
        while (true)
        {
            CheckPlayerInRange();
            yield return new WaitForSeconds(1f) ;
        }
    }

    protected virtual void CheckPlayerInRange()
    {
        if (_player == null||_attaking)
        {
            return;
        }
        if (Vector2.Distance(transform.position, _player.transform.position) < _angerRange)
        {
            _isAngry = true;
            TurnToPlayer();
            ChangeState(EnemyState.Shoot);
        }
        else
            _isAngry = false;
    }

    protected void TurnToPlayer()
    {
        if (_player.transform.position.x - transform.position.x > 0 && !_faceRight)
            Flip();
        else if (_player.transform.position.x - transform.position.x < 0 && _faceRight)
            Flip();
    }

    protected override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);
        switch (state) 
        {
            case EnemyState.Shoot:
                _enemyRb.velocity = Vector2.zero;
                _attaking = true;
                break;
            

        }


    }
    protected override void ResetState()
    {
        base.ResetState();
        _enemyAnimator.SetBool(EnemyState.Shoot.ToString(), false);
        _enemyAnimator.SetBool(EnemyState.Hurt.ToString(), false);
    }

    protected override void EndState()
    {
       // base.ChangeState();
        switch (_currentState)
        {
            case EnemyState.Shoot:
                _attaking = false;
                break;

        }
        base.EndState();
        if(!_isAngry)
         ChangeState(EnemyState.Idle);
    }

    protected virtual void DoStateAction()
    {
        switch (_currentState) {
            case EnemyState.Shoot:
                Shoot();
                break;
        }
        
    }
}
