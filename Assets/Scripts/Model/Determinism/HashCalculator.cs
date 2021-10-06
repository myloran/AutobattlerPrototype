using System.Security.Cryptography;
using System.Text;
using Model.NAI.Commands;
using Newtonsoft.Json;
using Shared.Addons.Examples.FixMath;

namespace Model.Determinism {
  public class HashCalculator {
    public void Calculate<T>(T obj, PriorityCommand priorityCommand, F32 currentTime) {
      var hash = GetObjectHash(obj);
      var hex = BytesToHex(hash);
      hashResult += "\n" + hex + " - " + priorityCommand;
      log.Info($"{currentTime}: {priorityCommand}, {nameof(hex)}: {hex}");
    }

    byte[] GetObjectHash<T>(T obj) {
      // var settings = new JsonSerializerSettings() {
      //   ContractResolver = new PrivateContractResolver(),
      //   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      // };
      var serialized = JsonConvert.SerializeObject(obj, Formatting.Indented);
      var stringified = typeof(T).Name + serialized;
      log.Info($"hash: {stringified}");
      var bytes = Encoding.UTF8.GetBytes(stringified);
      using var sha1 = SHA1.Create();
      return sha1.ComputeHash(bytes);
    }

    string BytesToHex(byte[] bytes) {
      var buffer = new StringBuilder(bytes.Length * 2);
      foreach (var byt in bytes) {
        buffer.Append(byt.ToString("X2"));
      }

      return buffer.ToString();
    }

    public void Reset() => hashResult = string.Empty;
    public void PrintReport() => log.Info($"{nameof(hashResult)}: {hashResult}");

    string hashResult = string.Empty;
    static readonly Shared.Addons.OkwyLogging.Logger log = Shared.Addons.OkwyLogging.MainLog.GetLogger(nameof(HashCalculator));
  }
}