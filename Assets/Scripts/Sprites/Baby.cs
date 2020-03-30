using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    Transform targetTransform;
    float speed;
    Animator animator;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        gameManager.OnGameStart += OnGameStart;
    }

    void OnGameStart() {
        //temporary
        SetSpeed(3f);
        TargetRandomDoor();
    }

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

    void SetTargetTransform(Transform transform) {
        this.targetTransform = transform;
    }

}
