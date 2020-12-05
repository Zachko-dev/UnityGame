using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_controller : MonoBehaviour
{
    [SerializeField] private int _maxHP;
    private int _currentHP;
    [SerializeField] private int _maxMP;
    private int _currentMP;
    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _mpSlider;
    [SerializeField] GameObject _damageText;

    Movement_controller _playerMovement;

    public bool IsDead => _currentHP <= 0;
    public bool _canBeDamaged { get; private set; } = true;

    void Start()
    {
        _playerMovement = GetComponent<Movement_controller>();
        _playerMovement.OnGetHurt += OnGetHurt;
        _currentHP = _maxHP;
        _currentMP = _maxMP;
        _hpSlider.maxValue = _maxHP;
        _hpSlider.value = _maxHP;
        _mpSlider.maxValue = _maxMP;
        _mpSlider.value = _maxMP;
    }

    private void RefreshHUD()
    {
        _hpSlider.value = _currentHP;
        _mpSlider.value = _currentMP;
    }

    public void TakeTamage(int damage, DamageType type = DamageType.Casual, Transform enemy = null)
    {

        if (IsDead || !_canBeDamaged || damage == 0)
            return;
      
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            OnDeath();
        }

        switch (type)
        {
            case DamageType.PowerStrike:
                _playerMovement.GetHurt(enemy.position);
                break;
        }
        _hpSlider.value = _currentHP;
        Instantiate(_damageText, transform.position + new Vector3(0f, 1.25f), _damageText.transform.rotation)
            .GetComponentInChildren<DamageText>().SetDamage(damage);
        StartCoroutine(MakeRed(0.25f));
        RefreshHUD();
    }

    private void OnGetHurt(bool canBeDamaged)
    {
        _canBeDamaged = canBeDamaged;
        StartCoroutine(MakeRed(0.25f));
        RefreshHUD();
    }

    public void RestoreHP(int hp)
    {
        _currentHP += hp;
        if (_currentHP > _maxHP)
        {
            _currentHP = _maxHP;
        }

        _hpSlider.value = _currentHP;
        Instantiate(_damageText, transform.position + new Vector3(0f, 1.25f), _damageText.transform.rotation)
            .GetComponentInChildren<DamageText>().SetDamage(hp, restore: true);
        StartCoroutine(MakeGreen(0.2f));
        RefreshHUD();
    }

    private IEnumerator MakeRed(float seconds)
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(seconds);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private IEnumerator MakeGreen(float seconds)
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(seconds);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public bool ChangeMP(int value)
    {
        if (value < 0 && _currentMP < Mathf.Abs(value))
            return false;

        _currentMP += value;
        if (_currentMP > _maxMP)
            _currentMP = _maxMP;
        _mpSlider.value = _currentMP;

        RefreshHUD();
        return true;
    }

    public void AddHpRegen(int value)
    {
        StartCoroutine(StartRegenHp(value));
    }

    public void Poison(int value)
    {
        StartCoroutine(StartPoison(value));
    }

    private IEnumerator StartRegenHp(int value)
    {
        int count = 0;
        while (count++ != 10 && !IsDead)
        {
            RestoreHP(value);
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator StartPoison(int value)
    {
        int count = 0;
        while (count++ != 10 && !IsDead)
        {
            TakeTamage(value);
            yield return new WaitForSeconds(1);
        }
    }

    public void OnDeath()
    {
        _canBeDamaged = false;
        GetComponent<Movement_controller>().DisableMove();
        Debug.Log("Death");
        GetComponent<Animator>().SetBool("Death", true);
        GetComponent<Animator>().Play("Death");
    }

    private void Update()
    {
        if(IsDead)
            GetComponent<Movement_controller>().DisableMove();
    }

    private void Restart()
    {
        ServiceManager.Restart();
    }
}