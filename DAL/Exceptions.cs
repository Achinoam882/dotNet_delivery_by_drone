using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace IDAL

{
    namespace DO
    {
        [Serializable]
        public class AddExistingObjectException: Exception
        {
            public AddExistingObjectException() : base() { }
            public AddExistingObjectException(string message): base(message) { }
            public AddExistingObjectException(string message, Exception inner) : base(message, inner) { }
            protected AddExistingObjectException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
            public override string ToString()
            {
                return "Error adding an object with an existing ID";
            }


        }



        [Serializable]
       public  class  NonExistingObjectException : Exception
        {
            public NonExistingObjectException() : base() { }
            public NonExistingObjectException(string message) : base(message) { }
            public NonExistingObjectException(string message, Exception inner) : base(message, inner) { }
            protected NonExistingObjectException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

            public override string ToString()
            {
                return "Error updating an object with non-existing ID number";
            }
        }
    }
    
}
