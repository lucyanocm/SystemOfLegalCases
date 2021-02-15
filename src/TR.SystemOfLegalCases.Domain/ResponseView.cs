using System.Collections.Generic;

namespace TR.SystemOfLegalCases.Domain
{
    public class ResponseView
    {
        public bool success { get; set; }
        public List<string> errors { get; set; }
    }
}
