using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Player : MovingObject {

    public float restartLevelDelay = 1f;
    public Text foodText;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    private Animator animator;
    private int healthPoints;

    protected override void Start() {
        animator = GetComponent<Animator>();
        healthPoints = GameManager.instance.player1HP;

        foodText.text = "Food: " + healthPoints;

        base.Start();
    }

    private void OnDisable() {
        GameManager.instance.player1HP = healthPoints;
    }

    void Update() {
        float horizontal = 0f;
        float vertical = 0f;
        bool shooted = false;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        shooted = Input.GetKey("f");

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
        else
            horizontal = 1;
        
        if (shooted)
            tryShoot(horizontal, vertical);
    }

    protected override void AttemptMove<T>(float xDir, float yDir) {
        foodText.text = "Food: " + healthPoints;

        base.AttemptMove<T>(xDir, yDir);

        if (Move(xDir, yDir)) {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Exit") {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        } 
    }

    private void Restart() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood(int loss) {
        animator.SetTrigger("playerHit");
        healthPoints -= loss;
        foodText.text = "-" + loss + " Food: " + healthPoints;
        CheckIfGameOver();
    }

    private void CheckIfGameOver() {
        if (healthPoints <= 0) {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        float val = 1f;
    }
}
