using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BusLib.Transaction
{
    public class BORegistration
    {
        public bool Save_User(int Id, string Name, string Email, string Phone, string Address, int StateId, int CityId)
        {
            bool Result = false;
            DataLib.SqlServer.OperationSQLServer Ope = new DataLib.SqlServer.OperationSQLServer();
            Ope.Clear();

            Ope.AddParams("Id", Id.ToString());
            Ope.AddParams("Name", Name.ToString());
            Ope.AddParams("Email", Email.ToString()); 
            Ope.AddParams("Phone", Phone.ToString());
            Ope.AddParams("Address", Address.ToString()); 
            Ope.AddParams("StateId", StateId.ToString());
            Ope.AddParams("CityId", CityId.ToString());
            
            Result = Convert.ToBoolean(Ope.ExNonQuery(BusLib.Config.Configuration.InterNetServerConnStr, "SP_Insert_Update_User", Ope.GetParams()));
            return Result;
        }

        public DataSet GetUser(int Id)
        {
            DataSet _DS = new DataSet();
            DataLib.SqlServer.OperationSQLServer Ope = new DataLib.SqlServer.OperationSQLServer();
            Ope.Clear();
            Ope.AddParams("Id", Id.ToString());
            Ope.FillDataSet(BusLib.Config.Configuration.InterNetServerConnStr, _DS, "Result", "SP_Get_User", Ope.GetParams());
            Ope = null;
            return _DS;
        }

        public bool DeleteUser(int Id)
        {
            bool Result = false;
            DataLib.SqlServer.OperationSQLServer Ope = new DataLib.SqlServer.OperationSQLServer();
            Ope.Clear();
            Ope.AddParams("Id", Id.ToString());
            Result = Convert.ToBoolean(Ope.ExNonQuery(BusLib.Config.Configuration.InterNetServerConnStr, "SP_Delete_User", Ope.GetParams()));
            return Result;
        }

        public DataSet GetState()
        {
            DataSet _DS = new DataSet();
            DataLib.SqlServer.OperationSQLServer Ope = new DataLib.SqlServer.OperationSQLServer();
            Ope.Clear();            
            Ope.FillDataSet(BusLib.Config.Configuration.InterNetServerConnStr, _DS, "Result", "SP_Get_State", Ope.GetParams());
            Ope = null;
            return _DS;
        }

        public DataSet GetCityFromStateId(int StateId)
        {
            DataSet _DS = new DataSet();
            DataLib.SqlServer.OperationSQLServer Ope = new DataLib.SqlServer.OperationSQLServer();
            Ope.Clear();
            Ope.AddParams("StateId", StateId.ToString());
            Ope.FillDataSet(BusLib.Config.Configuration.InterNetServerConnStr, _DS, "Result", "SP_Get_CityFromStateId", Ope.GetParams());
            Ope = null;
            return _DS;
        }

    }
}
