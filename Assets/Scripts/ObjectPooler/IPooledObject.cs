using UnityEngine;

public interface IPooledObject {
    void OnObjectInitiate(SpriteManager sm);
    void OnObjectSpawn();

    /**
        e.g.
        public class Baby : MonoBehaviour, IPooledObject {
            BabyManager babyManager;

            #region IPooledObject
            public void OnObjectInitiate(SpriteManager sm) {
                babyManager = (BabyManager) sm;
                this.transform.SetParent(sm.transform);
            }

            public void OnObjectSpawn() {
                SetSpeed(babyManager.BabySpeed);
                TargetNearestDoor();
            }
            #endregion
        }
    */
}
