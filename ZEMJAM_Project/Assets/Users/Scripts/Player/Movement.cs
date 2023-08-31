using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class Movement : MonoBehaviour
{
    public static Movement Inst { get; set; }
    void Awake() => Inst = this;

    Rigidbody2D m_Rigidbody2D;
    SetAnimation m_SetAnimation;
    Collison m_Collison;

    public Sprite[] m_ComboSprite;
    public GameObject m_Combo;

    public int bouncedCount;
    public float count;
    public float boostPower;

    public bool isAttacking;
    float attackTimer;

    Vector2 lastVelocity;

    private void Start()
    {
        Time.timeScale = 1;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SetAnimation = GetComponent<SetAnimation>();
        m_Collison = GetComponent<Collison>();
    }

    private void Update()
    {
        if (GameManager.Inst.isSetting || HealthManager.Inst.curhp <= 0) return;

        CinemachineManager.Inst.isJoom = count > 0;

        lastVelocity = m_Rigidbody2D.velocity;

        isAttacking = attackTimer > 0;
        attackTimer -= Time.deltaTime;
    }

    public void Dash(float power, float angle)
    {
        count = power;
        bouncedCount = 0;

        transform.rotation = Quaternion.Euler(0, 0, angle);
        m_Rigidbody2D.velocity = -transform.right * 17;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void CrashEnemy(Collision2D collision)
    {
        if (count == 0)
        {
            StartCoroutine(Bounced(collision.transform));
            return;
        }

        int totalDamage = collision.transform.GetComponent<EnemyDefence>().AttemptAttack(bouncedCount + 1);
        if (totalDamage < 0)
            FailedAttack(collision);
        else
            StartCoroutine(SucceedAttack(collision, totalDamage));
    }
    public void CrashWall(Collision2D collision)
    {
        if(count > 1)
        {
            m_Rigidbody2D.velocity = MoveReflect(collision);
            StartCoroutine(m_Collison.CapsuleAble());
            ComboPlus();
            count--;
        }
        else
        {
            foreach (GameObject enemy in SummonManager.Inst.enemyList)
            {
                if (enemy && enemy.GetComponent<EnemyDashSign>())
                    enemy.GetComponent<EnemyDashSign>().AfterDash();
            }
            m_Rigidbody2D.velocity = Vector2.zero;

            count = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Item"))
        {
            collision.GetComponent<Item>().UseItem();
        }
    }

    Vector2 MoveReflect(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var dir = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        return dir * Mathf.Max(speed, 0f);
    }

    private void ComboPlus()
    {
        SpriteRenderer combo = Instantiate(m_Combo, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();

        combo.sprite = m_ComboSprite[bouncedCount > 14 ? 14 : bouncedCount];
        combo.transform.position += new Vector3(transform.position.x > 0 ? -0.75f : 0.75f, transform.position.y > 0 ? -0.7f : 1f);
        combo.transform.localScale = new Vector3(1f + (float)bouncedCount / 25f, 1f + (float)bouncedCount / 25f, 1);

        bouncedCount++;
    }

    public IEnumerator SucceedAttack(Collision2D collision, int totalDamage)
    {
        var saveVelocity = MoveReflect(collision);

        for (float i = 0; i < 0.22f; i += Time.deltaTime)
        {
            m_Rigidbody2D.velocity = Vector2.zero;
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        collision.transform.GetComponent<EnemyDefence>().OnDamage();
        StartCoroutine(m_Collison.CapsuleAble());
        m_SetAnimation.AttackTrigger();
        attackTimer = 0.4f;

        CinemachineShake.Instance.ShakeCamera(5, 0.5f);
        ComboPlus();

        m_Rigidbody2D.velocity = saveVelocity;
    }

    public void FailedAttack(Collision2D collision)
    {
        StartCoroutine(Hit(collision.transform));
        StartCoroutine(m_Collison.CapsuleAble());
    }

    public void TakeMirror()
    {
        count = bouncedCount;
    }
    public void InvItem()
    {
    }
    public IEnumerator Hit(Transform target)
    {
        HealthManager.Inst.OnDamage();

        yield return StartCoroutine(Bounced(target));

        /*if (HealthManager.Inst.curhp <= 0)
        {
            m_Animator.SetBool("isDeath", true);
            if(m_Collison.onDown && m_Animator.GetBool("isDeath"))
            {
                Fade.instance.Fadein();
                Invoke(nameof(Death), 0.5f);
            }
        }*/
    }
    public IEnumerator Bounced(Transform target)
    {
        count = 0;
        gameObject.layer = 8;

        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.AddForce(new Vector2(target.position.x - transform.position.x > 0 ? -0.5f : 0.5f, 1), ForceMode2D.Impulse);
        m_Rigidbody2D.gravityScale = 5;

        if (HealthManager.Inst.curhp <= 0)
            while (!m_Collison.onDown)
                yield return YieldInstructionCache.WaitForFixedUpdate;
        else
            while (!m_Collison.onCollision)
                yield return YieldInstructionCache.WaitForFixedUpdate;

        m_Rigidbody2D.gravityScale = 0;
        lastVelocity = Vector2.zero;

        gameObject.layer = 3;
    }
    private void Death()
    {
        SceneManager.LoadScene("Death");
    }
}
