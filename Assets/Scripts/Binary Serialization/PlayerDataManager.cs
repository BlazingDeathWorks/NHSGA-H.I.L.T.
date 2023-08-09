using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }
    public bool LoadLoot => loadLoot;
    [SerializeField] private bool loadLoot;
    private const string FILE_NAME = "/player.bin";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SavePlayerData(List<PropertyNavigationButton> pnbs)
    {
        PlayerData data = new PlayerData(pnbs);
        BinarySaveSystem.SaveSystem(data, Application.persistentDataPath + FILE_NAME);
    }

    public PlayerData LoadPlayerData()
    {
        return BinarySaveSystem.LoadSystem<PlayerData>(Application.persistentDataPath + FILE_NAME);
    }
}
