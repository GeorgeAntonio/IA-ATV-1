using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerEnemy : MonoBehaviour
{
    [SerializeField] public GameObject focus;
    [SerializeField] public float delay;
    [SerializeField] public float velocidade;
    private Animator anim;

    double x = 0f;
    float y = 0f;
    float tempo = 0f;

    float subx = 0;
    float suby = 0;

    public int vida = 100;

    float delayCalc = 1f;
    public float dist = 0;
    bool search = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "bullet")
        {
            Destroy(other.gameObject);
            vida -= Random.Range(20, 30);
        }
        if(vida < 0)
        {
            Destroy(gameObject);
            GameController gameController = FindObjectOfType<GameController>();
            int maisMunicao = Random.Range(5, 7);
            int municao = gameController.getMunicaoReserva();
            gameController.setMunicaoReserva(municao + maisMunicao);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isAttacking", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isAttacking", false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        anim = GetComponent <Animator>();
        anim.SetBool("isRunning", true);
        //Debug.Log( GameObject.FindGameObjectsWithTag("Player").Length != 0 );
        if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
        {
            focus = GameObject.FindGameObjectsWithTag("useless")[0];
        }
        else
        {
            if(!search && Time.time - tempo > delayCalc)
            {
                tempo = Time.time;
                dist = Mathf.Sqrt( Mathf.Pow((focus.transform.position.x - transform.position.x), 2) + (Mathf.Pow((focus.transform.position.y - transform.position.y), 2)) );
                if(dist < 20)
                {
                    search = true;
                }
            }
            if (search && Time.time - tempo > delay)
            {
                tempo = Time.time;
                x = Mathf.Sqrt(Mathf.Pow((focus.transform.position.x - transform.position.x), 2));
                y = Mathf.Sqrt(Mathf.Pow((focus.transform.position.y - transform.position.y), 2));

                if (y == 0)
                {
                    y += 0.01f;
                }

                if (x / y < 0.2)
                {
                    subx = 0;
                    if (transform.position.y > focus.transform.position.y)
                    {
                        suby = -velocidade;
                    }
                    else
                    {
                        suby = velocidade;
                    }
                }
                else if (x / y >= 0.2 && x / y < 0.40)
                {
                    if (transform.position.y > focus.transform.position.y)
                    {
                        suby = -velocidade * (1 - 0.2f);
                    }
                    else
                    {
                        suby = velocidade * (1 - 0.2f);
                    }
                    if (transform.position.x < focus.transform.position.x)
                    {
                        subx = velocidade * (1 - 0.8f);
                    }
                    else
                    {
                        subx = -velocidade * (1 - 0.8f);
                    }
                }
                else if (x / y >= 0.4 && x / y < 0.6)
                {
                    if (transform.position.y > focus.transform.position.y)
                    {
                        suby = -velocidade * (1 - 0.2f); ;
                    }
                    else
                    {
                        suby = velocidade * (1 - 0.2f);
                    }
                    if (transform.position.x < focus.transform.position.x)
                    {
                        subx = velocidade * (1 - 0.6f);
                    }
                    else
                    {
                        subx = -velocidade * (1 - 0.6f);
                    }
                }
                else if (x / y >= 0.6 && x / y < 0.8)
                {
                    if (transform.position.x < focus.transform.position.x)
                    {
                        subx = velocidade * (1 - 0.5f);
                    }
                    else
                    {
                        subx = -velocidade * (1 - 0.5f);
                    }
                    if (transform.position.y > focus.transform.position.y)
                    {
                        suby = -velocidade * (1 - 0.4f);
                    }
                    else
                    {
                        suby = velocidade * (1 - 0.4f);
                    }
                }
                else if (x / y >= 0.8 && x / y < 1)
                {
                    if (transform.position.y > focus.transform.position.y)
                    {
                        suby = -velocidade * (1 - 0.4f);
                    }
                    else
                    {
                        suby = velocidade * (1 - 0.4f);
                    }
                    if (transform.position.x < focus.transform.position.x)
                    {
                        subx = velocidade * (1 - 0.45f);
                    }
                    else
                    {
                        subx = -velocidade * (1 - 0.45f);
                    }
                }
                else if (x / y >= 1 && x / y < 1.25)
                {
                    if (transform.position.y > focus.transform.position.y)
                    {
                        suby = -velocidade * (1 - 0.45f);
                    }
                    else
                    {
                        suby = velocidade * (1 - 0.45f);
                    }
                    if (transform.position.x < focus.transform.position.x)
                    {
                        subx = velocidade * (1 - 0.4f);
                    }
                    else
                    {
                        subx = -velocidade * (1 - 0.4f);
                    }
                }
                else if (x / y >= 1.25 && x / y < 1.666)
                {
                    if (transform.position.y > focus.transform.position.y)
                    {
                        suby = -velocidade * (1 - 0.5f);
                    }
                    else
                    {
                        suby = velocidade * (1 - 0.5f);
                    }
                    if (transform.position.x < focus.transform.position.x)
                    {
                        subx = velocidade * (1 - 0.4f);
                    }
                    else
                    {
                        subx = -velocidade * (1 - 0.4f);
                    }
                }
                else if (x / y >= 1.666 && x / y < 2.5)
                {
                    if (transform.position.y > focus.transform.position.y)
                    {
                        suby = -velocidade * (1 - 0.6f);
                    }
                    else
                    {
                        suby = velocidade * (1 - 0.6f);
                    }
                    if (transform.position.x < focus.transform.position.x)
                    {
                        subx = velocidade * (1 - 0.2f);
                    }
                    else
                    {
                        subx = -velocidade * (1 - 0.2f);
                    }
                }
                else if (x / y >= 2.5 && x / y < 5)
                {
                    if (transform.position.y > focus.transform.position.y)
                    {
                        suby = -velocidade * (1 - 0.8f);
                    }
                    else
                    {
                        suby = velocidade * (1 - 0.8f);
                    }
                    if (transform.position.x < focus.transform.position.x)
                    {
                        subx = velocidade * (1 - 0.2f);
                    }
                    else
                    {
                        subx = -velocidade * (1 - 0.2f);
                    }
                }
                else if (x / y >= 5 && x == 0)
                {
                    suby = 0;
                    if (transform.position.x < focus.transform.position.x)
                    {
                        subx = velocidade;
                    }
                    else
                    {
                        subx = -velocidade;
                    }
                }
                /*if (subx < 0)
                {
                    flip(); // Chama a fun��o flip se o personagem estiver se movendo para a esquerda
                }*/
                //Vector2 movimento = new Vector2(transform.position.x + (subx/100), transform.position.y + (suby/100));
                //Vector2 movimento = new Vector2( subx, suby );
                transform.Translate(subx / 10, suby / 10, 0);
                //transform.position = movimento * Time.deltaTime;
                //Debug.Log(movimento);
            }
        }
    }
    
    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
