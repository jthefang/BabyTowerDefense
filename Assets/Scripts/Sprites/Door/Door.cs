using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IPooledObject
{
    DoorManager doorManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #region IPooledObject
    public void OnObjectInitiate(SpriteManager sm) {
        doorManager = (DoorManager) sm;
        this.transform.SetParent(sm.transform);
    }

    public void OnObjectSpawn() {
        //pass
    }
    #endregion
}
