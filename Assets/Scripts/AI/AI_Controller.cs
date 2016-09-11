using UnityEngine;
using System.Collections;

public abstract class AI_Controller {

    public MovingObject target;
    public MovingObject currentObject;

    public void changeTarget(MovingObject target) {
        this.target = target;
    }

    public abstract void run();
}
