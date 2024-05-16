using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GagueControl : MonoBehaviour
{
    public Slider hpBar;
    public Slider staminaBar;
    public Slider HungerBar;
    public Slider ThirstBar;

    Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HPGagueApply(_player.Stat.HP, _player.Stat.MaxHP);
        StaminaGagueApply(_player.Stat.Stamina, _player.Stat.MaxStamina);
        HungryGagueApply(_player.Stat.Hunger, _player.Stat.MaxHunger);
        ThirstGagueApply(_player.Stat.Thirsty, _player.Stat.MaxThirsty);
    }

    private void HPGagueApply(float minStat, float MaxStat)
    {
        hpBar.value = minStat / MaxStat;
    }
    private void StaminaGagueApply(float minStat, float MaxStat)
    {
        staminaBar.value = minStat / MaxStat;
    }
    private void HungryGagueApply(float minStat, float MaxStat)
    {
        HungerBar.value = minStat / MaxStat;
    }
    private void ThirstGagueApply(float minStat, float MaxStat)
    {
        ThirstBar.value = minStat / MaxStat;
    }
}
