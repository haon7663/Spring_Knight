using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class Movement : MonoBehaviour
{
    public static Movement instance;

    public SpriteRenderer m_SwordSpriteRenderer;

    private Rigidbody2D mRigidbody2D;
    private SpriteRenderer mSpriteRenderer;
    private Animator mAnimator;
    private Animator mSwordAnimator;
    private AudioSource mAudioSource;
    private AudioSource mSwordAudioSource;

    private Collison m_Collison;
    private Hp m_Hp;

    private Camera m_MainCamera;

    public Material DefaultMateral;
    public Material WhiteMateral;

    public GameObject m_Ray;
    public GameObject m_PowerBar;
    public GameObject m_After;
    public GameObject m_Spin;
    public GameObject m_Barrior;
    public Sprite[] m_ComboSprite;
    private SpriteRenderer m_SpinSpriteRenderer;

    public Transform m_CameraCanvas;
    public Transform m_DefenceCanvas;
    public RectTransform JoyPanel;
    public RectTransform JoyStick;
    public RectTransform[] JoyLiner;

    public Image m_PowerFilled;
    public Text m_PowerText;
    public GameObject m_Combo;

    public LayerMask m_EnemyLayer;

    public Vector3[] offset = new Vector3[5];

    private Vector3 startTouchPos;
    private Vector3 endTouchPos;
    private Vector3 saveVelocity;
    private Vector2 LastVelocity;

    public Vector3 m_Dir;

    public int m_BounceCount;
    private float power;
    private float ableAngle;
    private float atkTime;
    private float m_TimeScale;

    public float m_MaxPower;
    public float m_Power;
    public float m_Count = -1;
    public float m_Angle;
    public float m_BoostPower = 1;
    public float m_SlopeAngel;
    public float m_AfterImageCount;

    private float m_InvTime;
    private float filled = 0;

    public bool m_isInv;
    public bool m_Swing;
    public bool m_isAtk;

    private bool isHit = false;
    private bool isSpin = false;
    private bool onCol;

    private float mTimer;
    private float resetJoy = 0.1f;

    private bool isTouchDown = false;

    Touch touch;
    private void Start()
    {
        instance = this;
        Time.timeScale = 1f;
        mAudioSource = GetComponent<AudioSource>();
        mSwordAudioSource = m_SwordSpriteRenderer.GetComponent<AudioSource>();
        mSwordAnimator = m_SwordSpriteRenderer.GetComponent<Animator>();

        mRigidbody2D = GetComponent<Rigidbody2D>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpinSpriteRenderer = m_Spin.GetComponent<SpriteRenderer>();
        mSwordAnimator = m_SwordSpriteRenderer.GetComponent<Animator>();
        mAnimator = GetComponent<Animator>();
        m_Collison = GetComponent<Collison>();
        m_Hp = GetComponent<Hp>();
        m_MainCamera = Camera.main;

        isTouchDown = false;
    }

    internal static class YieldInstructionCache
    {
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
        private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if (!waitForSeconds.TryGetValue(seconds, out wfs))
                waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
            return wfs;
        }
    }


    private void Update()
    {
        if (GameManager.Gm.DoSetting) return;

        mSpriteRenderer.material = m_InvTime > 0 ? WhiteMateral : DefaultMateral;
        if (m_InvTime > 0) m_InvTime -= Time.deltaTime;
        if (m_Hp.curhp <= 0) return;

        Dash();

        Time.timeScale = Mathf.Lerp(Time.timeScale, 1, Time.deltaTime * 5);
        for (int i = 0; i < 5; i++)
        {
            if(Physics2D.Raycast(transform.position + offset[i], (Vector3)mRigidbody2D.velocity, m_BoostPower * 2, m_EnemyLayer) && !isHit) Time.timeScale = 0.35f;
        }

        m_isAtk = atkTime > 0;
        if (atkTime > 0)
            atkTime -= Time.deltaTime;

        mSwordAnimator.SetBool("isAttack", atkTime > 0);

        GameManager.Gm.isJoom = m_Count >= 0 && !isSpin;
        m_Ray.SetActive((Input.GetMouseButton(0) || (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)) && m_Count < 0 && power >= resetJoy);
        m_PowerBar.SetActive((Input.GetMouseButton(0) || (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)) && m_Count < 0 && power >= resetJoy);


        if(m_Count >= 0)
        {
            mTimer += Time.deltaTime;
            if(mTimer > m_AfterImageCount)
            {
                if (m_BounceCount >= 3)
                {
                    GameObject afterImage = Instantiate(m_After, transform.position, Quaternion.identity);
                    afterImage.GetComponent<SpriteRenderer>().flipX = mSpriteRenderer.flipX;
                    afterImage.GetComponent<SpriteRenderer>().sprite = mSpriteRenderer.sprite;
                    mTimer = 0;
                }
                if (m_BounceCount >= 10)
                {
                    GameObject afterSpinImage = Instantiate(m_After, transform.position, Quaternion.identity);
                    afterSpinImage.GetComponent<SpriteRenderer>().sprite = m_SpinSpriteRenderer.sprite;
                }
                else if(m_BounceCount >= 7)
                {
                    GameObject afterSwordImage = Instantiate(m_After, transform.position, Quaternion.identity);
                    afterSwordImage.GetComponent<SpriteRenderer>().sprite = m_SwordSpriteRenderer.sprite;
                    afterSwordImage.GetComponent<SpriteRenderer>().flipX = m_SwordSpriteRenderer.flipX;
                }
            }                
        }

        if (m_BounceCount >= 10 && m_Count >= 0)
        {
            if (!m_Spin.activeSelf)
            {
                mAnimator.SetBool("isSpin", true);
                mAnimator.SetTrigger("spin");
                m_Spin.SetActive(true);
            }
            m_Spin.transform.Rotate(0, 0, 2000 * Time.deltaTime);

            Collider2D enemy = Physics2D.OverlapCircle(transform.position, 2, m_EnemyLayer);
            if (enemy)
            {
                CinemachineShake.Instance.ShakeCamera(7, 0.3f);
                enemy.transform.GetComponent<Enemy>().OnDamage();
                enemy.transform.GetComponent<Defence>().DefenceBreak(enemy.transform.GetComponent<Defence>().m_Defence);
                m_BoostPower = 1.75f;
                DOTween.To(() => m_BoostPower, x => m_BoostPower = x, 1f, 0.15f + m_Count * 0.1f).SetEase(Ease.OutQuint);
            }
        }
        else if(m_Spin.activeSelf)
        {
            m_Spin.SetActive(false);
            mAnimator.SetBool("isSpin", false);
        }

    }
    private void FixedUpdate()
    {
        if (isHit || GameManager.Gm.DoSetting) return;
        if (m_Count < 0 || m_Collison.onCollision)
        {
            mRigidbody2D.velocity = Vector2.zero;
            if(!m_isAtk)
            {
                if (m_Collison.onRight) mSpriteRenderer.flipX = true;
                else if (m_Collison.onLeft) mSpriteRenderer.flipX = false;
            }
        }
        else
        {
            if (!m_isAtk)
            {
                if (mRigidbody2D.velocity.x > 0)
                    mSpriteRenderer.flipX = true;
                else if (mRigidbody2D.velocity.x < 0)
                    mSpriteRenderer.flipX = false;
            }
        }
        m_SwordSpriteRenderer.flipX = mSpriteRenderer.flipX;

        if (!isCoroutine && gameObject.layer == 3)
        {
            mRigidbody2D.velocity = saveVelocity * m_BoostPower;
            LastVelocity = saveVelocity;
        }
    }

    private void Dash()
    {
        if (m_Count < 0)
        {
            if (Input.GetMouseButton(0) || (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
            {
                if(!isTouchDown)
                {
                    isTouchDown = true;
                    JoyPanel.gameObject.SetActive(true);
                    JoyStick.gameObject.SetActive(true);
#if (UNITY_EDITOR)
                    startTouchPos = m_MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
#elif (UNITY_ANDROID)
                    startTouchPos = m_MainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
#endif
                }


                SetDistance();
                m_Ray.transform.rotation = Quaternion.Euler(0, 0, m_Angle + 90);
                m_PowerBar.transform.position = transform.position + new Vector3(transform.position.x > 0 ? -0.7f : 0.7f, transform.position.y > 0 ? -0.4f : 0.4f);

                filled = Mathf.Lerp(filled, m_Power / m_MaxPower, Time.deltaTime * 12);
                m_PowerFilled.fillAmount = filled;
                m_PowerText.text = m_Power.ToString();

                JoyPanel.position = startTouchPos;
                JoyStick.position = endTouchPos;

                var inputDir = endTouchPos - startTouchPos;
                var inputMag = inputDir.magnitude;
                var clampedDir = inputMag <= 1.5f ?
                    inputDir : inputDir.normalized * 1.51f;
                m_Ray.transform.localScale = new Vector3(1, (inputMag <= 1.51f ? inputMag : 1.5f) * 2);

                for (int i = 0; i < 5; i++)
                {
                    JoyLiner[i].position = clampedDir * (i + 1) / 5 + startTouchPos;
                }

                JoyStick.position = clampedDir + startTouchPos;
            }
            if (Input.GetMouseButtonUp(0) || touch.phase == TouchPhase.Ended)
            {
                if (power >= resetJoy)
                {
                    SetDistance();
                    m_Count = m_Power;
                    m_BounceCount = 0;

                    mAudioSource.Play();
                    transform.rotation = Quaternion.Euler(0, 0, m_Angle);
                    transform.position += -transform.right * 0.5f;
                    mRigidbody2D.velocity = -transform.right * 17;
                    saveVelocity = -transform.right * 17;
                    transform.rotation = Quaternion.Euler(0, 0, 0);

                    m_BoostPower = 2.25f;
                    DOTween.To(() => m_BoostPower, x => m_BoostPower = x, 1f, 0.4f + m_Power * 0.25f).SetEase(Ease.OutQuint);
                }
                JoyPanel.gameObject.SetActive(false);
                JoyStick.gameObject.SetActive(false);
                isTouchDown = false;
            }
        }
    }
    private void SetDistance()
    {
        #if (UNITY_EDITOR)
        endTouchPos = m_MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        #elif (UNITY_ANDROID)
        endTouchPos = m_MainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
        #endif
        Vector3 offset = startTouchPos - endTouchPos;
        float sqrlen = offset.sqrMagnitude;
        power = Mathf.Sqrt(sqrlen);
        m_Power = (int)((power + 0.5f - resetJoy) * m_MaxPower / 1.5f);
        if (m_Power > m_MaxPower) 
            m_Power = m_MaxPower;

        SetAngle();
        m_Dir = (endTouchPos - startTouchPos).normalized;
    }
    private void SetAngle()
    {
        m_Angle = Mathf.Atan2(endTouchPos.y - startTouchPos.y, endTouchPos.x - startTouchPos.x) * Mathf.Rad2Deg;

        if (m_Collison.onDown && m_Collison.onLeft)
        {
            m_Angle = m_Angle <= -177 && m_Angle >= 0 ? -177 : m_Angle;
            m_Angle = m_Angle >= -93 && m_Angle < 0 ? -93 : m_Angle;
        }
        else if (m_Collison.onDown && m_Collison.onRight)
        {
            m_Angle = m_Angle >= -3 && m_Angle <= 180 ? -3 : m_Angle;
            m_Angle = m_Angle <= -87 && m_Angle > -180 ? -87 : m_Angle;
        }
        else if (m_Collison.onUp && m_Collison.onLeft)
        {
            m_Angle = m_Angle <= 93 && m_Angle >= -90 ? 93 : m_Angle;
            m_Angle = m_Angle >= 177 || m_Angle < -90 ? 177 : m_Angle;
        }
        else if (m_Collison.onUp && m_Collison.onRight)
        {
            m_Angle = m_Angle >= 87 && m_Angle <= -135 ? 87 : m_Angle;
            m_Angle = m_Angle <= 3 && m_Angle > -135 ? 3 : m_Angle;
        }
        else if (m_Collison.onUp)
        {
            m_Angle = m_Angle <= 3 && m_Angle >= -90 ? 3 : m_Angle;
            m_Angle = m_Angle >= 177 || m_Angle < -90 ? 177 : m_Angle;
        }
        else if (m_Collison.onDown)
        {
            m_Angle = m_Angle >= -3 && m_Angle <= 90 ? -3 : m_Angle;
            m_Angle = m_Angle <= -177 || m_Angle > 90 ? -177 : m_Angle;
        }
        else if (m_Collison.onLeft)
        {
            m_Angle = m_Angle <= 93 && m_Angle >= 0 ? 93 : m_Angle;
            m_Angle = m_Angle >= -93 && m_Angle < 0 ? -93 : m_Angle;
        }
        else if (m_Collison.onRight)
        {
            m_Angle = m_Angle >= 87 && m_Angle <= 180 ? 87 : m_Angle;
            m_Angle = m_Angle <= -87 && m_Angle > -180 ? -87 : m_Angle;
        }
    }

    private bool isCoroutine = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy") && m_Count >= 0)
        {
            //Time.timeScale = 0.15f;
            if (!isCoroutine) StartCoroutine(Attack(collision));
        }
        else if (collision.transform.CompareTag("Wall") && m_Count > 0)
        {
            onCol = true;
            mAudioSource.Play();
            var speed = LastVelocity.magnitude;
            var dir = Vector2.Reflect(LastVelocity.normalized, collision.contacts[0].normal);

            mRigidbody2D.velocity = dir * Mathf.Max(speed, 0f);
            saveVelocity = dir * Mathf.Max(speed, 0f);

            ComboPlus();
            m_Count--;
        }
        else if (collision.transform.CompareTag("Wall"))
        {
            foreach (GameObject enemy in GameManager.Gm.m_EnemyArray)
            {
                if (enemy && enemy.GetComponent<EnemyDashSign>())
                    enemy.GetComponent<EnemyDashSign>().AfterDash();
            }
            onCol = true;
            mRigidbody2D.velocity = Vector2.zero;
            saveVelocity = Vector2.zero;
            m_SlopeAngel = Vector2.Angle(collision.contacts[0].normal, Vector2.up);
            m_Count--;
        }
        else if (collision.transform.CompareTag("Damage") && m_Count >= 0)
        {
            //Time.timeScale = 0.5f;
            if (!m_isInv) m_Hp.OnDamage();
            StartCoroutine(InvTime());
            StartCoroutine(Hit(collision.transform));
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall") && mRigidbody2D.velocity == Vector2.zero)
        {
            m_Count = -1;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            onCol = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Item"))
        {
            collision.GetComponent<Item>().UseItem();
        }
    }
    private void ComboPlus()
    {
        SpriteRenderer combo = Instantiate(m_Combo, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();

        combo.sprite = m_ComboSprite[m_BounceCount > 14 ? 14 : m_BounceCount];
        combo.transform.position += new Vector3(transform.position.x > 0 ? -0.75f : 0.75f, transform.position.y > 0 ? -0.7f : 1f);
        combo.transform.localScale = new Vector3(1f + (float)m_BounceCount / 25f, 1f + (float)m_BounceCount / 25f, 1);

        m_BounceCount++;
    }
    private IEnumerator Attack(Collision2D collision)
    {
        Collision2D saveCollision = collision;
        int N = m_BounceCount - collision.transform.GetComponent<Enemy>().m_Power + 1;
        var speed = LastVelocity.magnitude;
        var dir = Vector2.Reflect(LastVelocity.normalized, collision.contacts[0].normal);

        isCoroutine = true;
        if (N >= 0)
        {
            if (N >= 3) GameManager.Gm.m_Score++;
            m_Swing = true;
            atkTime = 0.4f;
            mSpriteRenderer.flipX = saveCollision.transform.position.x > transform.position.x ? true : false;

            for (float i = 0; i < 0.22f; i += Time.deltaTime)
            {
                if (!saveCollision.transform.CompareTag("Enemy")) break;
                mRigidbody2D.velocity = Vector2.zero;
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }            
            if(saveCollision.transform.CompareTag("Enemy"))
            {
                mAnimator.SetTrigger("attack");
                mSwordAudioSource.Play();
                mSwordAnimator.SetTrigger("swing");
                CinemachineShake.Instance.ShakeCamera(5, 0.3f);
                saveCollision.transform.GetComponent<Enemy>().OnDamage();
                saveCollision.transform.GetComponent<Defence>().DefenceBreak(collision.transform.GetComponent<Defence>().m_Defence);
                ComboPlus();
            }

        }
        else
        {
            if (!m_isInv) m_Hp.OnDamage();
            StartCoroutine(InvTime());
            StartCoroutine(Hit(saveCollision.transform));
        }
        mRigidbody2D.velocity = dir * Mathf.Max(speed, 0f);
        saveVelocity = dir * Mathf.Max(speed, 0f);

        m_BoostPower = 1.75f;
        DOTween.To(() => m_BoostPower, x => m_BoostPower = x, 1f, 0.15f + m_Count * 0.1f).SetEase(Ease.OutQuint);
        isCoroutine = false;
    }
    public void takeMirror(Transform target)
    {
        Debug.Log("mirror");
        m_BoostPower = 1.75f;
        m_Count = m_BounceCount;
    }
    private IEnumerator InvTime()
    {
        m_Barrior.SetActive(false);
        m_isInv = true;
        m_InvTime = 0.1f;
        CinemachineShake.Instance.ShakeCamera(2, 0.3f);
        for (int i = 0; i < 2; i++)
        {
            mSpriteRenderer.color = new Color(1, 1, 1, 0.4f);
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
            mSpriteRenderer.color = new Color(1, 1, 1, 0.8f);
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
        mSpriteRenderer.color = new Color(1, 1, 1, 0.4f);
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        mSpriteRenderer.color = new Color(1, 1, 1, 1f);
        m_isInv = false;

        yield return null;
    }
    public void InvItem()
    {
        m_isInv = true;
        mSpriteRenderer.color = new Color(1, 1, 1, 0.5f);
        m_Barrior.SetActive(true);
    }
    bool Onemore = false;
    public IEnumerator Hit(Transform target)
    {
        isHit = true;
        m_Count = -1;
        
        gameObject.layer = 8;

        mAnimator.SetBool("isHit", true);
        mSpriteRenderer.flipX = target.position.x - transform.position.x > 0;
        mRigidbody2D.velocity = Vector2.zero;
        mRigidbody2D.AddForce(new Vector2(target.position.x - transform.position.x > 0 ? -0.5f : 0.5f, 1), ForceMode2D.Impulse);
        mRigidbody2D.gravityScale = 5;
        if(m_Hp.curhp <= 0)
        {
            while (!m_Collison.onDown)
            {
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
        }
        else
        {
            while (!m_Collison.onCollision || !onCol)
            {
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
        }
        mRigidbody2D.gravityScale = 0;
        mAnimator.SetBool("isHit", false);
        isHit = false;

        LastVelocity = Vector2.zero;
        saveVelocity = Vector2.zero;

        gameObject.layer = 3;
        if (m_Hp.curhp <= 0)
        {
            mAnimator.SetBool("isDeath", true);
            if(m_Collison.onDown && !Onemore)
            {
                Onemore = true;
                Fade.instance.Fadein();
                Invoke(nameof(death), 0.5f);
            }
        }
    }
    private void death()
    {
        SceneManager.LoadScene("Death");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startTouchPos, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.3f);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, m_Angle) * transform.forward * 2);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(endTouchPos, 0.2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startTouchPos, endTouchPos);

        Gizmos.color = Color.cyan;
        if(mRigidbody2D != null)
        {
            for(int i = 0; i < 5; i++)
                Gizmos.DrawRay(transform.position + offset[i], (Vector3)mRigidbody2D.velocity * 1f);
        }
    }
}
