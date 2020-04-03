using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour, IPooledObject
{
    Transform targetTransform;
    float speed;
    Animator animator;

    BabyManager babyManager;

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget() {
        if (this.targetTransform != null) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, 				targetTransform.transform.position, Time.deltaTime * speed);
        }
    }

    #region IPooledObject
    public void OnObjectInitiate(SpriteManager sm) {
        babyManager = (BabyManager) sm;
        this.transform.SetParent(sm.transform);
        animator = GetComponent<Animator>();
    }

    public void OnObjectSpawn() {
        SetSpeed(babyManager.BabySpeed);
        TargetRandomDoor();
        FaceTargetDoor();
    }
    #endregion
    
    public void SetSpeed(float speed) {
        this.speed = speed;
        animator.SetFloat("Speed", speed);
    }

    void TargetRandomDoor() {
        GameObject randomDoor = DoorManager.Instance.GetRandomActiveSprite();
        if (randomDoor == null) {
            return;
        }
        
        this.targetTransform = randomDoor.gameObject.transform;
    }

    void TargetNearestDoor() {
        TargetNearestSpriteByManager(DoorManager.Instance);
    }

    void TargetNearestSpriteByManager(SpriteManager spriteManager) {
        GameObject minDistanceSprite = spriteManager.GetNearestActiveSpriteToPosition(this.transform.position);
        if (minDistanceSprite == null) {
            return;
        }

        this.targetTransform = minDistanceSprite.transform;
    }

    void FaceTargetDoor() {
        Vector3 targetDirection = targetTransform.position - this.transform.position;
        targetDirection.Normalize();
        //this.transform.right = targetDirection; //rotates baby to face target direction
        
        bool facingLeft = targetDirection.x < 0;
        if (facingLeft) {
            this.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    #region Collision
    void OnTriggerEnter2D(Collider2D other) {
        if (GameManager.Instance.IsPlaying) {
            if (other.gameObject.GetComponent<Door>() != null) {
                OnDoorCollision();
                return;
            } 
            
            Roomba roomba = other.gameObject.GetComponent<Roomba>();
            if (roomba != null) {
                OnRoombaCollision(roomba);
                return;
            }
        }
    }

    void OnDoorCollision() {
        //just escape for now (back into the ObjectPool)
        babyManager.DestroySprite(this.gameObject);
    }

    void OnRoombaCollision(Roomba roomba) {
        //just disappear for now (back into the ObjectPool)
        bool pickedUpBaby = roomba.PickupBaby(this);
        if (pickedUpBaby) {
            babyManager.DestroySprite(this.gameObject);
        }
    }
    #endregion

}
