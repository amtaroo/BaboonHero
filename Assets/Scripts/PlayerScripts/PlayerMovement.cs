using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float boostedSpeed = 10f;
    public float boostDuration = 5f;

    public float magnetRadius = 5f;
    public float magnetDuration = 10f;

    private float currentSpeed;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private Coroutine magnetCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (magnetCoroutine != null)
        {
            AttractItems();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SpeedBoost"))
        {
            StartCoroutine(SpeedBoost());
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Magnet"))
        {
            if (magnetCoroutine != null)
            {
                StopCoroutine(magnetCoroutine);
            }

            magnetCoroutine = StartCoroutine(DeactivateMagnetAfterDuration(magnetDuration));
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("DoublePointsItem"))
        {
            ChallengeModeManager.Instance.ActivateDoublePoints();
            Destroy(collision.gameObject);
        }


        if (collision.CompareTag("TimeFreeze"))
        {
            Debug.Log("TimeFreeze item collected!");
            TimeFreezeManager.Instance.FreezeTime(); // เรียกใช้งานฟังก์ชันหยุดเวลา
            Destroy(collision.gameObject);
        }



    }

    private IEnumerator SpeedBoost()
    {
        currentSpeed = boostedSpeed;

        yield return new WaitForSeconds(boostDuration);

        currentSpeed = normalSpeed;
    }

    private IEnumerator DeactivateMagnetAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        magnetCoroutine = null;
    }

    private void AttractItems()
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, magnetRadius);

        foreach (Collider2D item in items)
        {
            if (item.gameObject.CompareTag("Trash"))
            {
                Vector2 direction = transform.position - item.transform.position;
                item.transform.position = Vector2.MoveTowards(item.transform.position, transform.position, Time.deltaTime * 5f);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}