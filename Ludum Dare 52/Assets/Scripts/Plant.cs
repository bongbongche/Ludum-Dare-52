using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Plant : MonoBehaviour
{
    [Header("Grade 1 Status")]
    public float plantHp;
    private float plantMaxHp;
    private float supplyHp;
    private float supplyGap;
    private float instantHp;
    public float cost;

    [Header("Grade 2 Status")]
    private float secondPlantHp;
    private float secondMaxPlantHp;
    private float secondSupplyHp;
    private float secondSupplyGap;
    private float secondInstantHp;
    private float secondCost;

    [Header("Grade 3 Status")]
    private float thirdPlantHp;
    private float thirdMaxPlantHp;
    private float thirdSupplyHp;
    private float thirdSupplyGap;
    private float thirdInstantHp;
    private float thirdCost;

    [Header("Variables")]
    public bool isClicked = false;
    public int grade;
    public Sprite[] plantSprite;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI deleteText;
    public Slider hpbar;
    public Button upgradeButton;

    private SpriteRenderer plantSpriteRenderer;
    private GameManager gameManager;
    private AudioSource audioSource;
    private float elapsedTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        plantSpriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();

        plantHp = gameManager.plantHp;
        plantMaxHp = gameManager.plantMaxHp;
        supplyHp = gameManager.supplyHp;
        supplyGap = gameManager.supplyGap;
        instantHp = gameManager.instantHp;
        cost = gameManager.cost;

        secondPlantHp = gameManager.secondPlantHp;
        secondMaxPlantHp = gameManager.secondMaxPlantHp;
        secondSupplyHp = gameManager.secondSupplyHp;
        secondSupplyGap = gameManager.secondSupplyGap;
        secondInstantHp = gameManager.secondInstantHp;
        secondCost = gameManager.secondCost;

        thirdPlantHp = gameManager.thirdPlantHp;
        thirdMaxPlantHp = gameManager.thirdMaxPlantHp;
        thirdSupplyHp = gameManager.thirdSupplyHp;
        thirdSupplyGap = gameManager.thirdSupplyGap;
        thirdInstantHp = gameManager.thirdInstantHp;
        thirdCost = gameManager.thirdCost;

        grade = 0;
        gameManager.grade_0 += 1;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > supplyGap)
        {
            elapsedTime = 0;
            gameManager.playerHp += supplyHp;
        }

        if(grade == 0)
        {
            hpbar.value = plantHp / plantMaxHp;
        }
        else if(grade == 1)
        {
            hpbar.value = plantHp / secondMaxPlantHp;
        }
        else
        {
            hpbar.value = plantHp / thirdMaxPlantHp;
        }

        // �ǰ� ������ ����
        if(plantHp <= 0)
        {
            // ���� �Ŵ������� ���ŵƴٰ� �˷���
            if(grade == 0)
            {
                gameManager.grade_0 -= 1;
            }
            else if(grade == 1)
            {
                gameManager.grade_1 -= 1;
            }
            else if(grade == 2)
            {
                gameManager.grade_2 -= 1;
            }
            Destroy(gameObject);
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
            else if (isClicked == false && grade == 2)
            {
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                upgradeText.text = "";
                upgradeButton.gameObject.SetActive(false);
                deleteText.text = "+" + thirdInstantHp + " recovery";
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
                gameManager.playerHp -= cost;
                gameManager.grade_0 -= 1;
                gameManager.grade_1 += 1;
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
                gameManager.playerHp -= cost;
                gameManager.grade_1 -= 1;
                gameManager.grade_2 += 1;
            }
        }
    }

    // �Ĺ� ����
    public void PlantDelete()
    {
        gameManager.playerHp += instantHp;

        // ���� �Ŵ������� ���ŵƴٰ� �˷���
        if (grade == 0)
        {
            gameManager.grade_0 -= 1;
        }
        else if (grade == 1)
        {
            gameManager.grade_1 -= 1;
        }
        else if (grade == 2)
        {
            gameManager.grade_2 -= 1;
        }

        Destroy(gameObject);
    }

    // �˾�â ������
    public void ManageExit()
    {
        isClicked = false;
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
}
