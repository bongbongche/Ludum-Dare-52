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

    private float time = 0.0f;
    private PlayerController playerController;
    private Vector2 RandomSpawnPos;
    private float xInnerRange = 10;
    private float yInnerRange = 6;
    private float xOuterRange = 3;
    private float yOuterRange = 3;
    private int spawnDirection;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        sumHp = maxHp;
        // title 스크린 뜨게 하려면 false로 바꿀 것. Game Start 화면 active 시키고.
        isGameActive = true;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive == true)
        {
            // 초 단위로 시간 읽기
            time += Time.deltaTime;

            // 초 단위로 스코어 업데이트
            scoreText.text = "Time: " + Mathf.Ceil(time).ToString();

            // 플레이어 체력 업데이트
            playerHp = sumHp - time;

            // 현재 플레이어의 체력이 최대 체력을 넘지 못하게 제어
            if(playerHp > maxHp)
            {
                hpbar.value = maxHp / maxHp;
                hpText.text = Mathf.Ceil(maxHp).ToString() + "/" + maxHp;  // HP 텍스트 업데이트
            }
            else
            {        
                hpbar.value = playerHp / maxHp;
                hpText.text = Mathf.Ceil(playerHp).ToString() + "/" + maxHp;  // HP 텍스트 업데이트
            }

            // 적 스폰 시간 변수
            elapsedTime += Time.deltaTime;
        }

        // 플레이어의 체력이 0이 됐을 때 게임 종료
        if(playerHp <= 0)
        {
            GameOver();
        }

        // 정해진 인터벌에 정해진 수의 적 생성
        if(elapsedTime >= enemySpawnInterval)
        {
            for (int i = 0; i < enemySpawnNum; i++)
            {
                RandomSpawn();
            }
            elapsedTime = 0;
            enemySpawnNum += enemySpawnPlus;
        }
    }

    // 게임 오버
    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        playerController.canMove = false;
        isGameActive = false;
    }

    // 게임 재시작
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 게임 시작
    public void GameStart()
    {
        isGameActive = true;
        titleScreen.gameObject.SetActive(false);
    }

    // 동서남북으로 랜덤 스폰 위치 생성
    private Vector2 MakeRandomSpawnPos()
    {
        spawnDirection = Random.Range(0, 4);

        // 위쪽
        if (spawnDirection == 0)    
        {
            RandomSpawnPos.x = Random.Range(-xInnerRange, xInnerRange);
            RandomSpawnPos.y = Random.Range(yInnerRange, yInnerRange + yOuterRange);
        }
        // 아래쪽
        else if (spawnDirection == 1)   
        {
            RandomSpawnPos.x = Random.Range(-xInnerRange, xInnerRange);
            RandomSpawnPos.y = Random.Range(-yInnerRange - yOuterRange, -yInnerRange);
        }
        // 왼쪽
        else if (spawnDirection == 2)    
        {
            RandomSpawnPos.x = Random.Range(-xInnerRange - xOuterRange, -xInnerRange);
            RandomSpawnPos.y = Random.Range(-yInnerRange, yInnerRange);
        }
        // 오른쪽
        else if (spawnDirection == 3)
        {
            RandomSpawnPos.x = Random.Range(xInnerRange, xInnerRange + xOuterRange);
            RandomSpawnPos.y = Random.Range(-yInnerRange, yInnerRange);
        }

        return RandomSpawnPos;
    }

    // 랜덤 스폰 위치 기반으로 스폰
    private void RandomSpawn()
    {
        Instantiate(enemyPrefab, MakeRandomSpawnPos(), transform.rotation);
    }
}
