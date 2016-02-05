using System;

using ProjMngTrack.BusinessObjects;
using ProjMngTrack.WebApp.Factory;
using ProjMngTrack.WebApp.CtrlAEnum;
using ProjMngTrack.Common;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace ProjMngTrack.WebApp.Common
{
    public static class LookupCacheManager
    {
        public static void CacheLookupValues()
        {                        
            CommonBO objCommonBO = new CommonBO();            
            ICommon objBase = null;
            string sKey = null;

            try
            {
                ICacheManager objCacheMgr = CacheFactory.GetCacheManager();
                BasePage objPage = new BasePage();
                objBase = CommonFactory.CreateCommonObject(objPage.ObjectCreatorOption);

                foreach (string sValue in Enum.GetNames(typeof(LookupCacher)))
                {
                    sKey = StringEnum.GetStringValue((Enum)Enum.Parse(typeof(LookupCacher), sValue));

                    if (!objCacheMgr.Contains(sKey))
                    {
                        objCommonBO.FilterType = sKey;
                        objCacheMgr.Add(sKey, objBase.GetDropdownValuesToCache(objCommonBO));
                    }
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
    }
}
