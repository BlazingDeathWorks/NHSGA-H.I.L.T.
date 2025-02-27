using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door Instance { get; private set; }
    private int keysNeeded;
    private int keysCollected;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Open();
        }
    }

    private void Open()
    {
        if (keysCollected >= keysNeeded)
        {
            GetComponent<Collider2D>().enabled = false;
            //Save Data
            PlayerData data = new PlayerData(new Dictionary<string, bool>());
            for (int i = 0; i < DoorManager.Instance.PNBS.Count; i++)
            {
                IDEManager.Instance.SetCurrentClass(DoorManager.Instance.PNBS[i].Parent);
                if (DoorManager.Instance.PNBS[i].gameObject.activeSelf)
                {
                    if (DoorManager.Instance.PNBS[i].Text.color == Color.yellow)
                    {
                        data.PropertyNavigationButtons.Add(DoorManager.Instance.PNBS[i].gameObject.name, true);
                    } else
                    {
                        data.PropertyNavigationButtons.Add(DoorManager.Instance.PNBS[i].gameObject.name, false);
                    }
                }
            }
            PlayerDataManager.Instance?.SavePlayerData(data);
            SceneController.Instance.NextScene();
        }
    }

    public void RegisterNewKey()
    {
        keysNeeded++;
    }

    //Call when Enemy Dies
    public void CollectKey()
    {
        keysCollected++;
    }
}
