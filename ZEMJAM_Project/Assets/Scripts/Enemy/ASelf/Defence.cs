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
        defence = enemy.m_Power;

        canvas = GameObject.Find("Defence_Canvas");
        hpBar = Instantiate(hpBarPrf, canvas.transform).GetComponent<RectTransform>();
        hpBar.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.9f, 0));

        SetDefence();
    }

    public void SetDefence()
    {
        for (int i = 0; i < defence; i++)
        {
            defences[i] = hpBar.GetChild(i).gameObject;
            if (!defences[i].activeSelf) defences[i].SetActive(true);

            defences[i].transform.position = DefenceManager.Inst.GetDefPosition(i, defence);
        }

    }
    void Update()
    {
        hpBar.position = transform.position + new Vector3(0, 1);
    }

    /*public void DefencePos()
    {
        for (int i = 0; i < m_Defence; i++)
        {
            if(!defences[i].activeSelf) defences[i].SetActive(true);
        }

        if (m_Defence == 1)
        {
            defences[0].GetComponent<RectTransform>().localPosition = new Vector3(0, -9, 0);
        }
        else if (m_Defence == 2)
        {
            defences[0].GetComponent<RectTransform>().localPosition = new Vector3(setPos, -9, 0);
            defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, -9, 0);
        }
        else if (m_Defence == 3)
        {
            defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*2, -9, 0);
            defences[1].GetComponent<RectTransform>().localPosition = new Vector3(0, -9, 0);
            defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos*2, -9, 0);
        }
        else if (m_Defence == 4)
        {
            defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, -9, 0);
            defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, -9, 0);
            defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos, -9, 0);
            defences[3].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, -9, 0);
        }
        else if (m_Defence == 5)
        {
            defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, 0, 0);
            defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, 0, 0);
            defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos, 0, 0);
            defences[3].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, 0, 0);
            defences[4].GetComponent<RectTransform>().localPosition = new Vector3(0, -18, 0);
        }
        else if (m_Defence == 6)
        {
            defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, 0, 0);
            defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, 0, 0);
            defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos, 0, 0);
            defences[3].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, 0, 0);
            defences[4].GetComponent<RectTransform>().localPosition = new Vector3(setPos, -18, 0);
            defences[5].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, -18, 0);
        }
        else if (m_Defence == 7)
        {
            defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, 0, 0);
            defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, 0, 0);
            defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos, 0, 0);
            defences[3].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, 0, 0);
            defences[4].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*2, -18, 0);
            defences[5].GetComponent<RectTransform>().localPosition = new Vector3(0, -18, 0);
            defences[6].GetComponent<RectTransform>().localPosition = new Vector3(setPos*2, -18, 0);
        }
        else if (m_Defence == 8)
        {
            defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, 0, 0);
            defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, 0, 0);
            defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos, 0, 0);
            defences[3].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, 0, 0);
            defences[4].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, -18, 0);
            defences[5].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, -18, 0);
            defences[6].GetComponent<RectTransform>().localPosition = new Vector3(setPos, -18, 0);
            defences[7].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, -18, 0);
        }
    }    */
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
