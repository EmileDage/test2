using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_talking : Npc_Basic
{

    private void Start()
    {
        conversation = this.gameObject.GetComponent<DialogueTrigger>();
    }

    public override void Interact(Player joueur)//quand joueur interagit avec NPC
    {
        if (!talked)
        { //le joueur na pas parler au npc une premiere fois yet  
            if (!manager.FadeOut)
            {
                conversation.TriggerDialogueStart();
                talked = true;
            }

        }
        else{
            conversation.TriggerDialogueIdleChat();
            talked = false;
        }
       
    }
}
