using System;
using System.IO;

namespace CJS_BackupNSync
{
    class Program
    {

        /*TODO:
         1. add prompts for file paths
         2. remove or simplfy writelines
         3. add prompt logic as to whether or not to provide feedback or silently manipulate files
         4. can add log instead of feedback
        */

        static void Main(string[] args)
        {
            string sourceDirectory = @"testbackupforfiles";
            string targetDirectory = @"targetDir";

            Console.WriteLine("Beginning Copy / Sync from {0} to {1} ... ", sourceDirectory, targetDirectory);
            Copy(sourceDirectory, targetDirectory);

            Console.WriteLine("File operations completed.");
            Console.WriteLine("Press any key to close ...");
            Console.ReadKey();
        }

        private static void Copy(string source, string target)
        {
            DirectoryInfo diSource = new DirectoryInfo(source);
            DirectoryInfo diTarget = new DirectoryInfo(target);

            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo diSource, DirectoryInfo diTarget)
        {
            if (!diSource.Exists)
            {
                Console.WriteLine("Source directory does not exist.  Can't write to target without a source.");
                return;
            }

            //if target directory does not exist, then create
            if (diTarget.Exists)
            {
                Console.WriteLine("Directory exists for: {0} ... ", diTarget);
            }
            else
            {
                Directory.CreateDirectory(diTarget.FullName);
                Console.WriteLine("Directory created for {0}", diTarget);
            }

            foreach (FileInfo fi in diSource.GetFiles())
            {
                DateTime created = fi.CreationTime;
                DateTime lastMod = fi.LastWriteTime;

                if (File.Exists(Path.Combine(diTarget.FullName, fi.Name)))
                {
                    string tFileName = Path.Combine(diTarget.FullName, fi.Name);
                    FileInfo f2 = new FileInfo(tFileName);
                    DateTime lm = f2.LastWriteTime;
                    Console.WriteLine(@"File {0} already exists, continue to next file ...", tFileName);              // last modified {3}", diTarget.FullName, fi.Name, tFileName, lm);

                    try
                    {
                        if (lastMod > lm)
                        {
                            Console.WriteLine(
                                @"Source file {0}\{1} last modified {2} is newer than the target file {3}\{4} last modified {5}",
                                fi.DirectoryName, fi.Name, lastMod.ToString(), diTarget.FullName, fi.Name,
                                lm.ToString());
                            fi.CopyTo(Path.Combine(diTarget.FullName, fi.Name), true);
                        }
                        else
                        {
                            Console.WriteLine(@"Destination File {0}\{1} Skipped", diTarget.FullName, fi.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(@"Error: {0}", ex.Message);
                    }

                }
                else
                {
                    Console.WriteLine(@"Copying {0}\{1}", diTarget.FullName, fi.Name);
                    fi.CopyTo(Path.Combine(diTarget.FullName, fi.Name), true);
                }
            }

            //copy each subdirectory
            foreach (DirectoryInfo diSourceSub in diSource.GetDirectories())
            {
                DirectoryInfo nextTargetSub = diTarget.CreateSubdirectory(diSourceSub.Name);
                CopyAll(diSourceSub, nextTargetSub);
            }







        }

    }
}

