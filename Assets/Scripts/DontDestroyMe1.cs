using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMe1 : MonoBehaviour
{
    public static DontDestroyMe1 Instance;
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
