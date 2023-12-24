using UnityEngine;
using DialogueEditor;


public class conversation : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;


    private void OnTriggerStay(Collider other)  
    {
        if (other.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                ConversationManager.Instance.StartConversation(myConversation);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ConversationManager.Instance.EndConversation();
        }
    }
}
