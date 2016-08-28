﻿using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class ApproachingPlayerAI : AIController {
        public bool allow_attack;
        public float distance_judgment_close_range;

        public bool close_range { get; private set; }

        private GameObject target;
        private int target_update_time_span;
        private string area_name;

        private bool coroutine_running = false;

        // Use this for initialization
        public override void Start() {
            base.Start();
            target_update_time_span = 60;
            area_name = AreaManager.Instance.getAreaNameFromObject(self.gameObject);
        }

        // Update is called once per frame
        public override void Update() {
            base.Update();

            if ( isStopped ) {
                return;
            }

            if ( Time.frameCount % target_update_time_span == 0 ) {
                target = Enemy.findNearestPlayer(self.transform.position);
            }

            if ( target == null ) {
                horizontal_input = 0.0f;
                return;
            }

            if ( target.GetComponent<Player>().getAreaName() != area_name ) {
                return;
            }

            if ( !coroutine_running )
                StartCoroutine(switchDirection());

            if ( allow_attack ) {
                if ( close_range )
                    inputs[(int)InputCode.ATTACK] = true;
                else
                    inputs[(int)InputCode.ATTACK] = false;
            }
        }

        private IEnumerator switchDirection() {
            float target_x = target.transform.position.x;
            float self_x = self.transform.position.x;

            if ( Mathf.Abs(target_x - self_x) <= distance_judgment_close_range ) {
                close_range = true;
            }
            else {
                close_range = false;
            }

            if ( target_x < self_x ) {
                yield return new WaitForSeconds(0.5f);
                horizontal_input = -1.0f;
            }
            else {
                yield return new WaitForSeconds(0.5f);
                horizontal_input = +1.0f;
            }
        }

    }
}
