using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace HP.TS.Devops.Dictionary
{
    public class DictionaryAction : HP.TS.Devops.Core.Action
    {
        public DictionaryAction(string connectString)
            : base(connectString)
        { }
        public DataSet RetrieveKeysByType(string type)
        {
            throw new NotImplementedException();
        }
        public bool CheckKeyExistByType(string type, string key)
        {
            SqlParameter[] ParamList = new SqlParameter[] { 
                    new SqlParameter("@Key", key), 
                    new SqlParameter("@Type", type), 
                    new SqlParameter("@Return",System.Data.SqlDbType.Int){ Direction = System.Data.ParameterDirection.ReturnValue }
                };
            Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, System.Data.CommandType.StoredProcedure, "SP_CheckKeyExistByType", ParamList);
            if (Int32.Parse(ParamList[2].Value.ToString()) > 0)
            {
                return true;
            }
            return false;
        }
        public string RetrieveValueByTypeAndKey(string type, string key)
        {
            SqlParameter[] ParamList = new SqlParameter[] { 
                    new SqlParameter("@Type", type), 
                    new SqlParameter("@Key", key), 
                    new SqlParameter("@Value",System.Data.SqlDbType.NVarChar,500){ Direction = System.Data.ParameterDirection.Output }
                };
            Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(this.ConnectString, System.Data.CommandType.StoredProcedure, "SP_RetrieveValueByTypeAndKey", ParamList);
            return ParamList[2].Value.ToString();
        }
        public string RetrieveServiceUrl(string environment)
        {
            return this.RetrieveValueByTypeAndKey("DataCenterUrl", environment);
        }
    }
}
