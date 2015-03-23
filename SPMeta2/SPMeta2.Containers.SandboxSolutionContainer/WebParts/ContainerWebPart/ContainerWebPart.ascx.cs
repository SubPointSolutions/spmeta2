using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace SPMeta2.Containers.SandboxSolutionContainer.WebParts.ContainerWebPart
{
    [ToolboxItemAttribute(false)]
    public partial class ContainerWebPart : WebPart
    {
        #region properties

        [WebBrowsable(true),
            WebDisplayName("String Property"),
            WebDescription("String Property Description"),
            Category("Test Category"),
            Personalizable(PersonalizationScope.Shared)]
        public string StringProperty { get; set; }

        [WebBrowsable(true),
          WebDisplayName("Integer Property"),
          WebDescription("Integer Property Description"),
          Category("Test Category"),
          Personalizable(PersonalizationScope.Shared)]
        public int IntegerProperty { get; set; }

        #endregion

        #region methods

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ContainerWebPart()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitProps();

        }

        private void InitProps()
        {
            lIntegerProp.Text = IntegerProperty.ToString();
            lStringProp.Text = string.IsNullOrEmpty(StringProperty) ? "NULL" : StringProperty;
        }

        #endregion

    }
}
