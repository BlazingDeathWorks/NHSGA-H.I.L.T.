using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    private void Start()
    {
        Door.Instance.RegisterNewKey();
    }
}
