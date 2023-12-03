using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public FoodType CurrenFoodType { get { return _currentFoodType; } }

    [SerializeField] private FoodData _foodData;
    
    private Bounds _gridBounds;
    private FoodType _currentFoodType;

    private void Start()
    {
        _gridBounds = GameManager.Instance.GridBounds;

        RandomizePosition();
    }

    private void RandomizePosition()
    {
        float x = Mathf.Round(Random.Range(_gridBounds.min.x, _gridBounds.max.x));
        float y = Mathf.Round(Random.Range(_gridBounds.min.y, _gridBounds.max.y));

        transform.position = new Vector3(x, y, 0);

        _currentFoodType = _foodData.GetRandomFoodType();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RandomizePosition();
    }
}
