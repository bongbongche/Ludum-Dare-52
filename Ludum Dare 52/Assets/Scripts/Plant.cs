using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Plant : MonoBehaviour
{
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

    [Header("Variables")]
    public bool isClicked = false;
    public int grade;
    public Sprite[] plantSprite;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI deleteText;
    public Slider hpbar;

    private SpriteRenderer plantSpriteRenderer;
    private GameManager gameManager;
    private float elapsedTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        plantSpriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        grade = 0;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > supplyGap)
        {
            elapsedTime = 0;
            gameManager.sumHp += supplyHp;
        }

        if(grade == 0)
        {
            hpbar.value = plantHp / plantMaxHp;
        }
        else if(grade == 1)
        {
            hpbar.value = secondPlantHp / secondMaxPlantHp;
        }
        else
        {
            hpbar.value = thirdPlantHp / thirdMaxPlantHp;
        }

        if(plantHp <= 0)
        {
            PlantDelete();
        }
    }

    // �Ĺ� ������ �˾�â
    private void OnMouseDown()
    {
        if(gameManager.isGameActive == true)
        {
            if (isClicked == false && grade == 0)    // 3�ܰ谡 �ƴ� ���� �˾�â ���� ����
            {
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                upgradeText.text = "HP: " + plantHp + " -> " + secondPlantHp + "\n" +
                    "+" + supplyHp + "HP / s -> +" + secondSupplyHp + "HP / s\n" +
                    "Cost: " + secondCost;
                deleteText.text = "+" + instantHp + " recovery";
                isClicked = true;
            }
            else if (isClicked == false && grade == 1)    // 3�ܰ谡 �ƴ� ���� �˾�â ���� ����
            {
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                upgradeText.text = "HP: " + secondPlantHp + " -> " + thirdPlantHp + "\n" +
                    "+" + secondSupplyHp + "HP / s -> +" + thirdSupplyHp + "HP / s\n" +
                    "Cost: " + thirdCost;
                deleteText.text = "+" + secondInstantHp + " recovery";
                isClicked = true;
            }
        }
    }

    // �Ĺ� ���׷��̵�
    public void PlantUpgrade()
    {
        if(grade < 2)   // 3�ܰ谡 �ƴ� ���� ���׷��̵� ����
        {
            if (grade == 0)
            {
                isClicked = false;
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
                plantSpriteRenderer.sprite = plantSprite[0];


                // 2�ܰ� ���� ����
                grade = 1;
                plantHp = secondPlantHp;
                supplyHp = secondSupplyHp;
                instantHp = secondInstantHp;
                cost = secondCost;

                // �Ĺ� ��ġ ���
                gameManager.sumHp -= cost;
            }
            else if (grade == 1)
            {
                isClicked = false;
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
                plantSpriteRenderer.sprite = plantSprite[1];
                transform.localScale -= new Vector3(0.2f, 0.2f, 0); // ��������Ʈ�� �ٲٸ� ������ �ʹ� Ŀ���� �۰� ����


                // 3�ܰ� ���� ����
                grade = 2;
                plantHp = thirdPlantHp;
                supplyHp = thirdSupplyHp;
                instantHp = thirdInstantHp;
                cost = thirdCost;

                // �Ĺ� ��ġ ���
                gameManager.sumHp -= cost;
            }
        }
    }

    // �Ĺ� ����
    public void PlantDelete()
    {
        gameManager.sumHp += instantHp;

        Destroy(gameObject);
    }

    // �˾�â ������
    public void ManageExit()
    {
        isClicked = false;
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
}
