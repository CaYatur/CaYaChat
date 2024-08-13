using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CaYaChat.CaYaChatApp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CaYaChat
{
    public static class MongoDBService
    {
        private static readonly IMongoDatabase _database;
        private const int MaxMessageCount = 15;

        public static string Message { get; set; }

        static MongoDBService()
        {

            try
            {
                //MessageBox.Show("MMM!");
                //MessageBox.Show(CaYaChatApp.ConnectionString);
                //MessageBox.Show(Message);
                //MessageBox.Show(MongoDBAddress.ConnectionString);
                //var client = new MongoClient(CaYaChatApp.ConnectionString);
                var client = new MongoClient(MongoDBAddress.ConnectionString);
                //var client = new MongoClient(Message);
                _database = client.GetDatabase(MongoDBAddress.DatabaseName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MongoDB bağlantısı yapılamadı: {ex.Message}");
                MessageBox.Show($"MongoDB bağlantısı yapılamadı: {ex.Message}");
                _database = null; // Bağlantı hatası durumunda _database null olarak ayarlanır
            }
        }

        public static async Task SendMessageAsync(string username, string message)
        {
            var messagesCollection = _database.GetCollection<BsonDocument>("messages");
            var document = new BsonDocument
            {
                { "username", username },
                { "message", message },
                { "timestamp", DateTime.UtcNow }
            };
            await messagesCollection.InsertOneAsync(document);
            await CleanupOldMessagesAsync();
        }

        public static async Task<List<ChatMessage>> GetMessagesAsync()
        {
            var messagesCollection = _database.GetCollection<ChatMessage>("messages");
            var filter = Builders<ChatMessage>.Filter.Empty;
            var sort = Builders<ChatMessage>.Sort.Ascending("timestamp");
            var messages = await messagesCollection.Find(filter).Sort(sort).ToListAsync();
            foreach (var message in messages)
            {
                message.Timestamp = message.Timestamp.ToLocalTime(); // Yerel saate dönüştür
            }
            return messages;
        }

        private static async Task CleanupOldMessagesAsync()
        {
            var messagesCollection = _database.GetCollection<ChatMessage>("messages");
            var count = await messagesCollection.CountDocumentsAsync(Builders<ChatMessage>.Filter.Empty);

            if (count > MaxMessageCount)
            {
                var sort = Builders<ChatMessage>.Sort.Ascending("timestamp");
                var oldMessages = await messagesCollection.Find(Builders<ChatMessage>.Filter.Empty)
                    .Sort(sort)
                    .Limit((int)(count - MaxMessageCount))
                    .ToListAsync();

                var idsToDelete = oldMessages.ConvertAll(m => m.Id);

                var deleteFilter = Builders<ChatMessage>.Filter.In(m => m.Id, idsToDelete);
                await messagesCollection.DeleteManyAsync(deleteFilter);
            }
        }

        public static async Task SaveOneTimeMessageAsync(string message)
        {
            var oneTimeCollection = _database.GetCollection<BsonDocument>("OneTime");

            // Sil mevcut mesajı
            await oneTimeCollection.DeleteManyAsync(new BsonDocument());

            // Yeni mesajı ekle
            var oneTimeMessage = new BsonDocument { { "message", message } };
            await oneTimeCollection.InsertOneAsync(oneTimeMessage);
        }

        public static async Task<string> GetOneTimeMessageAsync()
        {
            var oneTimeCollection = _database.GetCollection<BsonDocument>("OneTime");
            var oneTimeMessage = await oneTimeCollection.Find(new BsonDocument()).FirstOrDefaultAsync();

            return oneTimeMessage != null ? oneTimeMessage.GetValue("message").AsString : string.Empty;
        }

        public static async Task SetOneTimeMessageAsync(string message)
        {
            var oneTimeCollection = _database.GetCollection<BsonDocument>("OneTime");

            // Mesajı güncelle veya ekle
            var filter = Builders<BsonDocument>.Filter.Exists("message");
            var update = Builders<BsonDocument>.Update.Set("message", message);

            var options = new UpdateOptions { IsUpsert = true };
            await oneTimeCollection.UpdateOneAsync(filter, update, options);
        }

        public static async Task CheckAndInitializeBotStatusAsync()
        {
            var botCollection = _database.GetCollection<BsonDocument>("BOT");
            var filter = Builders<BsonDocument>.Filter.Eq("name", "status");
            var update = Builders<BsonDocument>.Update.SetOnInsert("active", true);
            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var result = await botCollection.FindOneAndUpdateAsync(filter, update, options);

            // Eğer isterseniz, güncellenmiş veya yeni belgeyi kontrol edebilirsiniz.
            bool isActive = result != null && result.GetValue("active", false).AsBoolean;
            Console.WriteLine($"Bot active status is: {isActive}");
        }

        public static async Task<bool> IsBotActiveAsync()
        {
            var botCollection = _database.GetCollection<BsonDocument>("BOT");
            var filter = Builders<BsonDocument>.Filter.Eq("name", "status");
            var document = await botCollection.Find(filter).FirstOrDefaultAsync();

            if (document != null && document.Contains("active"))
            {
                return document["active"].AsBoolean;
            }

            return false; // Varsayılan olarak bot kapalı
        }

        public static async Task SetBotStatusAsync(bool status)
        {
            var botCollection = _database.GetCollection<BsonDocument>("BOT");
            var filter = Builders<BsonDocument>.Filter.Eq("name", "status");
            var update = Builders<BsonDocument>.Update.Set("active", status);

            var updateOptions = new UpdateOptions { IsUpsert = true };
            await botCollection.UpdateOneAsync(filter, update, updateOptions);
        }

        public static async Task AddOrUpdateBotResponseAsync(string command, string response)
        {
            var botResponsesCollection = _database.GetCollection<BsonDocument>("BotDataBaseResponse");
            var filter = Builders<BsonDocument>.Filter.Eq("command", command);
            var update = Builders<BsonDocument>.Update.Set("response", response);

            var updateOptions = new UpdateOptions { IsUpsert = true };
            await botResponsesCollection.UpdateOneAsync(filter, update, updateOptions);
        }

        public static async Task<string> GetBotResponseAsync(string command)
        {
            var botResponsesCollection = _database.GetCollection<BsonDocument>("BotDataBaseResponse");
            var filter = Builders<BsonDocument>.Filter.Eq("command", command);
            var document = await botResponsesCollection.Find(filter).FirstOrDefaultAsync();

            if (document != null && document.Contains("response"))
            {
                return document["response"].AsString;
            }

            return null; // Yanıt bulunamazsa null döner
        }

        public static async Task SaveBotResponseToMongoDBAsync(string messageToAdd, string responseToAdd)
        {
            var botResponsesCollection = _database.GetCollection<BsonDocument>("BotDataBaseResponse");
            var filter = Builders<BsonDocument>.Filter.Eq("command", messageToAdd);
            var update = Builders<BsonDocument>.Update.Set("response", responseToAdd);

            var updateOptions = new UpdateOptions { IsUpsert = true };
            await botResponsesCollection.UpdateOneAsync(filter, update, updateOptions);
        }

        public static async Task RemoveBotResponseAsync(string command)
        {
            var botResponsesCollection = _database.GetCollection<BsonDocument>("BotDataBaseResponse");
            var filter = Builders<BsonDocument>.Filter.Eq("command", command);
            var result = await botResponsesCollection.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new Exception("Silinecek yanıt bulunamadı.");
            }
        }







        public static async Task<LicenseStatus> CheckLicenseAsync(string licenseKey)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>("LicenceSystem");
                var filter = Builders<BsonDocument>.Filter.Eq("Licence", licenseKey);
                var document = await collection.Find(filter).FirstOrDefaultAsync();

                if (document != null)
                {
                    var id = document["ID"].AsInt32;
                    bool isBanned = document.GetValue("Banned", false).AsBoolean;
                    bool isOwner = document.GetValue("Owner", false).AsBoolean;

                    //MessageBox.Show($"Lisansın ID'si: {id}");
                    SharedData.UserID = id;

                    if (isBanned)
                    {
                        throw new Exception("Bu lisans yasaklı.");
                    }

                    return new LicenseStatus
                    {
                        Exists = true,
                        IsOwner = isOwner
                    };
                }
                else
                {
                    // Lisans mevcut değil
                    return new LicenseStatus
                    {
                        Exists = false
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lisans kontrolü sırasında bir hata oluştu: {ex.Message}");
                throw;
            }
        }

        public class LicenseStatus
        {
            public bool Exists { get; set; }
            public bool IsOwner { get; set; }
        }






        public static async Task<bool> UpdateBanStatusByIdAsync(int id, bool banStatus)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>("LicenceSystem");
                var filter = Builders<BsonDocument>.Filter.Eq("ID", id);
                var update = Builders<BsonDocument>.Update.Set("Banned", banStatus);
                var result = await collection.UpdateOneAsync(filter, update);

                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ban durumu güncellenirken bir hata oluştu: {ex.Message}");
                return false;
            }
        }




        public static async Task<BsonDocument> GetLicenseDocumentByIdAsync(int id)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>("LicenceSystem");
                var filter = Builders<BsonDocument>.Filter.Eq("ID", id);
                var document = await collection.Find(filter).FirstOrDefaultAsync();

                return document;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Belge aranırken bir hata oluştu: {ex.Message}");
                return null;
            }
        }



        public static async Task<List<BsonDocument>> GetAllLicensesAsync()
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>("LicenceSystem");
                var documents = await collection.Find(new BsonDocument()).ToListAsync();
                return documents;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lisanslar getirilirken bir hata oluştu: {ex.Message}");
                return new List<BsonDocument>();
            }
        }


        public static async Task<bool> AddLicenseAsync(int id, string licenseKey, bool isOwner, bool isBanned)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>("LicenceSystem");

                var licenseDocument = new BsonDocument
                {
                    { "ID", id },
                    { "Licence", licenseKey },
                    { "Owner", isOwner },
                    { "Banned", isBanned }
                };

                await collection.InsertOneAsync(licenseDocument);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lisans eklenirken bir hata oluştu: {ex.Message}");
                return false;
            }
        }


        public static async Task<bool> DeleteLicenseByIdAsync(int id)
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>("LicenceSystem");

                var filter = Builders<BsonDocument>.Filter.Eq("ID", id);
                var result = await collection.DeleteOneAsync(filter);

                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lisans silinirken bir hata oluştu: {ex.Message}");
                return false;
            }
        }


        public static async Task<int> GetNextAvailableIdAsync()
        {
            try
            {
                var collection = _database.GetCollection<BsonDocument>("LicenceSystem");

                // En yüksek ID'yi bulmak için sort ve limit işlemi yapıyoruz
                var sort = Builders<BsonDocument>.Sort.Descending("ID");
                var filter = new BsonDocument();

                var maxIdDocument = await collection
                    .Find(filter)
                    .Sort(sort)
                    .Limit(1)
                    .FirstOrDefaultAsync();

                int newId = 0; // Varsayılan olarak 0 kullanıyoruz
                if (maxIdDocument != null && maxIdDocument.Contains("ID"))
                {
                    newId = maxIdDocument["ID"].AsInt32 + 1;
                }

                return newId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ID alınırken bir hata oluştu: {ex.Message}");
                return -1; // Hata durumunda -1 döndürmek, geçersiz bir ID olduğunu belirtir
            }
        }

        public static async Task<LicenseStatus> CheckIDLicenseAsync(int id)
        {
            var collection = _database.GetCollection<BsonDocument>("LicenceSystem");
            var filter = Builders<BsonDocument>.Filter.Eq("ID", id);
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                bool isBanned = result["Banned"].AsBoolean;
                bool isOwner = result["Owner"].AsBoolean;

                if (isBanned)
                {
                    //MessageBox.Show("Bu ID yasaklı.");
                    throw new Exception("Bu ID yasaklı.");
                }

                return new LicenseStatus
                {
                    Exists = true,
                    IsOwner = isOwner
                };
            }

            return new LicenseStatus { Exists = false };

        }

        public static async Task<LicenseStatus> CheckIDOwnerAsync(int id)
        {
            var collection = _database.GetCollection<BsonDocument>("LicenceSystem");
            var filter = Builders<BsonDocument>.Filter.Eq("ID", id);
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                bool isBanned = result["Banned"].AsBoolean;
                bool isOwner = result["Owner"].AsBoolean;

                if (isOwner)
                {
                    //MessageBox.Show("Bu ID yasaklı.");
                    throw new Exception("Bu ID yönetici.");
                }

                return new LicenseStatus
                {
                    Exists = true,
                    IsOwner = isOwner
                };
            }

            return new LicenseStatus { Exists = false };

        }


        //
        public static async Task SaveFligranValueAsync(string value)
        {
            var collection = _database.GetCollection<BsonDocument>("Fligran");

            // Mevcut değer varsa sil
            await collection.DeleteManyAsync(new BsonDocument());

            // Yeni değeri ekle
            var document = new BsonDocument { { "valueField", value } };
            await collection.InsertOneAsync(document);
        }

        public static async Task DeleteFligranAsync()
        {
            var collection = _database.GetCollection<BsonDocument>("Fligran");
            await collection.DeleteManyAsync(new BsonDocument());
        }

        public static async Task<string> GetFligranValueAsync()
        {
            var collection = _database.GetCollection<BsonDocument>("Fligran");
            var document = await collection.Find(new BsonDocument()).FirstOrDefaultAsync();

            return document != null && document.Contains("valueField") ? document["valueField"].AsString : string.Empty;
        }
        //

        //
        public static async Task SaveFligranValueAsync2(string value)
        {
            var collection = _database.GetCollection<BsonDocument>("BotName");

            // Mevcut değer varsa sil
            await collection.DeleteManyAsync(new BsonDocument());

            // Yeni değeri ekle
            var document = new BsonDocument { { "valueField", value } };
            await collection.InsertOneAsync(document);
        }

        public static async Task DeleteFligranAsync2()
        {
            var collection = _database.GetCollection<BsonDocument>("BotName");
            await collection.DeleteManyAsync(new BsonDocument());
        }

        public static async Task<string> GetFligranValueAsync2()
        {
            var collection = _database.GetCollection<BsonDocument>("BotName");
            var document = await collection.Find(new BsonDocument()).FirstOrDefaultAsync();

            return document != null && document.Contains("valueField") ? document["valueField"].AsString : string.Empty;
        }
        //

        //
        public static async Task SaveFligranValueAsync3(string value)
        {
            var collection = _database.GetCollection<BsonDocument>("BotResponseDefault");

            // Mevcut değer varsa sil
            await collection.DeleteManyAsync(new BsonDocument());

            // Yeni değeri ekle
            var document = new BsonDocument { { "valueField", value } };
            await collection.InsertOneAsync(document);
        }

        public static async Task DeleteFligranAsync3()
        {
            var collection = _database.GetCollection<BsonDocument>("BotResponseDefault");
            await collection.DeleteManyAsync(new BsonDocument());
        }

        public static async Task<string> GetFligranValueAsync3()
        {
            var collection = _database.GetCollection<BsonDocument>("BotResponseDefault");
            var document = await collection.Find(new BsonDocument()).FirstOrDefaultAsync();

            return document != null && document.Contains("valueField") ? document["valueField"].AsString : string.Empty;
        }
        //



    }
}