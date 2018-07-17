using System.Collections.Generic;

namespace Paths.ConsoleApp
{
    public class PathPart
    {
        public string Text { get; set; }

        public string TextWithMaximumLength { get; set; }

        public PathPart Parent { get; set; }

        public List<PathPart> Children { get; set; }

        public int Level { get; set; }

        public string GetFullPath()
        {
            if (Parent == null)
            {
                return Text;
            }

            return $"{Parent?.GetFullPath()}\\{Text}";
        }

        public override string ToString()
        {
            return GetFullPath();
        }

        public string GetFullPathInColumns()
        {
            if (Parent == null)
            {
                return TextWithMaximumLength;
            }

            return $"{Parent?.GetFullPathInColumns()}\\{TextWithMaximumLength}";
        }
    }
}