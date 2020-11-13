using Microsoft.AspNetCore.Identity;
using System;

namespace AuthServer.PasswordHasher
{
    public class BCryptPasswordHasher<TUser> : PasswordHasher<TUser> where TUser : class
    {
        #region Properties

        public override string HashPassword(TUser user, string password) => BCrypt.Net.BCrypt.HashPassword(password);

        #endregion

        #region Logic

        public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null) { throw new ArgumentNullException(nameof(hashedPassword)); }
            if (providedPassword == null) { throw new ArgumentNullException(nameof(providedPassword)); }

            if (BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword))
            {
                return PasswordVerificationResult.Success;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }

        #endregion
    }
}