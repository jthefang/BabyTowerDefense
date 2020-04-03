using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crib : MonoBehaviour, IPooledObject
{
    CribManager cribManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    #region IPooledObject
    public void OnObjectInitiate(SpriteManager sm) {
        cribManager = (CribManager) sm;
        this.transform.SetParent(sm.transform);
    }

    public void OnObjectSpawn() {
        //pass
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
