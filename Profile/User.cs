using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SocioPress.Profile.Struck;
using System.Net.Http;

namespace SocioPress.Profile
{
    public class User
    {
        #region Fields
        /// <summary>
        /// Instance of Profile User Data Class.
        /// </summary>
        private static User instance;
        public static User Instance
        {
            get
            {
                if (instance == null)
                    instance = new User();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication for our Backend.
        /// </summary>
        HttpClient client;
        public User()
        {
            client = new HttpClient();
        }
        #endregion
        #region Method
        public async void GetData(string wp_id, string session_key, Action<bool, string> callback)
        {
            string getRequest = "?";
            getRequest += "wpid" + wp_id;
            getRequest += "&snky" + session_key;

            var response = await client.GetAsync(BaseClass.BaseDomainUrl + "/sociopress/api/v1/user/profile" + getRequest);
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
