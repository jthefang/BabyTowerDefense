using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : SpriteManager
{
    #region Singleton
    public static DoorManager Instance;
    void Awake() {
        Instance = this;
    }
    #endregion 

    /**
        Randomly spawns n doors
        Possible door tile positions include any position on the top/bot rows and left/right cols
    */
    public void SpawnNDoors(int n) {
        TilemapInfo tilemapInfo = TilemapInfo.Instance;
        Vector3Int bottomLeft = tilemapInfo.GetBottomLeftCornerTilePosition();
        Vector3Int topRight = tilemapInfo.GetTopRightCornerTilePosition();
        
        for (int i = 0; i < n; i++) {
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
            SpawnSprite(doorWorldPosition, Quaternion.identity);
        }
    }
}
