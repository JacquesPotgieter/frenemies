using UnityEngine;
using System.Collections;

public class GlobalSettings : MonoBehaviour {

    public static GlobalSettings Instance = null;

    public bool AI_Player_1 = false;
    public bool AI_Player_2 = false;
    public bool DebugMode = false;

    public int startingLevel = 4;
    public int soundLevel = 100;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }
}
