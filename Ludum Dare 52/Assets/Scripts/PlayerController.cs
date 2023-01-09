using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public float horizontalInput;
    public float verticalInput;
    public float moveSpeed = 1.0f;
    public float plantingSpeed = 1.5f;
    public float attackPower = 10f;
    public float attackDelay = 1f;
    public bool canMove;
    public bool canPlant;
    public Vector3 playerIntPos;

    [Header("Variables")]
    public GameObject firstPlantPrefabs;
    public BoxCollider2D[] attackColliders;

    private Rigidbody2D playerRb;
    private PlayerSpriteChange playerSpriteChangeScript;
    private GameManager gameManager;
    private float lastAttack;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerSpriteChangeScript = GetComponent<PlayerSpriteChange>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        attackColliders = gameObject.GetComponentsInChildren<BoxCollider2D>();

        canMove = true;
        canPlant = true;
        lastAttack = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어 이동
        if (canMove == true)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
        else
        {
            horizontalInput = 0;
            verticalInput = 0;
        }

        playerRb.velocity = new Vector2(horizontalInput * moveSpeed, playerRb.velocity.y);
        playerRb.velocity = new Vector2(playerRb.velocity.x, verticalInput * moveSpeed);

        if(gameManager.isGameActive == true)
        {
            // 플레이어의 현재 위치를 정수 단위로 반환. 칸마다 식물을 심기 위해서.
            playerIntPos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);

            // 식물 심기 (심을 수 있을 때만)
            if (Input.GetKeyDown(KeyCode.Space) && canPlant == true)
            {
                canPlant = false;
                canMove = false;    // 식물을 심을 때는 움직일 수 없음
                playerSpriteChangeScript.playerState = 3;   // 플레이어의 상태를 노동 상태로 업데이트
                StartCoroutine(PlantCannotMove());
            }
            else if (Input.GetKeyDown(KeyCode.Space) && canPlant == false)
            {
                //Debug.Log("Can't plant");
            }

            AttackEnemy();
        }

    }

    void Planting()
    {
        // 플레이어의 정수 단위 위치에서 식물 심기
        Instantiate(firstPlantPrefabs, playerIntPos, transform.rotation);
        // 1단계 식물 비용 빼기
        gameManager.playerHp -= firstPlantPrefabs.GetComponent<Plant>().cost;
        playerSpriteChangeScript.playerState = 0;   // 플레이어의 상태를 비노동 상태로 업데이트
        canPlant = true;
    }

    IEnumerator PlantCannotMove()
    {
        yield return new WaitForSeconds(plantingSpeed); // 플레이어의 노화 수준에 따라 식물 심는 속도가 달라짐
        Planting();
        canMove = true;
    }

    void AttackEnemy()
    {
        if(Time.time >= lastAttack + attackDelay)
        {
            lastAttack = Time.time;
            attackColliders[0].enabled = true;
            gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            attackColliders[1].enabled = true;
            gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().enabled = true;

            Invoke("EndAttack", 0.1f);
        }
    }

    void EndAttack()
    {
        attackColliders[0].enabled = false;
        gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        attackColliders[1].enabled = false;
        gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().GetDamage(attackPower);
        }
    }
}
