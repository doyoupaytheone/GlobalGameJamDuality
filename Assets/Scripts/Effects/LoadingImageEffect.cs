using UnityEngine;

public class LoadingImageEffect : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 0.4f;

    private RectTransform _logoTrans;

    private void Awake() => _logoTrans = GetComponent<RectTransform>();

    private void FixedUpdate()
    {
        if (GameManager.Instance.fadeCanvas.alpha >= 0.5) _logoTrans.Rotate(0, 0, rotateSpeed); //If the canvas is at least half way faded, start rotating
    }
}
