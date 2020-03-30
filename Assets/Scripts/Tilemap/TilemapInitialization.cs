using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapInitialization : MonoBehaviour, IDependentScript
{
    [SerializeField]
    GameObject doorPrefab;

    TilemapInfo tilemapInfo;
    Tilemap tilemap; 
    Tiles tiles;

    void Start() {
        tilemapInfo = TilemapInfo.Instance;
        tiles = Tiles.Instance;
        tilemap = tilemapInfo.GetTilemap();

        List<ILoadableScript> dependencies = new List<ILoadableScript>();
        dependencies.Add(tilemapInfo);
        dependencies.Add(tiles);
        ScriptDependencyManager.Instance.UpdateDependencyDicts(this, dependencies);
    }

    public void OnAllDependenciesLoaded() {
        Init();
    }

    void Init() {
        InitTilemap();
    }

    void InitTilemap() {
        tilemap.ClearAllTiles();
        InitFloor();
        InitDoors();
    }

    void InitFloor() {
        Vector2Int bottomLeft = tilemapInfo.GetBottomLeftCornerTilePosition();
        Vector2Int topRight = tilemapInfo.GetTopRightCornerTilePosition();

        Debug.Log("Bottom left: " + bottomLeft);
        Debug.Log("Top right: " + topRight);
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
        Vector2Int bottomLeft = tilemapInfo.GetBottomLeftCornerTilePosition();
        Vector2Int topRight = tilemapInfo.GetTopRightCornerTilePosition();
        for (int i = 0; i < tilemapInfo.NumDoors; i++) {
            int randRow = Random.Range(bottomLeft.y + 1, topRight.y - 1);
            int randCol = Random.Range(bottomLeft.x + 1, topRight.x - 1);
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
            Door newDoor = InitDoorAt(tilemapInfo.GetWorldCoordinatesOfTilePosition(doorTilePosition));
        }
    }

    Door InitDoorAt(Vector3 position) {
        GameObject doorObj = Instantiate(doorPrefab, position, Quaternion.identity);
        return doorObj.GetComponent<Door>();
    }

}
