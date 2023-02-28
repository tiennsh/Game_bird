using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public float fireRate;
    public int fireNumber;
    float m_curFireRate;
    float m_curFireNumber;
    public GameObject viewFinder;

    GameObject m_viewFinderClone;

    public override void Awake()
    {
        MakeSingleton(false);

        m_curFireRate = fireRate;
        
    }

    // Start is called before the first frame update
    public override void  Start()
    {
        if(viewFinder)
        {
            m_viewFinderClone = Instantiate(viewFinder, Vector3.zero, Quaternion.identity);
        }
        m_curFireNumber = fireNumber+1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0) && m_curFireNumber >0)
        {
            Shot(mousePos);
            m_curFireNumber--;
            GameGUIManager.Ins.UpdateFireNumber(m_curFireNumber / 3);
        }

        if(m_curFireNumber == 0)
        {
            LoadFire();
        }

        if(m_viewFinderClone)
        {
            m_viewFinderClone.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
        }
    }

    void Shot(Vector3 mousePos)
    {
        if(!GameManager.Ins.IsGameover)
        {

            Vector3 shootDir = Camera.main.transform.position - mousePos;

            shootDir.Normalize();

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, shootDir);

            if (hits != null && hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];

                    if (hit.collider && (Vector3.Distance((Vector2)hit.collider.transform.position, (Vector2)mousePos)) <= 0.4f)
                    {
                        if (hit.collider.name == "Question(Clone)")
                        {
                            Debug.Log(hit.collider.name);
                            Question question = hit.collider.GetComponent<Question>();
                            m_curFireNumber++;
                            question.Die();
                        }
                        else
                        {
                            Bird bird = hit.collider.GetComponent<Bird>();

                            if (bird)
                            {
                                if (bird.isBirdRed)
                                    GameManager.Ins.ShotDieBirdRed();
                                else
                                {
                                    m_curFireNumber++;
                                    GameGUIManager.Ins.UpdateFireNumber(m_curFireNumber / 3);
                                }

                                bird.Die();

                            }
                        }

                    }
                }
            }

            CineController.Ins.ShakeTrigger();
            AudioController.Ins.PlaySound(AudioController.Ins.shooting);

        }
    }

    void LoadFire()
    {
        m_curFireRate -= Time.deltaTime;

        if (m_curFireRate <= 0)
        {
            m_curFireNumber = fireNumber;

            m_curFireRate = fireRate;
        }

        GameGUIManager.Ins.UpdateFireRate(m_curFireRate / fireRate);
        GameGUIManager.Ins.UpdateFireNumber(m_curFireNumber/3);

    }

    public void RightQuestion()
    {
        m_curFireNumber = fireNumber;
        GameGUIManager.Ins.UpdateFireNumber(m_curFireNumber / 3);
    }
}
