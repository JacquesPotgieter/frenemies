using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObject {

    public int PlayerDamage;    
    public AudioClip EnemyAttack1;
    public AudioClip EnemyAttack2;

    private int _healthPoints = 100;
    private Animator _animator;
    private Transform _target;
    private bool _skipMove = true;
    private bool isDead = false;

    protected override void Start () {
        GameManager.Instance.AddEnemyToList(this);
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}

    public void UpdateEnemy() {
        if (!isDead) {
            MoveEnemy();
            CheckIfGameOver();
        }
    }

    protected override bool Move(float xDir, float yDir) {
        bool didMove = base.Move(xDir, yDir);
        if (didMove) {
            String movement = "";
            if (xDir > yDir)
                movement = xDir < 0 ? "WalkLeft" : "WalkRight";
            else
                movement = yDir < 0 ? "WalkUp" : "WalkDown";
            _animator.SetTrigger(movement);
        }

        return didMove;
    }    

    private void MoveEnemy() {
        int yDir = _target.position.y > transform.position.y ? 1 : -1;
        int xDir = _target.position.x > transform.position.x ? 1 : -1;

        if (_skipMove) {
            _skipMove = false;
            Move(xDir, yDir);
        }
        else
            _skipMove = true;
    }

    private void CheckIfGameOver() {
        if (_healthPoints <= 0) {
            _animator.SetTrigger("Dead");
            isDead = true;

            Destroy(gameObject);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Bullet") {
            MainBulletHit(collision);
        }
    }

    private void MainBulletHit(Collision2D collision) {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        this._healthPoints -= bullet.DamageDone;
    }
}
