using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    private Node target;

    public Text upgradeCost;
    public Button upgradeButton;

    public Text sellAmount;

    public GameObject ui;

    // make the ui appear on top of selected turret <- uses gameManager
    public void SetTarget(Node _target)
    {
        target = _target;

        transform.position = target.GetBuildPosition();

        if(!target.isUpgraded)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCost.text = "MAX";
            upgradeButton.interactable = false;
        }

        sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();

        ui.SetActive(true);
    }

    // hide ui after clicking on turret again <- uses gameManager
    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        GameManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        GameManager.instance.DeselectNode();
    }
}
