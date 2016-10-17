using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Enemy : MovingObject {
   
    public AI_Controller AI_controller;

    [HideInInspector] public Player lastShooter;
    [HideInInspector] public Text healthText;
    private int _healthPoints;
    private Animator _animator;
    private bool isDead = false;

    public void init(AI_Controller AI_controller, int health, int damage, RuntimeAnimatorController animator) {
        this.AI_controller = AI_controller;
        this._healthPoints = health;
        this.DamageDealt = damage;
        //this._animator.runtimeAnimatorController = animator;
    }

    protected override void Start () {
        _animator = GetComponent<Animator>();
        AssignEnemy.run(this);
        base.Start();
	}

    void Update() {
        if (!isDead) {
            CheckIfGameOver();
            float x = transform.position.x;
            float y = transform.position.y - 0.8f;

            healthText.transform.position = new Vector3(x, y, 0f);
            healthText.text = _healthPoints + "";
            //AI_controller.run();
        }
    }

    public override bool Move(float xDir, float yDir) {
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

    private void CheckIfGameOver() {
        if (_healthPoints <= 0) {
            GameManager.Instance.checkIfAllEnemiesKilled(this);
            _animator.SetTrigger("Dead");
            isDead = true;
            GameManager.Instance.enemies.Remove(this);
            GameManager.Instance.totalEnemiesKilled++;
            Destroy(healthText);
            Destroy(gameObject);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Bullet") {
            Bullet bullet = collision.collider.GetComponent<Bullet>();
            
            if (!bullet._shooter.tag.Equals("Enemy")) {
                lastShooter = bullet._shooter.GetComponent<Player>();
                if (bullet._mainFire)
                    MainBulletHit(collision);
                else
                    StartCoroutine(AltBulletHit(collision));
            }
        }
    }

    private void MainBulletHit(Collision2D collision) {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        this._healthPoints -= bullet.DamageDone;
    }
}
