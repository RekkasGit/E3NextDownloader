using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using E3NextLauncher.util;
using Ionic.Zip;
using Microsoft.WindowsAPICodePack.Dialogs;
using Octokit;

namespace E3NextDownloader
{
	public partial class Downloader : Form
	{
		string _versionNumber = "0.1";
		string _extractionPath = "";
		Task _downloadTask = null;
		static byte[] _e3nImageBytes;
		static Image _e3nImage;
		public Downloader()
		{
			InitializeComponent();
		}

		private void buttonSelectFolder_Click(object sender, EventArgs e)
		{
			var dialog = new CommonOpenFileDialog();
			dialog.IsFolderPicker = true;
			
			using(new CenterWinDialog(this))
			{
				CommonFileDialogResult result = dialog.ShowDialog();
				if (result == CommonFileDialogResult.Ok)
				{
					_extractionPath = dialog.FileName;
					textBox_DownloadLocation.Text = _extractionPath;
				}
			}
		}

		private void DownloadFile(string url, string filename, string topic)
		{
			using (var client = new HttpClientDownloadWithProgress(url, filename))
			{
				client.ProgressChanged += (totalFileSize, totalBytesDownloaded, progressPercentage) =>
				{
					//need to update the UI, but on their thread, so need to invoke
					this.labelStatus.Invoke((MethodInvoker)delegate
					{
						this.labelStatus.Text = $"Downloading {topic} {Math.Round(((Decimal)totalBytesDownloaded) / 1024 / 1024, 2)} Megabytes Downloaded";
					});
				};
				client.StartDownload().Wait();
			}
		}

		private bool FilesInFolder(string path)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(path);
			if(dirInfo.Exists ) {

				var files = dirInfo.GetFiles();

				if(files.Length > 0 )
				{
					return true;
				}
			}

			return false;
		}
		private bool EQGameInFolder(string path)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(path);
			if (dirInfo.Exists)
			{

				var files = dirInfo.GetFiles();

				if (files.Length > 0)
				{
				
					foreach(var file in files)
					{
						if (String.Compare(file.Name,"eqgame.exe", true)==0)
						{
							return true;
						}
					}
				
				}
			}

