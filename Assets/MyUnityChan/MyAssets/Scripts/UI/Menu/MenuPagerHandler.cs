using UnityEngine;
using System.Collections;

namespace MyUnityChan {
    public class MenuPagerHandler : ObjectBase {
        public MenuPager pager;

        public virtual void goPageTo(int i) {
            pager.goPageTo(i);
        }

        public virtual void goNextPage() {
            pager.goNextPage();
        }

        public virtual void goPrevPage() {
            pager.goPrevPage();
        }
    }
}