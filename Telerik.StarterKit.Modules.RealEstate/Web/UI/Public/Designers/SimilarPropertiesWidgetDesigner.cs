﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Sitefinity.Web.UI.ControlDesign;
using Telerik.Sitefinity.Web.UI.Fields;
using Telerik.Sitefinity.Web.UI;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

namespace Telerik.StarterKit.Modules.RealEstate.Web.UI.Public.Designers
{
    public class SimilarPropertiesWidgetDesigner : ControlDesignerBase
    {
        internal const string layoutTemplateName = "Telerik.StarterKit.Modules.RealEstate.Web.UI.Public.Resources.SimilarPropertiesWidgetDesignerTemplate.ascx";
        internal const string designerScriptName = "Telerik.StarterKit.Modules.RealEstate.Web.UI.Controls.Scripts.SimilarPropertiesWidgetDesigner.js";

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

        #endregion

        #region Overridden methods

        /// <summary>
        /// Initializes the controls.
        /// </summary>
        /// <param name="container"></param>
        /// <remarks>
        /// Initialize your controls in this method. Do not override CreateChildControls method.
        /// </remarks>
        protected override void InitializeControls(GenericContainer container)
        {
            base.DesignerMode = ControlDesignerModes.Simple;
            base.AdvancedModeIsDefault = false;
        }

        /// <summary>
        /// Gets the script descriptors.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var scriptDescriptors = new List<ScriptDescriptor>(base.GetScriptDescriptors());
            var descriptor = (ScriptControlDescriptor)scriptDescriptors.Last();
            //descriptor.AddComponentProperty("numberOfItemsToShowEditor", this.NumberOfNewsToShowEditor.ClientID);
            scriptDescriptors.Add(descriptor);
            return scriptDescriptors.ToArray();
        }


        /// <summary>
        /// Gets the script references.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            string assembly = typeof(SimilarPropertiesWidgetDesigner).Assembly.FullName;
            var scripts = new List<ScriptReference>(base.GetScriptReferences());
            scripts.Add(new ScriptReference(SimilarPropertiesWidgetDesigner.designerScriptName, assembly));
            return scripts.ToArray();
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                //we use div wrapper tag to make easier common styling
                return HtmlTextWriterTag.Div;
            }
        }

        #endregion

    }
}
