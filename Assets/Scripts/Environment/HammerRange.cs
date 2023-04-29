using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Wall"))
        {
            Debug.Log("Вы ударили стену");
        }

    }
}
