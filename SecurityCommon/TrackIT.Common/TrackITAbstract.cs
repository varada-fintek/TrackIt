using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TrackIT.Common
{
        [Serializable]
        public class TrackITAbstractType
        {
            public string SortOrder { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
            public string SortBy { get; set; }
            public byte[] RowVersion { get; set; }

            public Guid? CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public Guid? ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }

            public bool IsAdminRole { get; set; }
            public Guid? LoginUserID { get; set; }
            public string EmailID { get; set; }

            public DataSet dsResult { get; set; }
            public bool Active { get; set; }
            public int returnValue { get; set; }
            public string OutMessage { get; set; }
            public string ConnectionString { get; set; }

            public int is_Used { get; set; }
        }
    
}



