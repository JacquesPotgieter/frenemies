using UnityEngine;
using System.Collections;

public class Bullet : MovingObject {
    private Vector3 _direction;
    private MovingObject _shooter;

    public void Init(Vector3 direction, MovingObject shooter) {
        this._direction = direction;
        this._shooter = shooter;
    }

    // Use this for initialization
	void Start () {
        base.MoveTime = 0.05f;
        base.Start();
	}

    void Update() {
        Move(_direction.x, _direction.y);
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
