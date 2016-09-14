using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Player : MovingObject {
    public string PlayerNumber = "1";
    public Text HealthText;
    public AudioClip MoveSound1;
    public AudioClip MoveSound2;
    public AudioClip EatSound1;
    public AudioClip EatSound2;
    public AudioClip DrinkSound1;
    public AudioClip DrinkSound2;
    public AudioClip GameOverSound;

    public string HorizontalCtrl = "Horizontal_P1";
    public string VerticalCtrl = "Vertical_P1";
    public string HorizontalFireCtrl = "HorizontalFire_P1";
    public string VerticalFireCtrl = "VerticalFire_P1";
    public string ShootMainButton = "Fire1_P1";
    public string ShootAltButton = "Fire2_P1";

    private Animator _animator;
    private int _healthPoints;
    private bool isDead = false;

    public int getHealth()
    {
        return _healthPoints;
    }

    protected override void Start() {
        GameManager.Instance.players.Add(this);
        _animator = GetComponent<Animator>();
        if (PlayerNumber.Equals("1"))
            _healthPoints = GameManager.Instance.HealthP1 -10;
        else
            _healthPoints = GameManager.Instance.HealthP2;

        base.Start();
    }

    private void OnDisable() {
        if (PlayerNumber.Equals("1"))
            GameManager.Instance.HealthP1 = _healthPoints;
        else
            GameManager.Instance.HealthP2 = _healthPoints;
    }

    void Update() {
        if (!isDead) {
            CheckIfGameOver();

            float horizontal = Input.GetAxis(HorizontalCtrl);
            float vertical = Input.GetAxis(VerticalCtrl);
            float didShootMain = Input.GetAxis(ShootMainButton);
            float didShootAlt = Input.GetAxis(ShootAltButton);
            float horizontalFire = Input.GetAxis(HorizontalFireCtrl);
            float verticalFire = Input.GetAxis(VerticalFireCtrl);

            if (Math.Abs(horizontal) > Double.Epsilon || Math.Abs(vertical) > Double.Epsilon)
                Move(horizontal, vertical);
            else
                _animator.SetTrigger("Idle");

            if (Math.Abs(horizontalFire) < double.Epsilon && Math.Abs(verticalFire) < double.Epsilon)
                horizontalFire = 1;

            if (didShootMain > double.Epsilon)
                TryShoot(horizontalFire, verticalFire, true, DamageDealt);
            else if (didShootAlt > double.Epsilon)
                TryShoot(horizontalFire, verticalFire, false, DamageDealt);            
        }
    }

    public override bool Move(float xDir, float yDir) {
        bool didMove = base.Move(xDir, yDir);
        if (didMove) {
            SoundManager.Instance.RandomizeSfx(MoveSound1, MoveSound2);

            String movement = "";
            if (Mathf.Abs(xDir) > Mathf.Abs(yDir)) 
                movement = xDir < double.Epsilon ? "WalkLeft" : "WalkRight";
            else 
                movement = yDir > double.Epsilon ? "WalkUp" : "WalkDown";
            _animator.SetTrigger(movement);
        }

        return didMove;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Exit") {
            //Invoke("Restart", RestartLevelDelay);
            //enabled = false;
        }
    }

    private void CheckIfGameOver() {
        this.HealthText.text = "Health: " + _healthPoints;

        if (_healthPoints <= 0) {
            SoundManager.Instance.PlaySingle(GameOverSound);
            SoundManager.Instance.MusicSource.Stop();
            GameManager.Instance.GameOver();

            _animator.SetTrigger("Dead");
            this.isDead = true;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {        
        if (collision.collider.tag == "Bullet") {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            if (bullet._shooter == this) {
                Debug.Log("Hit self");
            } else {
                bool isPlayershot = bullet._shooter.tag == "Player";
                if (bullet._mainFire && isPlayershot)
                    MainBulletPlayer(bullet);

                if (bullet._mainFire && !isPlayershot)
                    MainBulletHit(bullet);

                if (!bullet._mainFire && isPlayershot)
                    AltBulletPlayer(bullet);

                if (!bullet._mainFire && !isPlayershot)
                    AltBulletHit(bullet);
            }            
        }
    }

    private void MainBulletPlayer(Bullet bullet) {
        this._healthPoints += bullet.DamageDone;
    }

    private void MainBulletHit(Bullet bullet) {      
        this._healthPoints -= bullet.DamageDone;        
    }

    private void AltBulletPlayer(Bullet bullet) {

    }

    private void AltBulletHit(Bullet bullet) {

    }
}
