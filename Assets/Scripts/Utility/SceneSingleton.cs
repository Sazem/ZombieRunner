using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T> {
    
    private static T instance = null;

    public static T Instance {
        get {
            if(instance == null) {
                instance = FindObjectOfType<T>();
                if(instance == null) {
                    Debug.LogError("SceneSingleton: someone tried to access SceneSingleton, but it coulndt be found on the scene");
                }
            }
            return instance;
        }
    }
}
