using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public HighScores m_HighScores; // Component for managing high scores
    public TextMeshProUGUI m_MessageText; // UI text for messages
    public TextMeshProUGUI m_TimerText; // UI text for timer

    public GameObject m_HighScorePanel; // Panel for displaying high scores
    public TextMeshProUGUI m_HighScoresText; // Text display for high scores

    public Button m_NewGameButton; // Button to start a new game
    public Button m_HighScoresButton; // Button to show high scores

    public GameObject[] m_swimmers; // Array of swimmer objects
    public GameObject m_passenger; // Passenger object

    private float m_startTimer = 3; // Start countdown timer
    private float m_gameTime = 0; // Elapsed game time
    public float GameTime { get { return m_gameTime; } } // Public getter for game time

    public enum GameState // Game state definitions
    {
        Start,
        Playing,
        GameOver
    };

    private GameState m_GameState; // Current game state
    public GameState State { get { return m_GameState; } } // Public getter for game state

    void Start()
    {
        m_GameState = GameState.GameOver; // Set initial game state to GameOver

        m_TimerText.text = ""; // Clear timer text
        m_MessageText.text = "Press Enter to Start A Chads Life"; // Display start message

        m_passenger.SetActive(false); // Deactivate passenger

        for (int i = 0; i < m_swimmers.Length; i++) // Activate all swimmers
        {
            m_swimmers[i].SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit(); // Quit application on Escape key
        }
        switch (m_GameState)
        {
            case GameState.Start:
                GameStateStart(); // Handle Start state
                break;
            case GameState.Playing:
                GameStatePlaying(); // Handle Playing state
                break;
            case GameState.GameOver:
                GameStateGameOver(); // Handle GameOver state
                break;
        }
    }

    private void GameStateStart()
    {
        m_startTimer -= Time.deltaTime; // Decrement start timer

        m_MessageText.text = "Get Ready " + (int)(m_startTimer + 1); // Update start message

        if (m_startTimer < 0)
        {
            m_MessageText.text = ""; // Clear message text
            m_gameTime = 0; // Reset game time
            m_startTimer = 3; // Reset start timer
            m_GameState = GameState.Playing; // Change state to Playing

            m_passenger.SetActive(false); // Deactivate passenger

            for (int i = 0; i < m_swimmers.Length; i++) // Activate all swimmers
            {
                m_swimmers[i].SetActive(true);
            }
        }
    }

    private void GameStatePlaying()
    {
        if (AllRescued() == true) // Check if all are rescued
        {
            m_GameState = GameState.GameOver; // Set state to GameOver
            m_MessageText.text = "Winner"; // Display winner message
            m_HighScores.AddScore(Mathf.RoundToInt(m_gameTime)); // Add current time to scores
            m_HighScores.SaveScoresToFile(); // Save scores to file

            m_NewGameButton.gameObject.SetActive(true); // Show new game button
            m_HighScoresButton.gameObject.SetActive(true); // Show high scores button
        }
        else
        {
            m_gameTime += Time.deltaTime; // Increment game time
            int seconds = Mathf.RoundToInt(m_gameTime); // Convert time to seconds

            m_TimerText.text = string.Format("{0:D2}:{1:D2}",
                (seconds / 60), (seconds % 60)); // Update timer display
        }
    }

    private void GameStateGameOver()
    {
        if (Input.GetKeyUp(KeyCode.Return) == true)
        {
            m_GameState = GameState.Start; // Reset game state to Start
            m_HighScorePanel.gameObject.SetActive(false); // Hide high score panel
            m_NewGameButton.gameObject.SetActive(false); // Hide new game button
            m_HighScoresButton.gameObject.SetActive(false); // Hide high scores button
        }
    }

    private bool AllRescued()
    {
        int numRescuedLeft = 0; // Counter for active swimmers

        for (int i = 0; i < m_swimmers.Length; ++i)
        {
            if (m_swimmers[i].activeSelf == true)
            {
                numRescuedLeft++; // Increment if swimmer is active
            }
        }
        return numRescuedLeft == 0 && !m_passenger.activeSelf; // Return true if no swimmers left and no active passenger
    }

    public void OnNewGame()
    {
        m_GameState = GameState.Start; // Set state to Start
        m_HighScorePanel.gameObject.SetActive(false); // Hide high score panel
        m_NewGameButton.gameObject.SetActive(false); // Hide new game button
        m_HighScoresButton.gameObject.SetActive(false); // Hide high scores button
    }

    public void OnHighScores()
    {
        m_MessageText.text = ""; // Clear message text

        m_HighScoresButton.gameObject.SetActive(false); // Hide high scores button
        m_HighScorePanel.SetActive(true); // Show high score panel

        string text = ""; // Initialize score display text
        for (int i = 0; i < m_HighScores.scores.Length; i++)
        {
            int seconds = m_HighScores.scores[i];
            text += string.Format("{0:D2}:{1:D2}\n",
                             (seconds / 60), (seconds % 60)); // Format and add each score
        }
        m_HighScoresText.text = text; // Display formatted scores
    }
}
