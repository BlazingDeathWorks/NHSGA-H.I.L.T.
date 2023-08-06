using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    private Text text;
    private Image image;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        image = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        Destroy(gameObject, 6.5f);
    }

    private void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + 2 * Time.deltaTime);
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 5 * Time.deltaTime);
    }

    public void SetMessage(string message)
    {
        text.text = message;
    }
}
