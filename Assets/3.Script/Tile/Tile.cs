using UnityEngine;

public class Tile : MonoBehaviour
{
    private Tower placedTower;

    public bool HasTower => placedTower != null;
    public Tower PlacedTower => placedTower;

    public bool CanBuildTower(int cost)
    {
        return !HasTower && GameManager.instance.stageController.gold >= cost;
    }

    public void SetTower(Tower tower)
    {
        placedTower = tower;
    }

    public void ClearTower()
    {
        placedTower = null;
    }

}
