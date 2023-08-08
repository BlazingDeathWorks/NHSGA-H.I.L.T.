using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            SceneController.Instance.NextScene();
        }
    }
}
