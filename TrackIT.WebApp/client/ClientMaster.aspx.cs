﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI;
using Infragistics.Documents.Excel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using TrackIT.WebApp.Common;
using TrackIT.WebApp.TrackITEnum;
using TrackIT.Common;

namespace TrackIT.WebApp.client
{
    public partial class ClientMaster : BasePage
    {
        #region Declarations
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        WebDataGrid lwdg_clientMasterGrid;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            lblCreateClient.Text = RollupText ("Client", "lblCreateClients");
            
            lwdg_clientMasterGrid = new WebDataGrid();
            pnl_clientGrid.Controls.Add(lwdg_clientMasterGrid);
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_clientMasterGrid);

            /*

            lwdg_projectsMasterGrid.InitializeRow += Lwdg_projectsMasterGrid_InitializeRow; ;

            //Query to Get Landing Page Grid Details
            DataSet lds_Result;
            lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select project_code,project_name from prj_projects");
            if (lds_Result.Tables[0].Rows.Count > 0)
            {
                lwdg_projectsMasterGrid.DataSource = lds_Result.Tables[0];
                lwdg_projectsMasterGrid.DataBind();

            }
            */
            lwdg_clientMasterGrid.InitializeRow += Lwdg_clientMasterGrid_InitializeRow;
            DataSet lds_Result;
            lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select * from pjt_client");
            if (lds_Result.Tables[0].Rows.Count > 0)
            {
                lwdg_clientMasterGrid.DataSource = lds_Result.Tables[0];
            }

        }

        private void Lwdg_clientMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}