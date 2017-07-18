using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    private Transform panel;
    private Transform prefab;

    private Agent agent;
    private List<Chest> chestList = new List<Chest>();

    void Awake()
    {
        panel = this.transform.FindChild("Panel");
        prefab = this.transform.FindChild("Prefab");
        prefab.gameObject.SetActive(false);
    }
}