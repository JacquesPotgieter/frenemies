using UnityEngine;
using System.Collections;

public class Bullet : MovingObject
{
    private Vector3 _direction;
    private MovingObject _shooter;

    public void init(Vector3 direction, MovingObject shooter) {
        this._direction = direction;
        this._shooter = shooter;
    }

    // Use this for initialization
	void Start () {
        base.MoveTime = 0.05f;
        base.Start();
	}

    void Update() {
        AttemptMove<Component>(_direction.x, _direction.y);
    }

    protected override void AttemptMove<T>(float xDir, float yDir) {
        base.AttemptMove<T>(xDir, yDir);

        float value = 1f;
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
