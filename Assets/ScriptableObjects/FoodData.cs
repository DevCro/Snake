using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "ScriptableObjects/FoodData", order = 1)]
public class FoodData : ScriptableObject
{
    [SerializeField] private List<FoodType> _myFoodTypes = new List<FoodType>();

    public FoodType GetRandomFoodType()
    {
        int random = UnityEngine.Random.Range(0, 101);

        foreach(FoodType foodType in _myFoodTypes)
        {
            if(foodType.chances >= random)
            {
                return foodType;
            }
        }

        return null;
    }
}
