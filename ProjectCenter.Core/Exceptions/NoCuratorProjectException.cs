using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Core.Exceptions
{
    public class NoCuratorProjectException : Exception
    {
        public NoCuratorProjectException(int id)
            : base($"Вы не являетесь куратором проекта с ID {id}!") { }
    }
}
