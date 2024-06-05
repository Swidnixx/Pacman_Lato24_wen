using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehaviour
{
    [SerializeField] private SpriteRenderer normalBody;
    [SerializeField] private SpriteRenderer flashingBody;
    [SerializeField] private SpriteRenderer blueBody;

    [SerializeField] private SpriteRenderer eyesRenderer;

    private bool eaten;

    private void ShowGhostBody(SpriteRenderer bodyToShow)
    {
        SpriteRenderer[] bodies = { normalBody, flashingBody, blueBody };

        foreach (SpriteRenderer body in bodies)
        {
            body.enabled = (body == bodyToShow);
        }
    }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        ShowGhostBody(blueBody);
        eyesRenderer.enabled = false;
        eaten = false;
    }

    public override void Disable()
    {
        base.Disable();

        ShowGhostBody(normalBody);
        eyesRenderer.enabled = true;
        eaten = false;
    }

}
