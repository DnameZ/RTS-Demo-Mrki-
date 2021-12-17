using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(fileName ="New Truck", menuName = "Truck")]
public class Truck : ScriptableObject
{
    public GameObject TruckEngine;

    public NavMeshAgent AGENT;

    public enum Type_Of_Resource_Transporting { Gold,Water,Wood,Electricty };

    public Type_Of_Resource_Transporting TypeOfResourceTransporting;

    public enum Operation_To_Do { Idle,Mine,Fill, GoingToResource,GoingToContainer };

    public Operation_To_Do OperationToDo;

    public Transform CurContainerToFill;

    public int max_Amount;
}
