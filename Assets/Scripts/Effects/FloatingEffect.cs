using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [SerializeField] float floatDistance = 0.25f;
    [SerializeField] float floatSpeed = 0.5f;
    [SerializeField] float floatVariance = 0.2f;

    private Transform objectTrans;
    private Vector2 startingPos;
    private int direction = 1;

    private void Awake() => objectTrans = GetComponent<Transform>();

    private void Start()
    {
        startingPos = objectTrans.position; //Sets the starting position
        floatSpeed += Random.Range(-floatVariance, floatVariance); //Sets a random float speed
    }

    private void Update()
    {
        //When the object has reached it's max float distance, change direction
        if (startingPos.y - objectTrans.position.y <= -floatDistance)
            direction = -1;
        if (startingPos.y - objectTrans.position.y >= floatDistance)
            direction = 1;

        //Moves the object in the current direction
        objectTrans.Translate(Vector2.up * direction * floatSpeed * Time.deltaTime);
    }
}
