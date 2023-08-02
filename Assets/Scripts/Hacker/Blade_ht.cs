using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Blade_ht : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private string currentBladeName;
    [SerializeField] private Transform slashSpawnPoint;
    [SerializeField] private Blade[] blades;
    private Blade currentBlade;
    private Vector2 originalPos;

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
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.hueShift.value = currentBlade.HueShiftValue;
        }
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
        if (currentBlade.BaseAttackSlash == null) return;
        Slash instance = Instantiate(currentBlade.BaseAttackSlash);
        originalPos = instance.transform.position;
        instance.transform.parent = slashSpawnPoint;
        instance.transform.localPosition = originalPos;
    }
    public void ThreeHitAttackSlash()
    {
        if (currentBlade.ThreeHitAttackSlash == null) return;
        Slash instance = Instantiate(currentBlade.ThreeHitAttackSlash);
        originalPos = instance.transform.position;
        instance.transform.parent = slashSpawnPoint;
        instance.transform.localPosition = originalPos;
    }
    public void AirAttackSlash()
    {
        if (currentBlade.AirAttackSlash == null) return;
        Slash instance = Instantiate(currentBlade.AirAttackSlash);
        originalPos = instance.transform.position;
        instance.transform.parent = slashSpawnPoint;
        instance.transform.localPosition = originalPos;
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
