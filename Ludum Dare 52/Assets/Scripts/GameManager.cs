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
        // title ��ũ�� �߰� �Ϸ��� false�� �ٲ� ��. Game Start ȭ�� active ��Ű��.
        isGameActive = true;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
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
        }

        // �÷��̾��� ü���� 0�� ���� �� ���� ����
        if(playerHp <= 0)
        {
            GameOver();
        }
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

}
