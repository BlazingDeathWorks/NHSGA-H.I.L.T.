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

    void Update()
    {
        text.color = new Color(1, 1, 1, text.color.a - 2 * Time.deltaTime);
    }
}
