using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingGameCamera : MonoBehaviour
{
    int width = Screen.width;
    int boundary = 50;
    int speed = 10;

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.transform.position.x >= -15 && Input.mousePosition.x < boundary)
            Camera.main.transform.Translate(-speed * Time.deltaTime, 0, 0);
        if (Camera.main.transform.position.x <= 15 && Input.mousePosition.x > width - boundary)
            Camera.main.transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
