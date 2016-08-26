using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObject {

    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove = true;
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

	protected override void Start () {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}

    protected override void AttemptMove<T>(float xDir, float yDir) {
        skipMove = !skipMove;
        if (skipMove)
            return;
        base.AttemptMove<T>(xDir, yDir);     
        base.tryShoot(-1 * target.position.x, -1 * target.position.y);   
    }

    public void MoveEnemy() {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        float value = 1;
    }
}
