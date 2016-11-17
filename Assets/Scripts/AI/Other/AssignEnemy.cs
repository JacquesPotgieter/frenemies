using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Assets.Scripts.AI.Controllers;
using Panda;

public class AssignEnemy : MonoBehaviour {

    private static string[] animations = { "EnemyOrgeFemale", "EnemyOrgeMale" };

    public static Enemy run(Vector2 position) {
        Enemy enemy = SelectType(position);
        enemy.enabled = false;

        int healthPoints = getHealthPoints();
        int damageDealt = getDamageDealt();
        RuntimeAnimatorController animator = getAnimatorController();

        enemy.init(healthPoints, damageDealt, animator);
        enemy.enabled = true;
        return enemy;
    }

    private static Enemy SelectType(Vector2 position) {
        ArrayList enemyTypes = new ArrayList();
        if (GlobalSettings.Instance.hiding)
            enemyTypes.Add("EnemyHiding");
        if (GlobalSettings.Instance.predMovement)
            enemyTypes.Add("EnemyPredictiveMovement");
        if (GlobalSettings.Instance.predShooting)
            enemyTypes.Add("EnemyPredictiveShooting");
        if (GlobalSettings.Instance.swarm)
            enemyTypes.Add("EnemySwarm");
        if (GlobalSettings.Instance.group)
            enemyTypes.Add("EnemyGroup");
        if (GlobalSettings.Instance.normal || enemyTypes.Count < 1)
            enemyTypes.Add("EnemyNormal");

        int pos = Random.Range(0, enemyTypes.Count);
        GameObject instance = Instantiate(Resources.Load("Enemy/" + enemyTypes[pos])) as GameObject;
        instance.transform.position = position;
        GameManager.Instance.AddEnemyToList(instance.GetComponent<Enemy>());

        return instance.GetComponent<Enemy>();
    }

    private static RuntimeAnimatorController getAnimatorController() {
        int pos = Random.Range(0, animations.Length);
        return Instantiate(Resources.Load("Animations/" + animations[pos])) as RuntimeAnimatorController;
    }

    private static int getHealthPoints() {
        return 20;
    }

    private static int getDamageDealt() {
        return 5;
    }
}
