using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Projects.Exceptions
{
    public class InvalidIssueDescriptionException : Exception
    {
        public InvalidIssueDescriptionException() : base("issue description cannot be null or whitespace")
        {
        }
    }
}
