using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    [System.Serializable]
    public class MenuNavbarButton<T> : MenuButton {
        [SerializeField]
        public T nav;
    }

}
