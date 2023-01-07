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
        // �÷��̾� �̵�
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
            // �Ĺ� �ɱ� (���� �� ���� ����)
            if (Input.GetKeyDown(KeyCode.Space) && canPlant == true)
            {
                canMove = false;    // �Ĺ��� ���� ���� ������ �� ����
                playerSpriteChangeScript.playerState = 3;   // �÷��̾��� ���¸� �뵿 ���·� ������Ʈ
                StartCoroutine(PlantCannotMove());
            }
            else if (Input.GetKeyDown(KeyCode.Space) && canPlant == false)
            {
                Debug.Log("Can't plant");
            }

            // �÷��̾��� ���� ��ġ�� ���� ������ ��ȯ. ĭ���� �Ĺ��� �ɱ� ���ؼ�.
            playerIntPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        }
    }

    void Planting()
    {
        // �÷��̾��� ���� ���� ��ġ���� �Ĺ� �ɱ�
        Instantiate(firstPlantPrefabs, playerIntPos, transform.rotation);
        playerSpriteChangeScript.playerState = 0;   // �÷��̾��� ���¸� ��뵿 ���·� ������Ʈ
    }

    IEnumerator PlantCannotMove()
    {
        yield return new WaitForSeconds(plantingSpeed); // �÷��̾��� ��ȭ ���ؿ� ���� �Ĺ� �ɴ� �ӵ��� �޶���
        Planting();
        canMove = true;
    }
}
