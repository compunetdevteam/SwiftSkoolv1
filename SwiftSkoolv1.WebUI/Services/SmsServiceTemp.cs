using SwiftSkool.Models;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Caching;

namespace SwiftSkoolv1.WebUI.Services
{
    public class SmsServiceTemp
    {

        private ConfigService _config;
        private Cache _cache;

        private string sessionId_cahe_key = "SmsSessionId_" + "GetSessionId";

        public SmsServiceTemp()
        {
            _config = new ConfigService();
            _cache = HttpContext.Current.Cache;
        }

        //Default method for making request to the SMS gateway. This method is not likely to be changed no matter what 
        //SMS gateway provider you want to use in the future.
        private string makeHttpRequest(string url)
        {
            //Initialize the web request
            var webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.ContentLength = 0;

            webReq.Method = "POST";//We're making a post request. This is the recommended method by the gateway.
            webReq.Timeout = 600000;//Set the timeout for the request

            var webResp = (HttpWebResponse)webReq.GetResponse();

            //Read the response and output it.
            Stream answer = webResp.GetResponseStream();
            StreamReader _answer = new StreamReader(answer);

            string result = _answer.ReadToEnd();

            return result;
        }

        //Instead of providing the subaccount and password in information every request to the SMS gateway, 
        //we use this method to get a session id from the gateway and pass to the service. 
        //This approach is more secure.
        public string Login()
        {
            string result = null;
            //Get the gateway url from the web.config AppSettings section
            string smsUrl = _config.SmsUrl;

            //Form the command for login to send to the gatway. You can download the api documentation 
            //from http://smslive247.com
            string smsCmd = "?cmd=login&owneremail=" + _config.SmsAccount + "&subacct=" + _config.SubAccount +
                            "&subacctpwd=" + _config.SubAccountPwd;

            try
            {
                return result = makeHttpRequest(smsUrl + smsCmd);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public string GetSessionId()
        {
            string result = null;//hold the returned message from the request
            bool success = false;//indicate if request is seccessful or not

            //First look for the session id in the cache items, if it does not exist, then trying making a request to the SMS gateway
            //for session id, otherwise get it from the cache
            if (_cache[sessionId_cahe_key] != null)
                result = (string)_cache.Get(sessionId_cahe_key);
            else
            {
                string response = Login();//Call the login method to get the session id from the sms gateway

                string errMsg = null;
                //process and return the response data from the login call in a friendly format
                result = GetResponseMessage(response, out success, out errMsg);

                if (success)//add the session id to the cache if the login request was successful
                {
                    _cache.Add(sessionId_cahe_key, result, null, DateTime.Now.AddHours(2), Cache.NoSlidingExpiration,
                               CacheItemPriority.Normal, null);
                }
            }
            return result;//return the session id
        }

        //Process the response from the sms gateway. By default, the gateway's response is in format below
        //OK: [RESPONSE-Message] -or- ERR: [ERROR NUMBER]: [ERROR DESCRIPTION]
        public string GetResponseMessage(string response, out bool success, out string errMsg)
        {
            //if the response contains 'OK', then the request was successful
            bool isSuccess = response.Substring(0, response.IndexOf(":") + 1).Contains("OK");
            //This holds the code returned from the request. Anything other than 0 means error
            string code = null;
            //This variable holds the description of the error message
            string errDesc = null;

            //get the code for the request
            if (isSuccess)
            {
                code = response.Substring(response.IndexOf(":") + 2);
            }
            else
            {
                code = response.Substring(response.IndexOf(":") + 2, response.LastIndexOf(":") - 1);
                errDesc = response.Substring(response.LastIndexOf(":") + 2);
            }

            success = isSuccess;
            errMsg = errDesc;
            return code;
        }

        public string Send(SMS sms)
        {
            string sessionId = GetSessionId(); //Get the session id
            string smsUrl = _config.SmsUrl; //Get the sms gateway url from the config file

            //Form the command for sending message. You can download the API documentation for full list of commands
            //from http://smslive247.com
            string smsCmd = String.Format("?cmd=sendmsg&sessionid={0}&message={1}&sender={2}" +
                                          "&sendto={3}&msgtype=0", sessionId, sms.Message, sms.SenderId, sms.Numbers);

            bool isSuccess = false;
            string errMsg = null;
            //Send sms message
            string response = makeHttpRequest(smsUrl + smsCmd);

            //Process the response from the gateway
            string code = GetResponseMessage(response, out isSuccess, out errMsg);

            //401 error code indicate invalid Session ID. If the session id is not valid, then delete it from cache and make a 
            //request to get a new session id from the sms gateway
            if (code == "401")
            {
                _cache.Remove(sessionId_cahe_key);//delete the session id from the cache

                sessionId = GetSessionId(); //Get the session id
                smsCmd = String.Format("?cmd=sendmsg&sessionid={0}&message={1}&sender={2}" +
                                          "&sendto={3}&msgtype=0", sessionId, sms.Message, sms.SenderId, sms.Numbers);

                return makeHttpRequest(smsUrl + smsCmd);//resend the sms to the gateway
            }

            return response;
        }
    }
}