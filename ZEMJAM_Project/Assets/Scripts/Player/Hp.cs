using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    public int maxhp;
    public int curhp;

    public Image[] HPBar;

    public Sprite mBHeart;
    public Sprite mHeart;

    private void Start()
    {
        curhp = maxhp;
    }
    public void OnDamage()
    {
        curhp--;
        if (curhp < 0) curhp = 0;
        for (int i = 0; i < 3; i++)
        {
            HPBar[i].sprite = mBHeart;
        }
        for (int i = 0; i < curhp; i++)
        {
            HPBar[i].sprite = mHeart;
        }
    }
    public void OnHealth()
    {
        curhp++;
        if (curhp > maxhp) curhp = maxhp;
        for (int i = 0; i < 3; i++)
        {
            HPBar[i].sprite = mBHeart;
        }
        for (int i = 0; i < curhp; i++)
        {
            HPBar[i].sprite = mHeart;
        }
    }
}
