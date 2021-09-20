using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CohesionTest.Db.Enums
{
    // Could add this to the database but honestly, I would just see that as overhead if it just a few statuses and it isn't adding any value.
    public enum CurrentStatusEnum
    {
        NotApplicable = 0,
        Created = 1,
        InProgress = 2,
        Complete = 3,
        Canceled = 4
    }
}
