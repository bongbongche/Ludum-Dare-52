using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteChange : MonoBehaviour
{
    public int playerOld; // 0 = û�� / 1 = �߳� / 2 = ��� (��Ȳ�� �°� ����)
    public int playerState; // 0 = ���ִ°� / 3 = �� �� (�߰� ���� �ʿ��ϸ� int �ø���)
    public bool playerStateChange; // ������ false, ���� ��������Ʈ ��ȭ �ʿ��� ��� true��
    public Sprite[] playerOldSprite; // 0 = û�� �׳� / 1 = �߳� �׳�/ 2 = ��� �׳�
    private GameManager gameManager; // gameManager ���� �ʿ�

    private int preOld; // player�� old ��ȭ ���� ����
    private int preState; // player�� state ��ȭ ���� ����
    private float maxHp;

    private SpriteRenderer playerSprite; //player sprite ��������
    // 3 = û�� �� ��/ 4 = �߳� ����/ 5 = ��� ���� (��������Ʈ ����ֱ�)


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerStateChange = false;
        playerSprite = GetComponent<SpriteRenderer>();
        maxHp = gameManager.GetComponent<GameManager>().maxHp;

        playerState = 0;
        preState = 0;
        playerOld = 0;
        preOld = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // ����Ű�� ���� ���ƺ��� ���� �޶����� �ϱ�
        float inputRaw = Input.GetAxisRaw("Horizontal");

        if (inputRaw > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        else if (inputRaw < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }


        if (gameManager.GetComponent<GameManager>().playerHp > (maxHp / 3) * 2)
        {
            playerOld = 0;
            if (playerOld != preOld)
            {
                PlayerStateChangeTrue();
                preOld = playerOld;
            }
        }
        else if (gameManager.GetComponent<GameManager>().playerHp < maxHp / 3)
        {
            playerOld = 2;
            if (playerOld != preOld)
            {
                PlayerStateChangeTrue();
                preOld = playerOld;
            }
        }
        else
        {
            playerOld = 1;
            if (playerOld != preOld)
            {
                PlayerStateChangeTrue();
                preOld = playerOld;
            }
        }//�� ���� old ��ȭ�ϸ� state ��ȭ�ϵ���

        if (playerState != preState)
        {
            PlayerStateChangeTrue();
            preState = playerState;
        }//�� ���� ���� ��ȭ�ϸ� ��ȭ�ϵ���

    }

    void LateUpdate()
    {
        if (playerStateChange)
        {

            playerSprite.sprite = playerOldSprite[playerOld + playerState];

            playerStateChange = false;
        }
    }

    public void PlayerStateChangeTrue()
    {
        playerStateChange = true;
    }

    public void PlayerStateChange(int a)
    {
        playerState = a;
    }// a = 0 or 3�� ����, PlayerStateChange() �Լ��� ���߿� �ʿ��� �� ���� ���
}
