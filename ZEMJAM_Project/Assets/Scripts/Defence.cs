using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defence : MonoBehaviour
{
    public int m_Defence;

    public GameObject[] Defences = new GameObject[8];

    public GameObject PrfHpBar;
    public GameObject m_HitParticle;
    private RectTransform hpBar;
    private GameObject canvas;

    public Vector3 DefPos = new Vector3(0, 0.9f, 0);

    private Enemy m_Enemy;

    private int setPos = 18;

    private void Start()
    {
        m_Enemy = GetComponent<Enemy>();
        m_Defence = m_Enemy.m_Power;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        hpBar = Instantiate(PrfHpBar, canvas.transform).GetComponent<RectTransform>();
        hpBar.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.9f, 0));
        for(int i = 0; i < m_Defence; i++)
        {
            Defences[i] = hpBar.GetChild(i).gameObject;
        }
        for (int i = 0; i < m_Defence; i++)
        {
            Defences[i].SetActive(true);
        }

        defencePos();
    }
    private void Update()
    {
        hpBar.position = Camera.main.WorldToScreenPoint(transform.position + DefPos);
    }

    private void defencePos()
    {
        if (m_Defence == 1)
        {
            Defences[0].GetComponent<RectTransform>().localPosition = new Vector3(0, -9, 0);
        }
        else if (m_Defence == 2)
        {
            Defences[0].GetComponent<RectTransform>().localPosition = new Vector3(setPos, -9, 0);
            Defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, -9, 0);
        }
        else if (m_Defence == 3)
        {
            Defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*2, -9, 0);
            Defences[1].GetComponent<RectTransform>().localPosition = new Vector3(0, -9, 0);
            Defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos*2, -9, 0);
        }
        else if (m_Defence == 4)
        {
            Defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, -9, 0);
            Defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, -9, 0);
            Defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos, -9, 0);
            Defences[3].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, -9, 0);
        }
        else if (m_Defence == 5)
        {
            Defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, 0, 0);
            Defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, 0, 0);
            Defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos, 0, 0);
            Defences[3].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, 0, 0);
            Defences[4].GetComponent<RectTransform>().localPosition = new Vector3(0, -18, 0);
        }
        else if (m_Defence == 6)
        {
            Defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, 0, 0);
            Defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, 0, 0);
            Defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos, 0, 0);
            Defences[3].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, 0, 0);
            Defences[4].GetComponent<RectTransform>().localPosition = new Vector3(setPos, -18, 0);
            Defences[5].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, -18, 0);
        }
        else if (m_Defence == 7)
        {
            Defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, 0, 0);
            Defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, 0, 0);
            Defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos, 0, 0);
            Defences[3].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, 0, 0);
            Defences[4].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*2, -18, 0);
            Defences[5].GetComponent<RectTransform>().localPosition = new Vector3(0, -18, 0);
            Defences[6].GetComponent<RectTransform>().localPosition = new Vector3(setPos*2, -18, 0);
        }
        else if (m_Defence == 8)
        {
            Defences[0].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, 0, 0);
            Defences[1].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, 0, 0);
            Defences[2].GetComponent<RectTransform>().localPosition = new Vector3(setPos, 0, 0);
            Defences[3].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, 0, 0);
            Defences[4].GetComponent<RectTransform>().localPosition = new Vector3(-setPos*3, -18, 0);
            Defences[5].GetComponent<RectTransform>().localPosition = new Vector3(-setPos, -18, 0);
            Defences[6].GetComponent<RectTransform>().localPosition = new Vector3(setPos, -18, 0);
            Defences[7].GetComponent<RectTransform>().localPosition = new Vector3(setPos*3, -18, 0);
        }
    }    
    public void DefenceBreak(int dam)
    {
        Instantiate(m_HitParticle, transform.position, Quaternion.identity);
        int saveDefence = m_Defence;
        m_Defence -= dam;
        for (int i = m_Defence; i < saveDefence; i++)
        {
            Defences[i].SetActive(false);
        }
    }
}
