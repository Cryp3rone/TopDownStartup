using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationWait : CustomYieldInstruction
{
    private Animation anim;

    public AnimationWait(Animation a)
    {
        this.anim = a;

        if (a.clip.isLooping) throw new System.Exception();

        a.Play();
    }

    public override bool keepWaiting => anim.isPlaying;
}

public static class AnimationExtension
{
    public static AnimationWait PlayAndWaitCompletion(this Animation @this)
        => new AnimationWait(@this);
}



public class PlayerBrain : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependencies")] EntityMovement _movement;

    [SerializeField, BoxGroup("Input")] InputActionProperty _moveInput;
    [SerializeField, BoxGroup("Input")] InputActionProperty _attackInput;
    [SerializeField, BoxGroup("Input")] InputActionProperty _reverseInput;

    bool isReversing;

    private void Start()
    {
        // Move
        _moveInput.action.started += UpdateMove;
        _moveInput.action.performed += UpdateMove;
        _moveInput.action.canceled += StopMove;
        // Attack
        //_attackInput.action.started += Attack;
        // Rollback 
        _reverseInput.action.started += PerformReverse;
        _reverseInput.action.canceled += StopReverse;

        isReversing = false;
    }




    void run()
    {
        var speedbase = 10;
        var armurespeed = 1.3;
        var coffeefactor = 1.2f;


        var s = speedbase * armurespeed * coffeefactor;
    }





    Coroutine _maCoroutine;
    public void RunCoucou()
    {
        if (_maCoroutine != null) return;

        int i = 10;
        _maCoroutine = StartCoroutine(coucouRoutine());
        IEnumerator coucouRoutine()
        {
            Animation a = GetComponent<Animation>();
            yield return new AnimationWait(a);
            yield return a.PlayAndWaitCompletion();

            var wait = new WaitForSeconds(10f);
            i++;
            yield return wait;
            i++;
            yield return wait;
            i++;
            yield return wait;

            _maCoroutine = null;
            yield break;
        }
    }

    private void OnDestroy()
    {
        // Move
        _moveInput.action.started -= UpdateMove;
        _moveInput.action.performed -= UpdateMove;
        _moveInput.action.canceled -= StopMove;

    }


    private void UpdateMove(InputAction.CallbackContext obj)
    {
        if (!isReversing)
            _movement.Move(obj.ReadValue<Vector2>().normalized);
    }
    private void StopMove(InputAction.CallbackContext obj)
    {
        _movement.Move(Vector2.zero);
    }

    private void PerformReverse(InputAction.CallbackContext obj)
    {
        isReversing = true;
        _movement.Move(Vector2.zero);
        _movement.Reverse(isReversing);

    }
    private void StopReverse(InputAction.CallbackContext obj)
    {
        isReversing = false;
        _movement.Reverse(isReversing);
    }
}
