using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public event EventHandler<OnSquareHitEventArgs> OnSquareHit = (sender, e) => { };
    public event EventHandler<OnSquareHeldEventArgs> OnSquareHeld = (sender, e) => { };
    public event EventHandler<OnSquareDestroyedEventArgs> OnSquareDestroyed = (sender, e) => { };

    public int SquareIndex;

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public virtual void Hit()
    {
        OnSquareHitEventArgs args = new OnSquareHitEventArgs();

        OnSquareHit.Invoke(this, args);
    }

    public virtual void Hold()
    {
        OnSquareHeldEventArgs args = new OnSquareHeldEventArgs();
        
        OnSquareHeld.Invoke(this, args);
    }

    public virtual void DestroySquare()
    {
        OnSquareDestroyedEventArgs args = new OnSquareDestroyedEventArgs();

        OnSquareDestroyed.Invoke(this, args);

        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        boxCollider.enabled = false;

        Color current = spriteRenderer.color;

        while (current.a > 0.0f)
        {
            current.a -= 7.0f * Time.deltaTime;

            spriteRenderer.color = current;

            yield return null;
        }

        Destroy(this.gameObject);
    }
}

public class OnSquareHitEventArgs : EventArgs
{

}

public class OnSquareHeldEventArgs : EventArgs
{

}

public class OnSquareDestroyedEventArgs : EventArgs
{

}