			return false;
		}
		private void DownloadEMU()
		{

			

			string downloadLocation = $@"{_extractionPath}\download.zip";
			string downloadURL = "https://github.com/RekkasGit/E3NextAndMQNextBinary/archive/refs/heads/main.zip";

			DownloadFile(downloadURL, downloadLocation," E3Next+MQ ");

			this.labelStatus.Invoke((MethodInvoker)delegate
			{
				this.labelStatus.Text = $"Done Downloading!";
			});
			System.Threading.Thread.Sleep(1000);

			using (ZipFile zip = ZipFile.Read(downloadLocation))
			{
				var result = zip.Any(entry => entry.FileName.Contains("E3NextAndMQNextBinary-main"));
				if (!result)
				{
					//some type of error
					this.labelStatus.Invoke((MethodInvoker)delegate
					{
						this.labelStatus.Text = $"Error could not find Sub folder in zip file.";
					});
					return;
				}
				var selection = (from e in zip.Entries
								 where (e.FileName).StartsWith("E3NextAndMQNextBinary-main/")

								 select e);
				this.labelStatus.Invoke((MethodInvoker)delegate
				{
					this.labelStatus.Text = $"Extracting files....";
				});
				//we don't want the github generated name, so we will just strip it out
				
				//selection count is updated each time we extract, so we  only increment when we are going to skip one
				for (Int32 i = 0; i < selection.Count();)
				{
					var e = selection.ElementAt(i);
					if (e.FileName == "E3NextAndMQNextBinary-main/")
					{
						i++;
						continue;
					}
					e.FileName = e.FileName.Replace("E3NextAndMQNextBinary-main/", "");
					this.labelStatus.Invoke((MethodInvoker)delegate
					{
						this.labelStatus.Text = $"Extracting {e.FileName}";
					});
					if(e.FileName.IndexOf("config/",0,StringComparison.OrdinalIgnoreCase)>-1)
					{
						e.Extract(_extractionPath, ExtractExistingFileAction.DoNotOverwrite);
					}
					else
					{
						e.Extract(_extractionPath, ExtractExistingFileAction.OverwriteSilently);
					}
				}
			}

			try
			{
				File.Delete(downloadLocation);
			}
			catch (Exception)
			{

			}
			this.labelStatus.Invoke((MethodInvoker)delegate
			{
				this.labelStatus.Text = $"Done extracting!";
			});
			System.Threading.Thread.Sleep(1000);
			this.labelStatus.Invoke((MethodInvoker)delegate
			{
				this.labelStatus.Text = $"";
			});
			this.buttonDownload.Invoke((MethodInvoker)delegate
			{
				this.buttonDownload.Enabled = true;
			});
			

		}

		
		private void DownloadLive()
		{

			if(String.IsNullOrWhiteSpace(_extractionPath))
			{
				return;
			}
			if (!DownloadMQLive()) return;
			if (!DownloadMQ2MonoLive()) return;
			if (!DownloadMonoFrameworkLive()) return;
			if (!DownloadE3Next()) return;

			//give time for the user to read the message
			System.Threading.Thread.Sleep(1000);
			this.labelStatus.Invoke((MethodInvoker)delegate
			{
				this.labelStatus.Text = $"";
			});
			this.buttonDownload.Invoke((MethodInvoker)delegate
			{
				this.buttonDownload.Enabled = true;
			});
		}
		private bool DownloadMQLive()
		{
			/*
			 * Download Macroquest Live
			 */

			GitHubClient gitclient = new GitHubClient(new ProductHeaderValue("E3NextDownloader"));
			var releases = gitclient.Repository.Release.GetAll("macroquest", "macroquest");
			releases.Wait();
			var latest = releases.Result.Where(x => x.TagName == "rel-live").First();
			if (latest == null)
			{
				this.labelStatus.Invoke((MethodInvoker)delegate
				{
					this.labelStatus.Text = $"Issue finding Live tag for MQ";
				});
				return false;
			}
			var latestID = latest.Id;
			var assets = gitclient.Repository.Release.GetAllAssets("macroquest", "macroquest", latestID).Result;
			var zipFile = assets[0];


			string downloadLocation = $@"{_extractionPath}\MQDownload.zip";
			string downloadURL = zipFile.BrowserDownloadUrl;

			DownloadFile(downloadURL, downloadLocation, " MacroQuest (Live)");
			this.labelStatus.Invoke((MethodInvoker)delegate
			{
				this.labelStatus.Text = $"Done Downloading!";
			});
			System.Threading.Thread.Sleep(1000);
			using (ZipFile zip = ZipFile.Read(downloadLocation))
			{

				this.labelStatus.Invoke((MethodInvoker)delegate
				{
					this.labelStatus.Text = $"Extracting files....";
				});
				//we don't want the github generated name, so we will just strip it out

				foreach (var e in zip.Entries)
				{
					this.labelStatus.Invoke((MethodInvoker)delegate
					{
						this.labelStatus.Text = $"Extracting {e.FileName}";
					});
					e.Extract(_extractionPath, ExtractExistingFileAction.OverwriteSilently);
				}


			}
			try { File.Delete(downloadLocation); } catch (Exception) { }

			this.labelStatus.Invoke((MethodInvoker)delegate
			{
				this.labelStatus.Text = $"Done extracting!";
			});
			return true;
		}
		private bool DownloadMQ2MonoLive()
		{
			/*
			 * Download MQ2Mono Live
			 */
			var gitclient = new GitHubClient(new ProductHeaderValue("E3NextDownloader"));
			var releases = gitclient.Repository.Release.GetAll("RekkasGit", "MQ2Mono");
			releases.Wait();
			var latest = releases.Result.Where(x => x.TagName == "re-live").First();
			if (latest == null)
			{
				this.labelStatus.Invoke((MethodInvoker)delegate
				{
					this.labelStatus.Text = $"Issue finding Live tag for MQ2Mono";
				});
				return false;
			}
			var latestID = latest.Id;
			var assets = gitclient.Repository.Release.GetAllAssets("RekkasGit", "MQ2Mono", latestID).Result;
			var zipFile = assets[0];


			var downloadLocation = $@"{_extractionPath}\MQ2MonoDownload.zip";
			var downloadURL = zipFile.BrowserDownloadUrl;

			DownloadFile(downloadURL, downloadLocation, "MQ2Mono.");

			using (ZipFile zip = ZipFile.Read(downloadLocation))
			{

				this.labelStatus.Invoke((MethodInvoker)delegate
				{
					this.labelStatus.Text = $"Extracting files....";
				});
				//we don't want the github generated name, so we will just strip it out

				foreach (var e in zip.Entries)
				{
					this.labelStatus.Invoke((MethodInvoker)delegate
					{
						this.labelStatus.Text = $"Extracting {e.FileName}";
					});
					e.Extract(_extractionPath + @"\plugins\", ExtractExistingFileAction.OverwriteSilently);
				}


			}
			try { File.Delete(downloadLocation); } catch (Exception) { }
			return true;
		}
		private bool DownloadMonoFrameworkLive()
		{
			/*
			 * Download Mono Framework x64
			 */
			if (checkBoxDownloadFramework.Checked)
			{
				var downloadLocation = $@"{_extractionPath}\MonoFrameworkx64Download.zip";
				var downloadURL = @"https://github.com/RekkasGit/MQ2Mono-Framework64/archive/refs/heads/main.zip";

				DownloadFile(downloadURL, downloadLocation, "Mono Framework x64.");

				using (ZipFile zip = ZipFile.Read(downloadLocation))
				{

					this.labelStatus.Invoke((MethodInvoker)delegate
					{
						this.labelStatus.Text = $"Extracting files....";
					});
					//we don't want the github generated name, so we will just strip it out
					var selection = (from e in zip.Entries
									 where (e.FileName).StartsWith("MQ2Mono-Framework64-main/")

									 select e);
					for (Int32 i = 0; i < selection.Count();)
					{
						var e = selection.ElementAt(i);
						if (e.FileName == "MQ2Mono-Framework64-main/" || e.FileName == "MQ2Mono-Framework64-main/README.md")
						{
							i++;
							continue;
						}
						e.FileName = e.FileName.Replace("MQ2Mono-Framework64-main/", "");
						this.labelStatus.Invoke((MethodInvoker)delegate
						{
							this.labelStatus.Text = $"Extracting {e.FileName}";
						});
						e.Extract(_extractionPath, ExtractExistingFileAction.OverwriteSilently);
					}


				}
				try { File.Delete(downloadLocation); } catch (Exception) { }
			}
			return true;
		}
		private bool DownloadE3Next()
		{
			/*
			 * Download the E3Next and Config folder
			 */

			var downloadLocation = $@"{_extractionPath}\E3NextDownload.zip";
			var downloadURL = String.Empty;

			var gitclient = new GitHubClient(new ProductHeaderValue("E3NextDownloader"));
			var releases = gitclient.Repository.Release.GetAll("RekkasGit", "E3Next");
			releases.Wait();
			var latest = releases.Result.First();
			var latestID = latest.Id;
			var assets = gitclient.Repository.Release.GetAllAssets("RekkasGit", "E3Next", latestID).Result;

			foreach (var asset in assets)
			{
				if (asset.Name == "e3.zip")
				{
					downloadURL = asset.BrowserDownloadUrl;
					DownloadFile(downloadURL, downloadLocation, "E3Next");

				}
			}
			if (String.IsNullOrEmpty(downloadURL))
			{
				this.labelStatus.Invoke((MethodInvoker)delegate
				{
					this.labelStatus.Text = $"Issue downloading E3Next";
				});
				this.buttonDownload.Invoke((MethodInvoker)delegate
				{
					this.buttonDownload.Enabled = true;
				});
				return false;
			}
			using (ZipFile zip = ZipFile.Read(downloadLocation))
			{

				this.labelStatus.Invoke((MethodInvoker)delegate
				{
					this.labelStatus.Text = $"Extracting files....";
				});
				//we don't want the github generated name, so we will just strip it out

				foreach (var e in zip.Entries.ToList())
				{
					this.labelStatus.Invoke((MethodInvoker)delegate
					{
						this.labelStatus.Text = $"Extracting {e.FileName}";
					});
					if (e.FileName == @"e3/config.zip")
					{
						e.FileName = "config.zip";
						e.Extract(_extractionPath);
					}
					else
					{
						e.Extract(_extractionPath + @"\mono\macros\", ExtractExistingFileAction.OverwriteSilently);

					}
				}
				
			}
			using (ZipFile zip = ZipFile.Read(_extractionPath + @"\config.zip"))
			{

				this.labelStatus.Invoke((MethodInvoker)delegate
				{
					this.labelStatus.Text = $"Extracting files....";
				});
				//we don't want the github generated name, so we will just strip it out

				foreach (var e in zip.Entries)
				{

					e.Extract(_extractionPath, ExtractExistingFileAction.DoNotOverwrite);


				}

			}
			try { File.Delete(_extractionPath + @"\config.zip"); } catch (Exception) { }
			try { File.Delete(downloadLocation); } catch (Exception) { }
			return true;
		}
		private void buttonDownload_Click(object sender, EventArgs e)
		{
		
			if (String.IsNullOrWhiteSpace(_extractionPath))
			{
				using(new CenterWinDialog(this))
				{
					MessageBox.Show("Please select a folder.");
				}
				
				return;
			}
			
			if(EQGameInFolder(_extractionPath))
			{
				using (new CenterWinDialog(this))
				{
					MessageBox.Show("Do not chose your EQ game folder. Please select another folder");
				}
				return;
			}

			if (FilesInFolder(_extractionPath) )
			{
				using (new CenterWinDialog(this))
				{
					if (MessageBox.Show(this, "Files exist in this directory do you wish to overwrite them? Generally better to go to a new folder.", "Overwrite files?", MessageBoxButtons.OKCancel) != DialogResult.OK)
					{
						return;
					}
				}
			}


			buttonDownload.Enabled = false;
			if (radioButtonEMU.Checked)
			{
				_downloadTask = Task.Run(() => DownloadEMU());
				return;
			}
			else if(radioButtonLive.Checked)
			{
				_downloadTask = Task.Run(() => DownloadLive());
				return;
			}
			buttonDownload.Enabled = true;
		}

		private void Downloader_Load(object sender, EventArgs e)
		{
			try
			{
				using (var filestream = File.OpenRead("E3Next.png"))
				{
					_e3nImageBytes = new byte[filestream.Length];
					filestream.Read(_e3nImageBytes, 0, (Int32)filestream.Length);
					//do stuff 
				}

				using (var stream = new MemoryStream(_e3nImageBytes))
				{
					_e3nImage = Image.FromStream(stream);
				}
				pictureBoxE3N.Image = _e3nImage;
			}
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.Message);
			}
		}
	}
	/// <summary>
	/// Code that can center any dialog/form/etch to the parent
	/// </summary>
	class CenterWinDialog : IDisposable
	{
		private int mTries = 0;
		private Form mOwner;

		public CenterWinDialog(Form owner)
		{
			mOwner = owner;
			owner.BeginInvoke(new MethodInvoker(findDialog));
		}

		private void findDialog()
		{
			// Enumerate windows to find the message box
			if (mTries < 0) return;
			EnumThreadWndProc callback = new EnumThreadWndProc(checkWindow);
			if (EnumThreadWindows(GetCurrentThreadId(), callback, IntPtr.Zero))
			{
				if (++mTries < 10) mOwner.BeginInvoke(new MethodInvoker(findDialog));
			}
		}
		private bool checkWindow(IntPtr hWnd, IntPtr lp)
		{
			// Checks if <hWnd> is a dialog
			StringBuilder sb = new StringBuilder(260);
			GetClassName(hWnd, sb, sb.Capacity);
			if (sb.ToString() != "#32770") return true;
			// Got it
			Rectangle frmRect = new Rectangle(mOwner.Location, mOwner.Size);
			RECT dlgRect;
			GetWindowRect(hWnd, out dlgRect);
			MoveWindow(hWnd,
				frmRect.Left + (frmRect.Width - dlgRect.Right + dlgRect.Left) / 2,
				frmRect.Top + (frmRect.Height - dlgRect.Bottom + dlgRect.Top) / 2,
				dlgRect.Right - dlgRect.Left,
				dlgRect.Bottom - dlgRect.Top, true);
			return false;
		}
		public void Dispose()
		{
			mTries = -1;
		}

		// P/Invoke declarations
		private delegate bool EnumThreadWndProc(IntPtr hWnd, IntPtr lp);
		[DllImport("user32.dll")]
		private static extern bool EnumThreadWindows(int tid, EnumThreadWndProc callback, IntPtr lp);
		[DllImport("kernel32.dll")]
		private static extern int GetCurrentThreadId();
		[DllImport("user32.dll")]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder buffer, int buflen);
		[DllImport("user32.dll")]
		private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
		[DllImport("user32.dll")]
		private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);
		private struct RECT { public int Left; public int Top; public int Right; public int Bottom; }
	}
}
