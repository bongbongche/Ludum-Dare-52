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
        gameManager.grade_0 += 1;
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

        // 피가 없으면 죽음
        if(plantHp <= 0)
        {
            // 게임 매니저에게 제거됐다고 알려줌
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

    // 식물 누르면 팝업창
    private void OnMouseDown()
    {
        if(gameManager.isGameActive == true)
        {
            if (isClicked == false && grade == 0)    // 3단계가 아닐 때만 팝업창 열기 가능
            {
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                upgradeText.text = "HP: " + plantHp + " -> " + secondPlantHp + "\n" +
                    "+" + supplyHp + "HP / s -> +" + secondSupplyHp + "HP / s\n" +
                    "Cost: " + secondCost;
                deleteText.text = "+" + instantHp + " recovery";
                isClicked = true;
            }
            else if (isClicked == false && grade == 1)    // 3단계가 아닐 때만 팝업창 열기 가능
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

    // 식물 업그레이드
    public void PlantUpgrade()
    {
        if(grade < 2)   // 3단계가 아닐 때만 업그레이드 가능
        {
            if (grade == 0)
            {
                isClicked = false;
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
                plantSpriteRenderer.sprite = plantSprite[0];


                // 2단계 나무 스펙
                grade = 1;
                plantHp = secondPlantHp;
                supplyHp = secondSupplyHp;
                instantHp = secondInstantHp;
                cost = secondCost;

                // 식물 설치 비용
                gameManager.sumHp -= cost;
                gameManager.grade_0 -= 1;
                gameManager.grade_1 += 1;
            }
            else if (grade == 1)
            {
                isClicked = false;
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
                plantSpriteRenderer.sprite = plantSprite[1];
                transform.localScale -= new Vector3(0.2f, 0.2f, 0); // 스프라이트만 바꾸면 나무가 너무 커져서 작게 변경


                // 3단계 나무 스펙
                grade = 2;
                plantHp = thirdPlantHp;
                supplyHp = thirdSupplyHp;
                instantHp = thirdInstantHp;
                cost = thirdCost;

                // 식물 설치 비용
                gameManager.sumHp -= cost;
                gameManager.grade_1 -= 1;
                gameManager.grade_2 += 1;
            }
        }
    }

    // 식물 제거
    public void PlantDelete()
    {
        gameManager.sumHp += instantHp;

        // 게임 매니저에게 제거됐다고 알려줌
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

    // 팝업창 나가기
    public void ManageExit()
    {
        isClicked = false;
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
}
