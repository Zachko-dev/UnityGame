using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotator : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool reverse;

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, (reverse ? -1 : 1) * speed * Time.deltaTime);
    }

}
