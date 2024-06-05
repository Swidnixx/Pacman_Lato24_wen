using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    private int lives = 3;
    private int score = 0;

    public int Lives => lives;
    public int Score => score;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        NewGame();
    }


    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;

        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        // reset pacman state
        pacman.ResetState();

        foreach(Ghost g in ghosts)
        {
            g.ResetState();
        }
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void PelletEaten(Pellet pellet)
    {
        SetScore(++score);

        if(!PelletsLeft())
        {
            //win
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3);
        }
    }

    bool PelletsLeft()
    {
        int childer = pellets.childCount;
        for(int i=0; i<childer; i++)
        {
            Transform child = pellets.GetChild(i);
            if (child.gameObject.activeSelf)
                return true;
        }
        return false;
    }

    public void PacmanEaten()
    {
        pacman.gameObject.SetActive(false);
        SetLives(lives - 1);
        if (lives > 0)
            Invoke(nameof(ResetState), 3);
        else
            GameOver();
    }

    void GameOver()
    {
        foreach(Ghost g in ghosts)
        {
            g.gameObject.SetActive(false);
        }
        gameOverText.gameObject.SetActive(true);
        gameOverText.enabled = true;
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
            NewGame();
    }
}
