using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using SocioPress.Profile.Struck;

namespace SocioPress.Message
{
    public class GetBy
    {
        #region Fields
        /// <summary>
        /// Instance of Get Message By Recepient Class.
        /// </summary>
        private static GetBy instance;
        public static GetBy Instance
        {
            get
            {
                if (instance == null)
                    instance = new GetBy();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public GetBy()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Recepient(string wp_id, string session_key, string recepient, string last_id, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("recepient", recepient);
            if (last_id != "" )
            {
                dict.Add("lid", last_id);
            }
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/sociopress/v1/messages/get/recepient", content);
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
