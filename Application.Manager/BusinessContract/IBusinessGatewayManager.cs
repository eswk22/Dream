using Application.DTO.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public interface IBusinessGatewayManager
    {
        GatewayDTO GetbyId(string Id);
        string Add(GatewayDTO Gatewaymessage);
        GatewayDTO Update(GatewayDTO Gatewaymessage);
        IEnumerable<GatewayDTO> Get();
        IEnumerable<GatewayDTO> GetbyStatus(string Status);
        GatewayDTO Save(GatewayDTO Gatewaymessage);
        GatewayDTO Delete(GatewayDTO Gatewaymessage);
        IEnumerable<GatewayDTO> GetbyInterval(int Seconds);
    }
}
