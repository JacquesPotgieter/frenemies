using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxcollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

	protected virtual void Start () {
        boxcollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
	}

    protected bool Move(float xDir, float yDir, out RaycastHit2D hit) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxcollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxcollider.enabled = true;

        if (hit.transform == null) {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected virtual void Shoot() {
        Object prefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Bullet.prefab", typeof(Bullet));
        GameObject clone = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;

        //Bullet bullet = GetComponent("Bullet") as Bullet;
        //GameObject toInstantiate = Instantiate(Resources.Load("Bullet")) as GameObject;
        //GameObject instance = Instantiate(toInstantiate, transform.position, Quaternion.identity) as GameObject;       
    }

    protected virtual void AttemptMove<T>(float xDir, float yDir)
        where T : Component {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }

    protected IEnumerator SmoothMovement(Vector3 end) {
        Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
        rb2D.MovePosition(newPosition);
        yield return null;
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}
