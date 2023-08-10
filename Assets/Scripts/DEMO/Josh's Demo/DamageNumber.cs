using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private float launchSpeed;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-launchSpeed, launchSpeed), launchSpeed);
    }

    public void SetText(float val)
    {
        text.text = ""+Mathf.CeilToInt(val);
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }

    void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 2 * Time.unscaledDeltaTime);
        if (text.color.a < 0) Destroy(gameObject);
    }
}
