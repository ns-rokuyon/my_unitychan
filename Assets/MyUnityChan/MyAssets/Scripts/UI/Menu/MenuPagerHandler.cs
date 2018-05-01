using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class MenuPagerHandler : ObjectBase {
        public List<MenuPager> pagers;

        public virtual void goPageTo(int i) {
            pagers.ForEach(pager => pager.goPageTo(i));
        }

        public virtual void goNextPage() {
            pagers.ForEach(pager => pager.goNextPage());
        }

        public virtual void goPrevPage() {
            pagers.ForEach(pager => pager.goPrevPage());
        }
    }
}