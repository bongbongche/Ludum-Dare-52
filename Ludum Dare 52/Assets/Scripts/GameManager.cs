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
    public int minute, second;

    private float scoreTime = 0.0f;
    //private float acceleratingTime = 0.0f;
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
    private AudioSource gameClearAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        sumHp = maxHp;
        // title 스크린 뜨게 하려면 false로 바꿀 것. Game Start 화면 active 시키고.
        isGameActive = false;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameClearAudioSource = gameClearScreen.GetComponent<AudioSource>();

        elapsedSpawnTime = 0;
        elapsedHpMinusTime = 0;
        elapsedAcceleratingTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive == true)
        {
            // 초 단위로 시간 읽기
            scoreTime += Time.deltaTime;

            elapsedAcceleratingTime += Time.deltaTime;
            if(elapsedAcceleratingTime >= accelerationInterval)
            {
                acceleration += accelerationPoint;
                elapsedAcceleratingTime = 0;
            }

            // 시간 업데이트
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

            // 플레이어 체력 업데이트
            elapsedHpMinusTime += Time.deltaTime;
            if(elapsedHpMinusTime >= supplyGap)
            {
                playerHp -= acceleration;
                elapsedHpMinusTime = 0;
            }

            // 현재 플레이어의 체력이 최대 체력을 넘지 못하게 제어
            if(playerHp > maxHp)
            {
                playerHp = maxHp;
                hpbar.value = maxHp / maxHp;
                hpText.text = Mathf.Ceil(maxHp).ToString() + "/" + maxHp;  // HP 텍스트 업데이트
            }
            else
            {        
                hpbar.value = playerHp / maxHp;
                hpText.text = Mathf.Ceil(playerHp).ToString() + "/" + maxHp;  // HP 텍스트 업데이트
            }

            // 적 스폰 시간 변수
            elapsedSpawnTime += Time.deltaTime;

            // 초당 회복력 표시
            hpPlusText.text = "+" + (grade_0 * supplyHp + grade_1 * secondSupplyHp + grade_2 * thirdSupplyHp).ToString("F1") + " HP/s";
            // 초당 피깎 표시
            hpMinusText.text = "-" + acceleration.ToString("F1") + " HP/s";
        }

        // 플레이어의 체력이 0이 됐을 때 게임 종료
        if(playerHp <= 0 && isGameActive == true)
        {
            GameOver();
        }

        if((clearTime - scoreTime) <= 0 && isGameActive == true)
        {
            GameClear();
        }

        // 정해진 인터벌에 정해진 수의 적 생성
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

    // 게임 오버
    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        playerController.canMove = false;
        isGameActive = false;
    }

    public void GameClear()
    {
        gameClearScreen.gameObject.SetActive(true);
        gameClearAudioSource.Play();
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
        playerController.canMove = true;
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
