using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MovingObject : MonoBehaviour {
    public float TimeBetweenShotsMain = 0.4f;
    public float TimeBetweenShotsAlt = 0.4f;
    public float TimeFrozenPerStep = 0.8f;
    public float HitsBeforeFrozen = 5;
    public float MoveTime = 0.15f;
    public LayerMask BlockingLayer;
    public int DamageDealt = 5;

    private BoxCollider2D _boxcollider;
    private Rigidbody2D _rb2D;
    protected float _inverseMoveTime;
    private bool _canShoot = true;

	protected virtual void Start () {
        _boxcollider = GetComponent<BoxCollider2D>();
        _rb2D = GetComponent<Rigidbody2D>();
        resetMoveTime();
	}

    protected void resetMoveTime() {
        _inverseMoveTime = 1f / MoveTime;
    }

    public virtual bool Move(float xDir, float yDir) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        SmoothMovement(end);
        return true;
    }

    public void TryShoot(float xDir, float yDir, bool mainFire, int bulletDamage) {
        if (_canShoot) {
            StartCoroutine(Shoot(xDir, yDir, mainFire, bulletDamage));
        }
    }

    private IEnumerator Shoot(float xDir, float yDir, bool mainFire, int bulletDamage) {
        Vector3 direction = new Vector3(xDir * 100f, yDir * 100f, 1f);

        float absX = Mathf.Abs(xDir);
        float absY = Mathf.Abs(yDir);

        if (absX > absY) {
            yDir = yDir / absX;
            xDir = xDir / absX;
        } else {
            xDir = xDir / absY;
            yDir = yDir / absY;
        }
        Vector3 startingPosition = transform.position;

        if (yDir < -0.5) 
            startingPosition.y -= (_boxcollider.size.y + 0.5f);   
        
        if (yDir > 0.5)
            startingPosition.y += (_boxcollider.size.y + 0.1f);

        if (xDir > 0.5)
            startingPosition.x += (_boxcollider.size.x + 0.2f);
        if (xDir < -0.5)
            startingPosition.x -= (_boxcollider.size.x + 0.2f);

        Object prefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullet.prefab", typeof (Bullet));
        Bullet clone = Instantiate(prefab, startingPosition, Quaternion.identity) as Bullet;
        clone.Init(direction, this, mainFire, bulletDamage);

        _canShoot = false;
        if (mainFire)
            yield return new WaitForSeconds(TimeBetweenShotsMain);
        else
            yield return new WaitForSeconds(TimeBetweenShotsAlt);
        _canShoot = true;
    }

    protected void SmoothMovement(Vector3 end) {
        Vector3 newPosition = Vector3.MoveTowards(_rb2D.position, end, _inverseMoveTime * Time.deltaTime);
        _rb2D.MovePosition(newPosition);
    }

    protected abstract void OnCollisionEnter2D(Collision2D collision);
}
