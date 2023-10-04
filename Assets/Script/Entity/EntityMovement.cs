using DG.Tweening;
using Game;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EntityMovement : MonoBehaviour
{
    [SerializeField, BoxGroup("Dependencies")] Rigidbody2D _rb;

    [SerializeField, BoxGroup("Configuration")] float _startSpeed;

    #region Events
    [SerializeField, Foldout("Event")] UnityEvent _onStartWalking;
    [SerializeField, Foldout("Event")] UnityEvent _onContinueWalking;
    [SerializeField, Foldout("Event")] UnityEvent _onStopWalking;
    public event UnityAction OnStartWalking { add => _onStartWalking.AddListener(value); remove=> _onStartWalking.RemoveListener(value); }
    public event UnityAction OnContinueWalking { add => _onContinueWalking.AddListener(value); remove=> _onContinueWalking.RemoveListener(value); }
    public event UnityAction OnStopWalking { add => _onStopWalking.AddListener(value); remove=> _onStopWalking.RemoveListener(value); }
    #endregion

    Vector2 MoveDirection { get; set; }
    Vector2 OldVelocity { get; set; }
    Vector2 OldMoveDirection { get; set; }
    Vector2 OriginPoint { get; set; }

    public Alterable<float> CurrentSpeed { get; private set; }

    [SerializeField] float delay = 1f;
    float duration;
    bool canUndo, isRewinding;
    List<CommandMovement> listCmdMove;

    #region EDITOR
#if UNITY_EDITOR
    private void Reset()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _startSpeed = 1f;
    }
#endif
    #endregion

    private void Awake()
    {

        CurrentSpeed = new Alterable<float>(_startSpeed);
        listCmdMove = new List<CommandMovement>();
        OriginPoint = transform.position;
        duration = 0f;
        canUndo = true;
    }

    private void FixedUpdate()
    {
        // FireEvents
        if (MoveDirection.magnitude < 0.01f && OldVelocity.magnitude > 0.01f)
            _onStopWalking?.Invoke();
        else if (MoveDirection.magnitude > 0.01f && OldVelocity.magnitude < 0.01f)
            _onStartWalking?.Invoke();
        else _onContinueWalking?.Invoke();

        // Physics
        _rb.AddForce(MoveDirection * _startSpeed * Time.fixedDeltaTime, ForceMode2D.Force);

        // Keep old data
        OldVelocity = _rb.velocity;
        OldMoveDirection = MoveDirection;
    }

    public void Move(Vector2 direction)
    {
        MoveDirection = direction.normalized;

        if (MoveDirection != OldMoveDirection || duration > delay)
        {
            listCmdMove.Add(new CommandMovement(_rb.gameObject, transform.position, OriginPoint, duration));
            duration = 0f;
            OriginPoint = transform.position;
        }
        else
        {
            duration += Time.deltaTime;
        }
    }

    public void Reverse(bool isReversing)
    {
        isRewinding = isReversing;
        if (isReversing)
            StartCoroutine(ReverseCorout());
    }
    
    IEnumerator ReverseCorout()
    {
        yield return new WaitForSeconds(0f);
        if (canUndo)
        {
            if(listCmdMove != null && listCmdMove.Count > 0)
            {
                canUndo = false;
                ICommand cmd = listCmdMove[listCmdMove.Count-1];
                listCmdMove.RemoveAt(listCmdMove.Count-1);
                cmd.Undo().OnComplete(() => canUndo = true);
            }
        }
        else if (isRewinding)
        {
            StartCoroutine(ReverseCorout());
        }
    }
    public void StopReverse()
    {
        if (listCmdMove != null && listCmdMove.Count > 0)
        {
            listCmdMove.Add(listCmdMove.Last().Stop());
        }
    }

    public void MoveToward(Transform target) => MoveDirection = (target.position - _rb.transform.position).normalized;

    public void AlterSpeed(float factor)
    {

    }
    public void ResetSpeed()
    {

    }


}
