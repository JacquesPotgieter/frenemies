using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObject {

    public int PlayerDamage;

    private Animator _animator;
    private Transform _target;
    private bool _skipMove = true;
    public AudioClip EnemyAttack1;
    public AudioClip EnemyAttack2;

	protected override void Start () {
        GameManager.Instance.AddEnemyToList(this);
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
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

    public void MoveEnemy() {
        int yDir = _target.position.y > transform.position.y ? 1 : -1;
        int xDir = _target.position.x > transform.position.x ? 1 : -1;

        if (_skipMove)
        {
            _skipMove = false;
            Move(xDir, yDir);
        }
        else
            _skipMove = true;
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        float value = 1;
    }
}
