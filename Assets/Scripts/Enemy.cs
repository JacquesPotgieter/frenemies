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

    protected override void AttemptMove<T>(float xDir, float yDir) {
        _skipMove = !_skipMove;
        if (_skipMove)
            return;
        base.AttemptMove<T>(xDir, yDir);     
        base.tryShoot(-1 * _target.position.x, -1 * _target.position.y);   
    }

    public void MoveEnemy() {
        int xDir = 0;
        int yDir = 0;

        yDir = _target.position.y > transform.position.y ? 1 : -1;
        xDir = _target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        float value = 1;
    }
}
