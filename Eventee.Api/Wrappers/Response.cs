namespace Eventee.Api.Wrappers
{
    public class Response<T>
    {
        public T? Children { get; set; }

        public Response(T? children)
        {
            Children = children;
        }
    }
}
