using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SocioPress.Controller.Struct;
using System.Net.Http;

namespace SocioPress.Profile
{
    public class Profile
    {
        #region Fields
        /// <summary>
        /// Instance of Profile Class with get data method.
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
        #region GetData Method
        public async void GetData(string wp_id, string session_key, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/sociopress/v1/profile/data", content);
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
