using System.Collections.Generic;

namespace APIEmployeeDepartmentProject.Services
{
    // A class matching JSON body that Codito code generator expects
    public class RandomCodeRequest
    {
        // Number of codes to generate
        public int CodesToGenerate { get; set; }

        // Making sure codito generates unique codes
        public bool OnlyUniques { get; set; }

        // List of character sets expected
        public List<string> CharactersSets { get; set; }
    }
}
