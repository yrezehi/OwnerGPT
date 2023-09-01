using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsers.Abstract
{
    public interface IContentParser
    {
        // the process of acquiring the document, whether getting it via internet, local file system or other objects
        Task<E> Get<T, E>(T content);

        // the process of converting the content to text representation
        Task<string> ToText<T>(T data);
        
        // the process of removing nosies of a 
        Task<string> Cleansing(string content);
    }
}
