using DG.Tweening;

namespace Game
{
    public interface ICommand
    {
        Sequence Undo();
        CommandMovement Stop();
    }
}
