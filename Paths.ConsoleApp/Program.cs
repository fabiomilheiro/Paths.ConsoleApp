using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Paths.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var actionTimer = new ActionTimer();

            var rootPathText = GetAppLocation();
            
            // You may override to test more folders
            // rootPathText = "C:\\dev";

            PathPart root = null;

            actionTimer.Execute("CreatePathTree", () => root = CreatePathTree(rootPathText, null, 0));
            
            actionTimer.Execute("PrintLeafFullPaths", () => PrintLeafFullPaths(root));

            Console.ReadKey();
        }

        private static void PrintLeafFullPaths(PathPart pathPart)
        {
            if (!HasChildren(pathPart))
            {
                Console.WriteLine(pathPart.GetFullPathInColumns());
                return;
            }

            foreach (var child in pathPart.Children)
            {
                PrintLeafFullPaths(child);
            }
        }

        private static bool HasChildren(PathPart pathPart)
        {
            return pathPart != null && pathPart.Children.Any();
        }

        private static void PrintPaths(PathPart pathPart)
        {
            Console.WriteLine($"{new string(' ', pathPart.Level)}{pathPart.Text}");

            foreach (var child in pathPart.Children)
            {
                PrintPaths(child);
            }
        }

        private static PathPart CreatePathTree(string pathPartText)
        {
            return CreatePathTree(pathPartText, null, 0);
        }

        private static PathPart CreatePathTree(string pathPartText, PathPart parent, int level)
        {
            var pathPart = new PathPart
            {
                Text = pathPartText,
                TextWithMaximumLength = pathPartText,
                Parent = parent,
                Level = level,
                Children = new List<PathPart>()
            };

            var fullPath = GetPartFullPath(pathPartText, parent);
            var subDirectoryPaths = Directory.GetDirectories(fullPath);

            foreach (var subDirectoryPath in subDirectoryPaths)
            {
                var subDirectoryName = Path.GetFileName(subDirectoryPath);

                pathPart.Children.Add(CreatePathTree(subDirectoryName, pathPart, level + 1));
            }

            if (!HasChildren(pathPart))
            {
                return pathPart;
            }

            var directoryChildrenTextMaximumLength = pathPart.Children.Max(c => c.Text.Length);

            foreach (var child in pathPart.Children)
            {
                var numberOfSpacesMissing = directoryChildrenTextMaximumLength - child.Text.Length;
                if (numberOfSpacesMissing > 0)
                {
                    child.TextWithMaximumLength += new string(' ', numberOfSpacesMissing);
                }
            }

            return pathPart;
        }

        private static string GetPartFullPath(string pathPartText, PathPart parent)
        {
            return Path.Combine(new[] { parent?.GetFullPath(), pathPartText }.Where(s => s != null).ToArray());
        }

        private static string GetAppLocation()
        {
            var parent =  Directory.GetParent($"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\..\\..\\..\\");

            while (parent.Name != "Paths.ConsoleApp")
            {
                parent = parent.Parent;
            }

            return parent.FullName;
        }
    }
}
