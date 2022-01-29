using UnityEngine;

public class ProjectileTrigger : MonoBehaviour
{
    [SerializeField] private Transform objectToMove;
    [SerializeField] private Transform finalPosition;

    private Vector2 startingPosition;
    private Vector2 zeroVelocity = Vector2.zero;
    private bool isMoving;
    private bool isAtStartingPos = true;

    private void Start() => startingPosition = objectToMove.position;

    private void Update()
    {
        if (!isMoving) return;

        if (isAtStartingPos)
        {
            objectToMove.position = Vector2.SmoothDamp(objectToMove.position, finalPosition.position, ref zeroVelocity, 1.25f);

            if (Vector2.Distance(objectToMove.position, finalPosition.position) < 0.001f)
            {
                isMoving = false;
                isAtStartingPos = false;
            }
        }
        else
        {
            objectToMove.position = Vector2.SmoothDamp(objectToMove.position, startingPosition, ref zeroVelocity, 1.25f);

            if (Vector2.Distance(objectToMove.position, startingPosition) < 0.001f)
            {
                isMoving = false;
                isAtStartingPos = true;
            }
        }
    }

    public void TriggerEffect() => isMoving = true;
}
