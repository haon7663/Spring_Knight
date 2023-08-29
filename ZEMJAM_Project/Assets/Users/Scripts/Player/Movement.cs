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

    public SpriteRenderer m_SwordSpriteRenderer;

    private Rigidbody2D m_Rigidbody2D;

    private Animator m_Animator;
    private Animator m_SwordAnimator;

    private Collison m_Collison;

    private Camera mainCamera;

    public GameObject m_Ray;
    public GameObject m_PowerBar;
    public GameObject m_After;
    public GameObject m_Spin;
    public GameObject m_Barrior;
    public Sprite[] m_ComboSprite;
    private SpriteRenderer m_SpinSpriteRenderer;

    public GameObject m_Combo;

    public LayerMask m_EnemyLayer;
    private Vector2 lastVelocity;

    public Vector3 m_Dir;

    private float power;
    private float ableAngle;
    private float m_TimeScale;

    public int bouncedCount;
    public float count;
    public float boostPower;
    public float m_SlopeAngel;
    public float m_AfterImageCount;

    public bool m_isInv;

    public bool isAttacking;
    float attackTimer;

    private bool isSpin = false;

    private float mTimer;

    private void Start()
    {
        Time.timeScale = 1f;
        m_SwordAnimator = m_SwordSpriteRenderer.GetComponent<Animator>();

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpinSpriteRenderer = m_Spin.GetComponent<SpriteRenderer>();
        m_SwordAnimator = m_SwordSpriteRenderer.GetComponent<Animator>();
        m_Animator = GetComponent<Animator>();
        m_Collison = GetComponent<Collison>();

        mainCamera = Camera.main;
    }


    private void Update()
    {
        if (GameManager.Inst.isSetting || HealthManager.Inst.curhp <= 0) return;

        CinemachineManager.Inst.isJoom = count > 0 && !isSpin;


        /*if (m_Count > 0)
        {
            mTimer += Time.deltaTime;
            if (mTimer > m_AfterImageCount)
            {
                if (m_BounceCount >= 3)
                {
                    GameObject afterImage = Instantiate(m_After, transform.position, Quaternion.identity);
                    afterImage.GetComponent<SpriteRenderer>().flipX = m_SpriteRenderer.flipX;
                    afterImage.GetComponent<SpriteRenderer>().sprite = m_SpriteRenderer.sprite;
                    mTimer = 0;
                }
                if (m_BounceCount >= 10)
                {
                    GameObject afterSpinImage = Instantiate(m_After, transform.position, Quaternion.identity);
                    afterSpinImage.GetComponent<SpriteRenderer>().sprite = m_SpinSpriteRenderer.sprite;
                }
                else if (m_BounceCount >= 7)
                {
                    GameObject afterSwordImage = Instantiate(m_After, transform.position, Quaternion.identity);
                    afterSwordImage.GetComponent<SpriteRenderer>().sprite = m_SwordSpriteRenderer.sprite;
                    afterSwordImage.GetComponent<SpriteRenderer>().flipX = m_SwordSpriteRenderer.flipX;
                }
            }
        }*/

        if (bouncedCount >= 10 && count > 0)
        {
            if (!m_Spin.activeSelf)
            {
                m_Animator.SetBool("isSpin", true);
                m_Animator.SetTrigger("spin");
                m_Spin.SetActive(true);
            }
            m_Spin.transform.Rotate(0, 0, 2000 * Time.deltaTime);

            Collider2D enemy = Physics2D.OverlapCircle(transform.position, 2, m_EnemyLayer);
            if (enemy)
            {
                CinemachineShake.Instance.ShakeCamera(7, 0.5f);
                enemy.transform.GetComponent<EnemyDefence>().AttemptAttack(enemy.transform.GetComponent<EnemyDefence>().defence);
                boostPower = 1.75f;
                DOTween.To(() => boostPower, x => boostPower = x, 1f, 0.15f + count * 0.1f).SetEase(Ease.OutQuint);
            }
        }
        else if (m_Spin.activeSelf)
        {
            m_Spin.SetActive(false);
            m_Animator.SetBool("isSpin", false);
        }

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
            m_SlopeAngel = Vector2.Angle(collision.contacts[0].normal, Vector2.up);

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

        Debug.Log("moveRe" + dir * Mathf.Max(speed, 0f));
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

        m_Animator.SetTrigger("attack");
        m_SwordAnimator.SetTrigger("swing");
        attackTimer = 0.4f;

        CinemachineShake.Instance.ShakeCamera(5, 0.5f);
        ComboPlus();

        m_Rigidbody2D.velocity = saveVelocity;
    }

    public void FailedAttack(Collision2D collision)
    {
        StartCoroutine(Hit(collision.transform));
    }

    public void takeMirror(Transform target)
    {
        count = bouncedCount;
    }
    public void InvItem()
    {
        m_isInv = true;
        m_Barrior.SetActive(true);
    }
    public IEnumerator Hit(Transform target)
    {
        count = 0;
        HealthManager.Inst.OnDamage();
        m_Animator.SetBool("isHit", true);
        //m_SpriteRenderer.flipX = target.position.x > transform.position.x;
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.AddForce(new Vector2(target.position.x - transform.position.x > 0 ? -0.5f : 0.5f, 1), ForceMode2D.Impulse);
        m_Rigidbody2D.gravityScale = 5;

        if(HealthManager.Inst.curhp <= 0)
            while (!m_Collison.onDown)
                yield return YieldInstructionCache.WaitForFixedUpdate;
        else
            while (!m_Collison.onCollision)
                yield return YieldInstructionCache.WaitForFixedUpdate;

        m_Rigidbody2D.gravityScale = 0;
        m_Animator.SetBool("isHit", false);

        lastVelocity = Vector2.zero;

        gameObject.layer = 3;
        if (HealthManager.Inst.curhp <= 0)
        {
            m_Animator.SetBool("isDeath", true);
            if(m_Collison.onDown && m_Animator.GetBool("isDeath"))
            {
                Fade.instance.Fadein();
                Invoke(nameof(Death), 0.5f);
            }
        }
    }
    private void Death()
    {
        SceneManager.LoadScene("Death");
    }
}
