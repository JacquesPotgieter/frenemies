using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Player : MovingObject {
    public float RestartLevelDelay = 1f;
    public Text FoodText;
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

    protected override void Start() {
        _animator = GetComponent<Animator>();
        _healthPoints = GameManager.Instance.HealthP1;

        FoodText.text = "Food: " + _healthPoints;

        base.Start();
    }

    private void OnDisable() {
        GameManager.Instance.HealthP1 = _healthPoints;
    }

    void Update() {
        float horizontal = Input.GetAxis(HorizontalCtrl);
        float vertical = Input.GetAxis(VerticalCtrl);
        float shooted = Input.GetAxis(ShootMainButton);
        float horizontalFire = Input.GetAxis(HorizontalFireCtrl);
        float verticalFire = Input.GetAxis(VerticalFireCtrl);

        if (Math.Abs(horizontal) > Double.Epsilon || Math.Abs(vertical) > Double.Epsilon)
            Move(horizontal, vertical);
        else
            _animator.SetTrigger("Idle");

        if (Math.Abs(horizontalFire) < double.Epsilon && Math.Abs(verticalFire) < double.Epsilon)
            horizontalFire = 1;
        if (shooted > double.Epsilon)
            TryShoot(horizontalFire, verticalFire);
    }

    protected override bool Move(float xDir, float yDir) {
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
            Invoke("Restart", RestartLevelDelay);
            enabled = false;
        } 
    }

    private void Restart() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood(int loss) {
        _animator.SetTrigger("playerHit");
        _healthPoints -= loss;
        FoodText.text = "-" + loss + " Food: " + _healthPoints;
        CheckIfGameOver();
    }

    private void CheckIfGameOver() {
        if (_healthPoints <= 0) {
            SoundManager.Instance.PlaySingle(GameOverSound);
            SoundManager.Instance.MusicSource.Stop();
            GameManager.Instance.GameOver();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("I was hit");
    }
}
