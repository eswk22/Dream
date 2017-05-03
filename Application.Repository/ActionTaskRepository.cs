using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Snapshot;

namespace Application.Repository
{
    public class ActionTaskRepository : MongoRepository<ActionTaskSnapshot>, IActionTaskRepository
    {

    }
}
