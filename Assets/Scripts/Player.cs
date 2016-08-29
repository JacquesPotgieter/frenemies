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

    public String HorizontalCtrl = "Horizontal_P1";
    public String VerticalCtrl = "Vertical_P1";
    public String ShootMainButton = "Fire1_P1";
    public String ShootAltButton = "Fire2_P1";

    private Animator _animator;
    private int _healthPoints;

    protected override void Start() {
        _animator = GetComponent<Animator>();
        _healthPoints = GameManager.instance.player1HP;

        FoodText.text = "Food: " + _healthPoints;

        base.Start();
    }

    private void OnDisable() {
        GameManager.instance.player1HP = _healthPoints;
    }

    void Update() {
        float horizontal = 0f;
        float vertical = 0f;
        bool shooted = false;

        horizontal = Input.GetAxisRaw(HorizontalCtrl);
        vertical = Input.GetAxisRaw(VerticalCtrl);
        shooted = Input.GetKey(ShootMainButton);

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
        else
            horizontal = 1;
        
        if (shooted)
            tryShoot(horizontal, vertical);
    }

    protected override void AttemptMove<T>(float xDir, float yDir) {
        FoodText.text = "Food: " + _healthPoints;

        base.AttemptMove<T>(xDir, yDir);

        if (Move(xDir, yDir)) {
            SoundManager.instance.RandomizeSfx(MoveSound1, MoveSound2);
        }

        CheckIfGameOver();
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
            SoundManager.instance.PlaySingle(GameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        float val = 1f;
    }
}
