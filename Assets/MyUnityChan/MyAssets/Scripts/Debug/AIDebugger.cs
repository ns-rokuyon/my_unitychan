using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    [RequireComponent(typeof(AIModel))]
    public class AIDebugger : ObjectBase {

        [TextArea(10, 20)]
        public string log_text;

        public int log_length_max = 50;

        public AIModel model { get; private set; }
        public Queue<string> logs;

        // Use this for initialization
        void Awake() {
            model = GetComponent<AIModel>();
            logs = new Queue<string>();
        }

        public void pushLog(string s) {
            s = "Frame(" + Time.frameCount + ") " + s;

            if ( logs.Count == log_length_max )
                logs.Dequeue();

            logs.Enqueue(s);
            log_text = "";
            logs.ToArray().ToList().ForEach(log => {
                log_text += log;
                log_text += "\n";
            });
        }
    }

}
