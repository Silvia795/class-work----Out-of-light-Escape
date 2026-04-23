using UnityEngine;
using System.Collections;

public class DamageableObject : MonoBehaviour, IDamage
{
    [Header("----- Stats -----")]
    [Range(1, 20)][SerializeField] int HP = 3;

    [Header("----- Components -----")]
    [SerializeField] Renderer model;

    [Header("----- Effects -----")]
    [SerializeField] float flashTime = 0.1f;

    Color colorOrig;

    void Start()
    {
        if (model == null)
            model = GetComponentInChildren<Renderer>();

        if (model != null)
            colorOrig = model.material.color;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(flashRed());
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
