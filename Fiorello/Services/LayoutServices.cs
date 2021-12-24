using Fiorello.DAL;
using System.Collections.Generic;
using System.Linq;

namespace Fiorello.Services
{
    public class LayoutServices
    {
        private AppDbContext _context;

        public LayoutServices(AppDbContext context)
        {
            _context = context;
        }

        public Dictionary<string,string> GetSetting()
        {
            return _context.Settings.AsEnumerable().ToDictionary(s => s.Key, s => s.Value);
        }
    }
}
