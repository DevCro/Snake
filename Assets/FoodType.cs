using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodTypeEnum { Increase, Decrease, SlowDown, SpeedUp, Reverse };

[Serializable]
public class FoodType
{
    public FoodTypeEnum foodTypeEnum;
    public int timeDuration;
    public int chances;
}
