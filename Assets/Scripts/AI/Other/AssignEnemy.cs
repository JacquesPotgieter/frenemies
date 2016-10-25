using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Assets.Scripts.AI.Controllers;
using Panda;

public class AssignEnemy : MonoBehaviour {

    private static string[] animations = { "EnemyOrgeFemale", "EnemyOrgeMale" };
    private static string[] enemyTypes = { "EnemyNormal", "EnemyHiding", "EnemyPredictiveMovement", "EnemyPredictiveShooting", "EnemySwarm" , "EnemyFlankOne", "EnemyFlankTwo" };

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
        int pos = Random.Range(0, enemyTypes.Length);
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
