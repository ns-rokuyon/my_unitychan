using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public interface Lockable {
        void doLock();
        void doUnlock();
    }
}