//using Google.Cloud.Firestore;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;

//namespace CaYaChat
//{
//    public static class FirebaseService
//    {
//        private static FirestoreDb _firestoreDb;

//        static FirebaseService()
//        {
//            try
//            {
//                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebase-config.json");
//                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
//                _firestoreDb = FirestoreDb.Create("cayachat");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Firebase initialization error: {ex.Message}");
//                throw;
//            }
//        }

//        public static async Task SendMessageAsync(string username, string message)
//        {
//            DocumentReference docRef = _firestoreDb.Collection("messages").Document();
//            Dictionary<string, object> data = new Dictionary<string, object>
//            {
//                { "username", username },
//                { "message", message },
//                { "timestamp", Timestamp.FromDateTime(DateTime.UtcNow) }
//            };
//            await docRef.SetAsync(data);
//        }

//        public static async Task<List<ChatMessage>> GetMessagesAsync()
//        {
//            Query query = _firestoreDb.Collection("messages").OrderBy("timestamp");
//            QuerySnapshot snapshot = await query.GetSnapshotAsync();
//            List<ChatMessage> messages = new List<ChatMessage>();
//            foreach (DocumentSnapshot document in snapshot.Documents)
//            {
//                if (document.Exists)
//                {
//                    string username = document.GetValue<string>("username");
//                    string message = document.GetValue<string>("message");
//                    Timestamp timestamp = document.GetValue<Timestamp>("timestamp");
//                    DateTime dateTime = timestamp.ToDateTime().AddHours(3); // UTC+3 saat dilimi

//                    messages.Add(new ChatMessage
//                    {
//                        Username = username,
//                        Message = message,
//                        Timestamp = dateTime
//                    });
//                }
//            }
//            return messages;
//        }
//    }

//    public class ChatMessage
//    {
//        public string Username { get; set; } = string.Empty;
//        public string Message { get; set; } = string.Empty;
//        public DateTime Timestamp { get; set; }
//    }
//}
