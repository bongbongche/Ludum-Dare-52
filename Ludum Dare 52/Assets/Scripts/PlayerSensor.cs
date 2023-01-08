using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    public Vector2 playerIntPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 센서를 플레이어가 식물을 심을 위치로 옮김
        playerIntPos = GetComponentInParent<PlayerController>().playerIntPos;
        transform.position = playerIntPos;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 플레이어가 심으려고 하는 자리에 식물이 있어서 충돌한다면 심을 수 없음
        if(collision.CompareTag("Plant"))
        {
            GetComponentInParent<PlayerController>().canPlant = false;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 심을 수 없는 장소를 벗어나면 다시 심을 수 있음.
        if (collision.CompareTag("Plant"))
        {
            GetComponentInParent<PlayerController>().canPlant = true;
        }
    }
}
