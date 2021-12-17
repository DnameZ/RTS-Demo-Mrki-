using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]

public class TruckContainer : MonoBehaviour
{
    public Truck truck;

    [SerializeField] ParticleSystem Gas;
    [SerializeField] string[] tagsToInteractWith;

    GameObject ObjectChanged;
    GameObject selectedGameObject;

    protected Container containerForResource;

    protected Renderer RendererOffLiquidMaterialContainer;
    protected Renderer RendererOffLiquidMaterialTruck;

    protected SetResources setResources;

    protected Animator anim;
    protected NavMeshAgent TruckEngine;

    const string Liquid = "Liquid";
    const string LiquidOne = "Liquid1";
    const string Water = "Water";
    const string Resource = "Resource";
    const string Gold = "Gold";
    const string Active = "isActive";

    public bool OnPlace = false;
    public bool OnMine = false;

    public int Current_Amount;

    protected float valueOfFilling;
    protected float Timer;
    protected float valueOfMining;


    private delegate bool DoTask();

    public void Start()
    {
        TruckEngine = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        truck.OperationToDo = Truck.Operation_To_Do.Idle;
    }

    public virtual string ResourceToFind()
    {
        return null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out setResources));
    }

    private void OnTriggerStay(Collider other)
    {
        CheckForCollisionTag(tagsToInteractWith, other);
        OperationHandeling();
    }

    public virtual void Mine()
    {
        
    }

    public virtual void Fill()
    {
        
    }

    protected float SmoothlyTransition(float startValue,float endValue)
    {
        float t = 0f;

        t = 0.5f * Time.deltaTime;

        float valueToChange = Mathf.Lerp(startValue, endValue, t);

        return valueToChange;

    }

    public virtual void GoToContainer()
    {
       
    }

    void CheckForCollisionTag(string[] tags,Collider colider)
    {
        foreach(string tag in tags)
        {
            if (colider.tag==tag)
            {
                switch (tag)
                {
                    case Gold + Resource:
                        CheckForTruckSpeed(CheckForAmount, TruckEngine, Vector3.zero);
                    break;

                    case Water + Resource:
                        CheckForTruckSpeed(CheckForAmount,TruckEngine,Vector3.zero);
                        GetRendererOfTruckLiquid(TruckEngine,LiquidOne);
                        break;

                    case Water:
                        colider.transform.parent.TryGetComponent(out containerForResource);
                        GetRendererOfContainer(colider,Liquid);
                        GetRendererOfTruckLiquid(TruckEngine, LiquidOne);
                        CheckForTruckSpeed(CheckOnPlace, TruckEngine, Vector3.zero);
                    break;

                    case Gold:
                        colider.transform.TryGetComponent(out containerForResource);
                        CheckForTruckSpeed(CheckOnPlace, TruckEngine, Vector3.zero);
                    break;
                }
            }
                  
        }
    }

    
    void CheckForTruckSpeed(DoTask doTask,NavMeshAgent agent, Vector3 speed)
    {
        if (agent.velocity == speed)
            doTask();        
    }

    Renderer GetRendererOfContainer(Collider colider,string ChildToFind)
    {
        GameObject LiquidChild=null;

        LiquidChild = colider.transform.parent.Find(ChildToFind).gameObject;
     
        return RendererOffLiquidMaterialContainer = ChangeShader.GetRendererOfObject(LiquidChild);
    }

    Renderer GetRendererOfTruckLiquid(NavMeshAgent agent,string ChildToFind)
    {
        GameObject LiquidChild=null;

        LiquidChild = agent.transform.Find(ChildToFind).gameObject;

        return RendererOffLiquidMaterialTruck= ChangeShader.GetRendererOfObject(LiquidChild);
    }

    bool CheckOnPlace()
    {
        if(Current_Amount!=0)
        {
            //anim.SetBool(Active, false);
            truck.OperationToDo = Truck.Operation_To_Do.Fill;
            OnPlace = true;
        }

        else if(Current_Amount == 0)
        {
            //anim.SetBool(Active, true);
            truck.OperationToDo = Truck.Operation_To_Do.GoingToResource;
            OnPlace = false;
        }

        return OnPlace;

    }

    void OperationHandeling()
    {
        switch(truck.OperationToDo)
        {
            case Truck.Operation_To_Do.GoingToResource:
                CustomSelectionTool.SelectionTool.MoveUnit(FindClosestResource().position, TruckEngine);
            break;

            case Truck.Operation_To_Do.GoingToContainer:
                GoToContainer();
            break;

            case Truck.Operation_To_Do.Fill:
                Fill();
            break;

            case Truck.Operation_To_Do.Mine:
                Mine();
            break;
        }
    }


    bool CheckForAmount()
    {
        if (Current_Amount < truck.max_Amount)
        {
            OnMine = true;
            //anim.SetBool(Active, false);
            //gas.Stop();
            truck.OperationToDo = Truck.Operation_To_Do.Mine;
        }
            
        else if (Current_Amount == truck.max_Amount)
        {
            OnMine = false;
            //anim.SetBool(Active, true);
            //gas.Play();
            truck.OperationToDo = Truck.Operation_To_Do.GoingToContainer;
        }

        return OnMine;

    }

    protected GameObject FindContainer(string TypeOfResource)
    {
        int MinimalAmount;

        List<GameObject> Containers= new List<GameObject>();

        List<int> ContainersAmount = new List<int>();

        GameObject ContainerWithLowestAmount=null;

        Containers.AddRange(GameObject.FindGameObjectsWithTag(TypeOfResource));

        foreach (GameObject container in Containers)
        {
            container.TryGetComponent(out containerForResource);

            ContainersAmount.Add(containerForResource.cur_Amount);
        }

        MinimalAmount = ContainersAmount.Min();

        foreach (GameObject container in Containers)
        {
            container.TryGetComponent(out containerForResource);

            if(containerForResource.cur_Amount==MinimalAmount)
            {
                ContainerWithLowestAmount = container.gameObject;
            }
        }

        return ContainerWithLowestAmount;

    }

    public Transform FindClosestResource()
    {
        float minDist = Mathf.Infinity;

        Vector3 CurPos = TruckEngine.transform.position;

        List<GameObject> FindResourcesForTruck = new List<GameObject>();

        FindResourcesForTruck.AddRange(GameObject.FindGameObjectsWithTag(ResourceToFind()));

        Transform ClosestResource=null;

        foreach (GameObject resource in FindResourcesForTruck)
        {
           resource.TryGetComponent(out setResources);

           float dist = Vector3.Distance(resource.transform.position, CurPos);

           if (dist < minDist)
           {
                 ClosestResource = resource.transform;

                 minDist = dist;
           }
        }
        return ClosestResource;
    }


    public void StartOperationMining()
    {
        //truck.OperationToDo = Truck.Operation_To_Do.GoingToResource;
        CustomSelectionTool.SelectionTool.MoveUnit(FindClosestResource().position, TruckEngine);
    }

}
