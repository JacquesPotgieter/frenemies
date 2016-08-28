using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {
    public float timeBetweenShots = 0.2f;
    public float moveTime = 0.15f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxcollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;
    private float lastShotFired = 0f;
    private bool canShoot = true;

	protected virtual void Start () {
        boxcollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
	}

    protected bool Move(float xDir, float yDir) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        StartCoroutine(SmoothMovement(end));
        return true;
    }

    public void tryShoot(float xDir, float yDir) {
        if (canShoot) {
            StartCoroutine(Shoot(xDir, yDir));
        }
    }

    private IEnumerator Shoot(float xDir, float yDir) {
        float offsetX = 0;
        float offSetY = 0;

        if (xDir > 0)
            offsetX = boxcollider.size.x;
        else if (xDir < 0)
            offsetX = -1*boxcollider.size.x;

        if (yDir > 0)
            offSetY = boxcollider.size.y;
        else if (yDir < 0)
            offSetY = -1*boxcollider.size.y;

        Vector3 direction = new Vector3(xDir, yDir, 0f);
        Vector3 startingPosition = transform.position + new Vector3(offsetX, offSetY, 0f);

        Object prefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullet.prefab", typeof (Bullet));
        Bullet clone = Instantiate(prefab, startingPosition, Quaternion.identity) as Bullet;
        clone.init(direction, this);

        canShoot = false;
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    protected virtual void AttemptMove<T>(float xDir, float yDir)
        where T : Component {
        bool canMove = Move(xDir, yDir);
    }

    protected IEnumerator SmoothMovement(Vector3 end) {
        Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
        rb2D.MovePosition(newPosition);
        yield return null;
    }

    protected abstract void OnCollisionEnter2D(Collision2D collision);
}
