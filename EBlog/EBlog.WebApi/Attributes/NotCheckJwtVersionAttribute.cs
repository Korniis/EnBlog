namespace EBlog.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Method)]
    public class NotCheckJwtVersionAttribute:Attribute
    {
    }
}
