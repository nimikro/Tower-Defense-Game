using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color notEnoughMoneyColor;
    public Vector3 positionOffset;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;

    private Renderer rend;
    private Color startColor;

    GameManager gameManager;

    void Start()
    { 
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        gameManager = GameManager.instance;
    }
    
    // get location of node where to build
    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    void OnMouseDown()
    {
        // fixes highlight bug when mouse is over a turret in the shop as well as a node
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
                    
        // if a turret exists on node -> make sure user can't build another turret
        if(turret != null)
        {
            gameManager.SelectNode(this);
            return;
        }

        if (!gameManager.CanBuild)
        {
            return;
        }
        // else if a turret doesn't exist, build turret
        BuildTurret(gameManager.GetTurretToBuild());
    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        // if player cant afford a turret -> show an error message
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not enough Money!");
            return;
        }
        // else subtract the money from player money
        PlayerStats.Money -= blueprint.cost;
        // (GameObject) cast is used on instantiate to save the new object in a turret gameobject so that we can destroy it afterwards
        // AND CLEAN UP THE HIERARCHY <---- REMEMBER THAT
        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        turretBlueprint = blueprint;

        GameObject effect = (GameObject)Instantiate(gameManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Debug.Log("Turret built! Money left: " + PlayerStats.Money);

    }

    public void UpgradeTurret()
    {
        // if player cant afford an upgrade -> show an error message
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough Money to upgrade!");
            return;
        }
        // else subtract the money from player money
        PlayerStats.Money -= turretBlueprint.upgradeCost;
        // destroy old turret
        Destroy(turret);

        // Build new turret
        // (GameObject) cast is used on instantiate to save the new object in a turret gameobject so that we can destroy it afterwards
        // AND CLEAN UP THE HIERARCHY <---- REMEMBER THAT
        GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        GameObject effect = (GameObject)Instantiate(gameManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
        
        isUpgraded = true;

        Debug.Log("Turret Upgraded! Money left: " + PlayerStats.Money);

    }

    public void SellTurret()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount();
        // do sell effect before destroying
        GameObject effect = (GameObject)Instantiate(gameManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(turret);
        turretBlueprint = null;
        isUpgraded = false;
    }

    // changes white node color to allow for user input
    void OnMouseEnter()
    {
        // fixes highlight bug when mouse is over a turret in the shop as well as a node
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        if(!gameManager.CanBuild)
        {
            return;
        }
        // if user has enough money -> use hoverColor on node, else use a red color
        if (gameManager.HasMoney)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = notEnoughMoneyColor;
        }
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
