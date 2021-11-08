using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserTurret;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    public void SelectStandardTurret()
    {
        Debug.Log("standard turret selected");
        gameManager.SelectTurretToBuild(standardTurret);
    }
    public void SelectMissileLauncher()
    {
        Debug.Log("Missile Launcher selected");
        gameManager.SelectTurretToBuild(missileLauncher);
    }
    public void SelectLaserTurret()
    {
        Debug.Log("Laser Turret selected");
        gameManager.SelectTurretToBuild(laserTurret);
    }

}
