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
    private List<Vector3> _pathPositions = new List<Vector3>();

    public static Count WallCount = new Count(10, 30);
    public GameObject Exit;
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;

    private Transform _boardHolder;
    private List<Vector3> _gridPositions = new List<Vector3>();

    void InitialiseList() {
        _gridPositions.Clear();

        for (int x = 1; x < BoardWidth; x++) {
            for (int y = 1; y < BoardHeight; y++) {
                _gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void RandomPathAtoB() {
        Boolean Foundpath = false;
        float curX = 0f;
        float curY = 0f;
        float PrevcurX = 0f;
        float PrevcurY = 0f;
        int PrevChoice = Random.Range(1, 5);
        Vector3 current = new Vector3(curX, curY, 0f);
        _pathPositions.Add(current);
        while (Foundpath == false) {
            int choice = Random.Range(1, 5);
            if (choice == PrevChoice)
                choice = Random.Range(1, 5);
            else {
                if (choice == 1) {
                    //Go up
                    PrevcurY = curY;
                    curY++;
                } else if (choice == 2) {
                    //Go down
                    PrevcurY = curY;
                    curY--;
                } else if (choice == 3) {
                    //Go left
                    PrevcurX = curX;
                    curX--;
                } else {
                    //Go right
                    PrevcurX = curX;
                    curX++;
                }

                if (curX < 0 || curX > (BoardWidth - 1) || curY < 0 || curY > (BoardHeight - 1)) {
                    curX = PrevcurX;
                    curY = PrevcurY;
                } else {
                    current = new Vector3(curX, curY, 0f);
                    _pathPositions.Add(current);
                    if (curX == BoardWidth - 1 && curY == BoardHeight - 1) {
                        Foundpath = true;
                        _pathPositions.Remove(current);
                    }

                }
            }
        }
    }

    void BoardSetup() {
        _boardHolder = new GameObject("Board").transform;
        
        for (int x = -1; x < BoardWidth + 1; x++) {
            for (int y = -1; y < BoardHeight + 1; y++) {
                GameObject toInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];

                if (x == -1 || x == BoardWidth || y == -1 || y == BoardHeight)
                    toInstantiate = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(_boardHolder);
            }
        }
    }


    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition() {
        int randomIndex = Random.Range(0, _gridPositions.Count);
        Vector3 randomPosition = _gridPositions[randomIndex];
        _gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++) {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            GameObject instance = Instantiate(tileChoice, randomPosition, Quaternion.identity) as GameObject;
            instance.transform.SetParent(_boardHolder);
        }
    }

    void Layoutpath() {
        foreach (Vector3 vec in _pathPositions) {
            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = Exit;

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            GameObject instance = Instantiate(tileChoice, vec, Quaternion.identity) as GameObject;
            instance.transform.SetParent(_boardHolder);
        }
    }



    public void SetupScene(int level){
        BoardSetup();
        //RandomPathAtoB();

        InitialiseList();

        Layoutpath();
        // Uncomment bellow to see procerurally generated map that is done in tutorial

        ////Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(WallTiles, WallCount.Minimum, WallCount.Maximum);

        ////Determine number of enemies based on current level number, based on a logarithmic progression
        int enemyCount = (int)Mathf.Log(level, 2f);

        ////Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount);

        //Instantiate the exit tile in the upper right hand corner of our game board
        GameObject instance = Instantiate(Exit, new Vector3(BoardWidth - 1, BoardHeight - 1, 0f), Quaternion.identity) as GameObject;
        instance.transform.SetParent(_boardHolder);
    }
}
