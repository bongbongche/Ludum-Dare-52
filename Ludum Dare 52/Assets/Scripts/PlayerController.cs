using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject firstPlantPrefabs;
    public float horizontalInput;
    public float verticalInput;
    public float moveSpeed = 1.0f;
    public float plantingSpeed = 1.5f;
    public bool canMove;
    public bool canPlant;
    public Vector2 playerIntPos;

    private Rigidbody2D playerRb;
    private PlayerSpriteChange playerSpriteChangeScript;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerSpriteChangeScript = GetComponent<PlayerSpriteChange>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        canMove = true;
        canPlant = true;
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
            // 식물 심기 (심을 수 있을 때만)
            if (Input.GetKeyDown(KeyCode.Space) && canPlant == true)
            {
                canMove = false;    // 식물을 심을 때는 움직일 수 없음
                playerSpriteChangeScript.playerState = 3;   // 플레이어의 상태를 노동 상태로 업데이트
                StartCoroutine(PlantCannotMove());
            }
            else if (Input.GetKeyDown(KeyCode.Space) && canPlant == false)
            {
                Debug.Log("Can't plant");
            }

            // 플레이어의 현재 위치를 정수 단위로 반환. 칸마다 식물을 심기 위해서.
            playerIntPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        }
    }

    void Planting()
    {
        // 플레이어의 정수 단위 위치에서 식물 심기
        Instantiate(firstPlantPrefabs, playerIntPos, transform.rotation);
        playerSpriteChangeScript.playerState = 0;   // 플레이어의 상태를 비노동 상태로 업데이트
    }

    IEnumerator PlantCannotMove()
    {
        yield return new WaitForSeconds(plantingSpeed); // 플레이어의 노화 수준에 따라 식물 심는 속도가 달라짐
        Planting();
        canMove = true;
    }
}
