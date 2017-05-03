using Application.DTO.Gateway;
using Application.Snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public interface IServiceGatewayManager
    {
        GatewaySnapshot GetSnapshotbyId(string Id);
        string Add(GatewaySnapshot Gatewaymessage);
        GatewaySnapshot Update(GatewaySnapshot Gatewaymessage);

        GatewaySnapshot Update(GatewayStatusMessage Gatewaymessage);

        IEnumerable<GatewaySnapshot> GetSnapshots();
        IEnumerable<GatewaySnapshot> GetSnapshotsbyStatus(string Status);
        IEnumerable<GatewaySnapshot> GetSnapshotsbyInterval(int Seconds);
        GatewaySnapshot Save(GatewaySnapshot Gatewaymessage);
        GatewaySnapshot Delete(GatewaySnapshot Gatewaymessage);
    }
}
