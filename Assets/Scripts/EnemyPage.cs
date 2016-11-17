using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyPage : MonoBehaviour {

    bool hasChanged = false;

    public void Start() {
        Toggle normal = GameObject.Find("chk_normal").GetComponent<Toggle>();
        Toggle hiding = GameObject.Find("chk_hiding").GetComponent<Toggle>();
        Toggle predMovement = GameObject.Find("chk_predictivemovement").GetComponent<Toggle>();
        Toggle predShooting = GameObject.Find("chk_predictiveshooting").GetComponent<Toggle>();
        Toggle swarm = GameObject.Find("chk_swarm").GetComponent<Toggle>();
        Toggle group = GameObject.Find("chk_group").GetComponent<Toggle>();

        normal.isOn = GlobalSettings.Instance.normal;
        hiding.isOn = GlobalSettings.Instance.hiding;
        predMovement.isOn = GlobalSettings.Instance.predMovement;
        predShooting.isOn = GlobalSettings.Instance.predShooting;
        swarm.isOn = GlobalSettings.Instance.swarm;
        group.isOn = GlobalSettings.Instance.group;
    }

    public void TriggerType(int type) {
        if (type == 1)
            GlobalSettings.Instance.normal = GlobalSettings.Instance.normal != true ? true : false;
        else if (type == 2)
            GlobalSettings.Instance.hiding = GlobalSettings.Instance.hiding != true ? true : false;
        else if (type == 3)
            GlobalSettings.Instance.predMovement = GlobalSettings.Instance.predMovement != true ? true : false;
        else if (type == 4)
            GlobalSettings.Instance.predShooting = GlobalSettings.Instance.predShooting != true ? true : false;
        else if (type == 5)
            GlobalSettings.Instance.swarm = GlobalSettings.Instance.swarm != true ? true : false;
        else
            GlobalSettings.Instance.group = GlobalSettings.Instance.group != true ? true : false;

    }
}
