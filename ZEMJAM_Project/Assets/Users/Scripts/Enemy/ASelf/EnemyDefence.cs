using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefence : MonoBehaviour
{
    EnemyBundle m_EnemyBundle;

    public int defence;
    [SerializeField] int minDefence;
    [SerializeField] int maxDefence;
    [SerializeField] GameObject defenceParent;
    [SerializeField] GameObject hitParticle;

    RectTransform defenceBar;
    GameObject[] defences = new GameObject[8];

    void Awake()
    {
        if (TryGetComponent(out EnemyBundle bundle))
            m_EnemyBundle = bundle;
    }

    void Start()
    {
        defenceBar = Instantiate(defenceParent, DefenceManager.Inst.Canvas).GetComponent<RectTransform>();

        if (maxDefence == 0) defence = minDefence;
        else defence = Random.Range(minDefence, maxDefence + 1);
        SetDefence();
    }

    public void SetDefence()
    {
        for (int i = 0; i < defence; i++)
        {
            defences[i] = defenceBar.GetChild(i).gameObject;
            if (!defences[i].activeSelf) defences[i].SetActive(true);

            var setPosition = DefenceManager.Inst.GetDefPosition(i, defence) * new Vector2(18, 9);
            defences[i].GetComponent<RectTransform>().localPosition = setPosition;
        }

    }
    void LateUpdate()
    {
        if(defenceBar != null)
            defenceBar.position = transform.position + new Vector3(0, 1);
    }

    public int AttemptAttack(int damage)
    {
        return damage - defence;
    }

    public void OnDamage(Vector3 velocity)
    {
        if (GetComponent<EnemyDestroy>()) GetComponent<EnemyDestroy>().AfterDestroy();

        SummonManager.Inst.RemoveEnemy(gameObject);
        Destroy(defenceBar.gameObject);

        StartCoroutine(m_EnemyBundle.rigid.BouncedOff(velocity));
    }
}
