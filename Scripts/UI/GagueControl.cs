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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HPGagueApply(Player.Stat.HP, Player.Stat.MaxHP);
        StaminaGagueApply(Player.Stat.Stamina, Player.Stat.MaxStamina);
        HungryGagueApply(Player.Stat.Hunger, Player.Stat.MaxHunger);
        ThirstGagueApply(Player.Stat.Thirsty, Player.Stat.MaxThirsty);
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
