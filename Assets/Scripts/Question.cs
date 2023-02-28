using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    public float xSpeed;
    public float minYspeed;
    public float maxYspeed;

    Rigidbody2D m_rb;
    bool m_moveLeftOnStart;

    bool m_isDead;
    float ySpeed;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        RandomVovingDirection();
        ySpeed = xSpeed;
    }

    private void Update()
    {
        MoveBird();

        if (transform.position.x > 10)
            Destroy(gameObject);
        if (transform.position.x < -10)
            Destroy(gameObject);
        if (transform.position.y < -2)
            ySpeed = xSpeed;
        if (transform.position.y > 2)
            ySpeed = -xSpeed;
    }

    public void MoveBird()
    {
        if (!m_isDead)
        {
            m_rb.velocity = m_moveLeftOnStart ?
            new Vector2(-xSpeed, ySpeed)
            : new Vector2(xSpeed, ySpeed);
        }
    }

    public void RandomVovingDirection()
    {
        m_moveLeftOnStart = transform.position.x > 0 ? true : false;
    }

    public void Die()
    {
        m_isDead = true;

        GameGUIManager.Ins.QuestionGame();

        GameManager.Ins.BirdKilled++;

        GameGUIManager.Ins.UpdateKilledCounting(GameManager.Ins.BirdKilled);

        Destroy(gameObject);
    }

}
