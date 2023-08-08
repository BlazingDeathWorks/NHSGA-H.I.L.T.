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
            SceneController.Instance.NextScene();
            skipActivated = true;
        }
    }
}
