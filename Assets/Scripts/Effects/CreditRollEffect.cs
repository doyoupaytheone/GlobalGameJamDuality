using System.Collections;
using UnityEngine;

public class CreditRollEffect : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 40f;

    private Transform thisTrans;
    private Vector2 startingPosition;
    private bool isRolling;

    private void Awake() => thisTrans = GetComponent<Transform>();

    private void Start() => startingPosition = thisTrans.position;

    private void Update()
    {
        if (isRolling) thisTrans.Translate(Vector2.left * rollSpeed * Time.deltaTime);
    }

    public void ToggleCreditRoll() => StartCoroutine(WaitToToggle(2.5f));
    
    private IEnumerator WaitToToggle(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        
        if (isRolling) thisTrans.position = startingPosition;
        isRolling = !isRolling;
    }
}
