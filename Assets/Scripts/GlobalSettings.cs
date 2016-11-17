using UnityEngine;
using System.Collections;

public class GlobalSettings : MonoBehaviour {

    public static GlobalSettings Instance = null;

    public bool AI_Player_1 = false;
    public bool AI_Player_2 = false;
    public bool DebugMode = false;

    public bool normal = true;
    public bool hiding = true;
    public bool predMovement = true;
    public bool predShooting = true;
    public bool swarm = true;
    public bool group = true;

    public int startingLevel = 4;
    public float soundLevel = 0.5f;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }
}
