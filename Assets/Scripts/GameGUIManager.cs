using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class GameGUIManager : Singleton<GameGUIManager>
{
    public GameObject homeGui;
    public GameObject gameGui;
    public GameObject pauseButton;

    public Dialog gameDialog;
    public Dialog pauseDialog;
    public Dialog questionDialog;

    public Image fireRateFilled;
    public Image fireNumberFilled;
    public Text timerText;
    public Text killedCountingText;
    public AnswerButton[] answerButtons;


    Dialog m_curDialog;

    public Dialog CurDialog { get => m_curDialog; set => m_curDialog = value; }

    public override void Awake()
    {
        MakeSingleton(false);
    }

    public void ShowGameGui(bool isShow)
    {
        if(gameGui)
            gameGui.SetActive(isShow);
        if(homeGui)
        {
            homeGui.SetActive(!isShow);
            UpdateFireRate(1);
        }
            

    }

    public void UpdateTimer(string time)
    {
        if (timerText)
            timerText.text = time;
    }

    public void UpdateKilledCounting(int killed)
    {
        if(killedCountingText)
            killedCountingText.text = "x" + killed.ToString();
    }

    public void UpdateFireRate(float rate)
    {
        if(fireRateFilled)
            fireRateFilled.fillAmount = rate;
    }

    public void UpdateFireNumber(float rate)
    {
        if(fireNumberFilled)
            fireNumberFilled.fillAmount = rate;
    }

    public void QuestionGame()
    {
        Time.timeScale = 0f;

        QuestionData qs = QuestionManager.Ins.GetRandomQuestion();

        questionDialog.UpdateDialog(qs.question,"");

        string[] wrongAnswers = new string[] { qs.answerA, qs.answerB, qs.answerC };

        ShuffleAnswers();

        var temp = answerButtons;
        if (temp != null && temp.Length > 0)
        {
            int wrongAnswersCount = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                int answerId = i;

                if (string.Compare(temp[i].tag, "RightAnswer") == 0)
                {
                    temp[i].SetAnswerText(qs.rightAnswer);
                }
                else
                {
                    temp[i].SetAnswerText(wrongAnswers[wrongAnswersCount]);
                    wrongAnswersCount++;
                }

                temp[answerId].btnComp.onClick.RemoveAllListeners();
                temp[answerId].btnComp.onClick.AddListener(() => CheckRightAnswerEvent(temp[answerId]));
            }
        }

        questionDialog.Show(true);
    }

    public void CheckRightAnswerEvent(AnswerButton answerButton)
    {
        if (answerButton.CompareTag("RightAnswer"))
        {
            GameManager.Ins.RightAnswer(true);
            Player.Ins.RightQuestion();
        }
        else
        {
            GameManager.Ins.RightAnswer(false);
        }
        questionDialog.Show(false);
        Time.timeScale = 1f;
    }

    public void ContinueGame()
    {
        Time.timeScale = 0f;

        pauseButton.SetActive(false);

        if (pauseDialog)
        {
            pauseDialog.Show(true);
            pauseDialog.UpdateDialog("GAME PAUSE", "BEST KILLED : x" + Prefs.bestScore);
            m_curDialog = pauseDialog;
        }
            
    }

    public void ResumeGame()
    {
        if (pauseDialog)
            m_curDialog.Show(false);

        Time.timeScale = 1f;

    }

    public void BackToHome()
    {
        ResumeGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShuffleAnswers()
    {
        if (answerButtons != null && answerButtons.Length > 0)
        {
            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (answerButtons[i] != null)
                {
                    answerButtons[i].tag = "Untagged";
                }
            }

            int randIdx = Random.Range(0, answerButtons.Length);

            if (answerButtons[randIdx])
            {
                answerButtons[randIdx].tag = "RightAnswer";
            }
        }
    }

    public void Replay()
    {
        if (m_curDialog)
            m_curDialog.Show(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void ExitGame()
    {
        ResumeGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Application.Quit();
    }
}
