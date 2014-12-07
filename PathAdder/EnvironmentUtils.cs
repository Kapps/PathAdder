using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathAdder {
    /// <summary>
    /// Helper class for managing environment variables.
    /// </summary>
    public static class EnvironmentUtils {
		/// <summary>
		/// Appends the given path to the environment machine variable.
		/// The path must be valid, according to the rules of <seealso cref="EnsureValidEnvironmentPath"/>.
		/// </summary>
		public static void AppendPath(string AppendPath) {
			EnsureValidEnvironmentPath(AppendPath);
			var ExistingPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
			var NewPath = ExistingPath + ";" + AppendPath;
			Environment.SetEnvironmentVariable("PATH", NewPath, EnvironmentVariableTarget.Machine);
		}

		/// <summary>
		/// Checks whether the given path is a valid path that may be added to the environment path variable.
		/// The path must be a valid directory and must exist.
		/// If the path is invalid or does not exist, an exception is thrown.
		/// If the path is already present in the machine environment variable, an exception is thrown.
		/// </summary>
		/// <param name="EnvPath"></param>
		public static void EnsureValidEnvironmentPath(string EnvPath) {
			var InvalidChars = Path.GetInvalidPathChars();
			if(String.IsNullOrWhiteSpace(EnvPath))
				throw new ArgumentException("No path specified.");
			if(EnvPath.Any(c => InvalidChars.Contains(c)))
				throw new ArgumentException("Path contains invalid characters in the name.");
			if(!Directory.Exists(EnvPath))
				throw new ArgumentException("Path does not point to a valid directory.");
			var ExistingPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
			if(ExistingPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Any(c => ArePathsEqual(c, EnvPath)))
				throw new ArgumentException("Path already exists in environment variable.");
		}

		private static string NormalizePath(string path) {
			// Courtesy of http://stackoverflow.com/questions/1266674/how-can-one-get-an-absolute-or-normalized-file-path-in-net/21058121#21058121
			return Path.GetFullPath(new Uri(path).LocalPath)
			   .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
			   .ToUpperInvariant();
		}

		private static bool ArePathsEqual(string p1, string p2) {
			// Don't crash because of an invalid environment variable.
			try {
				return NormalizePath(p1) == NormalizePath(p2);
			} catch {
				return false;
			}
		}
    }
}
