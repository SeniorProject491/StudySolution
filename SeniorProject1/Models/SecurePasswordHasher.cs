using System.Security.Cryptography;
using System;

namespace SeniorProject1.Models
{
    public class SecurePasswordHasher
    {

        /*
        size of the salt
        */
        private const int SaltSize = 16;

        /*
        Size of Hash
        */
        private const int HashSize = 20;

        /*
        create a secure hash from a password
        parameters
          password: a string of user's password to be hashed
          iterations: an int indiciating the number of iterations
        Returns: a string containing the hash of a password
        */
        public static string Hash(string password, int iterations)
        {
            //A salt is a non-secret, unique value in the database which is appended (depending on the used algorithm) to the password before it gets hashed.
            //create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            //create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            //combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            //convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            //format hash with extra information
            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }

        /*
        create a hash of a password by sending the password to the above function
        and using 1000 iterations
        Parameters
          password: a string of user's password to be hashed
        return: a string containing the hash of a password as created in above function
        */
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        /*
        Checks if hash contains supported hashcode
        Parameters
          hashString: a string containing the hash of a password
        return boolean indicating whether or not the hash is supported
        */
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$MYHASH$V1$");
        }


        /*
        verify password
        Parameters
          password: a string containing a non-hashed password entry
          hashedPassword: a string containing the hashed password
        return boolean indicating whether or not the password is valid
        */
        public static bool Verify(string password, string hashedPassword)
        {
            //check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            //extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            //get hashbytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            //get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            //get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
