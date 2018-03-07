using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NQR.CINC.Entities.EntityFramework
{
    public class AppContext
    {
        public static object GetItem(string key)
        {
            //If method is called from a web application
            if (HttpContext.Current != null)
            {
                //return item from the HttpContext.Items collection
                return HttpContext.Current.Items[key];
            }
            else
            {
                //Use a dictionary for non-web clients
                if (Items.ContainsKey(key))
                {
                    return Items[key];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Add a key value pair to either the HttpContext or a Dictionary
        /// </summary>
        /// <param name="key">name of the key</param>
        /// <param name="obj">object to store</param>
        public static void AddItem(string key, object obj)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[key] = obj;
            }
            else
            {
                Items[key] = obj;
            }
        }

        //to support non-web clients
        static Dictionary<string, object> Items = new Dictionary<string, object>();
    }
}
