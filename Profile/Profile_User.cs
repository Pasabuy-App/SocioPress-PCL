using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SocioPress.Profile.Struck;
using System.Net.Http;

namespace SocioPress.Profile
{
    public class Profile_User
    {
        #region Fields
        /// <summary>
        /// Instance of Profile User Data Class.
        /// </summary>
        private static Profile_User instance;
        public static Profile_User Instance
        {
            get
            {
                if (instance == null)
                    instance = new Profile_User();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Profile_User()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void GetData(string wp_id, string session_key, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/sociopress/v1/user/profile", content);
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
