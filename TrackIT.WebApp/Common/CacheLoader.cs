using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using AjaxControlToolkit;

using ProjMngTrack.BusinessLogic;
using ProjMngTrack.BusinessObjects;
using ProjMngTrack.WebApp.Factory;
using ProjMngTrack.WebApp.CtrlAEnum;

using ProjMngTrack.Common;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;



namespace ProjMngTrack.WebApp.Common
{
    public sealed class CacheLoader
    {
        private CacheLoader()
        {
        }

        #region "UDF"

        /// <summary>
        /// Enum Value to Reload the dropdown
        /// </summary>
        /// <param name="eValue"></param>
        public static void RefreshCacheDropdownValues(Enum eValue)
        {
            CommonBO objCommonBO = new CommonBO();
            ICommon objBase = null;
            BasePage objPage = new BasePage();
            string sKey = string.Empty;

            try
            {
                objBase = CommonFactory.CreateCommonObject(objPage.ObjectCreatorOption);
                ICacheManager objCacheMgr = CacheFactory.GetCacheManager();
                sKey = StringEnum.GetStringValue(eValue);
                if (objCacheMgr.Contains(sKey))
                {
                    objCacheMgr.Remove(sKey);
                    objCommonBO.FilterType = sKey;
                    objCacheMgr.Add(sKey, objBase.GetDropdownValuesToCache(objCommonBO));
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, BasePage.Rethrow_Policy))
                    throw;
            }
            finally
            {                
                if (objCommonBO != null) objCommonBO = null;
                if (objBase != null) objBase = null;
            }
        }

        public static void GetDropdownValuesToCache(DropDownList ddlDropdown, Enum eValue)
        {
            try
            {
                ICacheManager objCacheMgr = CacheFactory.GetCacheManager();
                string sKey = StringEnum.GetStringValue(eValue);
                if (objCacheMgr.Contains(sKey))
                {
                    ddlDropdown.DataSource = (DataTable)objCacheMgr.GetData(sKey);
                    ddlDropdown.DataValueField = "Value";
                    ddlDropdown.DataTextField = "TextValue";
                    ddlDropdown.DataBind();
                }
                ListItem li = new ListItem("Select", "");
                ddlDropdown.Items.Insert(0, li);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, BasePage.Rethrow_Policy))
                    throw;
            }
        }

        public static void GetDropdownValuesToCache(ComboBox ddlDropdown, Enum eValue)
        {
            try
            {
                ICacheManager objCacheMgr = CacheFactory.GetCacheManager();
                string sKey = StringEnum.GetStringValue(eValue);
                if (objCacheMgr.Contains(sKey))
                {
                    ddlDropdown.DataSource = (DataTable)objCacheMgr.GetData(sKey);
                    ddlDropdown.DataValueField = "Value";
                    ddlDropdown.DataTextField = "TextValue";
                    ddlDropdown.DataBind();
                }
                ListItem li = new ListItem("Select", "");
                ddlDropdown.Items.Insert(0, li);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, BasePage.Rethrow_Policy))
                    throw;
            }
        }
        #endregion
    }
}
