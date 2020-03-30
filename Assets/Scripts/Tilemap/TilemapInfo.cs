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
    List<Door> _doors;
    public void SetDoors(List<Door> doors) {
        this._doors = doors;
    }
    // Positions of the doors in world coordinates
    public List<Door> Doors {
        get {
            return _doors;
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

    public Vector2Int GetBottomLeftCornerTilePosition() {
        int minX = -HouseDimensions.x / 2;
        int minY = -HouseDimensions.y / 2;
        return new Vector2Int(minX, minY);
    }

    public Vector2Int GetTopRightCornerTilePosition() {
        int maxX = HouseDimensions.x / 2;
        int maxY = HouseDimensions.y / 2;
        return new Vector2Int(maxX, maxY);
    }

    public Vector3 GetWorldCoordinatesOfTilePosition(Vector3Int tilePosition) {
        return GetGrid().CellToWorld(tilePosition);
    }

}
