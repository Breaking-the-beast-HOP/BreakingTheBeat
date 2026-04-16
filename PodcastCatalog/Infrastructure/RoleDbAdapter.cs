using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Infrastructure
{
    internal class RoleDbAdapter
    {
        private int _id { get; set; }
        public RoleDbAdapter()
        {
            // Initialize the ID counter to highest existing ID in the database or to 0 if there are no entries
        }
        public int NewId()
        {
            _id++;
            return _id;
        }
    }
}
