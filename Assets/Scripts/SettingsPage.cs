using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsPage : MonoBehaviour {

    bool hasChanged = false;

    public void Awake() {
        Toggle aiPlayer1 = GameObject.Find("ai_player1").GetComponent<Toggle>();
        Toggle aiPlayer2 = GameObject.Find("ai_player2").GetComponent<Toggle>();

        aiPlayer1.isOn = GlobalSettings.Instance.AI_Player_1;
        aiPlayer2.isOn = GlobalSettings.Instance.AI_Player_2;
    }

    public void TriggerPlayerAI(int player) {
        StartCoroutine(doTrigger(player));
    }

    private IEnumerator doTrigger(int player) {
        if (!hasChanged) {
            hasChanged = true;
            if (player == 1)
                GlobalSettings.Instance.AI_Player_1 = GlobalSettings.Instance.AI_Player_1 != true ? true : false;
            else
                GlobalSettings.Instance.AI_Player_2 = !GlobalSettings.Instance.AI_Player_2 != true ? true : false;

            Debug.Log(GlobalSettings.Instance.AI_Player_1);
        }

        yield return new WaitForSeconds(0.3f);
        hasChanged = false;
    }
}
