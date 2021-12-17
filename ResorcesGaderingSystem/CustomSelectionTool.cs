using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CustomSelectionTool : MonoBehaviour
{
    public static CustomSelectionTool SelectionTool;

    private WaterCont WatercontainerForResource;

    public Button ButtonForMining;
    public GameObject CurSelectAbleObject;
    public GameObject ObjectChanged;

    const string Unit = "Unit";
    const string container = "Container";

    private void Awake() => SelectionTool = this;

    private void FixedUpdate() => DropARayCast();


    public void MoveUnit(Vector3 Destination, NavMeshAgent ObjectToMove)
    {
        ICommand command = new MoveObject(Destination, ObjectToMove);

        Invoker.AddComand(command);
    }

    void UnSelectUnit(GameObject ObjectChanged)
    {
        if (Input.GetMouseButtonDown(1))
        {
            ChangeShader.ChangeShaderOfObject(ChangeShader.Standard, ObjectChanged);
            SelectedUnits.Selected_Units.Remove(ObjectChanged);
            ObjectChanged = null;
        }
    }

    void RayOnCoolingContainer(RaycastHit hit)
    {
        if (hit.collider.gameObject.name == container)
            WaterCont.CoolingZone.gameObject.SetActive(true);
        else
        {
            if (WaterCont.CoolingZone != null)
                WaterCont.CoolingZone.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            hit.collider.gameObject.TryGetComponent(out WatercontainerForResource);
            collingSystem.CoolingSys.container = WatercontainerForResource;
        }


    }


    Ray DropARayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
 

            switch (hit.collider.transform.gameObject.tag==Unit)
            {
                case true:
                    ObjectChanged = hit.collider.transform.gameObject;
                    ChangeShader.ChangeShaderOfObject(ChangeShader.OnHoverAndPick, ObjectChanged);

                    if (Input.GetMouseButtonDown(0))
                    {
                        CurSelectAbleObject = hit.collider.gameObject;
                        CurSelectAbleObject.gameObject.TryGetComponent(out TruckContainer truck);
                        truck.StartOperationMining();
                        SelectedUnits.Selected_Units.Add(ObjectChanged);
                    }
                    break;

                case false:

                    if (ObjectChanged != null)
                    {
                        ChangeShader.ChangeShaderOfObject(ChangeShader.Standard, ObjectChanged);
                        ObjectChanged = null;
                    }

                    break;
            }

            RayOnCoolingContainer(hit);
            UnSelectUnit(ObjectChanged);
        }

        return ray;
    }
}
