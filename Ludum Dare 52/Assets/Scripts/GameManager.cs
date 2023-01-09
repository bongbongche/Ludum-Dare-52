using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI hpPlusText;
    public TextMeshProUGUI hpMinusText;
    public Slider hpbar;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject gameClearScreen;
    public GameObject enemyPrefab;
    public bool isGameActive;

    [Header("Player")]
    public float playerHp = 100.0f;
    public float maxHp = 100.0f;
    public float sumHp = 0.0f;

    [Header("Enemy Spawn")]
    public float enemySpawnInterval = 10f;
    public int enemySpawnNum = 10;
    public int enemySpawnPlus = 3;

    [Header("Plant Numbers")]
    public int grade_0 = 0;
    public int grade_1 = 0;
    public int grade_2 = 0;
    
    [Header("Grade 1 Status")]
    public float plantHp = 50.0f;
    public float plantMaxHp = 50.0f;
    public float supplyHp = 0.2f;
    public float supplyGap = 1.0f;
    public float instantHp = 5f;
    public float cost = 10f;

    [Header("Grade 2 Status")]
    public float secondPlantHp = 80.0f;
    public float secondMaxPlantHp = 80.0f;
    public float secondSupplyHp = 1.0f;
    public float secondSupplyGap = 1.0f;
    public float secondInstantHp = 15f;
    public float secondCost = 30f;

    [Header("Grade 3 Status")]
    public float thirdPlantHp = 150.0f;
    public float thirdMaxPlantHp = 150.0f;
    public float thirdSupplyHp = 2.0f;
    public float thirdSupplyGap = 1.0f;
    public float thirdInstantHp = 45f;
    public float thirdCost = 90f;

    [Header("Enemy Status")]
    public float enemySpeed = 4f;
    public float attackPower = 1f;
    public float attackInterval = 2f;
    public float enemyHp = 30f;
    public float enemyMaxHp = 30f;
    public float knockback = 100f;

    [Header("Time Variables")]
    public float acceleration = 1;
    public float accelerationPoint = 0.5f;
    public float accelerationInterval = 10f;
    public float clearTime = 300f;

    private float scoreTime = 0.0f;
    private float acceleratingTime = 0.0f;
    private PlayerController playerController;
    private Vector2 RandomSpawnPos;
    private float xInnerRange = 10;
    private float yInnerRange = 6;
    private float xOuterRange = 3;
    private float yOuterRange = 3;
    private int spawnDirection;
    private float elapsedSpawnTime;
    private float elapsedHpMinusTime;
    private float elapsedAcceleratingTime;
    private int minute, second;

    // Start is called before the first frame update
    void Start()
    {
        sumHp = maxHp;
        // title ��ũ�� �߰� �Ϸ��� false�� �ٲ� ��. Game Start ȭ�� active ��Ű��.
        isGameActive = true;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        elapsedSpawnTime = 0;
        elapsedHpMinusTime = 0;
        elapsedAcceleratingTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive == true)
        {
            // �� ������ �ð� �б�
            scoreTime += Time.deltaTime;

            elapsedAcceleratingTime += Time.deltaTime;
            if(elapsedAcceleratingTime >= accelerationInterval)
            {
                acceleration += accelerationPoint;
                elapsedAcceleratingTime = 0;
            }

            // �ð� ������Ʈ
            minute = ((int)Mathf.Ceil(clearTime - scoreTime)) % 3600 / 60;
            second = ((int)Mathf.Ceil(clearTime - scoreTime)) % 3600 % 60;

            //scoreText.text = "Time: " + Mathf.Ceil(clearTime - scoreTime).ToString();
            if(second < 10)
            {
                scoreText.text = "Time: " + minute + ":0" + second;
            }
            else
            {
                scoreText.text = "Time: " + minute + ":" + second;
            }

            // �÷��̾� ü�� ������Ʈ
            elapsedHpMinusTime += Time.deltaTime;
            if(elapsedHpMinusTime >= supplyGap)
            {
                playerHp -= acceleration;
                elapsedHpMinusTime = 0;
            }

            // ���� �÷��̾��� ü���� �ִ� ü���� ���� ���ϰ� ����
            if(playerHp > maxHp)
            {
                playerHp = maxHp;
                hpbar.value = maxHp / maxHp;
                hpText.text = Mathf.Ceil(maxHp).ToString() + "/" + maxHp;  // HP �ؽ�Ʈ ������Ʈ
            }
            else
            {        
                hpbar.value = playerHp / maxHp;
                hpText.text = Mathf.Ceil(playerHp).ToString() + "/" + maxHp;  // HP �ؽ�Ʈ ������Ʈ
            }

            // �� ���� �ð� ����
            elapsedSpawnTime += Time.deltaTime;

            // �ʴ� ȸ���� ǥ��
            hpPlusText.text = "+" + (grade_0 * supplyHp + grade_1 * secondSupplyHp + grade_2 * thirdSupplyHp).ToString("F1") + " HP/s";
            // �ʴ� �Ǳ� ǥ��
            hpMinusText.text = "-" + acceleration.ToString("F1") + " HP/s";
        }

        // �÷��̾��� ü���� 0�� ���� �� ���� ����
        if(playerHp <= 0)
        {
            GameOver();
        }

        if((clearTime - scoreTime) <= 0)
        {
            GameClear();
        }

        // ������ ���͹��� ������ ���� �� ����
        if(elapsedSpawnTime >= enemySpawnInterval)
        {
            for (int i = 0; i < enemySpawnNum; i++)
            {
                RandomSpawn();
            }
            elapsedSpawnTime = 0;
            enemySpawnNum += enemySpawnPlus;
        }

    }

    // ���� ����
    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        playerController.canMove = false;
        isGameActive = false;
    }

    public void GameClear()
    {
        gameClearScreen.gameObject.SetActive(true);
        playerController.canMove = false;
        isGameActive = false;
    }

    // ���� �����
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ���� ����
    public void GameStart()
    {
        isGameActive = true;
        titleScreen.gameObject.SetActive(false);
    }

    // ������������ ���� ���� ��ġ ����
    private Vector2 MakeRandomSpawnPos()
    {
        spawnDirection = Random.Range(0, 4);

        // ����
        if (spawnDirection == 0)    
        {
            RandomSpawnPos.x = Random.Range(-xInnerRange, xInnerRange);
            RandomSpawnPos.y = Random.Range(yInnerRange, yInnerRange + yOuterRange);
        }
        // �Ʒ���
        else if (spawnDirection == 1)   
        {
            RandomSpawnPos.x = Random.Range(-xInnerRange, xInnerRange);
            RandomSpawnPos.y = Random.Range(-yInnerRange - yOuterRange, -yInnerRange);
        }
        // ����
        else if (spawnDirection == 2)    
        {
            RandomSpawnPos.x = Random.Range(-xInnerRange - xOuterRange, -xInnerRange);
            RandomSpawnPos.y = Random.Range(-yInnerRange, yInnerRange);
        }
        // ������
        else if (spawnDirection == 3)
        {
            RandomSpawnPos.x = Random.Range(xInnerRange, xInnerRange + xOuterRange);
            RandomSpawnPos.y = Random.Range(-yInnerRange, yInnerRange);
        }

        return RandomSpawnPos;
    }

    // ���� ���� ��ġ ������� ����
    private void RandomSpawn()
    {
        Instantiate(enemyPrefab, MakeRandomSpawnPos(), transform.rotation);
    }
}
