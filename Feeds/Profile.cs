using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SocioPress.Profile.Struck;
using System.Net.Http;

namespace SocioPress.Feeds
{
    public class Profile
    {
        #region Fields
        /// <summary>
        /// Instance of Profile Feeds Class.
        /// </summary>
        private static Profile instance;
        public static Profile Instance
        {
            get
            {
                if (instance == null)
                    instance = new Profile();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Profile()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void GetFeeds(string wp_id, string session_key, string last_id, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            if (last_id != "")
            {
                dict.Add("lid", last_id);
            }
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/sociopress/v1/feeds/profile", content);
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
