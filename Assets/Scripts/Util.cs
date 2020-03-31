using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {
    public static void RemoveZRotation(Transform transform) {
        Quaternion q = transform.rotation;
        q.eulerAngles = new Vector3(0, 0, q.eulerAngles.z);
        transform.rotation = q;
    }

    public static Vector3 GetSizeOfSprite(GameObject spriteObject) {
        SpriteRenderer spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            Debug.LogError("Util.GetSizeOfSprite: Cannot get size for sprite object '" + spriteObject.name + "' with no SpriteRenderer");
            return new Vector3(0, 0, 0);
        }
        return spriteRenderer.bounds.size;
    }
}