using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsPage : MonoBehaviour {

    bool hasChanged = false;

    public void Start() {
        Toggle aiPlayer1 = GameObject.Find("ai_player1").GetComponent<Toggle>();
        Toggle aiPlayer2 = GameObject.Find("ai_player2").GetComponent<Toggle>();
        Toggle debug = GameObject.Find("debug").GetComponent<Toggle>();
        Slider level = GameObject.Find("Difficulty").GetComponent<Slider>();

        level.value = GlobalSettings.Instance.startingLevel;
        aiPlayer1.isOn = GlobalSettings.Instance.AI_Player_1;
        aiPlayer2.isOn = GlobalSettings.Instance.AI_Player_2;
        debug.isOn = GlobalSettings.Instance.DebugMode;
    }

    public void ChangeAudio() {
        Slider level = GameObject.Find("Master Volume Slider").GetComponent<Slider>();
        GlobalSettings.Instance.soundLevel = level.value;
    }

    public void ChangeDifficulty() {
        Slider level = GameObject.Find("Difficulty").GetComponent<Slider>();
        GlobalSettings.Instance.startingLevel = (int) level.value;
    }

    public void TriggerDebug() {
        GlobalSettings.Instance.DebugMode = GlobalSettings.Instance.DebugMode != true ? true : false;
    }

    public void TriggerPlayerAI(int player) {
        if (!hasChanged)
            StartCoroutine(doTrigger(player));
    }

    private IEnumerator doTrigger(int player) {
        hasChanged = true;
        if (player == 1)
            GlobalSettings.Instance.AI_Player_1 = GlobalSettings.Instance.AI_Player_1 != true ? true : false;
        else
            GlobalSettings.Instance.AI_Player_2 = GlobalSettings.Instance.AI_Player_2 != true ? true : false;

        yield return new WaitForSeconds(0.3f);
        hasChanged = false;
    }
}
