using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collison : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;

    [Space]
    [Header("Collisions")]
    [SerializeField] CapsuleCollider2D m_CapsuleCollider2D;
    public bool onCollision;
    public bool onUp;
    public bool onDown;
    public bool onRight;
    public bool onLeft;

    [Space]
    [Header("Values")]
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset, topOffset;

    private void Update()
    {
        onUp = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, collisionRadius, groundLayer);
        onDown = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onCollision = onUp || onDown || onRight || onLeft;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
            Movement.Inst.CrashEnemy(collision);

        else if (collision.transform.CompareTag("Wall"))
            Movement.Inst.CrashWall(collision);

        else if (collision.transform.CompareTag("Damage"))
            StartCoroutine(Movement.Inst.Hit(collision.transform));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + topOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }

    public IEnumerator CapsuleAble()
    {
        m_CapsuleCollider2D.enabled = false;
        yield return YieldInstructionCache.WaitForFixedUpdate;
        yield return YieldInstructionCache.WaitForFixedUpdate;
        m_CapsuleCollider2D.enabled = true;
    }
}
