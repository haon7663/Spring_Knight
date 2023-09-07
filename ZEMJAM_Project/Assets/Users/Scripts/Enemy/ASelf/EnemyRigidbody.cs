using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRigidbody : MonoBehaviour
{
    EnemyBundle m_EnemyBundle;
    Rigidbody2D m_Rigidbody2D;

    void Awake()
    {
        if (TryGetComponent(out EnemyBundle bundle))
            m_EnemyBundle = bundle;
        if (TryGetComponent(out Rigidbody2D rigid))
            m_Rigidbody2D = rigid;
    }
    public IEnumerator BouncedOff(Vector3 velocity)
    {
        gameObject.layer = 9;

        m_Rigidbody2D.mass = 1;
        m_Rigidbody2D.velocity = velocity * 2.5f;
        m_Rigidbody2D.drag = 12;

        yield return StartCoroutine(m_EnemyBundle.sprite.DeathFade());

        Destroy(gameObject);
    }
}
