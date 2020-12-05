using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _ySpeed_Min;
    [SerializeField] private float _ySpeed_Max;
    [SerializeField] private float _lifeTime;
    [SerializeField] private TMP_Text _text;
    private bool _restore = false;
    private int _side = 1;

    private bool _textMode => !string.IsNullOrEmpty(_displayText);

    private string _displayText;
    private int _fontSize;

    private void Start()
    {
        if (Random.Range(0, 3) > 0)
            _side = -1;

        if (!_textMode)
            _text.text = _restore ? $"+{_damage}" : $"-{_damage}";
        else
        {
            _text.fontSize = _fontSize;
            _text.text = _displayText;
        }

        Destroy(gameObject.transform.parent.gameObject, _lifeTime);
    }

    public void SetDamage(int damage, bool restore = false)
    {
        _damage = damage;
        _restore = restore;

        if (damage == 0)
            Destroy(gameObject.transform.parent.gameObject);
    }

    public void SetText(string text, int fontSize)
    {
        _displayText = text;
        _fontSize = fontSize;
    }

    private void FixedUpdate()
    {
        float xSway = Random.Range(0.01f, 0.085f);
        float ySway = Random.Range(_ySpeed_Min, _ySpeed_Max);

        gameObject.transform.Translate(new Vector3(xSway * _side, ySway * Time.deltaTime, 0f), Space.World);
    }

}
