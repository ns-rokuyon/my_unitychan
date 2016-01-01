using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class CommandRecorder : ObjectBase {

        public int buffer_size;
        public bool debug_mode;

        private RingBuffer<List<bool>> history;
        private Controller controller;
        private readonly int command_margin = 3;

        void Awake() {
            history = new RingBuffer<List<bool>>(buffer_size);
            controller = gameObject.GetComponent<Controller>();
        }

        void Update() {
            history.add(controller.getAllInputs());
            if ( debug_mode ) {
                logging(history.getHead());
            }
        }

        private void logging(List<bool> inputs) {
            string message = ""; 
            foreach ( bool input in inputs ) {
                if ( input )
                    message += "T, ";
                else
                    message += "F, ";
            }
            Debug.Log(message);
        }
        
        public bool command(List<Controller.InputCode> input_lists) {
            if ( !history.isFull() ) return false;

            int prev_frame = 0;
            int prev_frame_end = input_lists.Count + command_margin;
            if ( prev_frame_end >= buffer_size ) {
                prev_frame_end = buffer_size - 1;
            }

            List<bool> cmd_accepts = new List<bool>();
            for ( int i = 0; i < input_lists.Count; i++ ) {
                cmd_accepts.Add(false);
            }

            int cmdIdx = 0;
            Controller.InputCode input = input_lists[cmdIdx];
            while( prev_frame <= prev_frame_end ) {
                var inputs = history.getPrev(prev_frame);
                if ( inputs[(int)input] ) {
                    cmd_accepts[cmdIdx] = true;
                    cmdIdx++;
                    if ( cmdIdx >= cmd_accepts.Count ) {
                        break;
                    }
                    input = input_lists[cmdIdx];
                }
                prev_frame += 1;
            }

            if ( debug_mode ) logging(cmd_accepts);

            return cmd_accepts.Count(_ => _) == input_lists.Count;
        }
        
    }
}