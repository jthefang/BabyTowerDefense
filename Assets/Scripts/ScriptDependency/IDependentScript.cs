using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    A script that depends on other scripts to load first
*/
public interface IDependentScript {
    void OnAllDependenciesLoaded();

    /**
        e.g. 
        #region IDependentScript
        protected virtual void AddDependencies() {
            List<ILoadableScript> dependencies = new List<ILoadableScript>();
            dependencies.Add(ObjectPooler.Instance);
            ScriptDependencyManager.Instance.UpdateDependencyDicts(this, dependencies);
        }

        public void OnAllDependenciesLoaded() {
            Init();
            isInitialized = true;
        }
        #endregion

        OR 
        
        public class PlayerController2D : MonoBehaviour, IDependentScript {
            #region IDependentScript
            protected virtual void AddDependencies() {
                List<ILoadableScript> dependencies = new List<ILoadableScript>();
                dependencies.Add(propulsion);
                ScriptDependencyManager.Instance.UpdateDependencyDicts(this, dependencies);
            }

            public void OnAllDependenciesLoaded() {
                propulsionLoaded = true;
            }
            #endregion
            
            bool propulsionLoaded;

            void Start() {
                ...
                AddDependencies();
                propulsionLoaded = propulsion.IsInitialized();;
            }

            void FixedUpdate() {
                if (!propulsionLoaded) {
                    return;
                }
                ...
            }
            ...
        }

        OR use the Unity editor (preferred)
    */
}
