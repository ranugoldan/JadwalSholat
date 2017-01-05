using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAPPK.Model
{
    class PrayerTimeResponse
    {
        List<PrayerTime> items;

        public List<PrayerTime> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
            }
        }
    }
}
