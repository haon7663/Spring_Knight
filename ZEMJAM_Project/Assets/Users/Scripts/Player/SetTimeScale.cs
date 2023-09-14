using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeScale : MonoBehaviour
{
    Rigidbody2D m_Rigidbody2D;

    [SerializeField] Vector3[] offset;
    [SerializeField] LayerMask enemyLayer;

    [SerializeField] float defaultTimeScale;

    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (Movement.Inst.isIgnoreCollison) return;

        var setTime = defaultTimeScale;
        for (int i = 0; i < offset.Length; i++)
            if (Physics2D.Raycast(transform.position + offset[i], m_Rigidbody2D.velocity, 1, enemyLayer) && Movement.Inst.count > 0)
            {
                Time.timeScale = 0.15f;
                setTime = 0.15f;
            }


        Time.timeScale = Mathf.Lerp(Time.timeScale, setTime, Time.deltaTime * 9);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (m_Rigidbody2D)
            for (int i = 0; i < offset.Length; i++)
                Gizmos.DrawRay(transform.position + offset[i], m_Rigidbody2D.velocity);
    }
}
