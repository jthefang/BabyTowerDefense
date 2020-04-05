using UnityEngine;

[CreateAssetMenu(menuName="Sound/SoundEffect")]
public class SoundEffect : ScriptableObject {
    public AudioClip sound;
    public string name;

    public float volume = 1f;
    public float pitch = 1f;

    public void PlayOnAudioSource(AudioSource source) {
        if (sound == null) {
            return;
        }

        source.clip = sound;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
    }
}