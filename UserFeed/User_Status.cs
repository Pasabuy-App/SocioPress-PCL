using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SocioPress.Profile.Struck;
using System.Net.Http;

namespace SocioPress.Profile
{
    public class User_Status
    {
        #region Fields
        /// <summary>
        /// Instance of User Feed Type of Status Class.
        /// </summary>
        private static User_Status instance;
        public static User_Status Instance
        {
            get
            {
                if (instance == null)
                    instance = new User_Status();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public User_Status()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Submit(string wp_id, string session_key, string title, string info, string style, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("title", title);
                dict.Add("info", info);
                dict.Add("style", style);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/sociopress/v1/feed/status", content);
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
