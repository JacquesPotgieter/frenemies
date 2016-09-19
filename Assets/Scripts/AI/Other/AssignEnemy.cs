using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Assets.Scripts.AI.Controllers;

public class AssignEnemy : MonoBehaviour {

    public static void run(Enemy enemy) {
        int healthPoints = getHealthPoints();
        int damageDealt = getDamageDealt();
        RuntimeAnimatorController animator = getAnimatorController();
        AI_Controller controller = getController();
        controller.currentObject = enemy;

        enemy.init(controller, healthPoints, damageDealt, animator);

        Debug.Log(animator);
    }

    private static RuntimeAnimatorController getAnimatorController() {
        string asset = "AnimationsControllers/Characters";
        Debug.Log(Path.GetDirectoryName(asset));
        return null; //(RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load(asset, typeof(RuntimeAnimatorController)));
    }

    private static int getHealthPoints() {
        return 20;
    }

    private static int getDamageDealt() {
        return 5;
    }

    private static AI_Controller getController() {
        //float ran = Random.value;
        //if (ran < .5)
        //    return new EnemyNormalController();
        //else
        //    return new EnemySwarm_controller();
        return new EnemyPatrol_Controller();
    }
}
