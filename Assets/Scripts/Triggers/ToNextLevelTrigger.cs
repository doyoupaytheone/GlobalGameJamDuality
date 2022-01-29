using UnityEngine;

public class ToNextLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) GameManager.Instance.ToNextScene();
    }
}
