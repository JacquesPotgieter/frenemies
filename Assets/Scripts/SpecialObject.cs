using UnityEngine;
using System.Collections;

class SpecialObject:MonoBehaviour
{
    int hitCounter = 0;

    public void Init(Vector3 Position)
    {
        transform.position = Position;
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            hitCounter++;
        }
        if (hitCounter >= 5)
        {
            Destroy(gameObject);
        }
        if (collision.collider.name.Contains("P2"))
        {
            GameManager.Instance.SpecialObjectP2 = true;
            Destroy(gameObject);
        }
        if (collision.collider.name.Contains("P1"))
        {
            GameManager.Instance.SpecialObjectP1 = true;
            Destroy(gameObject);
            Debug.Log(GameManager.Instance.SpecialObjectP1 + "aaa" + GameManager.Instance.SpecialObjectP2);
        }



    }

   
}

