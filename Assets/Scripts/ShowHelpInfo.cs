using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class ShowHelpInfo : MonoBehaviour // отвечает за вывод текста над интерактивными элементами
{
    private BoxCollider2D collider2D;
    private Animator animator;
    private void Awake()
    {
        collider2D = GetComponent<BoxCollider2D>();
        collider2D.isTrigger = true;
        animator = GetComponent<Animator>();
        animator.SetBool("IsShowed", false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            animator.SetBool("IsShowed", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            animator.SetBool("IsShowed", false);
        }
    }
}
