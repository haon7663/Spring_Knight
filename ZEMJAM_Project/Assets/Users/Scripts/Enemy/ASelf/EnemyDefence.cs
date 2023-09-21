using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDefence : MonoBehaviour
{
    EnemyBundle m_EnemyBundle;

    public int index;
    public int defence;
    [SerializeField] float defPositionY;
    [SerializeField] int minDefence;
    [SerializeField] int maxDefence;
    [SerializeField] RectTransform defenceParent;
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
        defenceBar = Instantiate(defenceParent, DefenceManager.Inst.defenceBundle);

        if (maxDefence == 0) defence = minDefence;
        else defence = Random.Range(minDefence, maxDefence + 1);
        SetDefence();
    }

    public void SetDefence()
    {
        var def = defence - 1;
        var perDef = def > 4 ? 4 : def;
        for (int i = 0; i < perDef; i++)
        {
            defences[i] = defenceBar.GetChild(i).gameObject;
            if (!defences[i].activeSelf) defences[i].SetActive(true);

            var setPosition = DefenceManager.Inst.GetDefPosition(i, def) * 20 + new Vector2(0, defPositionY);
            if(perDef == 4)
                defences[i].GetComponent<Image>().sprite = DefenceManager.Inst.GetDefSprite(defence - i - 2);
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

    public void OnDamage(Transform target, Vector3 velocity)
    {
        if (GetComponent<EnemyDestroy>()) GetComponent<EnemyDestroy>().AfterDestroy();

        TileManager.Inst.TakeTile(index, false);
        SummonManager.Inst.RemoveEnemy(gameObject);
        Destroy(defenceBar.gameObject);
        if (TryGetComponent(out DemensionDestroyer demension))
            demension.PortalDestroy(0.15f);

        StartCoroutine(m_EnemyBundle.rigid.BouncedOff(target, Vector3.Magnitude(velocity)));
    }
}
