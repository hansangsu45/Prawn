
using UnityEngine;
using System.Collections;

public class TrainingPrawn : MonoBehaviour
{
    public Training training;

    public SpriteRenderer spr;
    public Collider2D col;
    public WaitForSeconds ws1 = new WaitForSeconds(0.2f);
    public float xMax, xMin;

    [SerializeField] float speed;

    private Vector2 dir;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            HitEffect();
        }
        else if(collision.CompareTag("Food"))
        {
            training.AteFood();
        }
    }

    private void HitEffect() //맞았을 때의 이벤트와 이펙트
    {
        training.HitPlayer();
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        col.enabled = false;

        for (int i=0; i<3; i++)
        {
            spr.enabled = false;
            yield return ws1;
            spr.enabled = true;
            yield return ws1;
        }

        col.enabled = true;
    }

    public void PointerEnter(bool right)
    {
        dir = right ? Vector2.right : Vector2.left;
    }
    public void PointerExit()
    {
        dir = Vector2.zero;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
        PosLimit();
    }

    private void PosLimit()
    {
        float x = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector2(x, transform.position.y);
    }
}
