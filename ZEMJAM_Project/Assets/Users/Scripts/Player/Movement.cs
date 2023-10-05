using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class Movement : MonoBehaviour
{
    public static Movement Inst { get; set; }
    void Awake() => Inst = this;

    Rigidbody2D m_Rigidbody2D;
    SetAnimation m_SetAnimation;
    PlayerState m_PlayerState;
    PlayerSpriteRenderer m_PlayerSpriteRenderer;
    SetTimeScale m_SetTimeScale;
    Collison m_Collison;
    SetLight m_SetLight;

    public Sprite[] m_ComboSprite;
    public GameObject m_Combo;

    public int bouncedCount;
    public float count;
    public float multiSpeed = 1;
    public float tileMultiSpeed = 1;

    public bool isIgnoreCollison;
    public bool isAttacking;
    float attackTimer, avoidTimer, saveAvoid;

    public GameObject slash;
    public GameObject hitEffect;
    public GameObject skill;

    [SerializeField] LayerMask enemyLayer;

    Vector2 normalVelocity, lastVelocity;
    bool isSpin;

    void Start()
    {
        Time.timeScale = 1;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SetAnimation = GetComponent<SetAnimation>();
        m_PlayerState = GetComponent<PlayerState>();
        m_PlayerSpriteRenderer = GetComponent<PlayerSpriteRenderer>();
        m_SetTimeScale = GetComponent<SetTimeScale>();
        m_Collison = GetComponent<Collison>();
        m_SetLight = GetComponent<SetLight>();
    }

    void Update()
    {
        if (GameManager.Inst.isSetting || GameManager.Inst.onDeath) return;

        CinemachineManager.Inst.isJoom = count > 0;

        lastVelocity = normalVelocity * multiSpeed * tileMultiSpeed;
        multiSpeed = Mathf.Lerp(multiSpeed, 1, Time.deltaTime);

        isAttacking = attackTimer > 0;
        attackTimer -= Time.deltaTime;
        avoidTimer += Time.deltaTime;
    }

    void LateUpdate()
    {
        if (gameObject.layer == 3)
            m_Rigidbody2D.velocity = lastVelocity;
    }

    public void Dash(float power, float angle)
    {
        count = power;
        bouncedCount = 0;
        HealthManager.Inst.OnFade(false);

        transform.rotation = Quaternion.Euler(0, 0, angle);
        SetNormalVelocity(-transform.right * 15);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        SetMultiSpeed(1.5f);
    }

    public void CrashEnemy(Collision2D collision)
    {
        if(saveAvoid == avoidTimer)
            return;
        saveAvoid = avoidTimer;

        attackTimer = 0;
        if (count == 0)
        {
            StartCoroutine(Bounced(collision.transform));
            return;
        }

        var enemy = collision.transform.GetComponent<EnemyDefence>();
        if (collision.gameObject.TryGetComponent(out AssassinMark mark))
        {
            bool isAssassinate = mark.AssassinKill(collision.contacts[0].normal);
            if (isAssassinate)
            {
                StartCoroutine(AssassinPenetrate(collision));
                return;
            }
        }
        int totalDamage = enemy.AttemptAttack(bouncedCount + 1);
        if (totalDamage < 0)
            FailedAttack(collision);
        else
            SucceedAttack(collision, enemy.defence);
    }
    public void CrashWall(Collision2D collision)
    {
        if (count > 1)
        {
            SetNormalVelocity(MoveReflect(collision));
            StartCoroutine(m_Collison.CapsuleAble());

            AddCombo();
            count--;
        }
        else
        {
            foreach (GameObject enemy in SummonManager.Inst.enemyList)
            {
                if (enemy.TryGetComponent(out EnemyDashSign enemyDashSign))
                    enemyDashSign.AfterDash();
            }
            SetNormalVelocity(Vector2.zero);
            HealthManager.Inst.OnFade(true);
            UIManager.Inst.SwapUI(false, 0.4f);

            count = 0;
        }
        UIManager.Inst.SetPower(count);
    }

    void OnTriggerEnter2D(Collider2D collision)
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

    void AddCombo()
    {
        SpriteRenderer combo = Instantiate(m_Combo, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();

        combo.sprite = m_ComboSprite[bouncedCount > 14 ? 14 : bouncedCount];
        combo.transform.position += new Vector3(transform.position.x > 0 ? -0.75f : 0.75f, transform.position.y > 0 ? -0.7f : 1f);

        bouncedCount++;
        UIManager.Inst.AddCombo(bouncedCount);

        var isRoyalKnight = Character.Inst.playerType == PlayerType.ROYALKNIGHT;
        if (isRoyalKnight && bouncedCount >= 10 && !isSpin)
            StartCoroutine(SpinSlash());
    }

    IEnumerator SpinSlash()
    {
        isSpin = true;
        GameObject spin = Instantiate(skill);

        while (count > 0)
        {
            var hit = Physics2D.OverlapCircle(transform.position, 1.45f, enemyLayer);
            if (hit)
            {
                Time.timeScale = 0.05f;
                CinemachineShake.Inst.ShakeCamera(13, 0.3f);
                m_SetLight.SuddenLight(1f, 0.4f);
                hit.transform.GetComponent<EnemyDefence>().OnDamage(transform, normalVelocity);
                Instantiate(hitEffect, hit.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 359)));

                AddCombo();
            }

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        Destroy(spin);
        isSpin = false;
        yield return null;
    }

    public void SucceedAttack(Collision2D collision, int defence, bool isPenetrate = false)
    {
        StartCoroutine(Attack(collision, defence));

        if (isPenetrate) return;
        var reflectVelocity = MoveReflect(collision);
        SetNormalVelocity(reflectVelocity);
    }

    IEnumerator Attack(Collision2D collision, int defence)
    {
        var saveVelocity = normalVelocity;
        SetMultiSpeed(1.25f);
        m_PlayerSpriteRenderer.SetTransformFlip(collision.transform);
        var slash = Instantiate(this.slash, transform.position, Quaternion.identity).GetComponent<SlashParticle>();
        slash.SetParticle(collision.transform.position.x < transform.position.x, collision.transform);
        Instantiate(hitEffect, collision.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 359)));

        m_SetAnimation.AttackTrigger();
        attackTimer = 0.35f;
        CinemachineShake.Inst.ShakeCamera(10 + defence, 0.25f + defence * 0.02f);
        m_SetLight.SuddenLight(0.8f, 0.4f);
        collision.transform.GetComponent<EnemyDefence>().OnDamage(transform, saveVelocity);
        AddCombo();

        m_SetTimeScale.isRigidTime = false;
        m_SetTimeScale.setTime = 0.01f;
        Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(0.2f);
        m_SetTimeScale.setTime = 0.2f;
        Time.timeScale = 0.2f;
        m_SetTimeScale.isRigidTime = true;
    }

    public void FailedAttack(Collision2D collision)
    {
        StartCoroutine(Hit(collision.transform));
    }

    public void TakeMirror()
    {
        count = bouncedCount;
    }
    public IEnumerator Hit(Transform target)
    {
        if (m_PlayerState.isInvincible)
            m_PlayerState.SetBarrier(false);
        else
            HealthManager.Inst.OnDamage();
        StartCoroutine(m_PlayerSpriteRenderer.GracePerioding());
        StartCoroutine(m_Collison.CapsuleAble());
        CinemachineShake.Inst.ShakeCamera(15, 0.3f);

        m_SetAnimation.Hit(true);

        Time.timeScale = 0.15f;
        if (HealthManager.Inst.curhp <= 0)
        {
            Time.timeScale = 0.035f;
            GameManager.Inst.ChangeState(GameState.DEATH);
        }
        UIManager.Inst.SetPower(0);

        yield return StartCoroutine(Bounced(target));

        if (HealthManager.Inst.curhp > 0)
            m_SetAnimation.Hit(false);
        else
            m_SetAnimation.Death();
    }
    public IEnumerator Bounced(Transform target)
    {
        count = 0;
        gameObject.layer = 8;

        isIgnoreCollison = HealthManager.Inst.curhp <= 0;
        m_PlayerSpriteRenderer.SetTransformFlip(target);
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

    bool isAssassinTrigger;
    IEnumerator AssassinPenetrate(Collision2D collision)
    {
        AssassinMultiSlash multiSlash = Instantiate(skill, collision.transform.position, Quaternion.identity).GetComponent<AssassinMultiSlash>();
        multiSlash.movement = this;

        var saveVelocity = normalVelocity;
        SetNormalVelocity(Vector2.zero);
        gameObject.layer = 8;
        m_PlayerSpriteRenderer.SetColor(new Color(0, 0, 0, 0), 0.1f);

        var enemySprite = collision.transform.GetComponent<EnemySprite>();

        m_SetTimeScale.isRigidTime = false;
        m_SetTimeScale.setTime = 1;
        Time.timeScale = 1;

        for (float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            if(isAssassinTrigger)
            {
                enemySprite.hitTimer = 0.04f;
                CinemachineShake.Inst.ShakeCamera(8, 0.075f);
                isAssassinTrigger = false;
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        m_SetTimeScale.isRigidTime = true;

        SetNormalVelocity(saveVelocity);
        gameObject.layer = 3;
        m_PlayerSpriteRenderer.SetColor(new Color(1, 1, 1, 1), 0.2f);

        SucceedAttack(collision, 8, true);
    }
    public void AssassinTrigger() => isAssassinTrigger = true;

    void SetNormalVelocity(Vector3 velocity)
    {
        normalVelocity = velocity;
        m_Rigidbody2D.velocity = normalVelocity;
    }

    public void SetMultiSpeed(float value)
    {
        multiSpeed = value;
    }

    public void Death()
    {
        StartCoroutine(UIManager.Inst.ShowResultPanel(false));
    }
}
