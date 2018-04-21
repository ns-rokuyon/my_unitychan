using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;

namespace MyUnityChan {
    public class ScalePagerStyle : MenuPagerStyle {
        [SerializeField]
        private float duration;

        [SerializeField]
        private Vector3 scale_down_to;

        public override Sequence nextPager(MenuPage new_current_page, IEnumerable<MenuPage> out_pages) {
            Sequence pager = DOTween.Sequence();

            Tween scaler = new_current_page.rt.DOScale(new_current_page.inited_scale, duration);
            pager.Join(scaler);

            out_pages.ToList().ForEach(page => {
                Tween _scaler = page.rt.DOScale(scale_down_to, duration);
                pager.Join(_scaler);
            });
            return pager;
        }

        public override Sequence prevPager(MenuPage new_current_page, IEnumerable<MenuPage> out_pages) {
            return nextPager(new_current_page, out_pages);
        }
    }
}