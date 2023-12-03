using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private int _initialSize = 3;
    [SerializeField] private float _slowMovementSpeed = 0.06f;
    [SerializeField] private float _normalMovementSpeed = 0.05f;
    [SerializeField] private float _fastMovementSpeed = 0.04f;
    [SerializeField] private Transform _partPrefab;

    private List<Transform> _parts = new List<Transform>();
    private Vector3 _startPosition;
    private Vector2Int _currentDirection;
    private Vector2Int _inputDirection;
    private Bounds _gridBounds;
    private Coroutine _speedChangeCoroutine;
    private float _movementTimer;
    private float _movementSpeed;

    private void Start()
    {
        _startPosition = transform.position;
        _gridBounds = GameManager.Instance.GridBounds;

        RestartGame();
    }

    private void Update()
    {
        CheckInput();

        _movementTimer += Time.deltaTime;

        if (_movementTimer >= _movementSpeed)
        {
            _movementTimer = 0;
            SetNewPosition();
        }
    }


    private void RestartGame()
    {
        foreach (Transform part in _parts)
        {
            if (part != transform)
            {
                Destroy(part.gameObject);
            }
        }

        _parts.Clear();
        _parts.Add(transform);

        transform.position = _startPosition;

        for (int i = 1; i < _initialSize; i++)
        {
            AddPart();
        }

        _inputDirection = Vector2Int.right;
        _currentDirection = Vector2Int.right;
        SetMovementSpeed(_normalMovementSpeed);

        GameManager.Instance.ResetScore();
    }

    private void CheckInput()
    {
        if (_currentDirection.x != 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _inputDirection = Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                _inputDirection = Vector2Int.down;
            }
        }
        else if (_currentDirection.y != 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _inputDirection = Vector2Int.left;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _inputDirection = Vector2Int.right;
            }
        }
    }

    private void SetMovementSpeed(float movementSpeed)
    {
        _movementSpeed = movementSpeed;
    }

    private void SetNewPosition()
    {
        _currentDirection = _inputDirection;

        //Set snake parts
        for (int i = _parts.Count - 1; i > 0; i--)
        {
            _parts[i].position = _parts[i - 1].position;
        }

        //Set snake head
        float newX = Mathf.Round(transform.position.x) + _currentDirection.x;
        float newY = Mathf.Round(transform.position.y) + _currentDirection.y;

        if (newX < _gridBounds.min.x)
        {
            newX = _gridBounds.max.x;
        }
        else if (newX > _gridBounds.max.x)
        {
            newX = _gridBounds.min.x;
        }
        else if (newY < _gridBounds.min.y)
        {
            newY = _gridBounds.max.y;
        }
        else if (newY > _gridBounds.max.y)
        {
            newY = _gridBounds.min.y;
        }


        transform.position = new Vector3(newX, newY, 0);
    }

    private void AddPart()
    {
        Transform newPart = Instantiate(_partPrefab);
        newPart.position = _parts[_parts.Count - 1].position;

        _parts.Add(newPart);
    }

    private void RemovePart()
    {
        if (_parts.Count > 1)
        {
            Destroy(_parts[_parts.Count - 1].gameObject);
            _parts.RemoveAt(_parts.Count - 1);
        }
        else
        {
            RestartGame();
        }
    }

    private IEnumerator ChangeSpeed(FoodType foodType)
    {
        if (foodType.foodTypeEnum == FoodTypeEnum.SlowDown)
        {
            SetMovementSpeed(_slowMovementSpeed);
        }
        else if (foodType.foodTypeEnum == FoodTypeEnum.SpeedUp)
        {
            SetMovementSpeed(_fastMovementSpeed);
        }

        yield return new WaitForSeconds(foodType.timeDuration);

        SetMovementSpeed(_normalMovementSpeed);
    }

    private void Reverse()
    {
        Vector3 reversedDir = (_parts[_parts.Count - 1].position - _parts[_parts.Count - 2].position).normalized;

        _currentDirection = Vector2Int.RoundToInt(reversedDir);
        _inputDirection = _currentDirection;

        for (int i = 0; i < _parts.Count; i++)
        {
            if (i >= _parts.Count - 1 - i)
            {
                break;
            }

            (_parts[i].position, _parts[_parts.Count - 1 - i].position) = (_parts[_parts.Count - 1 - i].position, _parts[i].position);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Food food = collision.GetComponent<Food>();
        if (food)
        {
            GameManager.Instance.AddScore();

            if (food.CurrenFoodType.foodTypeEnum == FoodTypeEnum.Increase)
            {
                AddPart();
            }
            else if (food.CurrenFoodType.foodTypeEnum == FoodTypeEnum.Decrease)
            {
                RemovePart();
            }
            else if (food.CurrenFoodType.foodTypeEnum == FoodTypeEnum.SlowDown || food.CurrenFoodType.foodTypeEnum == FoodTypeEnum.SpeedUp)
            {
                if (_speedChangeCoroutine != null)
                {
                    StopCoroutine(_speedChangeCoroutine);
                }
                _speedChangeCoroutine = StartCoroutine(ChangeSpeed(food.CurrenFoodType));
            }
            else if (food.CurrenFoodType.foodTypeEnum == FoodTypeEnum.Reverse)
            {
                Reverse();
            }
            else
            {
                Debug.Log("Food type not recognised!");
            }
        }
        else
        {
            if (_parts.Count > 4)
            {
                RestartGame();
            }
        }
    }
}
