using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteChange : MonoBehaviour
{
    public int playerOld; // 0 = 청년 / 1 = 중년 / 2 = 노년 (상황에 맞게 변경)
    public int playerState; // 0 = 서있는거 / 3 = 일 중 (추가 상태 필요하면 int 늘리기)
    public bool playerStateChange; // 원상태 false, 만약 스프라이트 변화 필요할 경우 true로
    public Sprite[] playerOldSprite; // 0 = 청년 그냥 / 1 = 중년 그냥/ 2 = 노년 그냥
    public GameManager gameManager; // gameManager 연결 필요

    private int preOld; // player의 old 변화 이전 상태
    private int preState; // player의 state 변화 이전 상태
    private float maxHp;

    private SpriteRenderer playerSprite; //player sprite 가져오기
    // 3 = 청년 일 중/ 4 = 중년 일중/ 5 = 노년 일중 (스프라이트 집어넣기)


    // Start is called before the first frame update
    void Start()
    {
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
        // 방향키에 따라 돌아보는 방향 달라지게 하기
        float inputRaw = Input.GetAxisRaw("Horizontal");

        if (inputRaw > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        else if (inputRaw < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
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
        }//이 셋은 old 변화하면 state 변화하도록

        if (playerState != preState)
        {
            PlayerStateChangeTrue();
            preState = playerState;
        }//일 중인 상태 변화하면 변화하도록

    }

    void LateUpdate()
    {
        if (playerStateChange)
        {

            playerSprite.sprite = playerOldSprite[playerOld + playerState];

            // 중년, 노년 구하기 전에 임시로 색깔만 바꾸기
            if(playerOld + playerState == 1 || playerOld + playerState == 4)
            {
                playerSprite.material.color = new Color(91/255f, 91/255f, 91/255f, 1);;
            }
            else if (playerOld + playerState == 2 || playerOld + playerState == 5)
            {
                playerSprite.material.color = new Color(43 / 255f, 43/255f, 43/255f, 1);
            }
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
    }// a = 0 or 3로 지정, PlayerStateChange() 함수는 나중에 필요할 때 따로 사용
}
