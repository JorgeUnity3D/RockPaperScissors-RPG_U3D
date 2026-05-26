using System;
using System.IO;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Intercepta Application.logMessageReceived y vuelca cada entrada a un archivo de texto en RPSLogs/.
	/// Se registra antes que cualquier manager (execution order -9989) para capturar logs desde el inicio de sesión.
	/// </summary>
	public class LoggerService : ServiceSubscriber<LoggerService>
	{
		private StreamWriter _writer;
		private bool         _footerWritten;

		// ── Unity lifecycle ────────────────────────────────────────────────────

		protected override void Awake()
		{
			base.Awake();
			OpenLogFile();
			Application.logMessageReceived += OnLogReceived;
		}

		private void OnApplicationQuit()
		{
			WriteFooter();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Application.logMessageReceived -= OnLogReceived;
			WriteFooter();
			_writer?.Close();
			_writer = null;
		}

		// ── File management ────────────────────────────────────────────────────

		private void OpenLogFile()
		{
			try
			{
				string dir      = GetLogDirectory();
				Directory.CreateDirectory(dir);
				string filename = $"rpslog-{DateTime.Now:yyyyMMdd-HHmm}.txt";
				string path     = Path.Combine(dir, filename);
				_writer         = new StreamWriter(path, append: false, System.Text.Encoding.UTF8) { AutoFlush = true };
				WriteHeader();
			}
			catch (Exception e)
			{
				Debug.LogWarning($"[LoggerService] Could not create log file: {e.Message}");
			}
		}

		private static string GetLogDirectory()
		{
#if UNITY_EDITOR
			string projectRoot = Path.GetDirectoryName(Application.dataPath) ?? Application.persistentDataPath;
			return Path.Combine(projectRoot, "RPSLogs");
#else
			return Path.Combine(Application.persistentDataPath, "RPSLogs");
#endif
		}

		// ── Header / footer ────────────────────────────────────────────────────

		private void WriteHeader()
		{
			const int W   = 63;
			string    bar = new string('═', W);

			_writer.WriteLine($"╔{bar}╗");
			_writer.WriteLine(BoxLine(Center("ROCK PAPER SCISSORS RPG  —  Session Log", W), W));
			_writer.WriteLine($"╠{bar}╣");
			_writer.WriteLine(BoxLine($"  Date     : {DateTime.Now:yyyy-MM-dd  HH:mm:ss}", W));
			_writer.WriteLine(BoxLine($"  Platform : {Application.platform}", W));
			_writer.WriteLine(BoxLine($"  Unity    : {Application.unityVersion}", W));
			_writer.WriteLine($"╚{bar}╝");
			_writer.WriteLine();
		}

		private void WriteFooter()
		{
			if (_writer == null || _footerWritten) return;
			_footerWritten = true;

			const int    W   = 65;
			const string sep = "─────────────────────────────────────────────────────────────────";
			_writer.WriteLine();
			_writer.WriteLine(sep.Substring(0, W));
			_writer.WriteLine($"  Session ended : {DateTime.Now:yyyy-MM-dd  HH:mm:ss}");
			_writer.WriteLine(sep.Substring(0, W));
		}

		private static string BoxLine(string content, int innerWidth) =>
			"║" + content.PadRight(innerWidth) + "║";

		private static string Center(string text, int innerWidth)
		{
			int padding = Math.Max(0, (innerWidth - text.Length) / 2);
			return text.PadLeft(padding + text.Length).PadRight(innerWidth);
		}

		// ── Log handler ────────────────────────────────────────────────────────

		private void OnLogReceived(string condition, string stackTrace, LogType type)
		{
			if (_writer == null) return;

			string tag = type switch
			{
				LogType.Log       => "[LOG  ]",
				LogType.Warning   => "[WARN ]",
				LogType.Error     => "[ERROR]",
				LogType.Exception => "[EXCP ]",
				LogType.Assert    => "[ASRT ]",
				_                 => "[?????]",
			};

			_writer.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {tag} {condition}");

			bool includeTrace = type is LogType.Error or LogType.Exception or LogType.Assert;
			if (includeTrace && !string.IsNullOrWhiteSpace(stackTrace))
			{
				foreach (string raw in stackTrace.Split('\n'))
				{
					string line = raw.Trim();
					if (!string.IsNullOrEmpty(line))
						_writer.WriteLine($"               → {line}");
				}
			}
		}
	}
}
