using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMover : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _range;
    private Vector2 _startPoint;
    private int _direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        _startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, _speed * _direction * Time.deltaTime, 0);
    }
}
