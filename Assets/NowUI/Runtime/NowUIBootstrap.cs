using System;
using UnityEngine;

namespace NowUIInternal
{
    public class NowUIBootstrap : MonoBehaviour
    {
        public event Action OnPreUpdate;

        public event Action OnUpdate;

        public event Action OnPostUpdate;

        private void Update()
        {
            OnPreUpdate?.Invoke();
            OnUpdate?.Invoke();
            OnPostUpdate?.Invoke();
        }
    }
}