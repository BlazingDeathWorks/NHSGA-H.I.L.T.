using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Blade_ht : MonoBehaviour
{
    [SerializeField] private Animator bladeAnimator;
    [SerializeField] private PlayerAudioController audioController;
    [SerializeField] private Volume volume;
    [SerializeField] private string currentBladeName;
    [SerializeField] private Transform slashSpawnPoint;
    [SerializeField] private Blade[] blades;
    private Blade currentBlade;
    private Vector2 originalPos;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
        bladeAnimator.runtimeAnimatorController = currentBlade.Ac;
        sr.material = currentBlade.Costume;
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
        audioController.PlaySlashSound1();
        Slash instance = Instantiate(currentBlade.BaseAttackSlash);
        originalPos = instance.transform.position;
        instance.transform.parent = slashSpawnPoint;
        instance.transform.localPosition = originalPos;
        instance.transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x) * Mathf.Abs(instance.transform.localScale.x), instance.transform.localScale.y, instance.transform.localScale.z);
        if (Mathf.Sign(instance.transform.localScale.x) == -1) instance.transform.localScale = new Vector3(-instance.transform.localScale.x, instance.transform.localScale.y, instance.transform.localScale.z);
    }
    public void ThreeHitAttackSlash()
    {
        if (currentBlade.ThreeHitAttackSlash == null) return;
        audioController.PlaySlashSound2();
        Slash instance = Instantiate(currentBlade.ThreeHitAttackSlash);
        originalPos = instance.transform.position;
        instance.damageMod = 1.5f;
        instance.transform.parent = slashSpawnPoint;
        instance.transform.localPosition = originalPos;
        instance.transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x) * Mathf.Abs(instance.transform.localScale.x), instance.transform.localScale.y, instance.transform.localScale.z);
        if (Mathf.Sign(instance.transform.localScale.x) == -1) instance.transform.localScale = new Vector3(-instance.transform.localScale.x, instance.transform.localScale.y, instance.transform.localScale.z);
    }
    public void AirAttackSlash()
    {
        if (currentBlade.AirAttackSlash == null) return;
        audioController.PlaySlashSound1();
        Slash instance = Instantiate(currentBlade.AirAttackSlash);
        originalPos = instance.transform.position;
        instance.transform.parent = slashSpawnPoint;
        instance.transform.localPosition = originalPos;
        instance.GetComponent<SpriteRenderer>().flipX = Mathf.Sign(transform.localScale.x) == 1 ? false : true;
    }
}

[System.Serializable]
public struct Blade
{
    public string BladeName => bladeName;
    [SerializeField] private string bladeName;

    public int HueShiftValue => hueShiftValue;
    [SerializeField] private int hueShiftValue;

    public Material Costume => costume;
    [SerializeField] private Material costume;

    public RuntimeAnimatorController Ac => ac;
    [SerializeField] private RuntimeAnimatorController ac;

    //Slash Variables Expand As More Moves Are Created
    public Slash BaseAttackSlash => baseAttackSlash;
    public Slash ThreeHitAttackSlash => threeHitAttackSlash;
    public Slash AirAttackSlash => airAttackSlash;
    [SerializeField] private Slash baseAttackSlash;
    [SerializeField] private Slash threeHitAttackSlash;
    [SerializeField] private Slash airAttackSlash;
}
