using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class DialogeManagerScript : MonoBehaviour
{
    public NPCConversation story;
    // Start is called before the first frame update
    void Start()
    {
        ConversationManager.Instance.StartConversation(story);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
