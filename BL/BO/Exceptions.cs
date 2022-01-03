using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    public class AddingProblemException : Exception
    {
        public AddingProblemException() : base() { }
        public AddingProblemException(string message) : base(message) { }
        public AddingProblemException(string message, Exception inner) : base(message, inner) { }
        protected AddingProblemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    [Serializable]
    public class UpdateProblemException : Exception
    {
        public UpdateProblemException() : base() { }
        public UpdateProblemException(string message) : base(message) { }
        public UpdateProblemException(string message, Exception inner) : base(message, inner) { }
        protected UpdateProblemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    [Serializable]
    public class GetDetailsProblemException : Exception
    {
        public GetDetailsProblemException() : base() { }
        public GetDetailsProblemException(string message) : base(message) { }
        public GetDetailsProblemException(string message, Exception inner) : base(message, inner) { }
        protected GetDetailsProblemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
