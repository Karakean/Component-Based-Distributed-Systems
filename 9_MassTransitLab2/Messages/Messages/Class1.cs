using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public interface IPubl
    {
        int number { get; set; }
    }

    public interface IUstaw
    {
        bool dziala { get; set; }
    }

    public interface IOdpA
    {
        string kto { get; set; }
    }

    public interface IOdpB
    {
        string kto { get; set; }
    }
}
