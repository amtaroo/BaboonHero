using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform holdSpot;
    public Transform dropSpot;
    public LayerMask pickUpMask;
    public float pickUpRadius = 0.7f;
    private GameObject itemHolding;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (itemHolding)
            {
                // ทิ้งขยะ
                PlayerDropTrash(itemHolding);
                itemHolding.transform.position = dropSpot.position;
                itemHolding.transform.parent = null;
                if (itemHolding.GetComponent<Rigidbody2D>())
                    itemHolding.GetComponent<Rigidbody2D>().simulated = true;
                itemHolding = null;
            }
            else
            {
                // เก็บขยะ
                Collider2D pickUpItem = Physics2D.OverlapCircle(transform.position, pickUpRadius, pickUpMask);
                if (pickUpItem)
                {
                    itemHolding = pickUpItem.gameObject;
                    PlayerPickUpTrash(itemHolding);
                    itemHolding.transform.position = holdSpot.position;
                    itemHolding.transform.parent = transform;
                    if (itemHolding.GetComponent<Rigidbody2D>())
                        itemHolding.GetComponent<Rigidbody2D>().simulated = false;
                }
            }
        }
    }
    //เช็คplayer ถือขยะ
    void PlayerPickUpTrash(GameObject trash)
    {
        TrashItem trashItem = trash.GetComponent<TrashItem>();
        if (trashItem != null)
        {
            trashItem.isBeingHeldByPlayer = true;
        }

    }
    //เช็คplayer ถือขยะ
    void PlayerDropTrash(GameObject trash)
    {
        TrashItem trashItem = trash.GetComponent<TrashItem>();
        if (trashItem != null)
        {
            trashItem.isBeingHeldByPlayer = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }
}
