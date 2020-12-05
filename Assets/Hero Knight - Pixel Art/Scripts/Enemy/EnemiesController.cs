using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesController : MonoBehaviour
{
    [SerializeField] private int _hp;
    [SerializeField] private int _maxHp;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] GameObject _damageText;

    private void Start()
    {
        _hpSlider.maxValue = _maxHp;
        _hpSlider.value = _hp;
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        _hpSlider.value = _hp;
        if (_hp <= 0)
            OnDeath();

        StartCoroutine(MakeRed(0.2f));
        Instantiate(_damageText, transform.position + new Vector3(-0.5f, 0.7f), _damageText.transform.rotation)
            .GetComponentInChildren<DamageText>().SetDamage(damage);
        //Debug.Log("Damage = " + damage);
        //Debug.Log("HP = " + _hp);
    }

    private IEnumerator MakeRed(float seconds)
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(seconds);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
