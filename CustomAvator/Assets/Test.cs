/**********************************************************
*Author: wangjiaying
*Date: 
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

    public GameObject target;

    void Start()
    {
        CustomAvator.Runtime.CustomAvatorManager.GetInstance.BuildingMesh(transform, true, target.transform);
    }

}
