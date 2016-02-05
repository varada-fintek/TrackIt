using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


//using TrackIT.BusinessObjects;
using TrackIT.WebApp.Common;

using TrackIT.WebApp.TrackITEnum;
using TrackIT.Common;
using TrackIT.Security;

using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI;
using Infragistics.Documents.Excel;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;


namespace TrackIT.WebApp.Setup
{
    public partial class Projects : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}