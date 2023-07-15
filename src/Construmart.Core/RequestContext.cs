using MediatR;

namespace Construmart.Core
{
    public class RequestContext<TResponse> : IRequest<TResponse>
    {
        public RequestContext()
        {
        }
    }
}