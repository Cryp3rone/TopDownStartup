using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IPoolable
    {
        void Activate();
        void Deactivate();
        void Destroy();
    }
}
