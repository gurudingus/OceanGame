using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighScores : MonoBehaviour
{
    public int[] scores = new int[10]; // Array to store top 10 scores

    string currentDirectory; // Stores the current directory path

    public string scoreFileName = "highscores.txt"; // Filename for storing scores

    void Start()
    {
        currentDirectory = Application.dataPath; // Set current directory to application data path
        LoadScoresFromFile(); // Load scores from file
    }

    void Update()
    {
    }

    public void LoadScoresFromFile()
    {
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName); // Check if score file exists
        if (!fileExists)
        {
            Debug.Log("The file " + scoreFileName + " does not exist. No scores will be loaded.", this); // Log file not found
            return;
        }

        scores = new int[scores.Length]; // Reset scores array

        StreamReader fileReader = new StreamReader(currentDirectory + "\\" + scoreFileName); // Create a stream reader for the file
        int scoreCount = 0; // Counter for loaded scores

        while (fileReader.Peek() != 0 && scoreCount < scores.Length) // Read scores from file
        {
            string fileLine = fileReader.ReadLine(); // Read a line from the file
            int readScore;
            if (int.TryParse(fileLine, out readScore)) // Try parsing score
            {
                scores[scoreCount] = readScore; // Store valid score
            }
            else
            {
                Debug.Log("Invalid line in scores file at " + scoreCount + ", using default value.", this); // Log parsing error
                scores[scoreCount] = 0; // Use default score on parse fail
            }
            scoreCount++;
        }
        fileReader.Close(); // Close file reader
    }

    public void SaveScoresToFile()
    {
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\" + scoreFileName); // Create a stream writer for the file
        for (int i = 0; i < scores.Length; i++) // Write all scores to the file
        {
            fileWriter.WriteLine(scores[i]); // Write score
        }
        fileWriter.Close(); // Close file writer
    }

    public void AddScore(int newScore)
    {
        int desiredIndex = -1; // Desired index for the new score
        for (int i = 0; i < scores.Length; i++) // Determine where to insert new score
        {
            if (scores[i] < newScore || scores[i] == 0)
            {
                desiredIndex = i; // Set insertion point
                break;
            }
        }

        if (desiredIndex < 0) // Check if new score is high enough
        {
            Debug.Log("Score of " + newScore + " not high enough for high scores list.", this); // Log score too low
            return;
        }

        for (int i = scores.Length - 1; i > desiredIndex; i--) // Shift lower scores down
        {
            scores[i] = scores[i - 1];
        }

        scores[desiredIndex] = newScore; // Insert new score
    }
}