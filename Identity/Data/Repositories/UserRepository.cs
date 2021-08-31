using System.Collections.Generic;
using System.Linq;
using System;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Dapper;

namespace Identity.Data.Repositories
{
    public class UserRepository : BaseRepository
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRepository(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public void Save(User user)
        {
            if (string.IsNullOrEmpty(user.Id))
                user.Id = Guid.NewGuid().ToString();

            var old = FindBySubjectId(user.Id);
            if (old == null)
            {
                user.Active = true;
                InsertSql("User", "Id, UserName, FullName, Email, Active", user);
            }
            else
            {
                UpdateSql("User", "UserName, FullName, Email, Active", user, "Id = @Id");
            }

            user.Roles?.ToList().ForEach(role => role.UserId = user.Id);
            
            if (!string.IsNullOrWhiteSpace(user.Password))
                UpdatePassword(user, user.Password);
        }

        public void UpdatePassword(User user, string password)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            user.SecurityStamp = Guid.NewGuid().ToString();
            UpdateSql("User", "PasswordHash, SecurityStamp", user, "Id = @Id");
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
            }
            return false;
        }

        public User FindBySubjectId(string subjectId)
        {
            using (var db = GetConn())
            {
                return db.QueryFirstOrDefault<User>("SELECT * FROM User WHERE Id = @subjectId", new { subjectId });
            }
        }

        public User FindByExternalProvider(string provider, string userId)
        {
            using (var db = GetConn())
            {
                return db.QueryFirstOrDefault<User>("SELECT * FROM User WHERE Id = @userId", new { userId });
            }
        }

        public User FindByUsername(string username)
        {
            using (var db = GetConn())
            {
                return db.QueryFirstOrDefault<User>("SELECT * FROM User WHERE Username = @username", new { username });
            }
        }

        public IEnumerable<User> GetAll()
        {
            using (var db = GetConn())
            {
                return db.Query<User>("SELECT * FROM User");
            }
        }
    }
}