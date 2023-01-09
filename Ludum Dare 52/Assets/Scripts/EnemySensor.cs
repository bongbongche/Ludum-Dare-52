using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 어그로 바꾸기
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 범위 안에 식물이 들어오면 식물로 타겟을 바꿈
        if(collision.gameObject.tag == "Plant")
        {
            gameObject.transform.parent.GetComponent<Enemy>().target = collision.gameObject;
        }
        // 범위 안에 플레이어가 들어오면 플레이어로 타겟을 바꿈
        else if(collision.gameObject.tag == "Player")
        {
            gameObject.transform.parent.GetComponent<Enemy>().target = collision.gameObject;
        }
    }

    // 식물이 죽으면 플레이어로 타겟을 바꿈
    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.transform.parent.GetComponent<Enemy>().target = GameObject.Find("Player");
    }
}
