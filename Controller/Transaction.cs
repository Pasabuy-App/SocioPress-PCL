using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SocioPress.Controller.Struct;
using System.Net.Http;

namespace SocioPress.Controller
{
    public class Transaction
    {
        #region Fields
        /// <summary>
        /// Instance of Transaction of User Class with get total of transaction method.
        /// </summary>
        private static Transaction instance;
        public static Transaction Instance
        {
            get
            {
                if (instance == null)
                    instance = new Transaction();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Transaction()
        {
            client = new HttpClient();
        }
        #endregion
        #region Get Total Transaction Method
        public async void GetTotal(string wp_id, string session_key, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/sociopress/v1/transactions/user/list/total", content);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                Token token = JsonConvert.DeserializeObject<Token>(result);

                bool success = token.status == "success" ? true : false;
                string data = token.status == "success" ? result : token.message;
                callback(success, data);
            }
            else
            {
                callback(false, "Network Error! Check your connection.");
            }
        }
        #endregion
    }
}
