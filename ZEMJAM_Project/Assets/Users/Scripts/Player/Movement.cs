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
    PlayerSpriteRenderer m_PlayerSpriteRenderer;
    Collison m_Collison;

    public Sprite[] m_ComboSprite;
    public GameObject m_Combo;

    public int bouncedCount;
    public float count;
    public float boostPower;
    public float multiSpeed = 1;

    public bool isAttacking;
    float attackTimer;

    [SerializeField] GameObject fireSlash;

    Vector2 normalVelocity, lastVelocity;

    private void Start()
    {
        Time.timeScale = 1;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SetAnimation = GetComponent<SetAnimation>();
        m_PlayerSpriteRenderer = GetComponent<PlayerSpriteRenderer>();
        m_Collison = GetComponent<Collison>();
    }

    private void Update()
    {
        if (GameManager.Inst.isSetting || HealthManager.Inst.curhp <= 0) return;

        CinemachineManager.Inst.isJoom = count > 0;

        multiSpeed = Mathf.Lerp(multiSpeed, 1, Time.deltaTime * 3);

        lastVelocity = normalVelocity * multiSpeed;

        isAttacking = attackTimer > 0;
        attackTimer -= Time.deltaTime;
    }

    void LateUpdate()
    {
        if(gameObject.layer == 3)
            m_Rigidbody2D.velocity = lastVelocity;
    }

    public void Dash(float power, float angle)
    {
        count = power;
        bouncedCount = 0;

        transform.rotation = Quaternion.Euler(0, 0, angle);
        SetNormalVelocity(-transform.right * 17.5f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        SetMultiSpeed(1.5f);
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
            SetNormalVelocity(MoveReflect(collision));
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
            SetNormalVelocity(Vector2.zero);

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
        var speed = normalVelocity.magnitude;
        var dir = Vector2.Reflect(normalVelocity.normalized, collision.contacts[0].normal);

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
        m_PlayerSpriteRenderer.SetTransformFlip(collision.transform);

        for (float i = 0; i < 0.2f; i += Time.deltaTime)
        {
            m_Rigidbody2D.velocity = Vector2.zero;
            if (collision == null) break;  
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        if (collision == null)
            yield return null;

        var slash = Instantiate(fireSlash, transform.position, Quaternion.identity).transform;
        slash.localScale = new Vector2(collision.transform.position.x > transform.position.x ? 1 : -1, 1);

        m_SetAnimation.AttackTrigger();
        attackTimer = 0.5f;
        CinemachineShake.Instance.ShakeCamera(6, 0.6f);
        collision.transform.GetComponent<EnemyDefence>().OnDamage();
        ComboPlus();
        Time.timeScale = 0.005f;
        SetMultiSpeed(1.5f);

        StartCoroutine(m_Collison.CapsuleAble());
        SetNormalVelocity(saveVelocity);
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
        normalVelocity = m_Rigidbody2D.velocity;
        m_Rigidbody2D.gravityScale = 5;

        if (HealthManager.Inst.curhp <= 0)
            while (!m_Collison.onDown)
                yield return YieldInstructionCache.WaitForFixedUpdate;
        else
            while (!m_Collison.onCollision)
                yield return YieldInstructionCache.WaitForFixedUpdate;

        m_Rigidbody2D.gravityScale = 0;
        lastVelocity = Vector2.zero;
        SetNormalVelocity(Vector2.zero);

        gameObject.layer = 3;
    }

    void SetNormalVelocity(Vector3 velocity)
    {
        normalVelocity = velocity;
        m_Rigidbody2D.velocity = normalVelocity;
    }

    public void SetMultiSpeed(float value)
    {
        multiSpeed = value;
    }

    void Death()
    {
        SceneManager.LoadScene("Death");
    }
}
