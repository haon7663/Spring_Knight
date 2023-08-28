using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Defence : MonoBehaviour
{
    public int defence;

    public GameObject[] defences = new GameObject[8];

    public GameObject hpBarPrf;
    public GameObject hitParticle;

    RectTransform hpBar;
    GameObject canvas;
    Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        defence = enemy.defence;

        hpBar = Instantiate(hpBarPrf, DefenceManager.Inst.Canvas).GetComponent<RectTransform>();

        SetDefence();
    }

    public void SetDefence()
    {
        for (int i = 0; i < defence; i++)
        {
            defences[i] = hpBar.GetChild(i).gameObject;
            if (!defences[i].activeSelf) defences[i].SetActive(true);

            var setPosition = DefenceManager.Inst.GetDefPosition(i, defence) * new Vector2(18, 9);
            defences[i].GetComponent<RectTransform>().localPosition = setPosition;
        }

    }
    void LateUpdate()
    {
        hpBar.position = transform.position + new Vector3(0, 1);
    }

    public void DefenceBreak(int dam)
    {
        Instantiate(hitParticle, transform.position, Quaternion.identity);
        int saveDefence = defence;
        defence -= dam;
        for (int i = defence; i < saveDefence; i++)
        {
            defences[i].SetActive(false);
        }
    }
}
