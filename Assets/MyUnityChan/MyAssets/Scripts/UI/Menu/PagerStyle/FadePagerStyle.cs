using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using System;

namespace MyUnityChan {
    public class FadePagerStyle : MenuPagerStyle {
        [SerializeField]
        private float duration;

        [SerializeField]
        private float fadeout_alpha;

        public override Sequence nextPager(MenuPage new_current_page, IEnumerable<MenuPage> out_pages) {
            Sequence pager = DOTween.Sequence();

            if ( new_current_page.canvas_group == null ) {
                DebugManager.warn("The current page has no CanvasGroup");
            }
            else {
                Tween fader = new_current_page.canvas_group.DOFade(1.0f, duration);
                pager.Join(fader);
            }

            out_pages.ToList().ForEach(page => {
                if ( page.canvas_group == null ) {
                    DebugManager.warn("The page has no CanvasGroup");
                }
                else {
                    Tween _fader = page.canvas_group.DOFade(fadeout_alpha, duration);
                    pager.Join(_fader);
                }
            });
            return pager;
        }

        public override Sequence prevPager(MenuPage new_current_page, IEnumerable<MenuPage> out_pages) {
            return nextPager(new_current_page, out_pages);
        }

        public override Sequence initPager(MenuPage first_page, IEnumerable<MenuPage> out_pages) {
            return nextPager(first_page, out_pages);
        }
    }
}