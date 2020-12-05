using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
[RequireComponent(typeof(Player_controller))]
public class Movement_controller : MonoBehaviour
{
    public event Action<bool> OnGetHurt = delegate { };
    private Animator _playerAnimator;
    private Rigidbody2D _playerRB;
    private Player_controller _playerController;
    [Header("Horizontal movement")]
    [SerializeField] private float _speed;
    [Range(0, 1)]
    private bool _faceRight = true;
    private bool _canMove = true;
    public bool IsFaceRight => _faceRight;

    [Header("Jumping")]
    [SerializeField] private bool _airControll;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _whatIsGround;
    private bool _grounded;
    private bool _canDoubleJump;
    [Header("Crawling")]
    [SerializeField] private Transform _cellCheck;
    [SerializeField] private Collider2D _headCollider;
    private bool _canStand;

    [Header("Casting")]
    [SerializeField] private GameObject _fireBall;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireBallSpeed;
    [SerializeField] private int _castCost;
    private bool _isCasting;

    [Header("Strike")]
    [SerializeField] private int _damage;
    [SerializeField] private Transform _strikePoint;
    [SerializeField] private float _strikeRange;
    [SerializeField] private LayerMask _enemies;
    private bool _isStriking;

    [Header("PowerStrike")]
    [SerializeField] private float _chargeTime;
    public float ChargeTime => _chargeTime;
    [SerializeField] private float _powerStrikeSpeed;
    [SerializeField] private Collider2D _strikeCollider;
    [SerializeField] private int _powerStrikeDamage;
    [SerializeField] private int _powerStrikeCost;
    private List<EnemiesController> _damagedEnemies = new List<EnemiesController>();
    [SerializeField] private float _pushForce;
    private float _lastHurtTime;

    public void DisableMove()
    {
        _canMove = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemiesController enemy = collision.collider.GetComponent<EnemiesController>();
        if (!_strikeCollider.enabled)
        {
            return;
        }

        if (enemy == null || _damagedEnemies.Contains(enemy))
            return;

        enemy.TakeDamage(_powerStrikeDamage);
        _damagedEnemies.Add(enemy);

    }
    void Start()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerController = GetComponent<Player_controller>();
    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_cellCheck.position, _radius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_strikePoint.position, _strikeRange);
    }
    void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }

    private void FixedUpdate()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheck.position, _radius, _whatIsGround);

        if (_playerAnimator.GetBool("Hurt") && _grounded && Time.time - _lastHurtTime > 0.5f)
            EndHurt();
    }

    public void Move(float move, bool jump, bool crawling)
    {
        #region Movement

        if (!_canMove)
        {
            _playerAnimator.SetFloat("Speed", Mathf.Abs(move));
            return;
        }

        if (move != 0 && (_grounded || _airControll))
        {
            _playerRB.velocity = new Vector2(_speed * move, _playerRB.velocity.y);
        }

        if (move > 0 && !_faceRight)
        {
            Flip();
        }
        else if (move < 0 && _faceRight)
        {
            Flip();
        }
        #endregion

        #region Jumping
        _grounded = Physics2D.OverlapCircle(_groundCheck.position, _radius, _whatIsGround);
        if (jump)
        {
            if (_grounded)
            {
                _playerRB.velocity = new Vector2(_playerRB.velocity.x, _jumpForce);
                _canDoubleJump = true;
            }
            else if (_canDoubleJump)
            {
                _playerRB.velocity = new Vector2(_playerRB.velocity.x, _jumpForce);
                _canDoubleJump = false;
            }
        }
        #endregion

        #region Crawling
        _canStand = !Physics2D.OverlapCircle(_cellCheck.position, _radius, _whatIsGround);
        if (crawling)
        {
            _headCollider.enabled = false;
        }
        else if (!crawling && _canStand)
        {
            _headCollider.enabled = true;
        }
        #endregion

        #region Animator
        _playerAnimator.SetFloat("Speed", Mathf.Abs(move));
        _playerAnimator.SetBool("Jump", !_grounded);
        _playerAnimator.SetBool("Crouch", !_headCollider.enabled);
        #endregion
    }
    public void StartCasting()
    {
        if (!_canMove)
            return;

        if (_isCasting || !_playerController.ChangeMP(-_castCost))
            return;
        _isCasting = true;
        _playerAnimator.SetBool("Casting", true);
    }

    private void CastFire()
    {
        if (!_canMove)
            return;

        GameObject fireBall = Instantiate(_fireBall, _firePoint.position, Quaternion.identity);
        fireBall.GetComponent<Rigidbody2D>().velocity = transform.right * _fireBallSpeed;
        fireBall.GetComponent<SpriteRenderer>().flipX = !_faceRight;
        Destroy(fireBall, 5f);
    }

    private void EndCasting()
    {
        _isCasting = false;
        _playerAnimator.SetBool("Casting", false);
    }

    public void StartStrike(float holdTime)
    {
        if (!_canMove)
            return;

        if (_isStriking || _playerRB.velocity != Vector2.zero)
            return;

        _canMove = false;
        if (holdTime >= _chargeTime)
        {
            if (!_playerController.ChangeMP(-_powerStrikeCost))
                return;

            _playerAnimator.SetBool("PowerStrike", true);

        }
        else
        {
            _playerAnimator.SetBool("Striking", true);
        }
        _isStriking = true;

    }
    public void GetHurt(Vector2 position)
    {
        _lastHurtTime = Time.time;
        //_canMove = false;
        //OnGetHurt(false);
        Vector2 pushDirection = new Vector2();
        pushDirection.x = position.x > transform.position.x ? -1 : 1;
        pushDirection.y = 1;
        //_playerAnimator.SetBool("Hurt", true);
        _playerRB.AddForce(pushDirection * _pushForce, ForceMode2D.Impulse);
    }

    private void ResetPlayer()
    {
        _playerAnimator.SetBool("Strike", false);
        _playerAnimator.SetBool("PowerStrike", false);
        _playerAnimator.SetBool("Casting", false);
        _playerAnimator.SetBool("Hurt", false);
        _playerAnimator.SetBool("Block", false);
        _isCasting = false;
        _isStriking = false;
        _canMove = true;
    }

    private void EndHurt()
    {
        ResetPlayer();
        OnGetHurt(true);
    }

    private void StartPowerStrike()
    {
        //if (!_canMove)
        //    return;

        _playerRB.velocity = transform.right * _powerStrikeSpeed;
        _strikeCollider.enabled = true;
        _canMove = false;
    }

    private void DisablePowerStrike()
    {
        _playerRB.velocity = Vector2.zero;
        _damagedEnemies.Clear();
        _canMove = true;
    }

    private void EndPowerStrike()
    {
        _playerAnimator.SetBool("PowerStrike", false);
        _canMove = true;
        _isStriking = false;
        _strikeCollider.enabled = false;
    }

    private void Strike()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_strikePoint.position, _strikeRange, _enemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemiesController enemy = enemies[i].GetComponent<EnemiesController>();
            enemy.TakeDamage(_damage);
        }

    }

    private void EndStrike()
    {
        _isStriking = false;
        _playerAnimator.SetBool("Striking", false);
        _canMove = true;
    }

    public void AddJumpBoost(int value)
    {
        StartCoroutine(StartJumpBoost(value));
    }

    private IEnumerator StartJumpBoost(int value)
    {
        float temp = _jumpForce;
        _jumpForce = value;
        yield return new WaitForSeconds(10f);
        _jumpForce = temp;
    }
}
