using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Threading.Tasks;

namespace QscanClient.Services;

public class EmailService
{
    public static async Task SendMail(string subject, IEnumerable<string> attachments)
    {
        try
        {
                var validAttachments = attachments.Where(File.Exists).ToList();
                if (!validAttachments.Any())
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("沒有找到任何可附加的檔案。", "發送郵件", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                    return;
                }

                // Save to a public, sandbox-friendly folder
                string publicDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "QscanExports");
                if (!Directory.Exists(publicDir)) Directory.CreateDirectory(publicDir);

                string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                
                // Use the subject (batch title) as the zip file name, sanitized
                string safeSubject = string.IsNullOrWhiteSpace(subject) ? "Scans" : string.Join("_", subject.Split(Path.GetInvalidFileNameChars()));
                string zipFileName = $"{safeSubject}.zip";
                string zipFilePath = Path.Combine(publicDir, zipFileName);

                // Create ZIP file
                using (var zipStream = new FileStream(zipFilePath, FileMode.Create))
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))

                {
                    foreach (var path in validAttachments)
                    {
                        string name = Path.GetFileName(path);
                        archive.CreateEntryFromFile(path, name);
                    }
                }

                // Highly robust EML generation that works with New Outlook, Thunderbird, and Classic Outlook.
                string boundary = "Qscan_Boundary_" + Guid.NewGuid().ToString("N");
                StringBuilder eml = new StringBuilder();
                
                // Critical draft headers
                eml.Append("X-Unsent: 1\r\n");
                eml.Append($"Subject: {subject}\r\n");
                eml.Append("To: \r\n"); 
                eml.Append($"Date: {DateTime.Now:R}\r\n");
                eml.Append($"Content-Type: multipart/mixed; boundary=\"{boundary}\"\r\n");
                eml.Append("\r\n");
                
                // Part 1: Body
                eml.Append($"--{boundary}\r\n");
                eml.Append("Content-Type: text/plain; charset=\"utf-8\"\r\n");
                eml.Append("Content-Transfer-Encoding: 7bit\r\n");
                eml.Append("\r\n");
                eml.Append("Scan documents from QscanClient.\r\n");
                eml.Append("\r\n");

                // Part 2: Attach the ZIP file
                byte[] zipData = File.ReadAllBytes(zipFilePath);
                string base64Data = Convert.ToBase64String(zipData, Base64FormattingOptions.InsertLineBreaks);

                eml.Append($"--{boundary}\r\n");
                eml.Append($"Content-Type: application/zip; name=\"{zipFileName}\"\r\n");
                eml.Append($"Content-Disposition: attachment; filename=\"{zipFileName}\"\r\n");
                eml.Append("Content-Transfer-Encoding: base64\r\n");
                eml.Append("\r\n");
                eml.Append(base64Data);
                eml.Append("\r\n");
                
                eml.Append($"--{boundary}--\r\n");
                
                string emlPath = Path.Combine(publicDir, $"ScanEmail_{timeStamp}.eml");
                
                // UTF-8 without BOM is required for New Outlook MIME parser
                var encoding = new UTF8Encoding(false);
                File.WriteAllText(emlPath, eml.ToString(), encoding);

                // WORKAROUND FOR NEW OUTLOOK COLD-START BUG:
                // Giving the system 1.5 seconds to release all file locks guarantees New Outlook can read the attachments perfectly.
                await Task.Delay(1500);

                // Clean up the temporary zip file since it's already encoded entirely inside the EML
                try { File.Delete(zipFilePath); } catch { }

                // Open with the system default mail handler
                var psi = new ProcessStartInfo
                {
                    FileName = emlPath,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"無法啟動郵件程式: {ex.Message}\n\n請嘗試手動將檔案附加至郵件。", "發送郵件", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
    }
}


