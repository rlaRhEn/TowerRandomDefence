using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    float speed = 1f;
    float lifeTime = 0.5f;
    [SerializeField]Text text;
    DamageTextFactory factory;


    private void OnEnable()
    {
        if (factory == null) return;

        CancelInvoke();
        Invoke(nameof(DeSpawn), lifeTime);

    }
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
    public void ShowDamageText(float damage)
    {
        text.text = damage.ToString();
    }
    public void Init(DamageTextFactory factory)
    {
        this.factory = factory;
        CancelInvoke();
        Invoke(nameof(DeSpawn), lifeTime);
    }
    public void DeSpawn()
    {
        factory.DeSpawn(this);
    }
}
