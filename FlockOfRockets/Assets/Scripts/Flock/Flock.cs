using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//This class is used to controll the flock itself by changing it's behaviour,instantiating agents and make them move
public class Flock : MonoBehaviour
{
    public new Camera camera;
    public EventSystem eventSystem;
    public SliderController slider;
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;
    [Range(10, 500)]
    public int startingCount = 50;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius{get{ return squareAvoidanceRadius; }}

    private Vector2 move;
    private float[] weights;
    void Start()
    {
        //Subscribing to events
        slider.onCoherenceValueChange += HandleCoherenceValueChange;
        slider.onSeperationValueChange += HandleSeperationValueChange;
        slider.onAlignmentValueChange += HandleAlignmentValueChange;
        //Initial weights
        weights = new float[] { 0.5f, 0.5f, 0.5f, 10f };
        //Setting the parameters
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        //Instantiating the agents
        for(int i=0;i<startingCount;i++)
        {
            FlockAgent newAgent = Instantiate(agentPrefab,Random.insideUnitCircle*startingCount*AgentDensity,Quaternion.Euler(Vector3.forward*Random.Range(0f,360f)),transform);
            newAgent.name = "Agent " + i;
            agents.Add(newAgent);
        }
    }

    void Update()
    {
        foreach(FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            //Check if mouse button is held to change overall behaviour
            if (Input.GetMouseButton(0) && Input.mousePosition.y>Screen.height/3)
            {
                move = (camera.ScreenToWorldPoint(Input.mousePosition) - agent.transform.position).normalized * 100;
            }
            else
            {
                move = behaviour.CalculateMove(agent, context, this);
                
            }
                move *= driveFactor;

            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
            
        }
    }
    //Look in the context to find nearby objects
    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach(Collider2D c in contextColliders)
        {
            if(c!=agent.AgentCollider)
            {
                context.Add(c.transform);
            }

        }
        return context;

    }
    //Functions to handle the weights.
    private void HandleCoherenceValueChange(float val)
    {
        weights = new float[] { val, -1, -1, -1 };

        behaviour.ChangeWeight(weights);
    }

    private void HandleSeperationValueChange(float val)
    {
        weights = new float[] { -1, val, -1, -1 };

        behaviour.ChangeWeight(weights);
    }

    private void HandleAlignmentValueChange(float val)
    {
        weights = new float[] { -1, -1, val, -1 };

        behaviour.ChangeWeight(weights);
    }

}
