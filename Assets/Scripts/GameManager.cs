using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // we are using the static BuildManager instance to create a single reference for the build manager that all nodes will share -> Singleton Pattern
    // we could use a public BuildManager on the "Node" script and reference the build manager, but then each node will have its own reference to the BuildManager
    // using the awake function the instance is going to save only one reference to the BuildManager that can be accessed from everywhere using public static
    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one GameManager in scene!");
        }
        instance = this;
    }

    private TurretBlueprint turretToBuild;
    private Node selectedNode;
    public NodeUI nodeUI;

    public GameObject buildEffect;
    public GameObject sellEffect;

    // CanBuild is a property because it is only allowed to get a value
    // Basicly we write a function and assign it immediatly to CanBuild
    public bool CanBuild { get { return turretToBuild != null; } }
    // check if user has enough money to build a turret
    public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }
    
    // either select a node or a turret
    public void SelectNode(Node node)
    {
        if(selectedNode == node)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        turretToBuild = null;
        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        DeselectNode();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }

}
