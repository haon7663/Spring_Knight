using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] Image[] healthParents;

    [SerializeField] Sprite disabledHeart;
    [SerializeField] Sprite abledHeart;

    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        helathParent.position = (player.position + new Vector3(0, player.position.y > 4.3f ? -1 : 1));
    }
    public void SetHealth(int maxhp)
    {
        this.maxhp = maxhp;
        healthParents = new Image[maxhp];
        healths = new Image[maxhp];
        for (int i = 0; i < maxhp; i++)
        {
            var health = healths[i] == null ? Instantiate(healthPrefab, helathParent) : healths[i].GetComponentInParent<RectTransform>();
            health.localPosition = new Vector2(((maxhp-1) * -30) + (60 * i), 0);
            healthParents[i] = health.GetComponent<Image>();
            healths[i] = health.GetChild(0).GetComponent<Image>();
        }
    }
    public void OnFade(bool isFadeIn)
    {
        for (int i = 0; i < maxhp; i++)
        {
            healths[i].DOFade(isFadeIn ? 1 : 0, 0.25f);
            healthParents[i].DOFade(isFadeIn ? 1 : 0, 0.25f);
        }
    }
    public void OnDamage(int damage = -1)
    {
        ManageHealth(damage);
        StartCoroutine(CameraEffect.Inst.OnDamage());
    }
    public void OnHealth(int health = 0)
    {
        ManageHealth(health);
    }

    void ManageHealth(int value)
    {
        curhp += value;
        if (curhp > maxhp) curhp = maxhp;
        for (int i = 0; i < maxhp; i++)
        {
            healths[i].sprite = disabledHeart;
        }
        for (int i = 0; i < curhp; i++)
        {
            healths[i].sprite = abledHeart;
        }
        OnFade(true);
    }
}
