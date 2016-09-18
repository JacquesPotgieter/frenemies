using UnityEngine;
using System.Collections;

public class Bullet : MovingObject {

    [HideInInspector] public int DamageDone = 5;
    [HideInInspector] public MovingObject _shooter;
    [HideInInspector] public bool _mainFire = true;

    private Vector3 _direction;    

    public void Init(Vector3 direction, MovingObject shooter, bool mainFire, int bulletDamage) {
        this._direction = direction;
        this._shooter = shooter;
        this._mainFire = mainFire;
        this.DamageDone = bulletDamage;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = 0;

        changeColour();
    }

    private void changeColour() {
        if (_mainFire)
            gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;
        else
            gameObject.GetComponent<SpriteRenderer>().material.color = Color.blue;

        if (_shooter.tag.Equals("Enemy")) {
            if (_mainFire)
                gameObject.GetComponent<SpriteRenderer>().material.color = Color.red;
            else
                gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
        }
    }

	void Start () {
        base.MoveTime = 0.1f;
        base.Start();
	}

    void Update() {
        if (_shooter != null)
            Move(_direction.x, _direction.y);
        else
            Destroy(gameObject);
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
