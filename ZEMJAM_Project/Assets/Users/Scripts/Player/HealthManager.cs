using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Inst { get; set; }
    void Awake() => Inst = this;

    [Header("Stats")]
    public int maxhp;
    public int curhp;

    [Header("Health UI")]
    public GameObject healthPrefab;
    public Transform helathParent;
    public Image[] healths;

    public Sprite disabledHeart;
    public Sprite abledHeart;

    public void SetHealth(int maxhp)
    {
        this.maxhp = maxhp;

        healths = new Image[maxhp];
        for (int i = 0; i < maxhp; i++)
        {
            var health = Instantiate(healthPrefab).GetComponent<RectTransform>();
            health.SetParent(helathParent);
            health.localPosition = new Vector2(12.5f + i * 100, -12.5f);
            healths[i] = health.GetComponent<Image>();
        }
    }
    public void OnDamage()
    {
        curhp--;
        if (curhp < 0) curhp = 0;
        for (int i = 0; i < maxhp; i++)
        {
            healths[i].sprite = disabledHeart;
        }
        for (int i = 0; i < curhp; i++)
        {
            healths[i].sprite = abledHeart;
        }
    }
    public void OnHealth(int health = 0)
    {
        curhp += health;
        if (curhp > maxhp) curhp = maxhp;
        for (int i = 0; i < maxhp; i++)
        {
            healths[i].sprite = disabledHeart;
        }
        for (int i = 0; i < curhp; i++)
        {
            healths[i].sprite = abledHeart;
        }
    }
}
