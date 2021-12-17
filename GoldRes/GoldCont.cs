using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoldCont : Container
{
    public static GoldCont goldContainer;

    public List<GameObject> EmptyPositions = new List<GameObject>();
    [SerializeField] GameObject Gold;
    [SerializeField] GameObject Positions;
    GameObject GoldHandelerChild;

    public int gridX;
    public int gridZ;

    public float gridSpacing = 1f;

    public Vector3 gridOrigin;

    const string GoldHandler = "GoldHandler";


    private void Awake()
    {
        goldContainer = this;
    }


    public override void Start()
    {
        GoldHandelerChild = this.transform.Find(GoldHandler).gameObject;
        gridOrigin = new Vector3(GoldHandelerChild.transform.position.x-7f,GoldHandelerChild.transform.position.y+1.3f,GoldHandelerChild.transform.position.z-7.5f);
        SpawnStorageForGold();
        base.Start();
    }

    void SpawnAGold()
    {
            GameObject Goldy = Instantiate(Gold, LocateFirstEmptyPosition().position, Quaternion.Inverse(Gold.transform.rotation));

            Goldy.transform.parent = EmptyPositions[EmptyPositions.IndexOf(LocateFirstEmptyPosition().gameObject)].gameObject.transform;
    }


    Transform LocateFirstEmptyPosition()
    {
        GameObject FirstEmptyPos = null;

        foreach(GameObject emptyPos in EmptyPositions)
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
                Vector3 SpawnPos = new Vector3(x * gridSpacing, 0, z * gridSpacing) + gridOrigin;
                GameObject PositionsForGold = Instantiate(Positions, SpawnPos, Quaternion.identity);
                PositionsForGold.transform.parent = GoldHandelerChild.gameObject.transform;
                EmptyPositions.Add(PositionsForGold);
            }
        }
    }
}
