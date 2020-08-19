using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase {
	#region General Firebase

	/// <summary>
	/// This class is for the general Firebase
	/// </summary>
	public sealed class Firebase {
		private static readonly Dictionary<string, Firebase> Instances = new Dictionary<string, Firebase>();

		public FirebaseAuth Auth {
			get;
			private set;
		} = null;
		public FirebaseDatabase Database {
			get;
			private set;
		} = null;
		public Firestore Firestore {
			get;
			private set;
		} = null;

		/// <summary>
		/// This function initializes the only Firebase instance on the app
		/// </summary>
		/// <param name="projectId">The Firebase project name</param>
		/// <param name="apiKey">The Web Api for the Firebase project</param>
		/// <returns>The only Firebase instance on the app</returns>
		public static Firebase InitializeFirebase(string projectId, string apiKey) {
			AuthObject authObject = new AuthObject(apiKey, projectId);
			Firebase firebase = new Firebase() {
				Auth = new FirebaseAuth(authObject),
				Database = new FirebaseDatabase(authObject),
				Firestore = new Firestore(authObject)
			};
			Instances.Add(projectId, firebase);
			return firebase;
		}

		private Firebase() { }

		/// <summary>
		/// This function initializes Firebase instance
		/// </summary>
		/// <param name="projectId">The Firebase project name</param>
		/// <param name="apiKey">The Web Api for the Firebase project</param>
		/// <returns>Firebase instance</returns>
		public Firebase(string projectId, string apiKey) {
			AuthObject authObject = new AuthObject(apiKey, projectId);
			Auth = new FirebaseAuth(authObject);
			Database = new FirebaseDatabase(authObject);
			Firestore = new Firestore(authObject);
			Instances.Add(projectId, this);
		}

		public static void RemoveFirebase(string projectId) => Instances.Remove(projectId);

		public static Firebase GetFirebase(string projectId) => Instances[projectId];
	}

	internal class AuthObject {

		internal AuthObject(string apiKey, string projectId) => (ApiKey, ProjectId) = (apiKey, projectId);

		internal string ApiKey { get; set; }
		internal string ProjectId { get; set; }
		internal FirebaseAuth.FirebaseUser User { get; set; }
	}

	#endregion General Firebase

	internal static class HttpClientExtensions {
		public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent iContent) {
			HttpMethod method = new HttpMethod("PATCH");
			HttpRequestMessage request = new HttpRequestMessage(method, requestUri) {
				Content = iContent
			};

			return await client.SendAsync(request);
		}
	}
}
