using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public interface IMessage1
    {
        string text1 { get; set; }
    }

    public interface IMessage2
    {
        string text2 { get; set; }
    }

    public interface IMessage3 : IMessage1, IMessage2
    {
    }
}
