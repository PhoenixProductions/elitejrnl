using System;
using System.IO;
namespace jrnl
{
	public abstract class JrnlCommand : ManyConsole.ConsoleCommand
	{
		public JrnlCommand() 
		{
		}
		public System.Collections.ArrayList ReadDirectoryFiles(string dir, string extension)
		{
			dir = Path.GetFullPath(dir);
			Console.WriteLine(dir);
			System.Collections.ArrayList files = new System.Collections.ArrayList();
			if (Directory.Exists(dir))
			{
				var dirinfo = new DirectoryInfo(dir);
				foreach (var e in dirinfo.GetFileSystemInfos())
				{
					if (e is FileInfo)
					{
						var ext = Path.GetExtension(((FileInfo)e).FullName);
						if (ext.ToLower() == extension.ToLower())
						{
							Console.Error.WriteLine(string.Format("Adding {0} to list", ((FileInfo)e).FullName));
							files.Add(((FileInfo)e).FullName);
						}
					}
				}
			}
			return files;
		}
	}
}
