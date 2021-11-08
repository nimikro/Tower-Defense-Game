using System.Collections;
using UnityEngine;


// unity will save and load these values for us using System.Serializable
// Monobehaviour is removed because we don't want to attach the script on an object
// this is how we create Blueprints to add to other classes
// without system.serializable the data will not show in the inspector
[System.Serializable]
public class TurretBlueprint
{
    public GameObject prefab;
    public int cost;
    public GameObject upgradedPrefab;
    public int upgradeCost;

    public int GetSellAmount()
    {
        return cost / 2;
    }
}
