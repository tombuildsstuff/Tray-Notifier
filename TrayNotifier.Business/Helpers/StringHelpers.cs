namespace TrayNotifier.Business.Helpers
{
    using System.Text;

    public static class StringHelpers
    {
        public static byte[] ToBytes(this string input)
        {
            return new ASCIIEncoding().GetBytes(input);
        }
    }
}