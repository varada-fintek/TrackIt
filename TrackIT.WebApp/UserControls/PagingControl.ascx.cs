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
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace ProjMngTrack.WebApp.UserControls
{
    public partial class PagingControl : BasePageUserControl
    {
        #region "Declarations"
        private int iPageIndex = -1;
        private int intPageCount;
        private string gridName;
        private string masterPageID = "";
        private int PagingPageCount;
        private int PagingPageIndex;
        private int _iPagingPageSize = 10;
        private int _iPageSize;

        public delegate void GridPagingEventHandler(object sender, GridPagingEventArgs e);
        public event GridPagingEventHandler SetPageIndex;
        public event GridPagingEventHandler GetPageIndex;
        #endregion

        #region "Properties"
        public string MasterPageID
        {
            get
            {
                return masterPageID;
            }
            set
            {
                masterPageID = value;
            }
        }
        public string GridName
        {
            get
            {
                return this.gridName;
            }
            set
            {
                this.gridName = value;
            }
        }
        public int PagingPageSize
        {
            get
            {
                return _iPagingPageSize;
            }
            set
            {
                _iPagingPageSize = value;
            }
        }
        public int PageIndex
        {
            get
            {
                return iPageIndex;
            }
            set
            {
                iPageIndex = value;
            }
        }
        public int PageSize
        {
            get
            {
                return _iPageSize;
            }
            set
            {
                _iPageSize = value;
            }
        }
        public int PageCount
        {
            get
            {
                return intPageCount;
            }
            set
            {
                intPageCount = value;
            }
        }
        #endregion

        #region "Page Load Event"
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.PageCount > 0 && this._iPagingPageSize > 0)
            {
                PagingPageCount = this.PageCount; // Convert.ToInt32(Math.Ceiling(Convert.ToDouble((Convert.ToDouble(this.PageCount) / Convert.ToDouble(this._iPagingPageSize)))));
                BtnPrev.ToolTip = "Previous " + this._iPagingPageSize + " Pages";
                BtnNext.ToolTip = "Next " + this._iPagingPageSize + " Pages";
                if (!IsPostBack)
                {
                    if (this.iPageIndex == -1)
                    {
                        this.PageIndex = 0;
                        this.hidPageIndex.Value = "0";
                    }
                    SetRepeaterControl();
                }
            }
        }
        #endregion

        #region "User Defined Methods"
        public void SetRepeaterControl()
        {
            try
            {
                DataTable DtPageIndex = new DataTable();
                DtPageIndex.Columns.Add("PageIndex", typeof(int));
                if (hidIndex.Value.Length != 0)
                {
                    if (Convert.ToInt32(hidIndex.Value) > this._iPagingPageSize)
                    {
                        this.PagingPageIndex = Convert.ToInt32(hidIndex.Value) - 1; // Convert.ToInt32(Math.Floor(Convert.ToDouble((Convert.ToDouble(Convert.ToInt32(hidIndex.Value) - 1) / Convert.ToDouble(this._iPagingPageSize)))));

                    }
                    else
                    {
                        this.PagingPageIndex = 0;
                    }
                    hidIndex.Value = string.Empty;
                }

                if (hidPageIndex.Value.Length != 0)
                {
                    this.PagingPageIndex = Convert.ToInt32(this.hidPageIndex.Value);
                    this.PageIndex = this.PagingPageIndex;
                }
                //if (this.PageCount > _iPagingPageSize)
                //{
                //    int iRemainingPages = 0;
                //    if ((this.PageCount - this.PagingPageIndex * this._iPagingPageSize) >= this._iPagingPageSize)
                //    {
                //        iRemainingPages = this._iPagingPageSize;
                //    }
                //    else
                //    {
                //        iRemainingPages = (this.PageCount - this.PagingPageIndex * this._iPagingPageSize) % this._iPagingPageSize;

                //    }

                //    for (int index = this.PagingPageIndex * this._iPagingPageSize; index < (this.PagingPageIndex * this._iPagingPageSize + iRemainingPages); index++)
                //    {
                //        DataRow dr = DtPageIndex.NewRow();
                //        dr["PageIndex"] = index + 1;
                //        DtPageIndex.Rows.Add(dr);
                //    }
                //}
                //else
                //{
                    for (int index = 0; index < this.PageCount; index++)
                    {
                        DataRow dr = DtPageIndex.NewRow();
                        dr["PageIndex"] = index + 1;
                        DtPageIndex.Rows.Add(dr);
                    }

               // }
                this.RptrPageIndex.DataSource = DtPageIndex;
                this.RptrPageIndex.DataBind();
                EnableDisableButtons();
                DtPageIndex = null;
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy)) throw;
            }
            finally
            {
            }

        }
        private void EnableDisableButtons()
        {
            if (this.PagingPageCount == 1)
            {
                this.BtnFirst.Enabled = false;
                this.BtnPrev.Enabled = false;
                this.BtnNext.Enabled = false;
                this.BtnLast.Enabled = false;
                this.BtnFirst.Style.Add(HtmlTextWriterStyle.Cursor, "default");
                this.BtnPrev.Style.Add(HtmlTextWriterStyle.Cursor, "default");
                this.BtnNext.Style.Add(HtmlTextWriterStyle.Cursor, "default");
                this.BtnLast.Style.Add(HtmlTextWriterStyle.Cursor, "default");
            }
            else
            {
                if (this.PagingPageIndex == 0 || this.PagingPageCount == 1)
                {
                    this.BtnFirst.Enabled = false;
                    this.BtnPrev.Enabled = false;
                    this.BtnFirst.Style.Add(HtmlTextWriterStyle.Cursor, "default");
                    this.BtnPrev.Style.Add(HtmlTextWriterStyle.Cursor, "default");
                }
                else
                {
                    this.BtnFirst.Enabled = true;
                    this.BtnPrev.Enabled = true;
                    this.BtnFirst.Style.Add(HtmlTextWriterStyle.Cursor, "hand");
                    this.BtnPrev.Style.Add(HtmlTextWriterStyle.Cursor, "hand");
                }
                if (this.PagingPageIndex == PagingPageCount - 1 || this.PagingPageCount == 1)
                {
                    this.BtnNext.Enabled = false;
                    this.BtnLast.Enabled = false;
                    this.BtnNext.Style.Add(HtmlTextWriterStyle.Cursor, "default");
                    this.BtnLast.Style.Add(HtmlTextWriterStyle.Cursor, "default");
                }
                else
                {
                    this.BtnNext.Enabled = true;
                    this.BtnLast.Enabled = true;
                    this.BtnNext.Style.Add(HtmlTextWriterStyle.Cursor, "hand");
                    this.BtnLast.Style.Add(HtmlTextWriterStyle.Cursor, "hand");
                }
            }

        }
        public void SetPageCount(int index, int pgCount)
        {
            this.PageIndex = index;
            this.hidPageIndex.Value = this.PageIndex.ToString();
            this.PageCount = pgCount;
            this.PagingPageIndex = index; // Convert.ToInt32(index / this._iPagingPageSize);
            //PagingPageCount = (this.PageCount - (this.PageCount % _iPagingPageSize) + _iPagingPageSize) / _iPagingPageSize;
            PagingPageCount = this.PageCount;
            //PagingPageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble((Convert.ToDouble(this.PageCount) / Convert.ToDouble(this._iPagingPageSize)))));
            //this.lblPages.Text = " (" + this.PageCount + ")";
            SetRepeaterControl();
        }
        #endregion

        #region "Repeater Item Data Bound Event"
        protected void RptrPageIndex_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (((LinkButton)(e.Item.FindControl("lnkPageIndex"))) != null)
                {
                    string lnkTxt = ((LinkButton)(e.Item.FindControl("lnkPageIndex"))).Text;

                    if (this.gridName != null)
                    {
                        string pageID = null;
                        pageID = this.masterPageID.Replace('_', '$') + "$" + this.gridName;
                        ((LinkButton)(e.Item.FindControl("lnkPageIndex"))).Attributes.Add("onclick", "return getPageIndexChange('" + lnkTxt + "','" + this.hidIndex.ClientID + "','" + this.hidPageIndex.ClientID + "','" + pageID + "');");
                    }
                    else
                    {
                        ((LinkButton)(e.Item.FindControl("lnkPageIndex"))).Attributes.Add("onclick", "document.getElementById('" + this.hidIndex.ClientID + "').value = " + lnkTxt + ";" + "document.getElementById('" + this.hidPageIndex.ClientID + "').value = " + lnkTxt + ";");
                    }

                    //((LinkButton)(e.Item.FindControl("lnkPageIndex"))).Style.Add(HtmlTextWriterStyle.TextDecoration, "none");
                    ((LinkButton)(e.Item.FindControl("lnkPageIndex"))).Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                    if (Convert.ToInt32(lnkTxt) == this.PageIndex + 1)
                        ((LinkButton)(e.Item.FindControl("lnkPageIndex"))).Style.Add(HtmlTextWriterStyle.Color, "#000000");
                    else
                    {
                        ((LinkButton)(e.Item.FindControl("lnkPageIndex"))).Style.Add(HtmlTextWriterStyle.Color, "Blue");
                    }
                }

            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy)) throw;
            }
        }
        #endregion

        #region "Link Button Event"
        protected void GetIndex(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkPageIndex = (LinkButton)sender;
                this.PageIndex = Convert.ToInt32(lnkPageIndex.Text) - 1;
                this.hidPageIndex.Value = this.PageIndex.ToString();
                lnkPageIndex.Style.Add(HtmlTextWriterStyle.Color, "#000000");
                GridPagingEventArgs GrdPaging = new GridPagingEventArgs();
                GrdPaging.PageIndex = Convert.ToInt32(lnkPageIndex.Text) - 1;
                this.PagingPageIndex = GrdPaging.PageIndex;
                this.PageIndex = (this.PagingPageIndex);
                this.SetPageIndex(this, GrdPaging);
                SetRepeaterControl();
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy)) throw;
            }
            finally
            { }

        }
        #endregion

        #region "Button Events"
        public void BtnFirst_Click(object sender, System.EventArgs e)
        {
            try
            {
                GridPagingEventArgs GrdPaging = new GridPagingEventArgs();
                this.GetPageIndex(this, GrdPaging);
                this.PagingPageIndex = 0;
                this.PageIndex = this.PagingPageIndex; // *this._iPagingPageSize;
                this.hidPageIndex.Value = this.PageIndex.ToString();
                GrdPaging.PageIndex = this.PageIndex;
                this.SetPageIndex(this, GrdPaging);
                SetRepeaterControl();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
            finally
            {
            }

        }

        public void BtnPrev_Click(object sender, System.EventArgs e)
        {
            try
            {
                GridPagingEventArgs GrdPaging = new GridPagingEventArgs();
                this.GetPageIndex(this, GrdPaging);
                this.PagingPageIndex = Convert.ToInt32(this.hidPageIndex.Value) - 1; // Convert.ToInt32(System.Math.Floor(Convert.ToDouble(Convert.ToDouble(Convert.ToInt32(this.hidPageIndex.Value) - this._iPagingPageSize) / Convert.ToDouble(this._iPagingPageSize))));
                this.PageIndex = this.PagingPageIndex;// *this._iPagingPageSize;
                this.hidPageIndex.Value = this.PageIndex.ToString();
                GrdPaging.PageIndex = this.PageIndex;
                this.SetPageIndex(this, GrdPaging);
                SetRepeaterControl();
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy)) throw;
            }
            finally
            {
            }
        }

        public void BtnNext_Click(object sender, System.EventArgs e)
        {
            try
            {
                GridPagingEventArgs GrdPaging = new GridPagingEventArgs();
                this.GetPageIndex(this, GrdPaging);
                this.PagingPageIndex = Convert.ToInt32(this.hidPageIndex.Value) + 1;// Convert.ToInt32(System.Math.Floor(Convert.ToDouble(Convert.ToDouble(Convert.ToInt32(this.hidPageIndex.Value) + this._iPagingPageSize) / Convert.ToDouble(this._iPagingPageSize))));
                this.PageIndex = (this.PagingPageIndex);// *this._iPagingPageSize;
                this.hidPageIndex.Value = this.PageIndex.ToString();
                GrdPaging.PageIndex = this.PageIndex;
                this.SetPageIndex(this, GrdPaging);
                SetRepeaterControl();
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy)) throw;
            }
            finally
            {
            }
        }

        public void BtnLast_Click(object sender, System.EventArgs e)
        {
            try
            {
                GridPagingEventArgs GrdPaging = new GridPagingEventArgs();
                this.GetPageIndex(this, GrdPaging);
                this.PagingPageIndex = PagingPageCount -1 ;
                this.PageIndex = this.PagingPageIndex; // this.PageCount - 1;
                this.hidPageIndex.Value = this.PageIndex.ToString();
                GrdPaging.PageIndex = this.PageIndex;
                this.SetPageIndex(this, GrdPaging);
                SetRepeaterControl();
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy)) throw;
            }
            finally
            {
            }
        }
        #endregion

        #region " Event Class"
        public class GridPagingEventArgs //: EventArgs
        {
            private int pageIndex;

            public int PageIndex
            {
                get { return pageIndex; }
                set { pageIndex = value; }
            }

        }
        #endregion
    }
}