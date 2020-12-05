using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    public enum Pickup
    {
        Hp,
        Mp,
        JumpBoost,
        HpRegeneration,
    }

    [SerializeField] private Pickup PickupType;
    [SerializeField] private int _value;
    private bool _taken;

    private void OnTriggerEnter2D(Collider2D info)
    {
        if (_taken)
            return;

        switch (PickupType)
        {
            case Pickup.Hp:
                info.GetComponent<Player_controller>().RestoreHP(_value);
                break;
            case Pickup.Mp:
                info.GetComponent<Player_controller>().ChangeMP(_value);
                break;
            case Pickup.JumpBoost:
                info.GetComponent<Movement_controller>().AddJumpBoost(_value);
                break;
            case Pickup.HpRegeneration:
                info.GetComponent<Player_controller>().AddHpRegen(_value);
                break;
        }

        Destroy(gameObject);
    }


}
