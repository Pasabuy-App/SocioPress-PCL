using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SocioPress.Profile.Struck;
using System.Net.Http;

namespace SocioPress.Activity
{
    public class ListingAct
    {
        #region Fields
        /// <summary>
        /// Instance of Listing of Activity by User ID, Store ID, Icon, Acitivty ID and open or not Class.
        /// </summary>
        private static ListingAct instance;
        public static ListingAct Instance
        {
            get
            {
                if (instance == null)
                    instance = new ListingAct();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public ListingAct()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Listing(string wp_id, string session_key, string stid, string icon, string open, string lid, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            if (stid != "")
            {
                dict.Add("stid", stid);
            }
            if (icon != "")
            {
                dict.Add("icon", icon);
            }
            if (open != "")
            {
                dict.Add("open", open);
            }
            if (lid != "")
            {
                dict.Add("lid", lid);
            }
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/sociopress/v1/activity/list/all", content);
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
