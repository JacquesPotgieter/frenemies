using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public GameObject sprite;
    public float speed = 2;

    private Vector3 direction;
    private Vector3 startingPosition;
    private float inverseSpeed;

    public void init(Vector3 direction, Vector3 position) {
        this.direction = direction;
        this.startingPosition = position;

        inverseSpeed = 1 / speed;
    }

	// Use this for initialization
	void Start () {
        Instantiate(sprite, transform.position, Quaternion.identity);

        transform.position = startingPosition;

        Debug.Log("bullet created");
    }

    void Update() {

    }
}
