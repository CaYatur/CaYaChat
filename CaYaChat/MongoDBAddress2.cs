namespace CaYaChat
{
    public static class MongoDBAddress2
    {
        public static string Message { get; set; }

        // MongoDB bağlantı dizesini buraya ekleyin
        //public static readonly string ConnectionString = "";
        public static readonly string ConnectionString = Message;
        public static readonly string DatabaseName = "CaYaChatSystem";
    }
}




//using MongoDB.Bson;
//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using static CaYaChat.CaYaChatApp;

//namespace CaYaChat
//{
//    public static class MongoDBService
//    {
//        private static readonly IMongoDatabase _database;
//        private const int MaxMessageCount = 15;

//        static MongoDBService()
//        {
//            var client = new MongoClient("mongodb+srv://<user>:<password>@<cluster>/<db>");
//            _database = client.GetDatabase("CaYaChatSystem");
//        }

//        public static async Task SendMessageAsync(string username, string message)
//        {
//            var messagesCollection = _database.GetCollection<BsonDocument>("messages");
//            var document = new BsonDocument
//            {
//                { "username", username },
//                { "message", message },
//                { "timestamp", DateTime.UtcNow }
//            };
//            await messagesCollection.InsertOneAsync(document);
//            await CleanupOldMessagesAsync();
//        }


//        public static async Task<List<ChatMessage>> GetMessagesAsync()
//        {
//            var messagesCollection = _database.GetCollection<ChatMessage>("messages");
//            var filter = Builders<ChatMessage>.Filter.Empty;
//            var sort = Builders<ChatMessage>.Sort.Ascending("timestamp");
//            var messages = await messagesCollection.Find(filter).Sort(sort).ToListAsync();
//            foreach (var message in messages)
//            {
//                message.Timestamp = message.Timestamp.ToLocalTime(); // Yerel saate dönüştür
//            }
//            return messages;
//        }


//        private static async Task CleanupOldMessagesAsync()
//        {
//            var messagesCollection = _database.GetCollection<ChatMessage>("messages");
//            var count = await messagesCollection.CountDocumentsAsync(Builders<ChatMessage>.Filter.Empty);

//            if (count > MaxMessageCount)
//            {
//                var sort = Builders<ChatMessage>.Sort.Ascending("timestamp");
//                var oldMessages = await messagesCollection.Find(Builders<ChatMessage>.Filter.Empty)
//                    .Sort(sort)
//                    .Limit((int)(count - MaxMessageCount))
//                    .ToListAsync();

//                var idsToDelete = oldMessages.ConvertAll(m => m.Id);

//                var deleteFilter = Builders<ChatMessage>.Filter.In(m => m.Id, idsToDelete);
//                await messagesCollection.DeleteManyAsync(deleteFilter);
//            }
//        }

//        public static async Task SaveOneTimeMessageAsync(string message)
//        {
//            var oneTimeCollection = _database.GetCollection<BsonDocument>("OneTime");

//            // Sil mevcut mesajı
//            await oneTimeCollection.DeleteManyAsync(new BsonDocument());

//            // Yeni mesajı ekle
//            var oneTimeMessage = new BsonDocument { { "message", message } };
//            await oneTimeCollection.InsertOneAsync(oneTimeMessage);
//        }

//        public static async Task<string> GetOneTimeMessageAsync()
//        {
//            var oneTimeCollection = _database.GetCollection<BsonDocument>("OneTime");
//            var oneTimeMessage = await oneTimeCollection.Find(new BsonDocument()).FirstOrDefaultAsync();

//            return oneTimeMessage != null ? oneTimeMessage.GetValue("message").AsString : string.Empty;
//        }


//        public static async Task SetOneTimeMessageAsync(string message)
//        {
//            var oneTimeCollection = _database.GetCollection<BsonDocument>("OneTime");

//            // Mesajı güncelle veya ekle
//            var filter = Builders<BsonDocument>.Filter.Exists("message");
//            var update = Builders<BsonDocument>.Update.Set("message", message);

