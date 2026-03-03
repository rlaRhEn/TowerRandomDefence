using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    private void Awake()
    {
        OffRange();

    }

    public void OnRange(Vector3 pos, float range)
    {
        gameObject.SetActive(true);

        float diameter = range ;
        transform.localScale = Vector3.one * diameter;
        transform.position = pos;
    }

    public void OffRange()
    {
        gameObject.SetActive(false);
    }
}
