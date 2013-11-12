using Common;

namespace Server.Model
{
	
	public class NoSuchClientException : NWOException
	{
		public NoSuchClientException() {}
		public NoSuchClientException(string message) : base (message) {}
		public NoSuchClientException(string message, System.Exception inner) : base (message, inner) {}
		
		// Constructor needed for serialization 
		// when exception propagates from a remoting server to the client.
		protected NoSuchClientException(System.Runtime.Serialization.SerializationInfo info,
		                       System.Runtime.Serialization.StreamingContext context) {}
	}
}