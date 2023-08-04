using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    private Text message;

    private void Awake()
    {
        message = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        Destroy(gameObject, 6.5f);
    }

    public void SetMessage(string message)
    {
        this.message.text = message;
    }
}
