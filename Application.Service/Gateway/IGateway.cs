using Application.DTO.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteWorker.Gateway
{
    public interface IGateway
    {
        void Execute(GatewayCallerMessage dto);
    }
}
