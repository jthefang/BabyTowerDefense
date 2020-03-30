using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class Tiles : MonoBehaviour, ILoadableScript
{
    #region Singleton
    public static Tiles Instance;
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
    List<Tile> tiles;
    Dictionary<string, TileBase> tileByName;

    // Start is called before the first frame update
    void Start()
    {
        InitTileByNameDictionary();
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitTileByNameDictionary() {
        tileByName = new Dictionary<string, TileBase>();
        foreach (Tile t in tiles) {
            tileByName.Add(t.GetName(), t.GetTile());
        }
    }

    public TileBase GetTileByName(string tileName) {
        if (!tileByName.ContainsKey(tileName)) {
            return null;
        }
        return tileByName[tileName];
    }

}

[Serializable]
public class Tile {
    [SerializeField]
    string name;
    [SerializeField]
    TileBase tile;

    public string GetName() {
        return name;
    }

    public TileBase GetTile() {
        return tile;
    }
}