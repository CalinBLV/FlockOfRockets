using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This class is used to centralize all behaviours and mix them all based on weights
public class CompositeBehaviour : FlockBehaviour
{
    //Here I should've create a new model instead of two arrays but I exceeded the time
    public FlockBehaviour[] behaviours;
    public float[] weights;

 
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
       
        if (weights.Length != behaviours.Length)
            {
                Debug.LogError("Data mismatch in " + name, this);
                return Vector2.zero;
            }

            Vector2 move = Vector2.zero;
           
        for (int i = 0; i < behaviours.Length; i++)
            {
               
               Vector2 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i];

                if (partialMove != Vector2.zero)
                {
                    if (partialMove.sqrMagnitude > weights[i] * weights[i])
                    {
                        partialMove.Normalize();
                        partialMove *= weights[i];
                    }
                    move += partialMove;
                }
            }

            return move;
        
     
    }
    //Change weight of first three parameters
    public override void ChangeWeight(float[] weights)
    {
        for(int i=0;i<this.weights.Length;i++)
        {
            if(weights[i]!=-1)
            {
                this.weights[i] = weights[i];
            }
        }
    }
}
