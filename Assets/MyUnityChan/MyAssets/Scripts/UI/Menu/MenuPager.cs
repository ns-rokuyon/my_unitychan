using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using DG.Tweening;

namespace MyUnityChan {
    public abstract class MenuPagerStyle : GUIObjectBase {
        public abstract Sequence nextPager(MenuPage new_current_page, IEnumerable<MenuPage> out_pages);
        public abstract Sequence prevPager(MenuPage new_current_page, IEnumerable<MenuPage> out_pages);
    }

    public class MenuPager : GUIObjectBase, IGUIOpenable {
        [SerializeField]
        private List<MenuPage> pages;

        [SerializeField, ReadOnly]
        private int page_index;

        public MenuPagerStyle pager_style { get; private set; }
        public Sequence pager { get; private set; }

        public MenuPage current_page {
            get { return pages[page_index]; }
        }

        public IEnumerable<MenuPage> out_pages {
            get { return pages.Where(page => page != current_page); }
        }

        public int max_page_index {
            get { return pages.Count - 1; }
        }

        void Awake() {
            pager_style = GetComponent<MenuPagerStyle>();
        }

        public bool authorized(object obj) {
            return true;
        }

        public void close() {
            pageOut(pages);
        }

        public void open() {
            pageIn(current_page);
            pageOut(pages.Where(page => page != current_page));
        }

        public void terminate() {
            close();
        }

        public GameObject getGameObject() {
            return gameObject;
        }

        public void goPageTo(int i) {
            StartCoroutine(_goPageTo(i));
        }

        public IEnumerator _goPageTo(int i) {
            if ( i == page_index )
                yield break;

            if ( i < 0 || max_page_index < i )
                yield break;

            if ( i < page_index ) {
                goPrevPage();
                yield return new WaitUntil(() => pager == null);
                StartCoroutine(_goPageTo(i));
            }
            else {
                goNextPage();
                yield return new WaitUntil(() => pager == null);
                StartCoroutine(_goPageTo(i));
            }
        }

        public void goNextPage() {
            DebugManager.log("GoNextPage!");
            if ( page_index == max_page_index )
                return;

            if ( pager != null )
                return;

            page_index++;
            pageOut(out_pages);
            pager = pager_style.nextPager(current_page, out_pages);
            pager.OnComplete(() => {
                pager = null;
                pageIn(current_page);
            });
        }

        public void goPrevPage() {
            if ( page_index == 0 )
                return;

            if ( pager != null )
                return;

            page_index--;
            pageOut(out_pages);
            pager = pager_style.prevPager(current_page, out_pages);
            pager.OnComplete(() => {
                pager = null;
                pageIn(current_page);
            });
        }

        private void pageIn(MenuPage page) {
            page.open();
            page.onPageIn();
        }

        private void pageOut(IEnumerable<MenuPage> _pages) {
            _pages.ToList().ForEach(page => {
                page.close();
                page.onPageOut();
            });
        }
    }
}