using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator.

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
    public GameObject Exit;
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;

    private Transform _boardHolder;
    [HideInInspector] public Dictionary<Vector2, Boolean> _gridPositions = new Dictionary<Vector2, bool>();

    void InitialiseList() {
        _gridPositions.Clear();

        for (int x = 0; x <= BoardWidth; x++) {
            for (int y = 0; y <= BoardHeight; y++) {
                _gridPositions.Add(new Vector2(x, y), true);
            }
        }
    }

    void BoardSetup() {
        _boardHolder = new GameObject("Board").transform;

        for (int x = -1; x <= BoardWidth + 1; x++) {
            for (int y = -1; y <= BoardHeight + 1; y++) {
                GameObject toInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];

                if (x == -1 || x == BoardWidth + 1 || y == -1 || y == BoardHeight + 1)
                    toInstantiate = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x - 0.5f, y - 0.5f, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(_boardHolder);
            }
        }
    }
    
    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition(bool stationary) {
        int randomX = Random.Range(1, BoardWidth + 1);
        int randomY = Random.Range(1, BoardHeight + 1);
        Vector2 position = new Vector2(randomX, randomY);
        if (stationary) {
            _gridPositions.Remove(position);
            _gridPositions.Add(position, false);
        }
        return position - new Vector2(0.5f, 0.5f);
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum, Transform boardHolder, bool stationary) {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++) {
            Vector3 randomPosition = RandomPosition(stationary);
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            GameObject instance = Instantiate(tileChoice, randomPosition, Quaternion.identity) as GameObject;
            instance.transform.SetParent(boardHolder);
        }
    }

    public void SetupScene(int level){
        BoardSetup();
        //RandomPathAtoB();

        InitialiseList();

        // Uncomment bellow to see procerurally generated map that is done in tutorial

        ////Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(WallTiles, WallCount.Minimum, WallCount.Maximum, _boardHolder, true);

        ////Determine number of enemies based on current level number, based on a logarithmic progression
        int enemyCount = level;

        ////Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        Transform enemyHolder = new GameObject("Enemies").transform;
        LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount, enemyHolder, false);
    }
}
