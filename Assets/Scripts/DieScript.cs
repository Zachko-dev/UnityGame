using UnityEngine;

public class DieScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D info)
    {
        var player = info.GetComponent<Player_controller>();
        if (player != null)
            player.TakeTamage(999);
    }
}
