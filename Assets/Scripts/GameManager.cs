using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Bounds GridBounds { get { return _gridBounds.bounds; } }

    [SerializeField] private BoxCollider2D _gridBounds;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void ResetScore()
    {
        _score = 0;
        PrintScore();
    }

    public void AddScore()
    {
        _score += 1;
        PrintScore();
    }

    private void PrintScore()
    {
        _scoreText.text = "Score: " + _score;
    }

}