//            var options = new UpdateOptions { IsUpsert = true };
//            await oneTimeCollection.UpdateOneAsync(filter, update, options);
//        }

//        public static async Task CheckAndInitializeBotStatusAsync()
//        {
//            var botCollection = _database.GetCollection<BsonDocument>("BOT");
//            var filter = Builders<BsonDocument>.Filter.Eq("name", "status");
//            var update = Builders<BsonDocument>.Update.SetOnInsert("active", true);
//            var options = new FindOneAndUpdateOptions<BsonDocument>
//            {
//                IsUpsert = true,
//                ReturnDocument = ReturnDocument.After
//            };

//            var result = await botCollection.FindOneAndUpdateAsync(filter, update, options);

//            // Eğer isterseniz, güncellenmiş veya yeni belgeyi kontrol edebilirsiniz.
//            bool isActive = result != null && result.GetValue("active", false).AsBoolean;
//            Console.WriteLine($"Bot active status is: {isActive}");
//        }


//        public static async Task<bool> IsBotActiveAsync()
//        {
//            var botCollection = _database.GetCollection<BsonDocument>("BOT");
//            var filter = Builders<BsonDocument>.Filter.Eq("name", "status");
//            var document = await botCollection.Find(filter).FirstOrDefaultAsync();

//            if (document != null && document.Contains("active"))
//            {
//                return document["active"].AsBoolean;
//            }

//            return false; // Varsayılan olarak bot kapalı
//        }


//        public static async Task SetBotStatusAsync(bool status)
//        {
//            var botCollection = _database.GetCollection<BsonDocument>("BOT");
//            var filter = Builders<BsonDocument>.Filter.Eq("name", "status");
//            var update = Builders<BsonDocument>.Update.Set("active", status);

//            var updateOptions = new UpdateOptions { IsUpsert = true };
//            await botCollection.UpdateOneAsync(filter, update, updateOptions);
//        }



//        public static async Task AddOrUpdateBotResponseAsync(string command, string response)
//        {
//            var botResponsesCollection = _database.GetCollection<BsonDocument>("BotDataBaseResponse");
//            var filter = Builders<BsonDocument>.Filter.Eq("command", command);
//            var update = Builders<BsonDocument>.Update.Set("response", response);

//            var updateOptions = new UpdateOptions { IsUpsert = true };
//            await botResponsesCollection.UpdateOneAsync(filter, update, updateOptions);
//        }


//        public static async Task<string> GetBotResponseAsync(string command)
//        {
//            var botResponsesCollection = _database.GetCollection<BsonDocument>("BotDataBaseResponse");
//            var filter = Builders<BsonDocument>.Filter.Eq("command", command);
//            var document = await botResponsesCollection.Find(filter).FirstOrDefaultAsync();

//            if (document != null && document.Contains("response"))
//            {
//                return document["response"].AsString;
//            }

//            return null; // Yanıt bulunamazsa null döner
//        }

//        public static async Task SaveBotResponseToMongoDBAsync(string messageToAdd, string responseToAdd)
//        {
//            var botResponsesCollection = _database.GetCollection<BsonDocument>("BotDataBaseResponse");
//            var filter = Builders<BsonDocument>.Filter.Eq("command", messageToAdd);
//            var update = Builders<BsonDocument>.Update.Set("response", responseToAdd);

//            var updateOptions = new UpdateOptions { IsUpsert = true };
//            await botResponsesCollection.UpdateOneAsync(filter, update, updateOptions);
//        }



//        public static async Task RemoveBotResponseAsync(string command)
//        {
//            var botResponsesCollection = _database.GetCollection<BsonDocument>("BotDataBaseResponse");
//            var filter = Builders<BsonDocument>.Filter.Eq("command", command);
//            var result = await botResponsesCollection.DeleteOneAsync(filter);

//            if (result.DeletedCount == 0)
//            {
//                throw new Exception("Silinecek yanıt bulunamadı.");
//            }
//        }





//    }
//}