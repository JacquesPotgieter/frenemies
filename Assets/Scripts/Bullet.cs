using UnityEngine;
using System.Collections;

public class Bullet : MovingObject
{
    private Vector3 direction;
    private MovingObject shooter;

    public void init(Vector3 direction, MovingObject shooter) {
        this.direction = direction;
        this.shooter = shooter;
    }

    // Use this for initialization
	void Start () {
        base.moveTime = 0.05f;
        base.Start();
	}

    void Update() {
        AttemptMove<Component>(direction.x, direction.y);
    }

    protected override void AttemptMove<T>(float xDir, float yDir) {
        base.AttemptMove<T>(xDir, yDir);

        float value = 1f;
    }

    protected override void OnCantMove<T>(T component)
    {
        Debug.Log("Hit");
        Destroy(gameObject);
    }
}
