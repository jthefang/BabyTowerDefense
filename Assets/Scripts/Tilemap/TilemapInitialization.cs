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

    [SerializeField]
    GameObject doorPrefab;

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
        InitDoors();
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

    /**
        Randomly initializes numDoor tile positions for doors

        Possible door tile positions include any position on the top/bot rows and left/right cols
    */
    void InitDoors() {
        List<Door> doors = new List<Door>();

        Vector3Int bottomLeft = tilemapInfo.GetBottomLeftCornerTilePosition();
        Vector3Int topRight = tilemapInfo.GetTopRightCornerTilePosition();
        for (int i = 0; i < tilemapInfo.NumDoors; i++) {
            int randRow = UnityEngine.Random.Range(bottomLeft.y + 1, topRight.y - 1);
            int randCol = UnityEngine.Random.Range(bottomLeft.x + 1, topRight.x - 1);
            int row = randRow;
            int col = randCol;
            switch (i % 4) {
                case 0: //top
                    row = topRight.y;
                    break;
                case 1: //bot
                    row = bottomLeft.y;
                    break;
                case 2: //left
                    col = bottomLeft.x;
                    break;
                case 3: //right
                    col = topRight.x;
                    break;
            }

            Vector3Int doorTilePosition = new Vector3Int(col, row, 0);
            Vector3 doorWorldPosition = tilemapInfo.GetWorldCoordinatesOfTilePosition(doorTilePosition);
            Door newDoor = InitDoorAt(doorWorldPosition);
            doors.Add(newDoor);
        }

        tilemapInfo.SetDoors(doors);
    }

    Door InitDoorAt(Vector3 position) {
        GameObject doorObj = Instantiate(doorPrefab, position, Quaternion.identity);
        return doorObj.GetComponent<Door>();
    }

}
