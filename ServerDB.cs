using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Configuration;

namespace project2
{
    class ServerDB
    {
        ServerRequest request;
        public string _account;
        public string _supplier_service_gate;
        public Int64 _id;
        public string _supplier_id;
        public string _service_id;
        public string _date;
        public string _currency;
        public ServerDB()
        {
            SqlConnection connection = new SqlConnection(ConfigurationSettings.AppSettings["sql_db"]);
            var comand = new SqlCommand("select CR.account,CR.CreateDate,CR.currency_id,STS.supplier_service_gate,CR.id,SR.supplier_id,CR.service_id from CheckRequest CR inner join ServiceRoute SR on CR.agent_id=SR.agent_id and CR.service_id = SR.service_id inner join ServiceToSupplier STS on CR.service_id=STS.service_id and SR.supplier_id=STS.supplier_id where SR.supplier_id=2 and CR.status=5", connection);
            connection.Open();
            var result = comand.ExecuteReader();
            while (result.Read())
            {
                Console.WriteLine(result.GetString(0));
                _account = result.GetString(0);
                _date = result.GetString(1);
                _currency = result.GetString(2);
                _supplier_service_gate = result.GetString(3);
                _id = result.GetInt64(4);
                _supplier_id = result.GetString(5);
                _service_id = result.GetString(6);
            }
            connection.Close();

        }
        public void ChangeStatus()
        {
            Int32 status = request._status;
            SqlConnection connection = new SqlConnection(ConfigurationSettings.AppSettings["sql_db"]);
            var sql_query=new SqlCommand("update CheckRequest set CR.status=@status where CR.id=@id", connection);
            sql_query.Parameters.Add("@id", SqlDbType.BigInt);
            sql_query.Parameters["@id"].Value = _id;
            sql_query.Parameters.Add("@status",SqlDbType.Int);
            sql_query.Parameters["@status"].Value = status;
            sql_query.ExecuteNonQuery();
        }
    }
}
