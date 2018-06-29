using Framework.Constract.Constant;
using System.Collections.Generic;

namespace Framework.Constract.SeedWork
{
    public class RequestResult
    {
        public RequestState State { get; set; }
        public IList<ValidationError> ErrorMessages { get; set; } = new List<ValidationError>();
        public object Data { get; set; }
    }
}
