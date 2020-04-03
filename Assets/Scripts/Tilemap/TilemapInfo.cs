using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TilemapInfo : MonoBehaviour, ILoadableScript
{
    #region Singleton
    public static TilemapInfo Instance;
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

    [SerializeField]
    Vector2Int _houseDimensions;
    public Vector2Int HouseDimensions {
        get {
            return _houseDimensions;
        }
    }
    
    #region Doors
    [SerializeField]
    int _numDoors; 
    public int NumDoors {
        get {
            return _numDoors;
        }
    }
    #endregion

    void Start() {
        Init();
    }

    void Init() {
        isInitialized = true;
    }

    public Grid GetGrid() {
        return this.GetComponent<Tilemap>().layoutGrid; 
    }

    public Tilemap GetTilemap() {
        return this.GetComponent<Tilemap>();
    }

    public Vector3Int GetBottomLeftCornerTilePosition() {
        int minX = -HouseDimensions.x / 2;
        int minY = -HouseDimensions.y / 2;
        return new Vector3Int(minX, minY, 0);
    }

    public Vector3Int GetTopRightCornerTilePosition() {
        int maxX = HouseDimensions.x / 2;
        int maxY = HouseDimensions.y / 2;
        return new Vector3Int(maxX, maxY, 0);
    }

    public Vector3 GetWorldCoordinatesOfTilePosition(Vector3Int tilePosition) {
        return GetGrid().CellToWorld(tilePosition);
    }

}
