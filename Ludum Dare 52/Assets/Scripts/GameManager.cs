using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI titleText;
    public Slider hpbar;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public float playerHp = 100.0f;
    public float maxHp = 100.0f;
    public float sumHp = 0.0f;
    public bool isGameActive;

    private float time = 0.0f;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        sumHp = maxHp;
        // title 스크린 뜨게 하려면 false로 바꿀 것. Game Start 화면 active 시키고.
        isGameActive = true;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive == true)
        {
            // 초 단위로 시간 읽기
            time += Time.deltaTime;

            // 초 단위로 스코어 업데이트
            scoreText.text = "Score: " + Mathf.Ceil(time).ToString();

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
        }

        // 플레이어의 체력이 0이 됐을 때 게임 종료
        if(playerHp <= 0)
        {
            GameOver();
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

}
