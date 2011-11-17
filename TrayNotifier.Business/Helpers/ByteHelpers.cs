namespace TrayNotifier.Business.Helpers
{
    using System.IO;

    public static class ByteHelpers
    {
        public static string ConvertToString(this byte[] bytes)
        {
            string item;

            using (var client = new MemoryStream(bytes))
                using (var stream = new StreamReader(client))
                    item = stream.ReadToEnd();

            return item;
        }
    }
}