using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ChessProgrammingFundamentalsPractice
{
    public static class Clone
    {
        public static T DeepCopyItem<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            T result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }

        public static IList<T> CloneList<T>(this IList<T> listToClone) where T :ICloneable
        {
            return listToClone.Select(item=> (T)item.Clone()).ToList();
        }

        
    }
}
