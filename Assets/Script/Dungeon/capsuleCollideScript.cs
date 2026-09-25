using UnityEngine;

public class capsuleCollideScript : MonoBehaviour
{
    [SerializeField] CapsuleCollider _capsuleCollider;


    public event System.Action<Collision> OnCollided;

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collied with: " + col.gameObject.name);

        OnCollided?.Invoke(col);
    }
}
