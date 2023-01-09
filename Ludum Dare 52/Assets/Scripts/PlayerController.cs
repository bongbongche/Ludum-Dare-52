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
    private float xBound = 8.5f;
    private float yBound = 5f;
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
            // �÷��̾��� ���� ��ġ�� ���� ������ ��ȯ. ĭ���� �Ĺ��� �ɱ� ���ؼ�.
            playerIntPos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);

            // �Ĺ� �ɱ� (���� �� ���� ����)
            if (Input.GetKeyDown(KeyCode.Space) && canPlant == true)
            {
                canPlant = false;
                canMove = false;    // �Ĺ��� ���� ���� ������ �� ����
                playerSpriteChangeScript.playerState = 3;   // �÷��̾��� ���¸� �뵿 ���·� ������Ʈ
                StartCoroutine(PlantCannotMove());
            }
            else if (Input.GetKeyDown(KeyCode.Space) && canPlant == false)
            {
                //Debug.Log("Can't plant");
            }

            AttackEnemy();
        }

        if(transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, 0);
        }
        if(transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, 0);
        }
        if(transform.position.y > yBound - 0.5f)
        {
            transform.position = new Vector3(transform.position.x, yBound - 0.5f, 0);
        }
        if(transform.position.y < -yBound)
        {
            transform.position = new Vector3(transform.position.x, -yBound, 0);
        }
        /*
        cameraPos = Camera.main.WorldToViewportPoint(transform.position);

        if (cameraPos.x < 0f) cameraPos.x = 0f;

        if (cameraPos.x > 1f) cameraPos.x = 1f;

        if (cameraPos.y < 0f) cameraPos.y = 0f;

        if (cameraPos.y > 1f) cameraPos.y = 1f;

        transform.position = Camera.main.ViewportToWorldPoint(cameraPos);
        */
    }

    void Planting()
    {
        // �÷��̾��� ���� ���� ��ġ���� �Ĺ� �ɱ�
        Instantiate(firstPlantPrefabs, playerIntPos, transform.rotation);
        // 1�ܰ� �Ĺ� ��� ����
        gameManager.playerHp -= firstPlantPrefabs.GetComponent<Plant>().cost;
        playerSpriteChangeScript.playerState = 0;   // �÷��̾��� ���¸� ��뵿 ���·� ������Ʈ
        canPlant = true;
    }

    IEnumerator PlantCannotMove()
    {
        yield return new WaitForSeconds(plantingSpeed); // �÷��̾��� ��ȭ ���ؿ� ���� �Ĺ� �ɴ� �ӵ��� �޶���
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
