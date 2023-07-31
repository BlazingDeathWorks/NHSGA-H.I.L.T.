using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade_ht : MonoBehaviour
{
    [SerializeField] private string currentBladeName;
    [SerializeField] private Transform slashSpawnPoint;
    [SerializeField] private Blade[] blades;
    private Blade currentBlade;

    private void Awake()
    {
        InitializeBlade();
    }

    private void InitializeBlade()
    {
        if (blades == null || blades.Length <= 0) return;
        for (int i = 0; i < blades.Length; i++)
        {
            if (currentBladeName == blades[i].BladeName)
            {
                currentBlade = blades[i];
            }
        }
        //Change the hue with a reference to the volume and use a tween to make it look nicer
    }

    //Hardcode Strategy for faster production
    public void ChangeCurrentBlade(string newBlade)
    {
        currentBladeName = newBlade;
        InitializeBlade();
    }

    //Instantiate Slash Methods Expand As More Moves Are Created
    public void BaseAttackSlash()
    {
        Instantiate(currentBlade.BaseAttackSlash, slashSpawnPoint.position, Quaternion.identity);
    }
    public void ThreeHitAttackSlash()
    {
        Instantiate(currentBlade.ThreeHitAttackSlash, slashSpawnPoint.position, Quaternion.identity);
    }
    public void AirAttackSlash()
    {
        Instantiate(currentBlade.AirAttackSlash, slashSpawnPoint.position, Quaternion.identity);
    }
}

[System.Serializable]
public struct Blade
{
    public string BladeName => bladeName;
    [SerializeField] private string bladeName;

    public int HueShiftValue => hueShiftValue;
    [SerializeField] private int hueShiftValue;

    //Slash Variables Expand As More Moves Are Created
    public Slash BaseAttackSlash => baseAttackSlash;
    public Slash ThreeHitAttackSlash => threeHitAttackSlash;
    public Slash AirAttackSlash => airAttackSlash;
    [SerializeField] private Slash baseAttackSlash;
    [SerializeField] private Slash threeHitAttackSlash;
    [SerializeField] private Slash airAttackSlash;
}
