using System;

namespace Common
{

		public class NWOException : System.ApplicationException
		{
			public NWOException() {}
    		public NWOException(string message) : base (message) {}
			public NWOException (string message, System.Exception inner) : base (message, inner) {}

			// Constructor needed for serialization 
    		// when exception propagates from a remoting server to the client.
    		protected NWOException(System.Runtime.Serialization.SerializationInfo info,
        	System.Runtime.Serialization.StreamingContext context) {}
		}

}

