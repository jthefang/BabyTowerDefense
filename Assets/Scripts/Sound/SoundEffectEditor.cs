using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundEffect), true)]
public class SoundEffectEditor : Editor {
    [SerializeField]
    private AudioSource _hiddenAudioPreviewer;

    private SerializedProperty sound;
    private SerializedProperty name;
    private SerializedProperty volume;
    private SerializedProperty pitch;

    public void OnEnable() {
        _hiddenAudioPreviewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();

        sound = serializedObject.FindProperty("sound");
        name = serializedObject.FindProperty("name");
        volume = serializedObject.FindProperty("volume");
        pitch = serializedObject.FindProperty("pitch");
    }

    public void OnDisable() {
        DestroyImmediate(_hiddenAudioPreviewer.gameObject);
    }

    public override void OnInspectorGUI() {
        //DrawDefaultInspector();
        serializedObject.UpdateIfDirtyOrScript();

        EditorGUILayout.ObjectField(sound, typeof(AudioClip), new GUIContent("Sound"));
        EditorGUILayout.PropertyField(name, new GUIContent("Name"));
        EditorGUILayout.Slider(volume, 0, 2f, "Volume");
        EditorGUILayout.Slider(pitch, 0, 2f, "Pitch");

        if (GUILayout.Button("Preview")) {
            ((SoundEffect) target).PlayOnAudioSource(_hiddenAudioPreviewer);
        }

        serializedObject.ApplyModifiedProperties();
    }
}