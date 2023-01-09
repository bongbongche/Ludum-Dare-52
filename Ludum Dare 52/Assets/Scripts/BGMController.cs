using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    private AudioSource audioSource;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive == true)
        {
            if (gameManager.minute >= 2)
            {
                audioSource.pitch = 1f;
            }
            else if (gameManager.minute == 1)
            {
                audioSource.pitch = 1.07f;
            }
            else if (gameManager.minute == 0)
            {
                audioSource.pitch = 1.15f;
            }
        }
    }
}
