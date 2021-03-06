﻿${
    // Enable extension methods by adding using Typewriter.Extensions.*
    using Typewriter.Extensions.Types;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.IO;
    // Uncomment the constructor to change template settings.
    Template(Settings settings)
    {
        settings
           .IncludeProject("Common360.Entities")
           .OutputFilenameFactory = (file) => {
                string[] fileParts = file.FullName.Split('\\');
                Boolean startNamespaceAppending = false;
                string prefixFolder = "";
                string last = fileParts.Last();
                // Extract correct namespace to create corresponding output directory, according to my target Angular/TypeScript project
                foreach (var item in fileParts) {
                    if (!item.Equals(last)) {
                        if (string.Equals(item, "Models", StringComparison.OrdinalIgnoreCase)) {
                            startNamespaceAppending = true;
                        } else if (startNamespaceAppending) {
                            prefixFolder += SpacesFromCamel(item) + "/";
                        }
                    }
                }
                // Extract correct type name depending on its type (class, enum, interface, ...)
                string typeName = "undefined";
                if (file.Classes.Count > 0) {
                    typeName = file.Classes.First().Name;
                } else if (file.Interfaces.Count > 0) {
                    typeName = file.Interfaces.First().Name;
                } else if (file.Enums.Count > 0) {
                    typeName = file.Enums.First().Name;
                }
                string fileName = prefixFolder + SpacesFromCamel(typeName) + ".ts";
                // Creates target directory if it does not exist
                Regex rgx = new Regex("^(.*)(Common360.Entities\\.[\\w\\.]+\\\\Models)(.*)$");
                string pathFileName = Path.GetDirectoryName(file.FullName);
                pathFileName = rgx.Replace(pathFileName, "$1\\Common360.Entities\\src\\app\\models\\" + prefixFolder);
                if (!Directory.Exists(pathFileName)) {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(pathFileName);
                }
                return fileName;
            };
    }
    /* Transform a C# project directory name to a "typescript/client" directory name (no upper case, etc.)*/
    string SpacesFromCamel(string value)
    {
        if (value.Length > 0)
        {
            var result = new List<char>();
            char[] array = value.ToCharArray();
            foreach (var item in array)
            {
                if (char.IsUpper(item) && result.Count > 0)
                {
                    result.Add('-');
                }
                result.Add(char.ToLower(item));
            }

            return new string(result.ToArray());
        }
        return value;
    }
}
$Enums(Common360.Entities.Enums*)[
export enum $Name {$Values[
    $name,]
}]