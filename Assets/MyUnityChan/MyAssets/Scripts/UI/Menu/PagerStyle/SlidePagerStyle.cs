using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;

namespace MyUnityChan {
    public class SlidePagerStyle : MenuPagerStyle {
        [SerializeField]
        private float duration;

        [SerializeField]
        private Vector2 offset;

        public override Sequence nextPager(MenuPage new_current_page, IEnumerable<MenuPage> out_pages) {
            Sequence pager = DOTween.Sequence();

            var rts = out_pages.Select(page => page.rt).ToList();
            rts.Add(new_current_page.rt);
            rts.ForEach(rt => {
                Tween slider = rt.DOAnchorPos(rt.anchoredPosition - offset, duration);
                pager.Join(slider);
            });
            return pager;
        }

        public override Sequence prevPager(MenuPage new_current_page, IEnumerable<MenuPage> out_pages) {
            Sequence pager = DOTween.Sequence();

            var rts = out_pages.Select(page => page.rt).ToList();
            rts.Add(new_current_page.rt);
            rts.ForEach(rt => {
                Tween slider = rt.DOAnchorPos(rt.anchoredPosition + offset, duration);
                pager.Join(slider);
            });
            return pager;
        }
    }
}