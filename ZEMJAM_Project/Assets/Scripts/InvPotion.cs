using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvPotion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(collision.GetComponent<Movement>().InvItem());
            Destroy(gameObject);
        }
    }
}
