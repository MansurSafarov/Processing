using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace project2
{
    class ServerRequest
    {
        //
        public string ServerUrl = "";
        public HttpWebRequest request;
        public string _account;
        public string _supplier_service_gate;
        public string _supplier_id;
        public int _status = 5;
        public string _result;
        /// <summary>
        /// Method to send request
        /// </summary>
        public void SendRequest(ServerDB db)
        {
            request = (HttpWebRequest)WebRequest.Create(ServerUrl);
            var xml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><request><auth login=\"test5\" sign=\"200820E3227815ED1756A6B531E7E0D2\" signAlg=\"MD5\" /><client terminal=\"5\" /><providers><checkPaymentRequisites><payment id=\"1427467640336\"><from currency={0} amount={1} /><to currency={2} service={3} amount={4} account={0} payType=\"1\" /><receipt id=\"88\" /></payment></checkPaymentRequisites></ providers ></ request >",db._account,db._supplier_service_gate,db._id,db._supplier_id);
            byte[] requestInFormOfBytes = System.Text.Encoding.ASCII.GetBytes(xml);
            request.Method = "POST";
            request.ContentLength = requestInFormOfBytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(requestInFormOfBytes, 0, requestInFormOfBytes.Length);
            requestStream.Close();
            //Get Response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
            string receivedResponse = respStream.ReadToEnd();
            Console.WriteLine(receivedResponse);
            
            CheckStatusAndResult(receivedResponse);
            db.ChangeStatus();
            respStream.Close();
            response.Close();

        }
        /// <summary>
        /// Method to cheking and finding result and status
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public string CheckStatusAndResult(string xml)
        {
            //Finding Status and Result from xml File;
            string status = "";
            string result = "";
            using (var connection = new SqlConnection(ConfigurationSettings.AppSettings["sql_db"]))
            {
                using (var comand=new SqlCommand("select status from SupplierCodes where supplier_id=4 and supplier_code=@supplier_code",connection))
                {
                    connection.Open();
                    comand.Parameters.Add("@supplier_code", SqlDbType.NVarChar);
                    comand.Parameters["@supplier_code"].Value = status + "," + result;
                    var comand_result = comand.ExecuteReader();
                    while (comand_result.Read())
                    {
                        _status = comand_result.GetInt32(0);
                    }
                    connection.Close();
                }
            }
            
            return _status+","+_result;
        }
    }
}
