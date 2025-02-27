using Newtonsoft.Json;

namespace MvcNetCoreTiendaCubosRubik.Extensions
{
    public static class SessionExtension
    {
        //1. esto es un metodo para recuperar cualquier objeto en Session
        public static T GetObject<T>(this ISession session, string key)
        {
            string json = session.GetString(key);
            if (json == null)
            {
                return default(T);
            }
            else
            {
                T data = JsonConvert.DeserializeObject<T>(json);
                return data;
            }
        }


        //2. esto es un metodo para guardar cualquier objeto en Session
        public static void SetObject(this ISession session, string key, object value)
        {
            string data = JsonConvert.SerializeObject(value);
            session.SetString(key, data);
        }

    }
}
