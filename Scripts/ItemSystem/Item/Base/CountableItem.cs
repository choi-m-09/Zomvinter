using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ������ �� �� �ִ� ������ </summary>
public abstract class CountableItem : Item
{
    public CountableItemData CountableData { get; set; }

    /// <summary> ���� ������ ���� </summary>

    private int amount;
    public int Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    /// <summary> 해당 오브젝트 최대 개수 값 리턴 </summary>
    public int MaxAmount => CountableData.MaxAmount;

    /// <summary> 최대 개수인지 체크 </summary>
    public bool IsMax => Amount >= CountableData.MaxAmount;

    /// <summary> 수량 0인지 검사 </summary>
    public bool IsEmpty => Amount <= 0;

    public CountableItem(CountableItemData data, int amount = 1) : base(data)
    {
        CountableData = data;
        SetAmount(amount);
    }

    private void Start()
    {
        SetAmount(amount);
    }

    /// <summary> ���� ����(���� ����) </summary>
    public void SetAmount(int amount)
    {
        Amount = Mathf.Clamp(amount, 0, MaxAmount);
    }
}
