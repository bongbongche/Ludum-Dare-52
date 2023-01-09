using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Status")]
    public float enemySpeed;
    public float attackPower;
    public float attackInterval;
    public float enemyHp;
    public float enemyMaxHp;
    public float knockback;

    [Header("Variables")]
    public Sprite[] enemySprites;
    public GameObject target;

    private float distance;
    private float elapsedTime;
    private Vector3 direction;
    private int directionX;
    private Rigidbody2D enemyRb;
    private SpriteRenderer enemySpriteRenderer;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        enemySpriteRenderer.sprite = enemySprites[Random.Range(0, 3)];
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        target = GameObject.Find("Player");

        enemySpeed = gameManager.enemySpeed;
        attackPower = gameManager.attackPower;
        attackInterval = gameManager.attackInterval;
        enemyHp = gameManager.enemyHp;
        enemyMaxHp = gameManager.enemyMaxHp;
        knockback = gameManager.knockback;

        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 적과 타겟의 거리를 리턴
        distance = Vector3.Distance(transform.position, target.transform.position);

        // 타겟 따라다니기
        EnemyMove();

        if(enemyHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        // 플레이어가 밀면 velocity 값이 변하는 것을 방지 
        enemyRb.velocity = Vector2.zero;
    }

    private void EnemyMove()
    {
        direction = target.transform.position - transform.position;
        directionX = (target.transform.position.x - transform.position.x < 0) ? -1 : 1; 

        transform.Translate(direction.normalized * enemySpeed * Time.deltaTime);
        if (directionX < 0)
        {
            enemySpriteRenderer.flipX = false;
        }
        else if (directionX > 0)
        {
            enemySpriteRenderer.flipX = true;
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > attackInterval)
            {
                gameManager.GetComponent<GameManager>().playerHp -= attackPower;
                elapsedTime = 0;
            }
        }
        else if(collision.gameObject.tag == "Plant")
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > attackInterval)
            {
                collision.gameObject.GetComponent<Plant>().plantHp -= attackPower;
                elapsedTime = 0;
            }
        }
    }

    public void GetDamage(float damage)
    {
        enemyHp -= damage;

        if(target.transform.position.x > transform.position.x)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-knockback, 0f));
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, 0f));
        }
    }
}
