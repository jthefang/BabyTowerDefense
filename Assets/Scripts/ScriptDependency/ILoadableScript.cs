using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface ILoadableScript {
    // should be triggered after the script is loaded
    event Action<ILoadableScript> OnScriptInitialized; 
    bool IsInitialized(); //dependent scripts will only wait on LoadableScripts where this is false (haven't been initialized yet), see ScriptDependencyManager.GenerateDependencyDict

    /** 
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

    void Start()
    {
        ...
        isInitialized = true;
    }
    */
}
