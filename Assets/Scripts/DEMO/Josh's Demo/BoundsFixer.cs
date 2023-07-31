using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class BoundsFixer : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;
    void Update()
    {
        map.CompressBounds();
    }
}
