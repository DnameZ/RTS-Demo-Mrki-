using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class collingSystem : MonoBehaviour
{
    public static collingSystem CoolingSys;

    [SerializeField] private Button coolDownButton;

    const string CoolDownBttn = "CoolDown";

    public WaterCont container;

    private void Awake()
    {
        CoolingSys = this;
    }

    private void OnEnable()
    {
        GameObject.Find(CoolDownBttn).TryGetComponent(out Button button);
        coolDownButton = button;

        if (coolDownButton != null) 
            coolDownButton.onClick.AddListener(StartChillingDown);

    }

    public void StartChillingDown()
    {
        if(container!=null)
        {
            container.ChillDown();
            container.CoolDownDstructures();
        }
            
    }
}
