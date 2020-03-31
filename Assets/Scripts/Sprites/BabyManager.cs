using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BabyManager : SpriteManager
{
    #region Singleton
    public static BabyManager Instance;
    void Awake() {
        Instance = this;
    }
    #endregion 

    [SerializeField]
    float _babySpeed = 3f;
    public float BabySpeed {
        get {
            return _babySpeed;
        }
    }

    [SerializeField]
    int spawnAmount = 1;
    [SerializeField]
    float spawnDelay = 5.0f;
    float timeTilSpawn;
    [SerializeField]
    bool shouldSpawn;

    CribManager cribManager;

    protected override void Init() {
        base.Init();
        cribManager = CribManager.Instance;
    }

    protected override void OnGameStart() {
        SpawnBabies();
        ResetSpawnCountdown();
        shouldSpawn = true;
    }

    void Update() {
        if (IsInitialized() && gameManager.IsPlaying && shouldSpawn) {
            Debug.Log(timeTilSpawn);
            this.timeTilSpawn -= Time.deltaTime;
            if (this.timeTilSpawn < 0) {
                SpawnBabies();
                ResetSpawnCountdown();
            }    
        }
    }

    void SpawnBabies() {
        for (int i = 0; i < this.spawnAmount; i++) {
            Vector3 cribPosition = cribManager.CurrCrib.gameObject.transform.position;
            SpawnSprite(cribPosition, Quaternion.identity);
        }
    }

    void ResetSpawnCountdown() {
        this.timeTilSpawn = this.spawnDelay;
    }

}
