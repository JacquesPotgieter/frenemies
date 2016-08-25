using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Player : MovingObject {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
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
    private int food;

	protected override void Start () {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Food: " + food;

        base.Start();
	}

    private void OnDisable() {
        GameManager.instance.playerFoodPoints = food;
    }

    void Update() {
        float horizontal = 0;
        float vertical = 0;
        bool shooted = false;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        shooted = Input.GetKey("f");

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);

        if (shooted)
            Shoot();
    }

    protected override void AttemptMove<T>(float xDir, float yDir) {
        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
        if (Move(xDir, yDir, out hit)) {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Exit") {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        } else if (other.tag == "food") {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        } else if (other.tag == "soda") {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component) {
        Wall hitWall = component as Wall;
        hitWall.DamagaWall(wallDamage);

        animator.SetTrigger("playerChop");
    }

    private void Restart() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood(int loss) {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver() {
        if (food <= 0) {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }
}
