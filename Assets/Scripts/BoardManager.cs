using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {

    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class Count {
        public int Minimum;             //Minimum value for our Count class.
        public int Maximum;             //Maximum value for our Count class.

        //Assignment constructor.
        public Count(int min, int max) {
            Minimum = min;
            Maximum = max;
        }
    }

    public int BoardWidth = 30;
    public int BoardHeight = 16;

    public static Count WallCount = new Count(10, 30);
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] OuterWallTiles;

    private Transform _boardHolder;
    
    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition() {
        int randomX = Random.Range(1, BoardWidth + 1);
        int randomY = Random.Range(1, BoardHeight + 1);
        Vector2 position = new Vector2(randomX, randomY);
        return position - new Vector2(0.5f, 0.5f);
    }


    IEnumerator CreateEnemies(Transform boardHolder, List<Vector2> positions, float timeDelay, List<ParticleSystem> particles) {
        yield return new WaitForSeconds(timeDelay);

        foreach (Vector2 position in positions) {
            Enemy instance = AssignEnemy.run(position);

            instance.transform.SetParent(boardHolder);

            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            GameObject healthInfo = Instantiate(Resources.Load("HealthInfo")) as GameObject;
            healthInfo.transform.SetParent(canvas.transform);
            instance.healthText = healthInfo.GetComponent<Text>();

        }

        foreach (ParticleSystem cur in particles)
            Destroy(cur.gameObject);
    }

    List<ParticleSystem> createSpawnParticles(List<Vector2> enemies) {
        List<ParticleSystem> particles = new List<ParticleSystem>();

        foreach (Vector2 curEnemy in enemies) {
            GameObject curParticleObj = Instantiate(Resources.Load("SpawnParticle")) as GameObject;
            ParticleSystem curParticle = curParticleObj.GetComponent<ParticleSystem>();
            float x = curEnemy.x;
            float y = curEnemy.y - 0.5f;
            curParticle.transform.position = new Vector2(x, y);

            particles.Add(curParticle);
        }

        return particles;
    }

    List<Vector2> ChooseSpawnSpots(int numberEnemies) {
        List<Vector2> enemies = new List<Vector2>();

        for (int i = 0; i < numberEnemies; i++) {
            Vector3 randomPosition;
            Node node = null;
            while (node == null) {
                randomPosition = RandomPosition();
                Point point = GameManager.Instance.grid.WorldToGrid(randomPosition);
                node = GameManager.Instance.grid.Nodes[point.X, point.Y];

                if (node.BadNode)
                    node = null;
            }

            enemies.Add(node.Position);
        }

        return enemies;
    }

    public void SpawnEnemies(int numberEnemies, Transform holder, float startDelay) {
        List<Vector2> enemies = ChooseSpawnSpots(numberEnemies);
        List<ParticleSystem> particles = createSpawnParticles(enemies);

        StartCoroutine(CreateEnemies(holder, enemies, startDelay, particles));          
    }
}
