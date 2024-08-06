namespace EBlog.WebApi.Helper
{
    public class JWTOptions
    {
        public string SigningKey { get; set; }
        public int ExpireSeconds { get; set; }
        public string Issuer { get; set; }
        public string Audience  { get; set; }
    }
}
