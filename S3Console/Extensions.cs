namespace S3Console
{
    public static class Extensions
    {
        public static bool IsSuccess(this System.Net.HttpStatusCode code)
        {
            return code == System.Net.HttpStatusCode.OK || code == System.Net.HttpStatusCode.Created;
        }
    }
}
