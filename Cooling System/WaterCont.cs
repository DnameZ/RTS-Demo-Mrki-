using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaterCont : Container
{
    [SerializeField] private Collider[] DStructersInRange;
    [SerializeField] private List<DStructure> DstructersToCoolDown = new List<DStructure>();

    public float sphereRadius;

    private Coroutine CoolGradaually;

    public LayerMask layer;

    public static Transform CoolingZone;

    const string coolingzone = "CoolingZone";
    const string OperationCooling = "GoDown";
    const string LowerTemperature = "loweringTemperature";

    private Animator anim;
        
    public DStructure dStructure;

    public override void Start()
    {
        anim = GetComponent<Animator>();
        base.Start();
    }

    private void OnEnable() => CoolingZone = this.gameObject.transform.Find(coolingzone);

    private void FixedUpdate() => DStructersInRange = Physics.OverlapSphere(this.transform.position, sphereRadius, layer);


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(this.transform.position, sphereRadius);
    }

    public void ChillDown()
    {
        while (DStructersInRange.Length != 0)
        {
            FindDStructureWithHighestTemperature();
            return;
        }
    }

    void FindDStructureWithHighestTemperature()
    {
        foreach (Collider Structure in DStructersInRange)
        {
            Structure.TryGetComponent(out dStructure);

            if (dStructure.DefensiveStructure.curTemperatureOfBulding > dStructure.DefensiveStructure.maxTemperatureOfBulding)
            {
                DstructersToCoolDown.Add(dStructure);
            }
            else
            {
                Debug.Log("it should't be added to bulding with high temp");
            }
        }
    }

    public IEnumerator CoolingDown()
    {
        while (dStructure.DefensiveStructure.curTemperatureOfBulding > 50)
        {
            foreach (DStructure dStructure in DstructersToCoolDown)
            {
                if (dStructure.DefensiveStructure.curTemperatureOfBulding <= 50)
                {
                    DstructersToCoolDown.Remove(dStructure);
                }
                yield return new WaitForSeconds(3);

                dStructure.DefensiveStructure.curTemperatureOfBulding -= 2;
            }

            yield return null;
        }
    }

    public void CoolDownDstructures()
    {
        if (DstructersToCoolDown.Count > 0)
            InvokeRepeating(LowerTemperature, 2, 2);
    }

    private void loweringTemperature()
    {
        foreach (DStructure dstructure in DstructersToCoolDown.ToArray())
        {
            dstructure.DefensiveStructure.curTemperatureOfBulding -= 2;

            if (dstructure.DefensiveStructure.curTemperatureOfBulding == 50)
                DstructersToCoolDown.Remove(dstructure);
        }

        if (DstructersToCoolDown.Count == 0)
            CancelInvoke(LowerTemperature);
    }
}
