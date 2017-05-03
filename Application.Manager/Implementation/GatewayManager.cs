using Application.DTO.Gateway;
using Application.Repository;
using Application.Snapshot;
using Application.Utility.Logging;
using Application.Utility.Translators;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager.Implementation
{
    public class GatewayManager : IBusinessGatewayManager, IServiceGatewayManager
    {

        private readonly IRepository<GatewaySnapshot> _IGatewayRepository;
        private readonly IEntityTranslatorService _translatorService;
        private readonly ILogger _logger;

        public GatewayManager(IRepository<GatewaySnapshot> iGatewayRepository,
            IEntityTranslatorService translatorService, ILogger logger)
        {
            _translatorService = translatorService;
            _IGatewayRepository = iGatewayRepository;
            _logger = logger;
        }

        public GatewayDTO GetbyId(string Id)
        {
            GatewayDTO result = null;
            try
            {
                result = _translatorService.Translate<GatewayDTO>(this.GetSnapshotbyId(Id));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to Gateway dto", ex, Id);
            }
            return result;
        }

        public GatewaySnapshot GetSnapshotbyId(string Id)
        {
            GatewaySnapshot result = null;
            try
            {
                result = _IGatewayRepository.GetById(Id);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Read the Gateway", ex, Id);
            }
            return result;
        }


        public string Add(GatewayDTO Gatewaymessage)
        {
            string result = null;
            try
            {
                GatewaySnapshot snapshot = _translatorService.Translate<GatewaySnapshot>(Gatewaymessage);
                result = this.Add(snapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to snapshot", ex, Gatewaymessage);
            }
            return result;
        }

        public string Add(GatewaySnapshot gatewaySnapshot)
        {
            string result = null;
            try
            {
                result = _IGatewayRepository.Add(gatewaySnapshot).Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Add the Gateway", ex, gatewaySnapshot);
            }
            return result;
        }


        public GatewayDTO Update(GatewayDTO Gatewaymessage)
        {
            GatewayDTO result = null;
            try
            {
                GatewaySnapshot snapshot = _translatorService.Translate<GatewaySnapshot>(Gatewaymessage);
                result = _translatorService.Translate<GatewayDTO>(this.Update(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate the Gateway DTO", ex, Gatewaymessage);
            }
            return result;
        }


        public GatewaySnapshot Update(GatewaySnapshot gatewaySnapshot)
        {
            GatewaySnapshot result = null;
            try
            {
                result = _IGatewayRepository.Update(gatewaySnapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to update the Gateway", ex, gatewaySnapshot);
            }
            return result;
        }

        public GatewaySnapshot Update(GatewayStatusMessage message)
        {
            GatewaySnapshot result = null;
            try
            {
                var snapshot = _translatorService.Translate<GatewaySnapshot>(message);
                result = this.Update(snapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to update the Gateway", ex, message);
            }
            return result;
        }

        public IEnumerable<GatewayDTO> Get()
        {
            IEnumerable<GatewayDTO> result = null;
            try
            {
                result = this.GetSnapshots().Select(m => _translatorService.Translate<GatewayDTO>(m)).AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Gateway", ex);
            }

            return result;
        }



        public IEnumerable<GatewaySnapshot> GetSnapshots()
        {
            IEnumerable<GatewaySnapshot> result = null;
            try
            {
                result = _IGatewayRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Gateway", ex);
            }

            return result;
        }



        public IEnumerable<GatewayDTO> GetbyStatus(string Status)
        {
            IEnumerable<GatewayDTO> result = null;
            try
            {
                result = GetSnapshotsbyStatus(Status).Select(m => _translatorService.Translate<GatewayDTO>(m)).AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to Gateway dto", ex, Status);
            }

            return result;
        }

        public IEnumerable<GatewaySnapshot> GetSnapshotsbyStatus(string Status)
        {
            IEnumerable<GatewaySnapshot> result = null;
            try
            {
                Expression<Func<GatewaySnapshot, bool>> expr = (x => x.Status.ToLower() == Status.ToLower());
                result = _IGatewayRepository.Find(expr);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Gateway by status", ex, Status);
            }

            return result;
        }



        public IEnumerable<GatewayDTO> GetbyInterval(int Seconds)
        {
            IEnumerable<GatewayDTO> result = null;
            try
            {
             result = this.GetSnapshotsbyInterval(Seconds).Select(m => _translatorService.Translate<GatewayDTO>(m)).AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Gateway", ex, Seconds);
            }

            return result;
        }

        public IEnumerable<GatewaySnapshot> GetSnapshotsbyInterval(int Seconds)
        {
            IEnumerable<GatewaySnapshot> result = null;
            try
            {
                Expression<Func<GatewaySnapshot, bool>> expr = (x =>
                (x.LastRunTime != null &&
                x.LastRunTime.AddSeconds(x.Interval) <= DateTime.UtcNow.AddSeconds(Seconds))
                ||
                // new record and not run for single time
                (x.LastRunTime == null && x.UpdatedOn == null &&
                x.CreatedOn.AddSeconds(x.Interval) <= DateTime.UtcNow.AddSeconds(Seconds))
                ||
                //new record and updated before run single time
                 (x.LastRunTime == null && x.UpdatedOn != null &&
                  x.UpdatedOn.AddSeconds(x.Interval) <= DateTime.UtcNow.AddSeconds(Seconds))
                );
                result = _IGatewayRepository.Find(expr);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Gateway", ex, Seconds);
            }

            return result;
        }

        public GatewayDTO Save(GatewayDTO Gatewaymessage)
        {
            GatewayDTO result = null;
            try
            {
                _logger.Info("Test message");
                GatewaySnapshot snapshot = _translatorService.Translate<GatewaySnapshot>(Gatewaymessage);
                result = _translatorService.Translate<GatewayDTO>(this.Save(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the Gateway", ex, Gatewaymessage);
            }
            return result;
        }


        public GatewaySnapshot Save(GatewaySnapshot Gatewaymessage)
        {
            GatewaySnapshot result = null;
            try
            {
                _logger.Info("Test message");
                if (Gatewaymessage.Id == string.Empty || Gatewaymessage.Id == null)
                {
                    Gatewaymessage.CreatedOn = DateTime.UtcNow;
                    result = _IGatewayRepository.Add(Gatewaymessage);
                }
                else
                {
                    Gatewaymessage.UpdatedOn = DateTime.UtcNow;
                    result = _IGatewayRepository.Update(Gatewaymessage);
                }

            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the Gateway", ex, Gatewaymessage);
            }
            return result;
        }

        public GatewayDTO Delete(GatewayDTO Gatewaymessage)
        {
            Gatewaymessage.IsActive = false;
            return this.Update(Gatewaymessage);
        }

        public GatewaySnapshot Delete(GatewaySnapshot gatewaySnapshot)
        {
            gatewaySnapshot.IsActive = false;
            return this.Update(gatewaySnapshot);
        }


    }
}
