using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    public static ConsoleManager Instance { get; private set; }

    [SerializeField] private Notification notification;
    private Notification currentNotification;
    private RectTransform rect;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            RequestMessage("I Love Men");
        }
    }

    public void RequestMessage(string message)
    {
        if (currentNotification)
        {
            Destroy(currentNotification.gameObject);
        }
        Notification instance = Instantiate(notification, Vector2.zero, Quaternion.identity);
        currentNotification = instance;
        instance.SetMessage(message);
        RectTransform instanceRect = instance.GetComponent<RectTransform>();
        instanceRect.SetParent(rect);
        instanceRect.localPosition = Vector2.zero;
        instanceRect.localScale = new Vector3(1, 1, 1);
    }
}
