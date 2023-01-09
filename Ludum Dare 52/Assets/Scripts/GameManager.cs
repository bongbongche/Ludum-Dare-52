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
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI titleText;
    public Slider hpbar;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
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

    private float time = 0.0f;
    private PlayerController playerController;
    private Vector2 RandomSpawnPos;
    private float xInnerRange = 10;
    private float yInnerRange = 6;
    private float xOuterRange = 3;
    private float yOuterRange = 3;
    private int spawnDirection;
    private float elapsedSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        sumHp = maxHp;
        // title ��ũ�� �߰� �Ϸ��� false�� �ٲ� ��. Game Start ȭ�� active ��Ű��.
        isGameActive = true;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        elapsedSpawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive == true)
        {
            // �� ������ �ð� �б�
            time += Time.deltaTime;

            // �� ������ ���ھ� ������Ʈ
            scoreText.text = "Time: " + Mathf.Ceil(time).ToString();

            // �÷��̾� ü�� ������Ʈ
            playerHp = sumHp - time;

            // ���� �÷��̾��� ü���� �ִ� ü���� ���� ���ϰ� ����
            if(playerHp > maxHp)
            {
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
        }

        // �÷��̾��� ü���� 0�� ���� �� ���� ����
        if(playerHp <= 0)
        {
            GameOver();
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

        Debug.Log("grade0 : " + grade_0 + ", grade1: " + grade_1 + ", grade2: " + grade_2);
    }

    // ���� ����
    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
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
