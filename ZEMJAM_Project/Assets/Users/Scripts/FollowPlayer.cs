using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FollowPlayer : MonoBehaviour
{
    Transform player;

    [SerializeField] bool isAfterImage;
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] GameObject afterImagePrf;
    float afterTimer;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        transform.position = player.position;

        if (!isAfterImage) return;
        afterTimer += Time.deltaTime;
        if (afterTimer > 0.05f)
        {
            SummonAfterImage(m_SpriteRenderer);
            afterTimer = 0;
        }
    }

    void SummonAfterImage(SpriteRenderer spriteRenderer)
    {
        SpriteRenderer afterImage = Instantiate(afterImagePrf, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();
        afterImage.sprite = spriteRenderer.sprite;
        afterImage.transform.localScale = transform.localScale;
    }
}
