using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary> ���� ������ - ���� ������ </summary>
public class PotionItem : CountableItem, IUsableItem
{
    public PotionItemData PotionData;
    public PotionItem(PotionItemData data, int amount = 1) : base(data, amount) {}

    public bool Use(Player _player)
    {
        switch (PotionData.ID)
        {
            case 10007:
                if (_player.Stat.HP < _player.Stat.MaxHP)
                {
                    Amount--;
                    _player.Stat.HP += PotionData.Value;
                    return true;
                }
                break;
            case 10008:
                if (_player.Stat.Hunger < _player.Stat.MaxHunger)
                {
                    Amount--;
                    _player.Stat.Hunger += PotionData.Value;
                    return true;
                }
                break;
            case 10009:
                if(_player.Stat.Thirsty < _player.Stat.MaxThirsty)
                {
                    Amount--;
                    _player.Stat.Thirsty += PotionData.Value;
                }
                break;
        }
        return false;
    }

    private void Awake()
    {
        base.CountableData = PotionData;
    }
}
