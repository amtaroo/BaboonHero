using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundaryMap : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // stop player
        }

    }
}