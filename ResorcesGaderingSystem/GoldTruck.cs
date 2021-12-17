using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoldTruck : TruckContainer
{
    [SerializeField] GameObject GoldStorageHandler;
    [SerializeField] GameObject Gold;
    [SerializeField] GameObject Positions;
    [SerializeField] List<GameObject> EmptyPositions = new List<GameObject>();

    public Vector3 gridOriginLevel0;
    public Vector3 gridOriginLevel1;

    const string GH = "GoldStorageHandler";
    const string Goldy = "Gold";

    public float gridSpacing;

    public int gridX;
    public int gridZ;

    private void Awake()
    {
        GoldStorageHandler = this.transform.Find(GH).gameObject;
        gridOriginLevel0 = new Vector3(GoldStorageHandler.transform.position.x-3.5f, GoldStorageHandler.transform.position.y+1f, GoldStorageHandler.transform.position.z-2f);
        gridOriginLevel1 = new Vector3(GoldStorageHandler.transform.position.x-3.5f, GoldStorageHandler.transform.position.y+1.8f, GoldStorageHandler.transform.position.z - 2f);
        SpawnStorageForGold();
    }

    public override string ResourceToFind()
    {
        string Res = truck.TypeOfResourceTransporting.ToString() + "Resource";

        return Res;
    }


    public override void GoToContainer()
    {
        if (truck.CurContainerToFill == null)
            truck.CurContainerToFill = FindContainer(Goldy).transform;
        else
            CustomSelectionTool.SelectionTool.MoveUnit(FindContainer(Goldy).transform.position, TruckEngine);
    }


    public override void Mine()
    {
        int DelayAmount = 5;
        Timer += Time.deltaTime;

        if (Timer >= DelayAmount)
        {
            SpawnGold(EmptyPositions);
            Timer = 0f;
            setResources.resources.AmountOfResource -= 5;
            Current_Amount += 5;
        }
    }

    public override void Fill()
    {
        GameObject PosToLook = GoldStorageHandler.transform.Find("EmptyPos(Clone)").gameObject;
        GameObject GoldToDestroy = null;
        int DelayAmount = 5;
        Timer += Time.deltaTime;

        if (Timer >= DelayAmount)
        {
            Timer = 0f;
            Current_Amount -= 5;
            containerForResource.cur_Amount += 5;
            SpawnGold(GoldCont.goldContainer.EmptyPositions);

            foreach(Transform child in GoldStorageHandler.transform)
            {
               foreach(Transform grandChild in child)
                {
                    GoldToDestroy = grandChild.gameObject;
                        
                    break;
                }
            }

            DestroyObject(GoldToDestroy);
        }
    }

    GameObject SpawnGold(List<GameObject> EmptyPosList)
    {
        GameObject GoldInTruck= Instantiate(Gold, LocateFirstEmptyPosition(EmptyPosList).position, Quaternion.Inverse(Gold.transform.rotation));
        GoldInTruck.transform.parent = EmptyPosList[EmptyPosList.IndexOf(LocateFirstEmptyPosition(EmptyPosList).gameObject)].gameObject.transform;

        return GoldInTruck;
    }

   

    Transform LocateFirstEmptyPosition(List<GameObject> EmptyPosList)
    {
        GameObject FirstEmptyPos = null;

        foreach (GameObject emptyPos in EmptyPosList)
        {
            if (emptyPos.transform.childCount == 0)
                FirstEmptyPos = emptyPos;   
        }

        return FirstEmptyPos.transform;
    }


    void SpawnStorageForGold()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 SpawnPosLevel0 = new Vector3(x * gridSpacing, 0, z * gridSpacing) + gridOriginLevel0;
                Vector3 SpawnPosLevel1 = new Vector3(x * gridSpacing, 0, z * gridSpacing) + gridOriginLevel1;

                GameObject PositionsForGoldLevel0 = Instantiate(Positions, SpawnPosLevel0, Quaternion.identity);
                GameObject PositionsForGoldLevel1 = Instantiate(Positions, SpawnPosLevel1, Quaternion.identity);

                PositionsForGoldLevel0.transform.parent = GoldStorageHandler.gameObject.transform;
                PositionsForGoldLevel1.transform.parent = GoldStorageHandler.gameObject.transform;

                EmptyPositions.Add(PositionsForGoldLevel0);
                EmptyPositions.Add(PositionsForGoldLevel1);
            }
        }
    }
}
