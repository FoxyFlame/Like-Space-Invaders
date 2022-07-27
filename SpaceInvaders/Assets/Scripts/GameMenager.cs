using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMenager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Score;
    [SerializeField] TextMeshProUGUI Lives;
    [SerializeField] TextMeshProUGUI FinalScore;
    [SerializeField] TextMeshProUGUI WinLoose;

    [SerializeField] GameObject StartGameInfo;
    [SerializeField] GameObject EndGameInfo;
    [SerializeField] GameObject Bunkers;

    int score;

    public bool gameStarted = false;
    bool newbattle = false;
    bool gameRestart = false;

    BonusShip bonusShip;
    Enemies enemies;

    void Start()
    {
        enemies = FindObjectOfType<Enemies>();
        bonusShip = FindObjectOfType<BonusShip>();
        StartGameInfo.SetActive(true);
        EndGameInfo.SetActive(false);
        Bunkers.SetActive(false);
    }

    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Return) && !gameStarted)
            {
                StartInfoOff();
                gameStarted = true;
                Bunkers.SetActive(true);
                bonusShip.RestartShip();
            }

            if (Input.GetKeyDown(KeyCode.Return) && newbattle)
            {
                newbattle = false;
                StartNextBattle();
            }

            if (Input.GetKeyDown(KeyCode.Return) && gameRestart)
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

    }

    public void Win()
    {
        EndGameInfo.SetActive(true);
        FinalScore.text = "your score: " + score;
        WinLoose.text = "Press Enter if you ready for next battle";
        gameStarted = false;
        newbattle = true;
    }

    void StartNextBattle()
    {
        CancelInvoke();
        bonusShip.RestartShip();
        EndGameInfo.SetActive(false);
        enemies.startedPosition -= new Vector3(0f, 4f, 0f);
        StartCoroutine(enemies.PrepareToBattle(enemies.prepareTime));
    }

    public void Loose()
    {
        EndGameInfo.SetActive(true);
        FinalScore.text = "your score: " + score;
        WinLoose.text = "Game over     press enter too restart";
        gameStarted = false;
        gameRestart = true;
        enemies.CancelAllInvoke();
        Time.timeScale = 0.0f;
    }

    public void SetScore(int priceAmount)
    {
        score += priceAmount;
        Score.text = "score: " + score;
    }

    public void SetLives(int lives)
    {
        Lives.text = "lives: " + lives;
    }

    void StartInfoOff()
    {
        StartGameInfo.SetActive(false);
    }
}
