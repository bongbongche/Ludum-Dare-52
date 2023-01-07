using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float plantHp = 50.0f;
    public float supplyHp = 5.0f;
    public float supplyGap = 2.0f;

    private GameManager gameManager;
    private float elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
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
    }
}
