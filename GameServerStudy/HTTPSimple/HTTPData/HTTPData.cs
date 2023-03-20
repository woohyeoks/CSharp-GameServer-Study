using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPSimple
{
    public class HTTPData
    {
        protected Dictionary<string, object> _headerDict = new Dictionary<string, object>();
        protected Dictionary<string, object> _bodyDict = new Dictionary<string, object>();
        protected Dictionary<string, object> _requestDict = new Dictionary<string, object>();
        public Dictionary<string, object> _responseDict = new Dictionary<string, object>();

        public void InitData()
        {
            _headerDict.Clear();
            _bodyDict.Clear();
            _requestDict.Clear();
            _responseDict.Clear();
        }

        public void SetHeaderField(string key, object val)
        {
            if (_headerDict.ContainsKey(key))
            {
                _headerDict[key] = val;
            }
            else
            {
                _headerDict.Add(key, val);
            }
        }

        public void SetBodyField(string key, object val)
        {
            if (_bodyDict.ContainsKey(key))
            {
                _bodyDict[key] = val;
            }
            else
            {
                _bodyDict.Add(key, val);
            }
        }

        public string GetReqJson()
        {
            SetRequestField();
            return JsonConvert.SerializeObject(_requestDict);
        }

        private void SetRequestField()
        {
            _requestDict.Clear();
            _requestDict.Add("header", _headerDict);
            _requestDict.Add("body", _bodyDict);
        }
        public void Deserialize(Dictionary<string, object> data)
        {
            _responseDict = data;
        }


    }
}
