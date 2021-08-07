using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is an abstract class that's going to be the parent of every behaviour.
public abstract class FlockBehaviour : MonoBehaviour
{
    public abstract Vector2 CalculateMove(FlockAgent agent, List<Transform> context ,Flock flock);
    public virtual void ChangeWeight(float[] weight){}


}
