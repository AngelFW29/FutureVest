using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureVest.Persistence.Common
{
    public class BasicEntity<TKey>
    {
        public required TKey Id { get; set; }
        public required string Name { get; set; }
    }
}
