
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public Transform Player;
    public float speed = 2.0f;
    public float chaseDistance = 5.0f;
    public GameObject GameOverPanel;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

            if (distanceToPlayer <= chaseDistance)
            {
                Vector3 direction = (Player.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;

                UpdateAnimation(direction);
            }
            else
            {
                
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", 0);
            }
        }
    }

    void UpdateAnimation(Vector3 direction)
    {
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Over!");
            GameOverPanel.SetActive(true);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
