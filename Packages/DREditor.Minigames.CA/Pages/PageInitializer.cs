// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PageHandlerBase) + ", " + nameof(PageInitializerBase) + ", " + nameof(IPagesInitializer) +
        "\n\n")]
    public class PageInitializer : PageInitializerBase, IPagesInitializer
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float xMultiplier = 9.91f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 localScale = new(0.65f, 0.65f, 1);


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected PanelInitializerBase panelInitializer;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject pagePrefab;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject pageAddonprefab;


        /// <summary>
        ///
        /// </summary>
        protected int totalPages;

        /// <summary>
        ///
        /// </summary>
        protected GameObject page;


        /// <summary>
        ///
        /// </summary>
        /// <param name="totalPages"></param>
        protected virtual void SetTotalPages(int totalPages) => this.totalPages = totalPages;


        /// <summary>
        ///
        /// </summary>
        public virtual void InitializePages(List<ArgumentPage> pages)
        {
            SetTotalPages(0);

            foreach (ArgumentPage pageData in pages)
                InitializePage(pageData);

            SetUpPageDisplay();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pageData"></param>
        protected virtual void InitializePage(ArgumentPage pageData)
        {
            InstantiatePage(pageData);

            InvokeSpawnInitializers();

            SpawnAddOns();

            SetTotalPages(totalPages + 1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pageData"></param>
        protected virtual void InstantiatePage(ArgumentPage pageData)
        {
            Sprite pageSprite = pageData.pageSprite;

            page =
                Instantiate(
                    pagePrefab,
                    new Vector3(
                        pages.transform.position.x + totalPages * xMultiplier,
                        pages.transform.position.y,
                        pages.transform.position.z),
                    Quaternion.Euler(0, 0, 0),
                    pages.transform);


            page.GetComponent<SpriteRenderer>().sprite = pageSprite;


            //initialises each question panel on the page.
            foreach (ArgumentQuestionPanel panelData in pageData.questionPanels)
                panelInitializer.InitializePanel(panelData, page.transform);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeSpawnInitializers()
        {
            var pageInitializers = transform.parent.GetComponentsInChildren<IPageInitializer>();

            foreach (var initializer in pageInitializers)
                initializer.OnPageSpawned(page, totalPages);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SpawnAddOns()
        {
            if (pageAddonprefab == null)
                return;

            //add extras to the page that we don't want on the miniPage
            GameObject addons = Instantiate(pageAddonprefab, page.transform);

            addons.transform.SetAsFirstSibling();

            if (totalPages == 0)
            {
                Color color = addons.transform.GetChild(1).GetComponent<SpriteRenderer>().color;

                color.a = 0;

                addons.transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;
            }
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void SetUpPageDisplay()
        {
            UpdateRotation();

            InitializeFirstPage();

            InvokePageInitializersSetup();

            UpdatePageListeners(0);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeFirstPage()
        {
            Transform firstPage = pages.transform.GetChild(0);

            firstPage.position = new Vector3(firstPage.transform.position.x, firstPage.transform.position.y, -1);

            firstPage.localScale = localScale;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokePageInitializersSetup()
        {
            var pageInitializers = GetComponentsInChildren<IPageInitializer>();

            foreach (var initializer in pageInitializers)
                initializer.SetUpPageDisplay();
        }
    }
}