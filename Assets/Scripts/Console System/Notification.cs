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

    private void FixedUpdate()
    {
        timeSinceAlive += Time.fixedDeltaTime;

        if (timeSinceAlive >= 3f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 6 * Time.fixedDeltaTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 6 * Time.fixedDeltaTime);
            if (text.color.a <= 0 || image.color.a <= 0) Destroy(gameObject);
        }
        else
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + 5 * Time.fixedDeltaTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 5 * Time.fixedDeltaTime);
        }
    }

    public void SetMessage(string message)
    {
        text.text = message;
    }
}
