using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Movement_controller))]

public class PC_InputController : MonoBehaviour
{
    Movement_controller _playerMovement;
    DateTime _strikeClickTime;
    float _move;
    bool _jump;
    bool _crawling;
    bool _canAtack;
    private void Start()
    {
        _playerMovement = GetComponent<Movement_controller>();
    }
    // Start is called before the first frame update

    void Update()
    {
    
    // Update is called once per frame
        _move = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jump = true;
        }
        _crawling = Input.GetKey(KeyCode.S);

        if (Input.GetKey(KeyCode.Q))
        {
            _playerMovement.StartCasting();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _strikeClickTime = DateTime.Now;
            _canAtack = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            float holdTime = (float)(DateTime.Now - _strikeClickTime).TotalSeconds;
            if (_canAtack)
                _playerMovement.StartStrike(holdTime);
            _canAtack = false;
        }
        if ((DateTime.Now - _strikeClickTime).TotalSeconds >= _playerMovement.ChargeTime * 2 && _canAtack)
        {
            _playerMovement.StartStrike(_playerMovement.ChargeTime);
            _canAtack = false;
        }
    

}

    private void FixedUpdate()
    {
        _playerMovement.Move(_move, _jump, _crawling);
        _jump = false;
    }
}
