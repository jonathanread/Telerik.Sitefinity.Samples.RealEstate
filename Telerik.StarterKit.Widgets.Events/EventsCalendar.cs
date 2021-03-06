﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Sitefinity.Web.UI;
using Telerik.Sitefinity.Modules.Events;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Events.Model;
using System.Web;
using Telerik.Sitefinity.Web;
using Telerik.Web.UI;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.OpenAccess;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using System.Text.RegularExpressions;
using System.Threading;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Web.UI.ControlDesign;

namespace Telerik.StarterKit.Widgets.Events
{
    [ControlDesigner(typeof(EventsCalendarDesigner))]
    public class EventsCalendar : SimpleView
    {
        private EventsManager eventsManager;
        private PageManager pageManager;
        private TaxonomyManager taxonomyManager;
        private PageNode currentPage;
        private string currentPageUrl;
        private Event currentEventItem;
        private List<Event> dataSource;

        #region Constants

        private const string layoutTemplateName = "Telerik.StarterKit.Widgets.Events.Resources.Views.EventsCalendarTemplate.ascx";
        private const string widgetNameRegularExpression = @"/!(?<urlPrefix>\w+)/.*";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the embedded layout template.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// Override this property to change the embedded template to be used with the dialog
        /// </remarks>
        protected override string LayoutTemplateName
        {
            get
            {
                return layoutTemplateName;
            }
        }

        protected EventsManager EventsManager
        {
            get
            {
                if (this.eventsManager == null)
                {
                    this.eventsManager = EventsManager.GetManager();
                }
                return this.eventsManager;
            }
        }

        protected PageManager PageManager
        {
            get
            {
                if (this.pageManager == null)
                {
                    this.pageManager = PageManager.GetManager();
                }
                return this.pageManager;
            }
        }

        protected TaxonomyManager TaxonomyManager
        {
            get
            {
                if (this.taxonomyManager == null)
                {
                    this.taxonomyManager = TaxonomyManager.GetManager();
                }
                return this.taxonomyManager;
            }
        }

        protected PageNode CurrentPage
        {
            get
            {
                if (this.currentPage == null)
                {
                    SiteMapNode currentNode = SiteMapBase.GetCurrentProvider().CurrentNode;
                    Guid currentPageId = new Guid(currentNode.Key);
                    this.currentPage = this.PageManager.GetPageNode(currentPageId);
                }

                return this.currentPage;
            }
        }

        protected string CurrentPageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.currentPageUrl))
                {
                    this.currentPageUrl = this.CurrentPage.GetFullUrl(Thread.CurrentThread.CurrentCulture, false);
                }
                return this.currentPageUrl;
            }
        }

        #endregion

        #region Control references

        /// <summary>
        /// Gets the Calendar control.
        /// </summary>
        /// <value>The instance of the Calendar control.</value>
        protected virtual System.Web.UI.WebControls.Calendar Calendar
        {
            get
            {
                return this.Container.GetControl<System.Web.UI.WebControls.Calendar>("Events", true);
            }
        }

        #endregion

        #region Overridden controls

        /// <summary>
        /// Renders the HTML opening tag of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
        }

        /// <summary>
        /// Renders the HTML closing tag of the control into the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
        }

        /// <summary>
        /// Initializes the controls.
        /// </summary>
        /// <param name="container"></param>
        /// <remarks>
        /// Initialize your controls in this method. Do not override CreateChildControls method.
        /// </remarks>
        protected override void InitializeControls(GenericContainer container)
        {
            if (!Page.IsPostBack)
            {
                this.Calendar.VisibleDate = DateTime.Today;
            }

            this.Calendar.SelectionMode = CalendarSelectionMode.None;
            this.Calendar.DayRender += new DayRenderEventHandler(Calendar_DayRender);
            this.Calendar.VisibleMonthChanged += new MonthChangedEventHandler(Calendar_VisibleMonthChanged);
            this.Calendar.DataBinding += new EventHandler(Calendar_DataBinding);

            if (!Page.IsPostBack)
            {
                this.Calendar.DataBind();
            }
        }

        private void Calendar_DataBinding(object sender, EventArgs e)
        {
            dataSource = this.GetEventsListDataSource();
        }

        private void Calendar_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            this.Calendar.DataBind();
        }

        private void Calendar_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.IsOtherMonth)
            {
                return;
            }

            bool eventsExist = dataSource.Where(eI => eI.EventStart.Day == e.Day.Date.Day).Count() > 0;

            if (!eventsExist)
            {
                return;
            }

            e.Cell.Controls.Clear();
            e.Cell.Controls.Add(new HyperLink()
            {
                Text = e.Day.DayNumberText,
                NavigateUrl = string.Format("~/{0}/{1}/{2}/{3}", "events", e.Day.Date.Year, e.Day.Date.Month, e.Day.Date.Day)
            });
        }

        private List<Event> GetEventsListDataSource()
        {
            var allEvents = App.WorkWith().Events().Get().ToList();
            var items = allEvents.Where(eI =>
                                eI.EventStart.Month == this.Calendar.VisibleDate.Month
                                && eI.EventStart.Year == this.Calendar.VisibleDate.Year);

            return items
                .Distinct()
                .Where(e => e.Status == ContentLifecycleStatus.Live)
                .ToList();
        }

        #endregion
    }
}
