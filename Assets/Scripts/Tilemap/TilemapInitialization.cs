using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TilemapInitialization : MonoBehaviour, IDependentScript, ILoadableScript
{   
    #region Singleton
    public static TilemapInitialization Instance;
    void Awake() {
        Instance = this;
    }
    #endregion 

    #region ILoadableScript
    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    bool isInitialized {
        get {
            return this._isInitialized;
        }
        set {
            this._isInitialized = value;
            if (this._isInitialized) {
                OnScriptInitialized?.Invoke(this);
            }
        }   
    }
    public bool IsInitialized () {
        return isInitialized;
    }
    #endregion

    TilemapInfo tilemapInfo;
    Tilemap tilemap; 
    Tiles tiles;

    void Start() {
        tilemapInfo = TilemapInfo.Instance;
        tiles = Tiles.Instance;
        tilemap = tilemapInfo.GetTilemap();

        AddDependencies();
    }

    #region IDependentScript
    void AddDependencies() {
        List<ILoadableScript> dependencies = new List<ILoadableScript>();
        dependencies.Add(tilemapInfo);
        dependencies.Add(tiles);
        ScriptDependencyManager.Instance.UpdateDependencyDicts(this, dependencies);
    }

    public void OnAllDependenciesLoaded() {
        isInitialized = true;
    }
    #endregion

    public void InitNewTilemap() {
        tilemap.ClearAllTiles();
        InitFloor();
        DoorManager.Instance.SpawnNDoors(tilemapInfo.NumDoors);
    }

    void InitFloor() {
        Vector3Int bottomLeft = tilemapInfo.GetBottomLeftCornerTilePosition();
        Vector3Int topRight = tilemapInfo.GetTopRightCornerTilePosition();
        for (int r = bottomLeft.y; r < topRight.y; r++) {
            for (int c = bottomLeft.x; c < topRight.x; c++) {
                Vector3Int tilePosition = new Vector3Int(c, r, 0);
                tilemap.SetTile(tilePosition, tiles.GetTileByName("Floor"));
            }
        }
    }

}
