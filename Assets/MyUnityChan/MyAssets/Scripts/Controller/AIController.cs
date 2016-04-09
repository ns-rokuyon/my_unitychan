using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class AIController : Controller {
        public bool isStopped = false;

        // Update is called once per frame
        public virtual void Update() {
            if ( PauseManager.isPausing() ) isStopped = true;
            else isStopped = false;
        }
    }
}