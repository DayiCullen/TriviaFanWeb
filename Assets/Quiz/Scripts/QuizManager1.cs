using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager1 : MonoBehaviour
{
    [SerializeField] private QuizUI quizUI;
    [SerializeField] private List<QuizDataScriptable1> quizData;
    [SerializeField] private float timeLimit = 30f;

    private string currentCategory = "";
    private int correctAnswercount = 0;
    private List<Question> questions;
    private Question selectedQuestion = new Question();
    private int scoreCount = 0;
    private float currentTimer;
    private int lifeRemaining = 3;
    private QuizDataScriptable1 dataScriptable1;

    private GameStatus1 gameStatus = GameStatus1.Next;

    public GameStatus1 GameStatus { get { return gameStatus; } }

    public List<QuizDataScriptable1> QuizData { get => quizData; }

    // Start is called before the first frame update
    public void StartGame(int index, string category)
    {
        currentCategory = category;
        correctAnswercount = 0;
        scoreCount = 0;
        currentTimer = timeLimit;
        lifeRemaining = 3;
        questions = new List<Question>();
        dataScriptable1 = quizData[index];
        questions.AddRange(dataScriptable1.questions);
       
        SelectQuestion();

        gameStatus = GameStatus1.Playing;
    }

    void SelectQuestion()
    {
        int val = UnityEngine.Random.Range(0, questions.Count);
        selectedQuestion = questions[val];

        quizUI.SetQuestion(selectedQuestion);

        questions.RemoveAt(val);
    }

    private void Update()
    {
        if(gameStatus == GameStatus1.Playing)
        {
            currentTimer -= Time.deltaTime;
            SetTimer(currentTimer);
        }
    }

    private void SetTimer(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(value);
        quizUI.TimerText.text = "Time: " + time.ToString("mm':'ss");

        if(currentTimer <= 0)
        {
            GameEnd();
        }

     }
    public bool Answer(string answered)
    {
        bool correctAns = false;

        if(answered == selectedQuestion.correctAns)
        {
            correctAnswercount++;
            correctAns = true;
            scoreCount += 50;
            quizUI.ScoreText.text = "Score: " + scoreCount;
        }
        else
        {
            lifeRemaining--;
            quizUI.ReduceLife(lifeRemaining);

            if(lifeRemaining <= 0)
            {
                GameEnd();
            }

        }

        if(gameStatus == GameStatus1.Playing)
        {
            if(questions.Count > 0)
            {
              Invoke("SelectQuestion", 0.4f);

            }
            else
            {
                GameEnd();
            }

        }

        return correctAns;
    }

    private void GameEnd()
    {
        gameStatus = GameStatus1.Next;

        if(lifeRemaining == 0)
        {
            quizUI.GameOverPanel.SetActive(true);

        }
        else if(currentTimer <= 0)
        {
            quizUI.GameTimerPanel.SetActive(true);
        }
        else
        {
            quizUI.GameComplete.SetActive(true);

        }

        //Puntos si agregamos
        if (correctAnswercount > PlayerPrefs.GetInt(currentCategory))
        {
            PlayerPrefs.SetInt(currentCategory, correctAnswercount);
        }

    }

}


[System.Serializable]
public class Question
{
    public string questionInfo;
    public QuestionType questionType;
    public Sprite questionImg;
    public AudioClip questionClip;
    public string videoFileName;
    public List<string> options;
    public string correctAns;
    
}

[System.Serializable]
public enum QuestionType
{
    TEXT,
    IMAGE,
    VIDEO,
    AUDIO
}


[System.Serializable]

public enum GameStatus1
{
    Next,
    Playing
}

