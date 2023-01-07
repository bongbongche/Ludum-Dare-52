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
        // ������ �÷��̾ �Ĺ��� ���� ��ġ�� �ű�
        playerIntPos = GetComponentInParent<PlayerController>().playerIntPos;
        transform.position = playerIntPos;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // �÷��̾ �������� �ϴ� �ڸ��� �Ĺ��� �־ �浹�Ѵٸ� ���� �� ����
        GetComponentInParent<PlayerController>().canPlant = false;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        // �÷��̾ ���� �� ���� ��Ҹ� ����� �ٽ� ���� �� ����.
        GetComponentInParent<PlayerController>().canPlant = true;
    }
}
