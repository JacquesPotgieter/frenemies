using UnityEngine;
using System.Collections;

public class AI_Controller {

    public MovingObject target;
    public MovingObject currentObject;

    public void changeTarget(MovingObject target) {
        this.target = target;
    }

    public virtual void run() {
        
    }

    public void Start() {
        
    }
}
