using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTruck : TruckContainer
{
    const string FillRef = "Fill_REF";
    const string Water = "Water";

    public override string ResourceToFind()
    {
        string Res = truck.TypeOfResourceTransporting.ToString() + "Resource";

        return Res;
    }

    public override void GoToContainer()
    {
        if (truck.CurContainerToFill == null)
            truck.CurContainerToFill = FindContainer(Water).transform;
        else
            CustomSelectionTool.SelectionTool.MoveUnit(truck.CurContainerToFill.Find("FillContainerPosition").position, TruckEngine);
    }

    public override void Mine()
    {
        float change = 1.2f;
        int DelayAmount = 5;
        Timer += Time.deltaTime;

        if (Timer >= DelayAmount)
        {
            Timer = 0f;
            setResources.resources.AmountOfResource -= 10;
            Current_Amount += 10;
            valueOfMining += change;
        }

        while (OnMine)
        {
            RendererOffLiquidMaterialTruck.material.SetFloat(FillRef, SmoothlyTransition(RendererOffLiquidMaterialTruck.material.GetFloat(FillRef), valueOfMining));
            return;
        }
    }

    public override void Fill()
    {
        float change = 0.1f;
        int DelayAmount = 5;
        Timer += Time.deltaTime;

        if (Timer >= DelayAmount)
        {
            Timer = 0f;
            Current_Amount -= 5;
            containerForResource.cur_Amount += 5;
            valueOfFilling += change;
            valueOfMining -= 0.68f;
        }

        while (OnPlace)
        {
            RendererOffLiquidMaterialContainer.material.SetFloat(FillRef, SmoothlyTransition(RendererOffLiquidMaterialContainer.material.GetFloat(FillRef), valueOfFilling));
            RendererOffLiquidMaterialTruck.material.SetFloat(FillRef, SmoothlyTransition(RendererOffLiquidMaterialTruck.material.GetFloat(FillRef), valueOfMining));
            return;
        }
    }
}


