using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToUISpace : MonoBehaviour
{
    public GameObject plant;
    public Canvas canvas;
    public Vector3 offset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //offset = transform.position - worldToUISpace(canvas, plant.transform.position);
        if (gameObject.transform.parent.name == "HP Bar")
        {
            offset = new Vector3(0, -0.2f, 0);
        }
        else
        {
            offset = new Vector3(0, -1f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Convert the player's position to the UI space then apply the offset
        transform.position = worldToUISpace(canvas, plant.transform.position);
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos + offset);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
}
