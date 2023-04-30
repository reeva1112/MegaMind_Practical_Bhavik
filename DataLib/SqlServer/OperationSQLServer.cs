using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace DataLib.SqlServer
{
    public class OperationSQLServer
    {
        public SqlConnection GConn;
        /// <summary>
        /// Set Global Data Adapter For SQl Server
        /// </summary>
        public SqlDataAdapter GDataAdapter = new SqlDataAdapter();
        /// <summary>
        /// Set Global Data Reader For Sql Server
        /// </summary>
        public SqlDataReader GDataReader;
        /// <summary>
        /// Set Global Data Command For Sql Server
        /// </summary>
        public SqlCommand GComm = new SqlCommand();

        /// <summary>
        /// Set Current Culture Info
        /// </summary>
        public System.Globalization.CultureInfo CultureInfoUS = new System.Globalization.CultureInfo("en-US", false);

        #region Utility Like PassWord Encription ....

        /// <summary>
        /// Method for Encoding Or Decoding Given Password
        /// </summary>
        /// <param name="pStr">PassWord String </param>
        /// <param name="pStrToEncodeOrDecode"> String For Encode Or Decode [E-Encode,D-Decode]</param>
        /// <returns>String</returns>
        public string ENCODE_DECODE(string pStr, string pStrToEncodeOrDecode)
        {
            string StrPass;
            string StrECode;
            string StrDCode;

            StrECode = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StrDCode = ")(*&^%$#@!";

            for (int IntLen = 1; IntLen <= 52; IntLen++)
            {
                StrDCode = StrDCode + (Char)(IntLen + 160);
            }

            StrPass = "";
            for (int IntCnt = 0; IntCnt <= pStr.Trim().Length - 1; IntCnt++)
            {
                var ChrSingle = char.Parse(pStr.Substring(IntCnt, 1));
                var IntPos = pStrToEncodeOrDecode == "E" ? StrECode.IndexOf(ChrSingle, 1) : StrDCode.IndexOf(ChrSingle, 1);
                if (pStrToEncodeOrDecode == "E")
                {
                    StrPass = StrPass + StrDCode.Substring(IntPos, 1);
                }
                else
                {
                    StrPass = StrPass + StrECode.Substring(IntPos, 1);
                }
            }
            return StrPass;
        }


        #endregion

        #region ConnetionManupulation


        public bool IsConnectionCheck(string ServerConnection)
        {
            SqlConnection NewCon;
            bool Result;
            NewCon = new SqlConnection(ServerConnection);
            try
            {
                OCon(NewCon);
                Result = true;
                CCon(NewCon);
            }
            catch (Exception)
            {
                CCon(NewCon);
                Result = false;
            }
            return Result;
        }

        /// <summary>
        /// Hash Table For Assigning Parameter Names And Values
        /// </summary>
        public Hashtable HTParam = new Hashtable();
        /// <summary>
        ///Variable Use For Hash Table Parameter Count
        /// </summary>
        public Int32 IntParamCount;
        /// <summary>
        /// Method For Checking Connection State And Return True Or False
        /// </summary>
        /// <param name="pConn">SqlConnection</param>
        /// <returns>True Or False</returns>
        public bool IsCon(SqlConnection pConn)
        {
            if (pConn.State == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region "Open Connection"
        /// <summary>
        /// Open A Connection Of Sql Server With given Connection
        /// </summary>
        /// <param name="pConn">SqlConnection</param>
        public void OCon(SqlConnection pConn)
        {
            if (pConn.State == System.Data.ConnectionState.Closed)
            {
                pConn.Open();

            }
        }
        /// <summary>
        /// Open A Connection Of Sql Server With given ConnectionString
        /// </summary>
        /// <param name="pConn">SqlConnection</param>
        /// <param name="pStrConn">ConenctionString</param>
        /// <param name="BlnOpenCon">Opening Conenction Flag</param>
        public void OCon(SqlConnection pConn, string pStrConn, bool BlnOpenCon)
        {
            CCon(pConn);
            if (BlnOpenCon)
            {
                pConn.Open();
            }
            if (pConn.State == System.Data.ConnectionState.Closed)
            {
                pConn.Open();
            }
        }
        /// <summary>
        /// Open A Connection Of Sql Server With Given Criteria
        /// </summary>
        /// <param name="pConn">SqlConnection</param>
        /// <param name="pStrServer">Server Name</param>
        /// <param name="pStrDBName">DataBase Name</param>
        /// <param name="pStrDBUser">DataBase User Name</param>
        /// <param name="pStrDBPass">DataBase Password</param>
        public void OCon(SqlConnection pConn, string pStrServer, string pStrDBName, string pStrDBUser, string pStrDBPass)
        {
            CCon(pConn);
            pConn.ConnectionString = "Data Source = " + pStrServer + "; Initial Catalog = " + pStrDBName + "; User Id = " + pStrDBUser + "; Password = " + pStrDBPass + "; Persist Security Info = True;";
            pConn.Open();
        }

        public SqlConnection OpenConnection(String ServerConnection)
        {

            SqlConnection NewCon;
            NewCon = new SqlConnection(ServerConnection);
            try
            {
                OCon(NewCon);
                GComm.Connection = NewCon;
                return NewCon;
            }
            catch (Exception)
            {
                CCon(NewCon);
                throw new Exception("Cant Connect to Server");
            }
        }

        public void OpenConnRed(String ServerConnection)
        {
            GConn = new SqlConnection(ServerConnection);
            try
            {
                OCon(GConn);
                GComm.Connection = GConn;

            }
            catch (Exception)
            {
                CCon(GConn);
                throw new Exception("Cant Connect to Server");
            }
        }

        #endregion

        #region Close Connection

        /// <summary>
        /// Close Connetion Of SqlServer
        /// </summary>
        /// <param name="pConn">Name of Connection</param>
        public void CCon(SqlConnection pConn)
        {
            if (pConn != null)
            {
                if (pConn.State == System.Data.ConnectionState.Open)
                {
                    pConn.Close();
                    pConn.Dispose();
                }
            }
        }
        /// <summary>
        /// Close Connetion Of SqlServer
        /// </summary>
        /// <param name="pConn">Name of Connection</param>

        public void CloseConnection(SqlConnection pConn)
        {
            if (pConn != null)
            {
                if (pConn.State == System.Data.ConnectionState.Open)
                {
                    pConn.Close();
                    pConn.Dispose();
                }
            }
            else
            {
            }
        }
        public void CloseConnRed()
        {
            if (GConn != null)
            {
                if (GConn.State == System.Data.ConnectionState.Open)
                {
                    GConn.Close();
                    GConn.Dispose();
                    GConn = null;
                }
            }
            else
            {
                GConn = null;
            }
        }

        #endregion Close Connection
        #endregion

        #region Record Manupulation

        /// <summary> Generate Primary Keys(DataTable)
        /// Business DataAdapter for Dataset Without Parameter Connetion With Primary Key
        /// </summary>
        /// <param name="pTab">Data Table </param>
        private string PKGen(DataTable pTab)
        {
            string Str = "";
            foreach (DataColumn DataColumnPrimaryKey in pTab.PrimaryKey)
            {
                switch (DataColumnPrimaryKey.DataType.Name.ToLower())
                {
                    case "string":
                        Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = '" + pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original] + "' ";
                        break;
                    case "double":
                    case "decimal":
                    case "integer":
                    case "int32":
                    case "int64":
                    case "int16":
                        Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = " + pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original];
                        break;
                    case "datetime":
                        if (pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original].GetType().ToString() == "System.DBNull")
                        {
                            Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = Null ";
                        }
                        else
                        {
                            Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = '" + SqlDate(pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original].ToString()) + "'";
                        }
                        break;
                    case "boolean":

                        if (pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original].GetType().ToString() == "System.DBNull")
                        {
                            Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = Null ";
                        }
                        else
                        {
                            Str = Str + " And [" + DataColumnPrimaryKey.ColumnName + "] = " + System.Convert.ToInt32(pTab.Rows[0][DataColumnPrimaryKey.ColumnName, DataRowVersion.Original]);
                        }
                        break;
                }
            }
            return Str;
        }


        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey">Parameter Name</param>
        /// <param name="pStrVal">Parameter Value</param>
        public void AddParams(String pStrKey, String pStrVal)
        {


            HTParam.Add(pStrKey, pStrVal);
            IntParamCount++;
        }
        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey">Parameter Name</param>
        /// <param name="pImage">Parameter Image</param>
        public void AddParams(String pStrKey, byte[] pImage)
        {
            //  clsTHE clsTHE = new OperationSQLServer.clsTHE();
            //   clsTHE.Obj1 = pImage;
            //   clsTHE.Session1 = HttpContext.Current.Session.SessionID;
            HTParam.Add(pStrKey, pImage);
            IntParamCount++;
        }


        public void AddParams(String pStrKey, DataTable _dt)
        {
            HTParam.Add(pStrKey, _dt);
            IntParamCount++;
        }




        //public  void AddParams(String pStrKey, Int16 pInt16Val)
        //{
        //    HTParam.Add(pStrKey, pInt16Val);
        //    IntParamCount++;
        //}
        //public  void AddParams(String pStrKey, Int32 pIntVal)
        //{
        //    HTParam.Add(pStrKey, pIntVal);
        //    IntParamCount++;
        //}
        //public  void AddParams(String pStrKey, Int64 pInt64Val)
        //{
        //    HTParam.Add(pStrKey, pInt64Val);
        //    IntParamCount++;
        //}
        //public  void AddParams(String pStrKey, Double pDoubleVal)
        //{
        //    HTParam.Add(pStrKey, pDoubleVal);
        //    IntParamCount++;
        //}
        //public  void AddParams(String pStrKey, Boolean pBoolVal)
        //{
        //    HTParam.Add(pStrKey, Microsoft.VisualBasic.Interaction.IIf(pBoolVal == true, 1, 0));
        //    IntParamCount++;
        //}
        /// <summary> Add ParaMeters To HashTable (Key,Value)
        /// Method For Adding Parameters To HashTable With Key And Vaue
        /// </summary>
        /// <param name="pStrKey"></param>
        /// <param name="pDatTime"></param>

        public void AddParams(String pStrKey, DateTime? pDatTime)
        {
            //   clsTHE clsTHE = new OperationSQLServer.clsTHE();            

            if (pDatTime == null || pDatTime.ToString() == "")
            {
                HTParam.Add(pStrKey, System.DBNull.Value);
            }
            else
            {
                HTParam.Add(pStrKey,
                    pDatTime.Value.ToShortDateString() == "01/01/0001"
                        ? DTDBTime(pDatTime).Value.ToString("hh:mm tt")
                        : DTDBDate(pDatTime).Value.ToString("MM/dd/yyyy"));
            }

            //clsTHE.Session1 = HttpContext.Current.Session.SessionID;
            HTParam.Add(pStrKey, pDatTime);
            IntParamCount++;
        }

        /// <summary> Get ParaMeters From HashTable
        /// Method For Get Parameters From HashTable With Key And Vaue
        /// </summary>
        /// <returns>SqlParameters Colection</returns>

        public SqlParameter[] GetParams()
        {
            Int16 IntI = 0;

            SqlParameter[] GetPara = new SqlParameter[HTParam.Count];
            foreach (DictionaryEntry DE in HTParam)
            {
                if (DE.Value.GetType().Name.ToUpper() == "BYTE[]")
                {
                    GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), SqlDbType.Image);
                    GetPara[IntI].Value = DE.Value;
                }
                else if (DE.Value.GetType().Name == "DBNull")
                {
                    GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), SqlDbType.DateTime);
                    GetPara[IntI].Value = System.DBNull.Value;
                }
                else
                {
                    GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), DE.Value.ToString());
                }

                IntI++;
            }
            Clear();
            return GetPara;
        }

        //public  SqlParameter[] GetParams()
        //{
        //    Int16 IntI = 0;
        //    SqlParameter[] GetPara = new SqlParameter[HTParam.Count];
        //    foreach (DictionaryEntry DE in HTParam)
        //    {
        //        switch (DE.Value.GetType().Name.ToUpper())
        //        {
        //            case "BYTE[]":
        //                GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), SqlDbType.Image);
        //                GetPara[IntI].Value = DE.Value;
        //                break;

        //            case "DBNULL":
        //                GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), SqlDbType.DateTime);
        //                GetPara[IntI].Value = System.DBNull.Value;
        //                break;

        //            case "INT16":
        //                GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), SqlDbType.SmallInt);
        //                GetPara[IntI].Value = DE.Value;
        //                break;

        //            case "INT32":
        //            case "INT":
        //                GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), SqlDbType.Int);
        //                GetPara[IntI].Value = DE.Value;
        //                break;

        //            case "INT64":
        //                GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), SqlDbType.BigInt);
        //                GetPara[IntI].Value = DE.Value;
        //                break;

        //            case "STRING":
        //                GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), DE.Value.ToString());
        //                break;

        //            case "DOUBLE":
        //                GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), SqlDbType.Decimal);
        //                GetPara[IntI].Value = DE.Value;
        //                break;
        //            case "Boolean":
        //                GetPara[IntI] = new SqlParameter("@" + DE.Key.ToString(), SqlDbType.Bit);
        //                GetPara[IntI].Value = DE.Value;
        //                break;
        //        }
        //        IntI++;
        //    }
        //    Clear();
        //    return GetPara;
        //}

        /// <summary> Clear All The Parameters From HashTable 
        /// 
        /// </summary>
        public void Clear()
        {
            HTParam.Clear();
        }

        #endregion

        #region Filling Of Data

        #region With Server Connection

        /// <summary> Fill Of DataSet with Stored Procedure(Dataset ,TableName,Procedure Name,PrimaryKey)
        /// Business DataAdapter for Dataset With Primary Key using Procedure Name
        /// </summary>
        /// <param name="ServerConnStr"></param>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Name of StoreProcedure</param>
        /// <param name="pParamList">SqlParameter Collection</param>
        /// <param name="pStrPrimaryKey">Primary Key</param>
        public void FillDataSet(String ServerConnStr, DataSet pDataSet, string pTableName, string pProcedureName, SqlParameter[] pParamList, string pStrPrimaryKey)
        {
            int CountParam = pParamList.Length;
            var pCon = OpenConnection(ServerConnStr);
            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GComm.Parameters.Add(pParamList[i]);
            }
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = pCon;
            GDataAdapter.SelectCommand = GComm;
            try
            {
                if (pDataSet.Tables[pTableName] != null)
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GDataAdapter.Fill(pDataSet, pTableName);
            }
            finally
            {
                GComm.Parameters.Clear();
                CCon(pCon);
            }
            if (pStrPrimaryKey != "")
            {
                var StrArray = pStrPrimaryKey.Split(',');
                var DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                {
                    DataColumnPrimaryKey[IntCount] = pDataSet.Tables[pTableName].Columns[IntCount];
                }
                pDataSet.Tables[pTableName].PrimaryKey = DataColumnPrimaryKey;
            }
        }

        /// <summary> Fill Of DataSet with Stored Procedure(Dataset ,TableName,Procedure Name)
        /// Business DataAdapter for Dataset Without Primary Key using Procedure Name
        /// </summary>
        /// <param name="ServerConnStr"></param>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Name of StoreProcedure</param>
        /// <param name="pParamList">SqlParameter Collection</param>
        public void FillDataSet(String ServerConnStr, DataSet pDataSet, string pTableName, string pProcedureName, SqlParameter[] pParamList)
        {
            SqlConnection pCon;
            int CountParam = pParamList.Length;
            pCon = OpenConnection(ServerConnStr);

            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GComm.Parameters.Add(pParamList[i]);
            }
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = pCon;
            GDataAdapter.SelectCommand = GComm;
            try
            {
                if (pDataSet.Tables[pTableName] != null)
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GDataAdapter.Fill(pDataSet, pTableName);
            }
            finally
            {
                GComm.Parameters.Clear();
                CCon(pCon);
            }
        }

        /// <summary> Fill Of DataSet with Stored Procedure(Dataset ,Procedure Name)
        /// Business DataAdapter for Dataset Without Primary Key using Procedure Name
        /// </summary>
        /// <param name="ServerConnStr">Server Connection string </param>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pProcedureName">Name of StoreProcedure</param>
        /// <param name="pParamList">SqlParameter Collection</param>
        public void FillDataSet(String ServerConnStr, DataSet pDataSet, string pProcedureName, SqlParameter[] pParamList)
        {

            SqlConnection pCon;
            int CountParam = pParamList.Length;
            pCon = OpenConnection(ServerConnStr);

            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GComm.Parameters.Add(pParamList[i]);
            }
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = pCon;
            GDataAdapter.SelectCommand = GComm;
            try
            {
                if (pDataSet != null)
                {
                    pDataSet.Clear();
                }
                GDataAdapter.Fill(pDataSet);
            }
            finally
            {
                GComm.Parameters.Clear();
                CCon(pCon);
            }
        }


        /// <summary> Fill Of DataSet with Stored Procesure(PrimaryKey)
        /// Business DataAdapter for Dataset With General Connetion
        /// </summary>
        /// <param name="ServerConnStr"></param>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Sql Store Procedure Name</param>
        /// <param name="pStrPrimaryKey">Name of Primary Key</param>
        public void FillDataSet(String ServerConnStr, DataSet pDataSet, string pTableName, string pProcedureName, string pStrPrimaryKey)
        {
            var pCon = OpenConnection(ServerConnStr);

            GComm.CommandText = pProcedureName;
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.Connection = pCon;
            GDataAdapter.SelectCommand = GComm;
            try
            {
                if (pDataSet.Tables[pTableName] != null)
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GDataAdapter.Fill(pDataSet, pTableName);
            }
            finally
            {
                CCon(pCon);
            }
            if (pStrPrimaryKey != "")
            {
                string[] StrArray;
                StrArray = pStrPrimaryKey.Split(',');
                var DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                {
                    DataColumnPrimaryKey[IntCount] = pDataSet.Tables[pTableName].Columns[IntCount];
                }
                pDataSet.Tables[pTableName].PrimaryKey = DataColumnPrimaryKey;
            }
        }

        /// <summary> Fill Of DataSet with Stored Procesure(Without Primary Key )
        /// Business DataAdapter for Dataset With General Connetion
        /// </summary>
        /// <param name="ServerConnStr"></param>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of DataTable</param>
        /// <param name="pProcedureName">Sql Store Procedure Name</param>
        public void FillDataSet(String ServerConnStr, DataSet pDataSet, string pTableName, string pProcedureName)
        {
            var pCon = OpenConnection(ServerConnStr);

            GComm.CommandText = pProcedureName;
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.Connection = pCon;
            GDataAdapter.SelectCommand = GComm;
            try
            {
                if (pDataSet.Tables[pTableName] != null)
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GDataAdapter.Fill(pDataSet, pTableName);
            }
            finally
            {
                CCon(pCon);
            }
        }

        #endregion


        #region Without Server Connection
        /// <summary> Fill Of DataSet with Stored Procedure(Dataset ,TableName,Procedure Name,PrimaryKey)
        /// Business DataAdapter for Dataset With Primary Key using Procedure Name
        /// </summary>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Name of StoreProcedure</param>
        /// <param name="pParamList">SqlParameter Collection</param>
        /// <param name="pStrPrimaryKey">Primary Key</param>
        public void FillDataSet(DataSet pDataSet, string pTableName, string pProcedureName, SqlParameter[] pParamList, string pStrPrimaryKey)
        {
            int CountParam = pParamList.Length;
            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GComm.Parameters.Add(pParamList[i]);
            }
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = GConn;
            GDataAdapter.SelectCommand = GComm;
            try
            {
                if (pDataSet.Tables[pTableName] != null)
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GDataAdapter.Fill(pDataSet, pTableName);
            }
            finally
            {
                GComm.Parameters.Clear();
            }
            if (pStrPrimaryKey != "")
            {
                var StrArray = pStrPrimaryKey.Split(',');
                var DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                {
                    DataColumnPrimaryKey[IntCount] = pDataSet.Tables[pTableName].Columns[IntCount];
                }
                pDataSet.Tables[pTableName].PrimaryKey = DataColumnPrimaryKey;
            }
        }
        /// <summary> Fill Of DataSet with Stored Procedure(Dataset ,TableName,Procedure Name)
        /// Business DataAdapter for Dataset Without Primary Key using Procedure Name
        /// </summary>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Name of StoreProcedure</param>
        /// <param name="pParamList">SqlParameter Collection</param>
        public void FillDataSet(DataSet pDataSet, string pTableName, string pProcedureName, SqlParameter[] pParamList)
        {
            int CountParam = pParamList.Length;


            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GComm.Parameters.Add(pParamList[i]);
            }
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = GConn;
            GDataAdapter.SelectCommand = GComm;
            try
            {
                if (pDataSet.Tables[pTableName] != null)
                {
                    pDataSet.Tables[pTableName].Rows.Clear();
                }
                GDataAdapter.Fill(pDataSet, pTableName);
            }
            finally
            {
                GComm.Parameters.Clear();
            }

        }

        /// <summary> Fill Of DataSet with Stored Procesure(PrimaryKey)
        /// Business DataAdapter for Dataset With General Connetion
        /// </summary>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of Table Name</param>
        /// <param name="pProcedureName">Sql Store Procedure Name</param>
        /// <param name="pStrPrimaryKey">Name of Primary Key</param>
        public void FillDataSet(DataSet pDataSet, string pTableName, string pProcedureName, string pStrPrimaryKey)
        {
            GComm.CommandText = pProcedureName;
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.Connection = GConn;
            GDataAdapter.SelectCommand = GComm;
            if (pDataSet.Tables[pTableName] != null)
            {
                pDataSet.Tables[pTableName].Rows.Clear();
            }
            GDataAdapter.Fill(pDataSet, pTableName);
            if (pStrPrimaryKey != "")
            {
                string[] StrArray;
                StrArray = pStrPrimaryKey.Split(',');
                var DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                {
                    DataColumnPrimaryKey[IntCount] = pDataSet.Tables[pTableName].Columns[IntCount];
                }
                pDataSet.Tables[pTableName].PrimaryKey = DataColumnPrimaryKey;
            }
        }
        /// <summary> Fill Of DataSet with Stored Procesure(Without Primary Key )
        /// Business DataAdapter for Dataset With General Connetion
        /// </summary>
        /// <param name="pDataSet">DataSet</param>
        /// <param name="pTableName">Name of DataTable</param>
        /// <param name="pProcedureName">Sql Store Procedure Name</param>
        public void FillDataSet(DataSet pDataSet, string pTableName, string pProcedureName)
        {
            GComm.CommandText = pProcedureName;
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.Connection = GConn;
            GDataAdapter.SelectCommand = GComm;
            if (pDataSet.Tables[pTableName] != null)
            {
                pDataSet.Tables[pTableName].Rows.Clear();
            }
            GDataAdapter.Fill(pDataSet, pTableName);
        }

        #endregion


        /// <summary> Fill Of DataTable With(StoreProcedure)
        /// Business DataAdapter for DataTable With Parameter Connetion
        /// </summary>
        /// <param name="ServerConnStr"></param>
        /// <param name="pDataTable">DataTable</param>
        /// <param name="pProcedureName">Store Procedure Name </param>
        /// <param name="pParamList">Parameter List Array</param>
        public void FillDataTable(String ServerConnStr, DataTable pDataTable, String pProcedureName, SqlParameter[] pParamList)
        {
            int CountParam = pParamList.Length;
            SqlConnection pCon;
            pCon = OpenConnection(ServerConnStr);

            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GComm.Parameters.Add(pParamList[i]);
            }
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = pCon;
            GDataAdapter.SelectCommand = GComm;
            try
            {
                if (pDataTable != null)
                {
                    pDataTable.Rows.Clear();
                }
                GDataAdapter.Fill(pDataTable);
            }
            finally
            {
                GComm.Parameters.Clear();
                CCon(pCon);
            }
        }

        /// <summary> Fill Of DataTable With Stored Procedure With Primary Key(DataTable,ProcedureName,ParaList,PrimaryKey)
        /// Business DataAdapter for DataTable With Primary Keys
        /// </summary>
        /// <param name="ServerConnStr"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pProcedureName"></param>
        /// <param name="pParamList"></param>
        /// <param name="pStrPrimaryKeys"></param>
        public void FillDataTable(String ServerConnStr, DataTable pDataTable, String pProcedureName, SqlParameter[] pParamList, string pStrPrimaryKeys)
        {
            int CountParam = pParamList.Length;
            var pCon = OpenConnection(ServerConnStr);

            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GComm.Parameters.Add(pParamList[i]);
            }
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = pCon;
            GDataAdapter.SelectCommand = GComm;
            try
            {
                if (pDataTable != null)
                {
                    pDataTable.Rows.Clear();
                }
                GDataAdapter.Fill(pDataTable);
            }
            finally
            {
                GComm.Parameters.Clear();
                CCon(pCon);
            }

            if (pStrPrimaryKeys != "")
            {
                var StrArray = pStrPrimaryKeys.Split(',');
                var DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                {
                    if (pDataTable != null) DataColumnPrimaryKey[IntCount] = pDataTable.Columns[IntCount];
                }

                if (pDataTable != null) pDataTable.PrimaryKey = DataColumnPrimaryKey;
            }
        }

        /// <summary> Fill Of DataTable With General Query(DataTable,Query)
        /// Business DataAdapter for DataTable With Parameter Connetion
        /// </summary>
        /// <param name="ServerConnStr"></param>
        /// <param name="pDataTable">Name of Table Name</param>
        /// <param name="pProcedureName">Stored Procedure Name</param>
        public void FillDataTable(String ServerConnStr, DataTable pDataTable, string pProcedureName)
        {
            if (ServerConnStr == null) throw new ArgumentNullException("ServerConnStr");
            SqlConnection pCon;
            pCon = OpenConnection(ServerConnStr);

            GComm.CommandText = pProcedureName;
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.Connection = pCon;
            GDataAdapter.SelectCommand = GComm;
            try
            {
                if (pDataTable != null)
                {
                    pDataTable.Rows.Clear();
                }
                GDataAdapter.Fill(pDataTable);
            }
            finally
            {
                CCon(pCon);
            }

        }

        /// <summary> Fill Of DataTable With General Query(DataTable,Query)
        /// Business DataAdapter for DataTable With Parameter Connetion
        /// </summary>
        /// <param name="ServerConnStr"></param>
        /// <param name="pDataTable">Name of Table Name</param>
        /// <param name="pProcedureName">Stored Procedure Name</param>
        /// <param name="pStrPrimaryKeys"></param>
        public void FillDataTable(String ServerConnStr, DataTable pDataTable, string pProcedureName, string pStrPrimaryKeys)
        {
            SqlConnection pCon;
            pCon = OpenConnection(ServerConnStr);

            GComm.CommandText = pProcedureName;
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.Connection = pCon;
            GDataAdapter.SelectCommand = GComm;
            try
            {
                if (pDataTable != null)
                {
                    pDataTable.Rows.Clear();
                }
                GDataAdapter.Fill(pDataTable);

                if (pStrPrimaryKeys != "")
                {
                    string[] StrArray;
                    StrArray = pStrPrimaryKeys.Split(',');
                    var DataColumnPrimaryKey = new DataColumn[StrArray.GetUpperBound(0) + 1];
                    for (int IntCount = 0; IntCount <= StrArray.GetUpperBound(0); IntCount++)
                    {
                        if (pDataTable != null) DataColumnPrimaryKey[IntCount] = pDataTable.Columns[IntCount];
                    }

                    if (pDataTable != null) pDataTable.PrimaryKey = DataColumnPrimaryKey;
                }
            }
            finally
            {
                CCon(pCon);
            }
        }


        /// <summary> Give DataReader With(Store Procedure)
        /// Use To Executer Store Procedure With Parameter List With General Connetion
        /// <param name="pProcedureName">Name of Store Procedure</param>
        /// <param>Parameter List Arraty
        ///     <name>pParamList</name>
        /// </param>
        /// <returns>SqlDataReader With Record</returns></summary>
        public SqlDataReader ExeRed(String ServerConnStr, String pProcedureName)
        {
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = GConn;
            try
            {
                return GComm.ExecuteReader();
            }
            catch (SqlException)
            { return null; }
            finally
            {
                GComm.Parameters.Clear();
            }
        }
        /// <summary> Give DataReader With(,ServerConn,Store Procedure)
        /// Use To Executer Store Procedure With Parameter List With General Connetion
        /// <param name="pProcedureName">Name of Store Procedure</param>
        /// <param name="pParamList">Parameter List Arraty</param>
        /// <returns>SqlDataReader With Record</returns></summary>
        public SqlDataReader ExeRed(String ServerConnStr, String pProcedureName, SqlParameter[] pParamList)
        {
            GConn = OpenConnection(ServerConnStr);

            int CountParam = pParamList.Length;

            for (int i = 0; i < CountParam; i++)
            {
                GComm.Parameters.Add(pParamList[i]);
            }
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = GConn;
            try
            {
                return GComm.ExecuteReader();
            }
            catch (SqlException)
            {
                return null;
            }
            finally
            {
                GComm.Parameters.Clear();
            }
        }
        /// <summary> Give DataReader With(,ServerConn,Store Procedure)
        /// Use To Executer Store Procedure With Parameter List With General Connetion
        /// <param name="pProcedureName">Name of Store Procedure</param>
        /// <param name="pParamList">Parameter List Arraty</param>
        /// <returns>SqlDataReader With Record</returns></summary>
        public SqlDataReader ExeRed(String pProcedureName, SqlParameter[] pParamList)
        {
            int CountParam = pParamList.Length;

            for (int i = 0; i < CountParam; i++)
            {
                GComm.Parameters.Add(pParamList[i]);
            }
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = GConn;
            try
            {
                return GComm.ExecuteReader();
            }
            catch (SqlException)
            {
                return null;
            }
            finally
            {
                GComm.Parameters.Clear();
            }
        }


        /// <summary> Give String With (Store Procedure)
        /// Use To Execute Store Procedure With Parameter List and With Connection As Perameter
        /// </summary>
        /// <param name="ServerConnStr"></param>
        /// <param name="pProcedureName">Name of Store Procedure</param>
        /// <param name="pParamList">Parameter List Arraty</param>
        /// <returns>Number of Affected Record Or If error raise then return -1</returns>
        public string ExeScal(String ServerConnStr, String pProcedureName, SqlParameter[] pParamList)
        {
            var pCon = OpenConnection(ServerConnStr);

            int CountParam = pParamList.Length;

            for (int i = 0; i < CountParam; i++)
            {
                if (pParamList[i] != null)
                    GComm.Parameters.Add(pParamList[i]);
            }
            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = pCon;
            try
            {
                var Str = GComm.ExecuteScalar().ToString();
                return Str;
            }
            finally
            {
                GComm.Parameters.Clear();
                CCon(pCon);
            }
        }


        /// <summary> Execute NonQuery With No of Affected Record With(Store Procedure)
        /// Use To Execute Store Procedure With Parameter List and With General Connetion
        /// </summary>
        /// <param name="ServerConnStr"></param>
        /// <param name="pProcedureName">Name of Store Procedure</param>
        /// <param name="pParamList">Parameter List Arraty</param>
        /// <returns>Number of Affected Record Or If error raise then return -1</returns>
        public int ExNonQuery(String ServerConnStr, String pProcedureName, SqlParameter[] pParamList)
        {
            SqlConnection pCon;
            pCon = OpenConnection(ServerConnStr);

            Int32 IntParams = pParamList.Length;
            GComm.Parameters.Clear();

            foreach (SqlParameter t in pParamList)
            {
                if ((t != null)) GComm.Parameters.Add(t);
            }

            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = pCon;

            try
            {
                return GComm.ExecuteNonQuery();
            }
            finally
            {
                GComm.Parameters.Clear();
                CCon(pCon);
            }
        }
        public int ExNonQuery(String ServerConnStr, String pProcedureName, SqlParameter[] pParamList, DataTable _dt)
        {

            SqlConnection pCon;
            pCon = OpenConnection(ServerConnStr);

            Int32 IntParams = pParamList.Length;
            GComm.Parameters.Clear();

            foreach (SqlParameter t in pParamList)
            {
                if ((t != null))
                {
                    if (t.ToString() == "@TableVar" || t.ToString() == "@SaleTable")
                    {
                        GComm.Parameters.AddWithValue(t.ToString(), _dt);

                    }
                    else
                    {
                        GComm.Parameters.Add(t);
                    }
                }
            }

            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = pCon;

            try
            {
                return GComm.ExecuteNonQuery();
            }
            finally
            {
                GComm.Parameters.Clear();
                CCon(pCon);
            }
        }

        public int ExNonQuery(String ServerConnStr, String pProcedureName)
        {
            SqlConnection pCon;
            pCon = OpenConnection(ServerConnStr);

            GComm.CommandType = CommandType.Text;
            GComm.CommandText = pProcedureName;
            GComm.Connection = pCon;
            try
            {
                return GComm.ExecuteNonQuery();
            }
            finally
            {
                CCon(pCon);
            }
        }
        public int ExNonQuery(String pProcedureName, SqlParameter[] pParamList)
        {
            Int32 IntParams = pParamList.Length;
            GComm.Parameters.Clear();

            foreach (SqlParameter t in pParamList)
            {
                if ((t != null)) GComm.Parameters.Add(t);
            }

            GComm.CommandType = CommandType.StoredProcedure;
            GComm.CommandText = pProcedureName;
            GComm.Connection = GConn;

            try
            {
                return GComm.ExecuteNonQuery();
            }
            finally
            {
                GComm.Parameters.Clear();
            }
        }


        #endregion

        #region Closing And Utility like Reader,RecordSet,Ulility Like HasRow,FindNewID,FindText

        /// <summary>Close An Open Reader(SqlDataReader) 
        /// Closes Open Data Reader
        /// </summary>
        /// <param name="pReader">SqlDataReader</param>
        public void ClsRed(SqlDataReader pReader)
        {
            if (pReader != null)
            {
                if (pReader.IsClosed == false)
                {
                    pReader.Close();
                }
            }
        }

        /// <summary>Method For Checking Rows In A given Reader 
        /// And Return True If Reader Has Rows Loaded Otherwise Returns False
        /// </summary>
        /// <param name="pReader">SqlDataReader</param>
        /// <returns></returns>
        public bool HasRows(SqlDataReader pReader)
        {
            if (pReader != null)
            {
                if (pReader.HasRows)
                {
                    return true;
                }

                return false;
            }
            return false;
        }

        #endregion

        #region Utility
        /// <summary>
        /// Method For Display Date In Sql [MM/DD/YYYY] Format
        /// </summary>
        /// <param name="pStrDate">Date String</param>
        /// <returns>String</returns>
        private string SqlDate(string pStrDate)
        {
            if (pStrDate.Length == 0)
            {
                return "null";
            }
            else
            {
                //return "'" + DateTime.Parse(pStrDate).ToString(new System.Globalization.CultureInfo("en-US", false)).ToString() + "'";
                return "" + DateTime.Parse(pStrDate).ToString("MM/dd/yy") + "";
            }
        }
        /// <summary>
        /// Method For Display Time In Sql [HH:MM AM/PM] Format
        /// </summary>
        /// <param name="pStrTime">Time String</param>
        /// <returns>String</returns>
        private string SqlTime(string pStrTime)
        {
            if (pStrTime.Length == 0)
            {
                return "null";
            }
            else
            {
                return Convert.ToDateTime(pStrTime).ToString("hh:mm tt");
            }
        }
        /// <summary>
        /// Data Access To Business Layer For Time
        /// </summary>
        /// <param name="pDateTime"></param>
        /// <returns></returns>
        private DateTime? DTDBTime(DateTime? pDateTime)
        {
            if (pDateTime == null || pDateTime.ToString() == "")
            {
                return null;
            }

            DateTime? DTRet = new DateTime(1, 1, 1, pDateTime.Value.Hour, pDateTime.Value.Minute, pDateTime.Value.Second);
            return DTRet;
            //return DateTime.Parse(pDateTime.ToString("hh:mm tt"));
        }
        /// <summary>
        /// Date Checking
        /// </summary>
        /// <param name="pDateTime"></param>
        /// <returns></returns>
        private DateTime? DTDBDate(DateTime? pDateTime)
        {
            if (pDateTime == null || pDateTime.ToString() == "")
            {
                return null;
            }

            return pDateTime;
        }
        #endregion
    }
}
