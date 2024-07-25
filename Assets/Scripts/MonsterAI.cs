/*
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public Transform Player;
    public float speed = 2.0f;
    public GameObject GameOverPanel;

    void Update()
    {
        Vector3 direction = (Player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Over!");
            GameOverPanel.SetActive(true);
            
            
        }
    }
}
*/
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public Transform Player;
    public float speed = 2.0f;
    public float chaseDistance = 5.0f; // ระยะที่มอนสเตอร์จะเริ่มไล่ตามผู้เล่น
    public GameObject GameOverPanel;

    void Update()
    {
        if (Player != null) // ตรวจสอบว่า Player ไม่เป็น null
        {
            float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

            if (distanceToPlayer <= chaseDistance)
            {
                Vector3 direction = (Player.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Over!");
            GameOverPanel.SetActive(true);
            // คุณอาจต้องการหยุดเกม หรือทำสิ่งอื่นๆ เพิ่มเติมเมื่อ Game Over
        }
    }
}
