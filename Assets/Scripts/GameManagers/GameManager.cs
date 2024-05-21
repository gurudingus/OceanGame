using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public HighScores m_HighScores;
    public TextMeshProUGUI m_MessageText;
    public TextMeshProUGUI m_TimerText;

    public GameObject m_HighScorePanel;
    public TextMeshProUGUI m_HighScoresText;

    public Button m_NewGameButton;
    public Button m_HighScoresButton;

    public GameObject[] m_swimmers;
    public GameObject m_passenger;

    private float m_startTimer = 3;
    private float m_gameTime = 0;
    public float GameTime {  get { return m_gameTime; } }

    public enum GameState
    {
        Start,
        Playing,
        GameOver
    };

    private GameState m_GameState;
    public GameState State {  get { return m_GameState; } }

    void Start()
    {
        m_GameState = GameState.GameOver;

        m_TimerText.text = "";
        m_MessageText.text = "Press Enter to Start A Chads Life";

        m_passenger.SetActive(false);

        for (int i = 0; i < m_swimmers.Length; i++)
        {
            m_swimmers[i].SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        } 
        switch(m_GameState)
        {
            case GameState.Start:
                GameStateStart();
                break;
            case GameState.Playing:
                GameStatePlaying(); 
                break;
            case GameState.GameOver:
                GameStateGameOver();
                break;
        }
    }

    private void GameStateStart()
    {
        m_startTimer -= Time.deltaTime;

        m_MessageText.text = "Get Ready " + (int)(m_startTimer + 1);

        if (m_startTimer < 0)
        {
            m_MessageText.text = "";
            m_gameTime = 0;
            m_startTimer = 3;
            m_GameState = GameState.Playing;

            m_passenger.SetActive(false);

            for (int i = 0; i < m_swimmers.Length; i++)
            {
                m_swimmers[i].SetActive(true);
            }
        }
    }

    private void GameStatePlaying()
    {
        if (AllRescued() == true)
        {
            m_GameState = GameState.GameOver;
            m_MessageText.text = "Winner";
            m_HighScores.AddScore(Mathf.RoundToInt(m_gameTime));
            m_HighScores.SaveScoresToFile();

            m_NewGameButton.gameObject.SetActive(true);
            m_HighScoresButton.gameObject.SetActive(true);
        }
        else
        {
            m_gameTime += Time.deltaTime;
            int seconds = Mathf.RoundToInt(m_gameTime);

            m_TimerText.text = string.Format("{0:D2}:{1:D2}",
                (seconds / 60), (seconds % 60));
        }
    }

    private void GameStateGameOver()
    {
        if (Input.GetKeyUp(KeyCode.Return) == true) 
        {
            m_GameState = GameState.Start;
            m_HighScorePanel.gameObject.SetActive(false);
            m_NewGameButton.gameObject.SetActive(false);
            m_HighScoresButton.gameObject.SetActive(false);
        }
    }

    private bool AllRescued()
    {
        int numRescuedLeft = 0;

        for (int i = 0; i < m_swimmers.Length; ++i)
        {
            if (m_swimmers[i].activeSelf == true)
            {
                numRescuedLeft++;
            }
        }
        if (numRescuedLeft <= 0 && m_passenger.activeSelf == false)
            return true;
        return false;
    }

    public void OnNewGame()
    {
        m_GameState = GameState.Start;
        m_HighScorePanel.gameObject.SetActive(false);
        m_NewGameButton.gameObject.SetActive(false);
        m_HighScoresButton.gameObject.SetActive(false);
    }

    public void OnHighScores()
    {
        m_MessageText.text = "";

        m_HighScoresButton.gameObject.SetActive(false);
        m_HighScorePanel.SetActive(true);

        string text = "";
        for (int i = 0; i < m_HighScores.scores.Length; i++)
        {
            int seconds = m_HighScores.scores[i];
            text += string.Format("{0:D2}:{1:D2}\n",
                             (seconds / 60), (seconds % 60));
        }
        m_HighScoresText.text = text;
    }
}
