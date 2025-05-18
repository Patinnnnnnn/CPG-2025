using UnityEngine;

public class InvertItem : MonoBehaviour
{
    public float effectDuration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        player p = other.GetComponent<player>();
        if (p != null)
        {
            p.InvertControls(Mathf.Infinity);
            Destroy(gameObject); // Remove o item da cena
        }
    }
}
