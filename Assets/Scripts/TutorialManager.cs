using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private bool skipActivated;
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && !skipActivated)
        {
            PlayerData data = new PlayerData(new Dictionary<string, bool>());
            for (int i = 0; i < DoorManager.Instance.PNBS.Count; i++)
            {
                IDEManager.Instance.SetCurrentClass(DoorManager.Instance.PNBS[i].Parent);
                if (DoorManager.Instance.PNBS[i].gameObject.activeSelf)
                {
                    if (DoorManager.Instance.PNBS[i].Text.color == Color.yellow)
                    {
                        data.PropertyNavigationButtons.Add(DoorManager.Instance.PNBS[i].gameObject.name, true);
                    }
                    else
                    {
                        data.PropertyNavigationButtons.Add(DoorManager.Instance.PNBS[i].gameObject.name, false);
                    }
                }
            }
            PlayerDataManager.Instance?.SavePlayerData(data);
            SceneController.Instance.NextScene();
            skipActivated = true;
        }
    }
}
