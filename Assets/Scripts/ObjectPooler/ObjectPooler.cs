using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
    Attach to an empty GameObject called `ObjectPooler`
        - Add Pools in the Unity editor for prefabs 

    Spawn objects from the ObjectPooler by:
        ObjectPooler.Instance.SpawnFromPool([tag], [spawn position], [spawn quaternion]);
    'Kill' objects that are in an object pool by deactivating them:
        ObjectPooler.Instance.DeactivateSpriteInPool([game object]);

    For each prefab you want to have an ObjectPool for, you can optionally make a SpriteManager for the prefab (e.g. BabyManager subclassing SpriteManager)
        - Make sure the prefab has a component that subclasses IPooledObject
        - When the prefab is first instantiated, we'll call IPooledObject.OnObjectInitiate(SpriteManager sm)
            - here you can make the object parented by SpriteManager (for organization purposes in the Unity hierarchy)
        - When the object is spawned, we'll call IPooledObject.OnObjectSpawn()
        e.g. public class Baby : MonoBehaviour, IPooledObject {
            BabyManager babyManager;
            public void OnObjectInitiate(SpriteManager sm) {
                babyManager = (BabyManager) sm;
                this.transform.SetParent(sm.transform);
            }
            public void OnObjectSpawn()  {
                this.targetDoor = RandomDoor();
                this.speed = babyManager.BabySpeed;
            }
        }
*/
public class ObjectPooler : MonoBehaviour, ILoadableScript {

    [Serializable] 
    public class Pool { 
        public string Tag;
        public GameObject Prefab;
        public SpriteManager SpriteManager;
        public int MaxNumber; //at this point we'll start reusing objects instead of instantiating new ones

        int _numActive = 0;
        public int NumActive {
            get {
                return this._numActive;
            }
            set {
                this._numActive = value;
            }
        }

        Queue<GameObject> queue;
        public Queue<GameObject> GetQueue() {
            return queue;
        }
        public void SetQueue(Queue<GameObject> q) {
            queue = q;
        }
    }

    #region Singleton
    public static ObjectPooler Instance;
    private void Awake() {
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

    public List<Pool> pools;
    Dictionary<string, Pool> tagToPool;
    Dictionary<GameObject, string> objectToTag;

    void Start() {
        Init();
    }

    void Init() {
        InitPools();
        isInitialized = true;
    }

    void InitPools() {
        tagToPool = new Dictionary<string, Pool>();
        objectToTag = new Dictionary<GameObject, string>();

        foreach (Pool pool in pools)  {
            tagToPool[pool.Tag] = pool;

            // create a Q for each pool
            Queue<GameObject> objectPool = new Queue<GameObject>();
            pool.SetQueue(objectPool);

            // populate Q with `size` objects
            for (int i = 0; i < pool.MaxNumber; i++) {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false); //not yet in the game
                objectPool.Enqueue(obj);
                objectToTag[obj] = pool.Tag;
                
                IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
                if (pooledObj != null) {
                    pooledObj.OnObjectInitiate(pool.SpriteManager);
                }
            }
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {
        if (!tagToPool.ContainsKey(tag)) {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        Queue<GameObject> objectQ = tagToPool[tag].GetQueue();
        GameObject objectToSpawn = objectQ.Dequeue();

        if (!objectToSpawn.activeSelf) { //else we're reusing already active object (i.e. reached max)
            tagToPool[tag].NumActive += 1;
        }
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null) {
            pooledObj.OnObjectSpawn();
        }

        objectQ.Enqueue(objectToSpawn); //if we reach our max number of objects, reuse this guy
        return objectToSpawn;
    }

    public GameObject GetSpritePrefab(string tag) {
        return tagToPool[tag].Prefab;
    }

    /**
        Deactivate/kills a sprite spawned from a pool maintained by this object pooler
    */
    public void DeactivateSpriteInPool(GameObject spriteObject) {
        if (!objectToTag.ContainsKey(spriteObject)) {
            Debug.LogError("Trying to deactivate a sprite game object that does not belong to this object pooler.");
            return;
        }

        spriteObject.SetActive(false);
        string spriteTag = objectToTag[spriteObject];
        tagToPool[spriteTag].NumActive -= 1;
    }

    public bool AllObjectsActiveForPool(string poolTag) {
        Pool p = tagToPool[poolTag];
        return p.NumActive >= p.MaxNumber;
    }

}
