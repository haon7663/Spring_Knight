using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] Transform player;

    [Header("Stats")]
    public int maxhp;
    public int curhp;

    [Header("Health UI")]
    [SerializeField] RectTransform healthPrefab;
    [SerializeField] Transform helathParent;
    [SerializeField] Image[] healths;

    [SerializeField] Sprite disabledHeart;
    [SerializeField] Sprite abledHeart;

    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        helathParent.position = mainCamera.WorldToScreenPoint(player.position + new Vector3(0, 1));
    }
    public void SetHealth(int maxhp)
    {
        this.maxhp = maxhp;
        healths = new Image[maxhp];
        for (int i = 0; i < maxhp; i++)
        {
            var health = healths[i] == null ? Instantiate(healthPrefab, helathParent) : healths[i].GetComponentInParent<RectTransform>();
            health.localPosition = new Vector2(((maxhp-1) * -30) + (60 * i), 0);
            healths[i] = health.GetChild(0).GetComponent<Image>();
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
