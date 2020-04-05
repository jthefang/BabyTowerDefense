using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

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
    AudioSource musicSource;
    [SerializeField]
    AudioSource soundFXSource;
    GameManager gameManager;

    void Start() {
        gameManager = GameManager.Instance;
        gameManager.OnGameStart += OnGameStart;

        isInitialized = true;
    }

    void OnGameStart() {
        PlayMainTheme();
    }

    public void PlayMainTheme() {
        musicSource.Play();
    }

    public void PlaySoundEffect(SoundEffect soundEffect) {
        if (soundEffect == null) {
            return;
        }
        
        soundFXSource.volume = soundEffect.volume;
        soundFXSource.pitch = soundEffect.pitch;
        soundFXSource.PlayOneShot(soundEffect.sound);
    }

    public void PlaySoundEffectWithRandomPitchInRange(SoundEffect soundEffect, float minPitch, float maxPitch) {
        if (soundEffect == null) {
            return;
        }
        
        soundFXSource.volume = soundEffect.volume;
        soundFXSource.pitch = Random.Range(minPitch, maxPitch);
        soundFXSource.PlayOneShot(soundEffect.sound);
    }

}

