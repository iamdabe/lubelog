﻿using CarCareTracker.External.Interfaces;
using CarCareTracker.Models;
using LiteDB;

namespace CarCareTracker.External.Implementations
{
    public class TokenRecordDataAccess : ITokenRecordDataAccess
    {
        private LiteDatabase db { get; set; }
        private static string tableName = "tokenrecords";
        public TokenRecordDataAccess(ILiteDBInjection liteDB)
        {
            db = liteDB.GetLiteDB();
        }
        public List<Token> GetTokens()
        {
            var table = db.GetCollection<Token>(tableName);
            return table.FindAll().ToList();
        }
        public Token GetTokenRecordByBody(string tokenBody)
        {
            var table = db.GetCollection<Token>(tableName);
            var tokenRecord = table.FindOne(Query.EQ(nameof(Token.Body), tokenBody));
            return tokenRecord ?? new Token();
        }
        public Token GetTokenRecordByEmailAddress(string emailAddress)
        {
            var table = db.GetCollection<Token>(tableName);
            var tokenRecord = table.FindOne(Query.EQ(nameof(Token.EmailAddress), emailAddress));
            return tokenRecord ?? new Token();
        }
        public bool CreateNewToken(Token token)
        {
            var table = db.GetCollection<Token>(tableName);
            table.Insert(token);
            return true;
        }
        public bool DeleteToken(int tokenId)
        {
            var table = db.GetCollection<Token>(tableName);
            table.Delete(tokenId);
            return true;
        }
    }
}