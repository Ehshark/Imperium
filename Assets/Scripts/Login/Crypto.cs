using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

public class Crypto
{
    // hashedPassword must be of the format of HashWithPassword (salt + Hash(salt+input)
    public bool VerifyHashedPassword(string hashedPassword, string password)
    {
        const int PBKDF2IterCount = 1000; // default for Rfc2898DeriveBytes
        const int PBKDF2SubkeyLength = 256 / 8; // 256 bits
        const int SaltSize = 128 / 8; // 128 bits

        if (hashedPassword == null)
        {
            throw new ArgumentNullException("hashedPassword");
        }
        if (password == null)
        {
            throw new ArgumentNullException("password");
        }

        byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

        // Verify a version 0 (see comment above) password hash.

        if (hashedPasswordBytes.Length != (1 + SaltSize + PBKDF2SubkeyLength) || hashedPasswordBytes[0] != 0x00)
        {
            // Wrong length or version header.
            return false;
        }

        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
        byte[] storedSubkey = new byte[PBKDF2SubkeyLength];
        Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, PBKDF2SubkeyLength);

        byte[] generatedSubkey;
        using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, PBKDF2IterCount))
        {
            generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
        }
        return ByteArraysEqual(storedSubkey, generatedSubkey);
    }

    // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
    [MethodImpl(MethodImplOptions.NoOptimization)]
    private bool ByteArraysEqual(byte[] a, byte[] b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a == null || b == null || a.Length != b.Length)
        {
            return false;
        }

        bool areSame = true;
        for (int i = 0; i < a.Length; i++)
        {
            areSame &= (a[i] == b[i]);
        }
        return areSame;
    }
}
