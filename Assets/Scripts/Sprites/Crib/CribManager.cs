using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CribManager : SpriteManager
{
    #region Singleton
    public static CribManager Instance;
    void Awake() {
        Instance = this;
    }
    #endregion 

    Crib _currCrib;
    public Crib CurrCrib {
        get {
            return this._currCrib;
        }
    }

    protected override void Init() {
        base.Init();
        spriteSpawnBounds.SetSpawnBounds(-2.5f, 2.5f, -1f, 1f);
    }

    protected override void OnGameStart() {
        GameObject cribObj = SpawnCribAtRandomPosition();
        _currCrib = cribObj.GetComponent<Crib>();
    }

    public GameObject SpawnCribAtRandomPosition() {
        Vector2 spawnLoc = spriteSpawnBounds.GetRandomPositionWithinBounds();
        return SpawnSprite(spawnLoc, Quaternion.identity);
    }

}
