using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Container : MonoBehaviour
{
    [SerializeField] private ResourceContainer resourceContainer;

    public int cur_Amount;

    public bool isFull;

    public virtual void Start()
    {
        
    }
}
