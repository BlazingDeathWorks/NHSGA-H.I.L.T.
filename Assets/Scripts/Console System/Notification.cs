using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    private Text text;
    private Image image;
    private float timeSinceAlive;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        image = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        timeSinceAlive += Time.deltaTime;

        if (timeSinceAlive >= 3f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 6 * Time.deltaTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 6 * Time.deltaTime);
            if (text.color.a <= 0 || image.color.a <= 0) Destroy(gameObject);
        }
        else
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + 5 * Time.deltaTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 5 * Time.deltaTime);
        }
    }

    public void SetMessage(string message)
    {
        text.text = message;
    }
}
