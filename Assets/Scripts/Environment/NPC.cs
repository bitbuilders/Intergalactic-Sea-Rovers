using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    private void Start()
    {
        base.Initialize(this);
    }

    protected override void Interact()
    {
        InteractionManager interactionManager = InteractionManager.Instance;
        var dialog = interactionManager.CreateInteraction(m_interacteeInfo, InteractionData.InteractionType.CONVERSATION, "test2", m_player.m_interacteeInfo);
        interactionManager.Interact(dialog, this);
    }
}
