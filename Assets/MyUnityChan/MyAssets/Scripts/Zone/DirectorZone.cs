using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using System.Linq;

namespace MyUnityChan {
    public class DirectorZone : SensorZone {
        public bool skip_setup;
        [ReadOnly] public bool played;

        public PlayableDirector director { get; protected set; }
        public System.Action<Playable> onTimelineEnd { get; protected set; }

        void Awake() {
            onTimelineEnd = (p) => { };
            director = GetComponent<PlayableDirector>();
            only_firsttime = false;

            if ( !skip_setup && director ) {
                onPlayerEnterCallback = (player, collider) => {
                    play(player, collider);
                };
            }
        }

        public void play(Player player, Collider collider) {
            if ( played )
                return;
            played = true;

            if ( player.manager.now == Const.CharacterName.MINI_UNITYCHAN ) {
                player.manager.switchPlayerCharacter();
                player = player.manager.getNowPlayerComponent();
            }
            player.freeze();

            var main_camera = player.getPlayerCamera();
            main_camera.fadeOut(Const.Frame.PLAYABLE_DIRECTOR_START_TRANSITION_FADE);

            delay(Const.Frame.AREA_TRANSITION_FADE, () => {
                // Disable main camera temporally
                main_camera.effect.disableFadeCanvas();
                main_camera.gameObject.SetActive(false);

                // Disable HUD temporally
                GameStateManager.hideHUD();

                // Move player to base position
                player.transform.position = transform.position;
                player.zeroVelocity();
                player.lookAhead();

                // Bind player animator
                // TODO: Outputs have no binding "PlayerAnimation" case
                PlayableBinding binding = director.playableAsset.outputs.First(c => c.streamName == "PlayerAnimation");
                director.SetGenericBinding(binding.sourceObject, player.getAnimator());

                // Callback on end
                onTimelineEnd = (p) => {
                    main_camera.gameObject.SetActive(true);
                    main_camera.effect.enableFadeCanvas();
                    main_camera.fadeIn(Const.Frame.PLAYABLE_DIRECTOR_START_TRANSITION_FADE,
                                       Const.Frame.PLAYABLE_DIRECTOR_START_TRANSITION_KEEP_BLACKOUT);
                };

                // Start playable timeline
                director.Play();
            });
        }
    }
}
