using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private GameObject fishPooling;

    private Vector2 targetPosition;

    private float speed = 0f;

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    private GameManager gameManager;
    
    private void Awake()
    {
        fishPooling = GameObject.Find("FishPooling");
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (transform.localPosition.x <= gameManager.fishMinPosition.x)
        {
            if(transform == gameManager.fishTransform)
            {
                speed = 1f;
            }
            else
            {
                speed = Random.Range(minSpeed, maxSpeed);
            }
        }

        targetPosition = transform.localPosition;
        targetPosition.x += speed * Time.deltaTime;
        transform.localPosition = targetPosition;

        if (transform.localPosition.x >= gameManager.fishMaxPosition.x)
        {
            FishDestroy();
        }
    }

    public void FishDestroy()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(fishPooling.transform, true);
    }
}