using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour, ILoadableScript
{
    #region Singleton
    public static SoundManager Instance;
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

    [SerializeField]
    List<SoundEffect> soundEffects;
    Dictionary<string, SoundEffect> nameToSoundEffect;

    AudioSource audioSource;
    GameManager gameManager;

    void Start() {
        audioSource = this.GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
        gameManager.OnGameStart += OnGameStart;

        InitNameToSoundEffectsDict();
        isInitialized = true;
    }

    void InitNameToSoundEffectsDict() {

    }

    void OnGameStart() {
        PlayMainTheme();
    }

    public void PlayMainTheme() {
        audioSource.Play();
    }

}

[Serializable]
public class SoundEffect {
    [SerializeField]
    string name;
    [SerializeField]
    AudioClip audioClip; 
}
