using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PanelManager : MonoBehaviour
{
    [SerializeField] GameObject TruePanel;
    [SerializeField] List<GameObject> Pannels = new List<GameObject>();

    public void PickAPanel(string typeOfPanel)
    {
        if (TruePanel != null)
            TruePanel.active = false;
        else
        {
            TruePanel = Pannels.Find(panel => panel.name == typeOfPanel);

            TruePanel.active = true;
        }       
    }

    public void CloseAPanel()
    {
        TruePanel.active = false;
        TruePanel = null;
    }
   
}
