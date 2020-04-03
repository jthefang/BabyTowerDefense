using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomba : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    int maxBabyCapacity;

    Transform targetTransform;
    GameManager gameManager;

    [SerializeField]
    int _numBabiesPickedUp;
    public int NumBabiesPickedUp {
        get {
            return this._numBabiesPickedUp;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsPlaying) {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget() {
        bool hasActiveTarget = this.targetTransform != null && targetTransform.gameObject.activeSelf;
        if (hasActiveTarget) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, 				targetTransform.transform.position, Time.deltaTime * speed);
            return;
        } 

        if (hasReachedMaxBabyCapacity()) {
            TargetNearestCrib();
        } else {
            TargetNearestBaby();
        }
    }

    void TargetNearestCrib() {
        TargetNearestSpriteByManager(CribManager.Instance);
    }

    void TargetNearestBaby() {
        TargetNearestSpriteByManager(BabyManager.Instance);
    }

    void TargetNearestSpriteByManager(SpriteManager spriteManager) {
        GameObject minDistanceSprite = spriteManager.GetNearestActiveSpriteToPosition(this.transform.position);
        if (minDistanceSprite == null) {
            this.targetTransform = null;
            return;
        }

        this.targetTransform = minDistanceSprite.transform;
    }

    bool hasReachedMaxBabyCapacity() {
        return NumBabiesPickedUp >= maxBabyCapacity;
    }

    #region Collision
    void OnTriggerStay2D(Collider2D other) {
        if (GameManager.Instance.IsPlaying) {
            Crib crib = other.gameObject.GetComponent<Crib>();
            if (crib != null) {
                OnCribCollision(crib);
                return;
            } 
        }
    }

    void OnCribCollision(Crib crib) {
        UnloadBabies(crib);
        TargetNearestBaby();
    }

    void UnloadBabies(Crib crib) {
        this._numBabiesPickedUp = 0;
    }

    /**
        Returns true if Roomba was able to pickup the baby
            false otherwise
    */
    public bool PickupBaby(Baby baby) {
        if (hasReachedMaxBabyCapacity()) {
            return false;
        }

        this._numBabiesPickedUp++;
        return true;
    }
    #endregion

}
