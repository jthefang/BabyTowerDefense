using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour, IPooledObject
{
    Transform targetTransform;
    float speed;
    Animator animator;

    BabyManager babyManager;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
    #endregion

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

    public void SetSpeed(float speed) {
        this.speed = speed;
        animator.SetFloat("Speed", speed);
    }

    void TargetRandomDoor() {
        List<Door> doors = TilemapInfo.Instance.Doors;
        if (doors == null || doors.Count <= 0) {
            return;
        }

        Door randDoor = doors[Random.Range(0, doors.Count)];
        SetTargetTransform(randDoor.gameObject.transform);
    }

    void TargetNearestDoor() {
        List<Door> doors = TilemapInfo.Instance.Doors;
        if (doors == null || doors.Count <= 0) {
            return;
        }

        float minDistanceSoFar = -1;
        Door minDistanceDoor = doors[0];
        foreach (Door d in doors) {
            float dist = Vector3.Distance(d.gameObject.transform.position, this.transform.position);
            bool minDistNotInitialized = minDistanceSoFar < 0;
            if (minDistNotInitialized || dist <= minDistanceSoFar) {
                minDistanceSoFar = dist; 
                minDistanceDoor = d;
            }
        }
        SetTargetTransform(minDistanceDoor.gameObject.transform);
    }

    void SetTargetTransform(Transform transform) {
        this.targetTransform = transform;
    }

}
