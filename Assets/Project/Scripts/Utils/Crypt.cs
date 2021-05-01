using System;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
	public enum HashAlgorithmType
	{
		MD5 = 0,
		SHA1 = 1,
		SHA256 = 2,
		SHA384 = 3,
		SHA512 = 4
	}

	public static class Crypt
	{
		private static RNGCryptoServiceProvider cryptRandom = new RNGCryptoServiceProvider();
		private static Random random = new Random();

		public static string ComputeHashWithRandomSalt(string plainText, out byte[] saltBytes, HashAlgorithmType hashAlgorithm = HashAlgorithmType.MD5)
		{
			saltBytes = GenerateSaltBytes();
			return ComputeHashWithSalt(plainText, saltBytes, hashAlgorithm);
		}

		public static string ComputeHashWithSalt(string plainText, byte[] saltBytes, HashAlgorithmType hashAlgorithm = HashAlgorithmType.MD5)
		{
			if (saltBytes == null)
				throw new NullReferenceException("Salt bytes is null");

			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			byte[] plainTextWithSaltBytes = MixBytesWithSalt(plainTextBytes, saltBytes);

			HashAlgorithm hash = SwithHashAlgorithm(hashAlgorithm);

			byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
			byte[] hashWithSaltBytes = MixBytesWithSalt(hashBytes, saltBytes);

			return Convert.ToBase64String(hashWithSaltBytes);
		}

		public static string ComputeHash(string plainText, HashAlgorithmType hashAlgorithm = HashAlgorithmType.MD5)
		{
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			HashAlgorithm hash = SwithHashAlgorithm(hashAlgorithm);
			byte[] hashBytes = hash.ComputeHash(plainTextBytes);

			return Convert.ToBase64String(hashBytes);
		}

		private static byte[] GenerateSaltBytes()
		{
			int minSaltSize = 4;
			int maxSaltSize = 8;

			int saltSize = random.Next(minSaltSize, maxSaltSize);

			var saltBytes = new byte[saltSize];
			cryptRandom.GetNonZeroBytes(saltBytes);

			return saltBytes;
		}
		private static byte[] MixBytesWithSalt(byte[] startBytes, byte[] saltBytes)
		{
			int size = startBytes.Length + saltBytes.Length;
			byte[] mixedBytes = new byte[size];

			for (int i = 0; i < startBytes.Length; i++)
			{
				mixedBytes[i] = startBytes[i];
			}

			for (int i = 0; i < saltBytes.Length; i++)
			{
				mixedBytes[i + startBytes.Length] = saltBytes[i];
			}

			return mixedBytes;
		}

		private static HashAlgorithm SwithHashAlgorithm(HashAlgorithmType hashAlgorithm)
		{
			switch (hashAlgorithm)
			{
				case HashAlgorithmType.MD5:
					return new MD5CryptoServiceProvider();

				case HashAlgorithmType.SHA1:
					return new SHA1Managed();

				case HashAlgorithmType.SHA256:
					return new SHA256Managed();

				case HashAlgorithmType.SHA384:
					return new SHA384Managed();

				case HashAlgorithmType.SHA512:
					return new SHA512Managed();

				default:
					return new MD5CryptoServiceProvider();
			}
		}
	}
}