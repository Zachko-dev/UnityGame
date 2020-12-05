using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] Vector2 _forces;
    private bool isActive = true;

    private void OnTriggerEnter2D(Collider2D info)
    {
        if (!isActive)
            return;

        info.GetComponent<Rigidbody2D>().AddForce(_forces, ForceMode2D.Impulse);
        StartCoroutine(Rest());
    }

    private IEnumerator Rest()
    {
        isActive = false;
        yield return new WaitForSeconds(1.5f);
        isActive = true;
    }

}
