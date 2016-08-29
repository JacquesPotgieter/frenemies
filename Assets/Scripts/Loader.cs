using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject GameManager;			//GameManager prefab to instantiate.
    //public GameObject soundManager;			//SoundManager prefab to instantiate.

    void Awake() {
        if (global::GameManager.Instance == null)
            Instantiate(GameManager);
    }
}
