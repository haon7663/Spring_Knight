using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeScale : MonoBehaviour
{
    Movement m_Movement;
    Rigidbody2D m_Rigidbody2D;

    [SerializeField] Vector3[] offset;
    [SerializeField] LayerMask enemyLayer;

    [SerializeField] float defaultTimeScale;

    void Start()
    {
        m_Movement = GetComponent<Movement>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        var setTime = defaultTimeScale;
        for (int i = 0; i < offset.Length; i++)
            if (Physics2D.Raycast(transform.position + offset[i], m_Rigidbody2D.velocity, 10000, enemyLayer) && m_Movement.count > 0)
                setTime = 0.4f;

        Time.timeScale = Mathf.Lerp(Time.timeScale, setTime, Time.deltaTime * 9);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (m_Rigidbody2D)
            for (int i = 0; i < 5; i++)
                Gizmos.DrawRay(transform.position + offset[i], m_Rigidbody2D.velocity);
    }
}
