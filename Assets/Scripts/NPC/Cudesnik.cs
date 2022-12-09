using System;
using UnityEngine;

public class Cudesnik : MonoBehaviour
{
    [SerializeField] private float appearDistance = 2f;
    [SerializeField] private float interactDistance = 1f;
    [SerializeField] private Canvas tutorialScreen;
    
    private bool _appear = false;
    private Animator _animator;
    private void Start()
    {
        InputHandler.Instance.onActionBtnUp += TryShowTutorial;
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float dist = (Player.Instance.transform.position - transform.position).magnitude;
        if (!_appear)
        {
            if (dist <= appearDistance)
            {
                _animator.SetTrigger("appear");
                _appear = true;
            }
        }

        if (tutorialScreen.isActiveAndEnabled && dist > appearDistance + 0.1f)
        {
            HideTutorial();
        }
    }

    private void TryShowTutorial()
    {
        float dist = (Player.Instance.transform.position - transform.position).magnitude;
        if (dist > interactDistance) return;
        if (tutorialScreen.isActiveAndEnabled) HideTutorial();
        else{ShowTutorial();}
    }

    private void ShowTutorial()
    {
        tutorialScreen.gameObject.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorialScreen.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        InputHandler.Instance.onActionBtnUp -= TryShowTutorial;
    }
}
