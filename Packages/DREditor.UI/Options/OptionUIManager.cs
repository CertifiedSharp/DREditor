//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MenuModifier) +
        "\n\n")]
    public class OptionUIManager : MenuModifier
    {
        /// <summary>
        ///
        /// </summary>
        protected List<OptionUIGroup> groups = new();


        /// <summary>
        ///
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            groups = GetComponentsInChildren<OptionUIGroup>().ToList();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            menu.BeforeStart.AddListener(BeforeSetUpGroups);

            menu.StartEvents.AddListener(SetUpGroups);

            menu.EndEvents.AddListener(UnSetUpGroups);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            menu.BeforeStart.RemoveListener(BeforeSetUpGroups);

            menu.StartEvents.RemoveListener(SetUpGroups);

            menu.EndEvents.RemoveListener(UnSetUpGroups);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void BeforeSetUpGroups() => groups.ForEach(n => n.BeforeSetupGroup());

        /// <summary>
        ///
        /// </summary>
        public virtual void SetUpGroups() => groups.ForEach(n => n.SetupGroup());

        /// <summary>
        ///
        /// </summary>
        public virtual void UnSetUpGroups() => groups.ForEach(n => n.UnSetupGroup());
    }
}