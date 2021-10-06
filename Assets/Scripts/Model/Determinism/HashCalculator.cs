using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Model.Determinism {
  public class HashCalculator {
    public byte[] GetObjectHash<T>(T obj) {
      var serialized = JsonConvert.SerializeObject(obj);
      var stringified = typeof(T).Name + serialized;
      var bytes = Encoding.UTF8.GetBytes(stringified);
      using (var sha1 = SHA1.Create()) {
        var result = sha1.ComputeHash(bytes);
        return result;
      }
    }

    public string BytesToHex(byte[] bytes) {
      var buffer = new StringBuilder(bytes.Length * 2);
      foreach (var byt in bytes) {
        buffer.Append(byt.ToString("X2"));
      }

      return buffer.ToString();
    }
  }
}