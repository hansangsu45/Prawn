
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed;
    public float yMin;

    public bool isEnemy;

    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        CheckScrOut();
    }

    void CheckScrOut()
    {
        if(transform.position.y<yMin)
        {
            GameManager.Instance.training.InsertQueue(gameObject, isEnemy);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.training.InsertQueue(gameObject, isEnemy);
        }
    }
}
