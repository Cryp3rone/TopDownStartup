using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class CommandMovement : MonoBehaviour, ICommand
    {
        private GameObject storedEntity;
        private Vector3 startPosition;
        private Vector3 endPosition;
        private float duration;
        private Sequence seq;

        public CommandMovement(GameObject storedEntity, Vector3 startPosition, Vector3 endPosition, float duration)
        {
            this.storedEntity = storedEntity;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.duration = duration;
        }

        public Sequence Undo()
        {
            seq = DOTween.Sequence();

            return seq.Append(storedEntity.transform.DOMove(startPosition, duration).SetEase(Ease.Linear));
        }

        public CommandMovement Stop()
        {
            CommandMovement cmdMove = new CommandMovement(storedEntity, storedEntity.transform.position, endPosition, duration - seq.position);
            seq.Kill();
            return cmdMove;
        }
    }
}
