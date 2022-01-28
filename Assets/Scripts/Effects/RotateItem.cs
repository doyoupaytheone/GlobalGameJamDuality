using UnityEngine;

public class RotateItem : MonoBehaviour
{
    public bool isRotatingX;
    public bool isRotatingY;
    public bool isRotatingZ;
    public float rotationSpeed;

    private Transform thisTrans;

    private void Awake() => thisTrans = this.GetComponent<Transform>();

    private void Update()
    {
        float x = thisTrans.rotation.x;
        float y = thisTrans.rotation.y;
        float z = thisTrans.rotation.z;

        if (isRotatingX) x += rotationSpeed;
        if (isRotatingY) y += rotationSpeed;
        if (isRotatingZ) z += rotationSpeed;

        if (isRotatingX || isRotatingY || isRotatingZ) thisTrans.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
    }
}
