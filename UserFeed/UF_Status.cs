using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SocioPress.Profile.Struck;
using System.Net.Http;

namespace SocioPress.Profile
{
    public class UF_Status
    {
        #region Fields
        /// <summary>
        /// Instance of User Feed Type of Status Class.
        /// </summary>
        private static UF_Status instance;
        public static UF_Status Instance
        {
            get
            {
                if (instance == null)
                    instance = new UF_Status();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication for our Backend.
        /// </summary>
        HttpClient client;
        public UF_Status()
        {
            client = new HttpClient();
        }
        #endregion
        #region Method
        public async void Submit(string wp_id, string session_key, string title, string info, string style, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("title", title);
                dict.Add("info", info);
                dict.Add("style", style);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/datavice/api/v1/feed/status", content);
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
