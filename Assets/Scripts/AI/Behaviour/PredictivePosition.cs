using UnityEngine;
using System.Collections;

public class PredictivePosition : MonoBehaviour {

    public static Vector3 run(Vector3 TargetCur, Vector3 TargetPrev )
    {
        //d = (x1, y1) - (x, y) = (x1 - x, y1 - y)
        //Vector3 vec = TargetCur - TargetPrev;
        //vec = vec * 5;
        //float newdistance = (Vector3.Distance(TargetPrev, TargetCur)) / Time.deltaTime;
        //float enemytime = (Vector3.Distance(TargetCur,TargetPrev)) / (0.05f);
        //Vector3 newVector = TargetCur - TargetPrev;
        //Vector3 enemydestination = TargetCur + (newVector.normalized * (newdistance * enemytime));
        Vector3 finalpos =TargetCur;
        //Vector3 velocity = (this.target.getRigidbody()).velocity;
        //velocity *= Time.deltaTime;


        Vector3 velocity = (TargetCur - TargetPrev) / Time.deltaTime;

        velocity *= Time.deltaTime;
        velocity *= 10;
        finalpos += velocity;

        return finalpos;
        
    }
}
