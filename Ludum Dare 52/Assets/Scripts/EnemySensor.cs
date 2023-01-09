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

    // ��׷� �ٲٱ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� �ȿ� �Ĺ��� ������ �Ĺ��� Ÿ���� �ٲ�
        if(collision.gameObject.tag == "Plant")
        {
            gameObject.transform.parent.GetComponent<Enemy>().target = collision.gameObject;
        }
        // ���� �ȿ� �÷��̾ ������ �÷��̾�� Ÿ���� �ٲ�
        else if(collision.gameObject.tag == "Player")
        {
            gameObject.transform.parent.GetComponent<Enemy>().target = collision.gameObject;
        }
    }

    // �Ĺ��� ������ �÷��̾�� Ÿ���� �ٲ�
    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.transform.parent.GetComponent<Enemy>().target = GameObject.Find("Player");
    }
}
