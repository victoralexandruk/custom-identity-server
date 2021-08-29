using System.Collections.Generic;
using System.Linq;
using System;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Data.Repositories
{
    public class UserRepository
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        // Replce this with your User persistence.
        private readonly List<User> _users = MemoryData.Users;

        public UserRepository(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public void Create(User user, string password)
        {
            user.Id = Guid.NewGuid().ToString();
            user.Active = true;
            _users.Add(user);
            user.Roles.ToList().ForEach(role => role.UserId = user.Id);
            UpdatePassword(user, password);
        }

        public void UpdatePassword(User user, string password)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            user.SecurityStamp = Guid.NewGuid().ToString();
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
            return _users.FirstOrDefault(x => x.SubjectId == subjectId);
        }

        public User FindByExternalProvider(string provider, string userId)
        {
            return _users.FirstOrDefault(x => x.Id == userId);
        }

        public User FindByUsername(string username)
        {
            return _users.FirstOrDefault(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }
    }
}