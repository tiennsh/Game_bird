using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestionManager : Singleton<QuestionManager>
{
    public QuestionData[] questions;

    QuestionData m_curQuestion;
    
    List<QuestionData> m_questions;

    public QuestionData CurQuestion { get => m_curQuestion; set => m_curQuestion = value; }

    public override void Awake()
    {
        m_questions = questions.ToList();

        MakeSingleton(false);
    }
    
    public QuestionData GetRandomQuestion()
    {
        if(m_questions != null && m_questions.Count > 0)
        {
            int randIdx = Random.Range(0, m_questions.Count);

            m_curQuestion = m_questions[randIdx];

            m_questions.RemoveAt(randIdx);
        }
        return m_curQuestion;
    }

    public void Reset()
    {
        m_questions = questions.ToList();
    }

}
