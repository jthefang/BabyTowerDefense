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
    
    #region IDependentScript
    protected virtual void AddDependencies() {
        List<ILoadableScript> dependencies = new List<ILoadableScript>();
        dependencies.Add(ObjectPooler.Instance);
        ScriptDependencyManager.Instance.UpdateDependencyDicts(this, dependencies);
    }

    public void OnAllDependenciesLoaded() {
        Init();
        isInitialized = true;
    }
    #endregion

    ObjectPooler objectPooler;
    List<GameObject> activeSprites;

    protected virtual void Init()
    {
        objectPooler = ObjectPooler.Instance;
        activeSprites = new List<GameObject>();
    }

    public void DestroyAll() {
        foreach (GameObject sprite in activeSprites) {
            objectPooler.DeactivateSpriteInPool(sprite);
        }
        activeSprites = new List<GameObject>();
    }

}
