using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

        m_passenger.SetActive(false);

        for (int i = 0; i < m_swimmers.Length; i++)
        {
            m_swimmers[i].SetActive(false);
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

        if (m_startTimer < 0)
        {
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
        }
        else
        {
            m_gameTime += Time.deltaTime;
        }
    }

    private void GameStateGameOver()
    {
        if (Input.GetKeyUp(KeyCode.Return) == true) 
        {
            m_GameState = GameState.Start;
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
}
