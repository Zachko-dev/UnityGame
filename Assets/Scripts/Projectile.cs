using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int _damage;
    private bool _damaged;

    private void OnTriggerEnter2D(Collider2D info)
    {
        if (_damaged)
            return;

        Player_controller target = info.GetComponent<Player_controller>();
        if (target != null)
        {
            _damaged = true;
            target.TakeTamage(_damage, DamageType.Projectile, transform);
        }

        Destroy(gameObject);

    }
}
