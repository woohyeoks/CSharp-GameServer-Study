using HTTPSimple;
using Newtonsoft.Json.Linq;

public class JsonHelper
{
    public static object Deserialize(string json)
    {
        return ToObject(JToken.Parse(json));
    }



    public static T Deserialize<T>(T httpData, string json) where T : HTTPData
    {
        var toDic = ToObject(JToken.Parse(json)) as Dictionary<string, object>;
        httpData.Deserialize(toDic);

        return httpData;
    }


    private static object ToObject(JToken token)
    {
        switch (token.Type)
        {
            case JTokenType.Object:
                {
                    var obj = token.Children<JProperty>()
                        .ToDictionary(prop => prop.Name,
                            prop => ToObject(prop.Value));
                    return obj;
                }
            case JTokenType.Array:
                {
                    var arr = token.Select(ToObject).ToList();
                    return arr;
                }
            default:
                {
                    var def = ((JValue)token).Value;
                    return def;
                }

        }

    }
}