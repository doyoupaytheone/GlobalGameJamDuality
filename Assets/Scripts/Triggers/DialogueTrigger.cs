using UnityEngine;

//Can place this trigger on an object to trigger a dialogue to play
public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogue;
    private DialogueManager DialogueManager;

    public bool triggered = false;

    private void Awake() => DialogueManager = FindObjectOfType<DialogueManager>();

    private void Start()
    {
        //If this is an opening dialouge (and tagged properly), it plays on start
        if (this.gameObject.CompareTag("OpeningDialogue")) TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        if (triggered) return;

        triggered = true;
        DialogueManager.StartDialogue(dialogue);
    }

    private void OnTriggerEnter(Collider other)
    {
        //If this is a locationally triggered dialogue, it plays the first time the player enters that location
        if (other.gameObject.CompareTag("Player") && !this.CompareTag("OpeningDialogue")) TriggerDialogue();
    }
}
