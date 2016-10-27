using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class MovingObject : MonoBehaviour {
    public float TimeBetweenShotsMain = 0.6f;
    public float TimeBetweenShotsAlt = 0.6f;
    public float TimeFrozenPerStep = 0.8f;
    public float HitsBeforeFrozen = 5;
    public float MoveTime = 0.15f;
    public LayerMask BlockingLayer;
    public int DamageDealt = 5;

    private int frozenHits = 1;
    private BoxCollider2D _boxcollider;
    private Rigidbody2D _rb2D;
    protected float _inverseMoveTime;
    protected Animator _animator;
    private bool _canShoot = true;

    public float getVelocity()
    {
        return _rb2D.velocity.x;
    }

	protected virtual void Start () {
        _boxcollider = GetComponent<BoxCollider2D>();
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        resetMoveTime();
	}

    protected void resetMoveTime() {
        _inverseMoveTime = 1f / MoveTime;
    }

    public virtual bool Move(float xDir, float yDir) {
        Vector2 start = transform.position;
        Vector2 normalized = new Vector2(xDir, yDir);
        normalized.Normalize();
        Vector2 end = start + normalized;
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
            startingPosition.y -= (_boxcollider.size.y + 0.6f);   
        
        if (yDir > 0.5)
            startingPosition.y += (_boxcollider.size.y + 0.1f);

        if (xDir > 0.5)
            startingPosition.x += (_boxcollider.size.x + 0.2f);
        if (xDir < -0.5)
            startingPosition.x -= (_boxcollider.size.x + 0.2f);

        UnityEngine.Object prefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullet.prefab", typeof (Bullet));
        Bullet clone = Instantiate(prefab, startingPosition, Quaternion.identity) as Bullet;
        clone.transform.SetParent(this.transform);
        clone.Init(direction, this, mainFire, bulletDamage);

        if (direction.x > double.Epsilon) {
            _animator.SetTrigger("ShootRight");
        } else
            _animator.SetTrigger("ShootLeft");

        _canShoot = false;

        float frozenTime = 3 * ((frozenHits - 1)/ HitsBeforeFrozen);
        if (mainFire)
            yield return new WaitForSeconds(TimeBetweenShotsMain + frozenTime);
        else
            yield return new WaitForSeconds(TimeBetweenShotsAlt + frozenTime);
        _canShoot = true;
    }

    protected void SmoothMovement(Vector3 end) {
        Vector3 newPosition = Vector3.MoveTowards(_rb2D.position, end, _inverseMoveTime * Time.deltaTime);
        _rb2D.MovePosition(newPosition);
    }

    protected IEnumerator AltBulletHit(Collision2D collision) {
        if (this.frozenHits < HitsBeforeFrozen) {
            this.frozenHits++;
            _inverseMoveTime = 1 / (MoveTime * frozenHits * 4);
            yield return new WaitForSeconds(TimeFrozenPerStep);
            this.frozenHits--;
            _inverseMoveTime = 1 / (MoveTime * frozenHits * 4);
        }

        if (frozenHits == 1)
            _inverseMoveTime = 1/MoveTime;
    }

    protected abstract void OnCollisionEnter2D(Collision2D collision);
}
