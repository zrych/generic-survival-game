using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image hpBar;
    public Image hungerBar;

    private PlayerStats stats;

    void Start()
    {
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        hpBar.fillAmount = stats.currentHP / stats.maxHP;
        hungerBar.fillAmount = stats.currentHunger / stats.maxHunger;
    }
}
