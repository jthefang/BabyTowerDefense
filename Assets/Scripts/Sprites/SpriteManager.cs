using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpriteManager : MonoBehaviour, ILoadableScript, IDependentScript
{
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

    protected ObjectPooler objectPooler;
    protected GameManager gameManager;
    protected HashSet<GameObject> activeSprites;

    [SerializeField]
    protected string spriteTag;
    [SerializeField]
    protected bool hasSpriteSpawnBounds = false;
    protected SpriteSpawnBounds spriteSpawnBounds;
    
    void Start() {
        AddDependencies();
    }

    #region IDependentScript
    protected virtual void AddDependencies() {
        List<ILoadableScript> dependencies = new List<ILoadableScript>();
        dependencies.Add(ObjectPooler.Instance);
        dependencies.Add(TilemapInitialization.Instance);
        ScriptDependencyManager.Instance.UpdateDependencyDicts(this, dependencies);
    }

    public void OnAllDependenciesLoaded() {
        Init();
        isInitialized = true;
    }
    #endregion

    protected virtual void Init()
    {
        objectPooler = ObjectPooler.Instance;
        gameManager = GameManager.Instance;
        gameManager.OnGameStart += OnGameStart;
        activeSprites = new HashSet<GameObject>();

        if (spriteTag == null) {
            Debug.LogError(this.GetType().Name + " does not have a sprite tag set.");
        } else if (hasSpriteSpawnBounds) {
            spriteSpawnBounds = new SpriteSpawnBounds(spriteTag);
        }
    }

    protected virtual void OnGameStart() {
        //pass
    }

    public GameObject SpawnSprite(Vector3 position, Quaternion rotation) {
        GameObject spriteObj = objectPooler.SpawnFromPool(spriteTag, position, rotation);
        activeSprites.Add(spriteObj);
        return spriteObj;
    }

    public void DestroySprite(GameObject sprite) {
        if (!activeSprites.Contains(sprite)) {
            Debug.LogError("Sprite game object " + sprite.name + " is not an active sprite in SpriteManager " + this.GetType().Name);
            return;
        }
        objectPooler.DeactivateSpriteInPool(sprite);
    }

    public void DestroyAll() {
        foreach (GameObject sprite in activeSprites) {
            objectPooler.DeactivateSpriteInPool(sprite);
        }
        activeSprites = new HashSet<GameObject>();
    }

}

/**
    Handles calculating bounds in which the given sprite can spawn
*/
public class SpriteSpawnBounds {
    protected Vector2 boundsPadding; 
    float minX, maxX, minY, maxY;
    string spriteObjectPoolTag;

    public SpriteSpawnBounds(string spriteObjectPoolTag) {
        this.spriteObjectPoolTag = spriteObjectPoolTag;
        SetBoundsPadding(GetDefaultBoundsPadding());
        InitDefaultSpawnBounds();
    }

    /**
        Default padding ensures that sprite spawns completely within bounds
            i.e. = 1/2 size of sprite in x and y
    */
    Vector2 GetDefaultBoundsPadding() {
        GameObject spriteObject = ObjectPooler.Instance.GetSpritePrefab(spriteObjectPoolTag);
        Vector3 spriteSize = Util.GetSizeOfSprite(spriteObject); 
        return new Vector2(spriteSize.x / 2, spriteSize.y / 2);
    }

    public void SetBoundsPadding(Vector2 padding) {
        this.boundsPadding = padding;
    }

    /**
        Default spawn bounds = tilemap floor size - sprite padding
    */
    void InitDefaultSpawnBounds() {
        TilemapInfo tilemapInfo = TilemapInfo.Instance;
        Vector3 bottomLeft = tilemapInfo.GetWorldCoordinatesOfTilePosition(tilemapInfo.GetBottomLeftCornerTilePosition());
        Vector3 topRight = tilemapInfo.GetWorldCoordinatesOfTilePosition(tilemapInfo.GetTopRightCornerTilePosition());

        this.minX = bottomLeft.x;
        this.maxX = topRight.x;
        this.minY = bottomLeft.y;
        this.maxY = topRight.y;
    }

    public void SetSpawnBounds(float minX, float maxX, float minY, float maxY) {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
    }

    public Vector2 GetRandomPositionWithinBounds() {
        float x = UnityEngine.Random.Range(this.minX + boundsPadding.x, this.maxX - boundsPadding.x);
        float y = UnityEngine.Random.Range(this.minY + boundsPadding.y, this.maxY - boundsPadding.y);
        return new Vector2(x, y);
    }

}
