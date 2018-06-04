using Application.DTO.Common;
using Application.Manager.BusinessContract;
using Application.Manager.ServiceContract;
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
    public class ParameterManager : IParameterBusinessManager, IParameterServiceManager
    {

        private readonly IRepository<ParameterSnapshot> _IParameterRepository;
        private readonly IEntityTranslatorService _translatorService;
        private readonly ILogger _logger;

        public ParameterManager(IRepository<ParameterSnapshot> iParameterRepository,
            IEntityTranslatorService translatorService, ILogger logger)
        {
            _translatorService = translatorService;
            _IParameterRepository = iParameterRepository;
            _logger = logger;
        }

        public ParameterDTO GetbyId(string Id)
        {
            ParameterDTO result = null;
            try
            {
                result = _translatorService.Translate<ParameterDTO>(this.GetSnapshotbyId(Id));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to Parameter dto", ex, Id);
            }
            return result;
        }

        public ParameterSnapshot GetSnapshotbyId(string Id)
        {
            ParameterSnapshot result = null;
            try
            {
                result = _IParameterRepository.GetById(Id);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Read the Parameter", ex, Id);
            }
            return result;
        }

        public IEnumerable<ParameterDTO> GetParameters()
        {
            IEnumerable<ParameterDTO> result = null;
            try
            {
                result = this.GetSnapshots().Select(m => _translatorService.Translate<ParameterDTO>(m)).AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Parameter", ex);
            }

            return result;
        }



        public IEnumerable<ParameterSnapshot> GetSnapshots()
        {
            IEnumerable<ParameterSnapshot> result = null;
            try
            {
                result = _IParameterRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Parameter", ex);
            }

            return result;
        }


        public bool CopyParameters(string OldParentId,string NewParentId)
        {
            bool result = false;
            try {
                Expression<Func<ParameterSnapshot, bool>> expr = (x => x.IsActive == true && x.ParentId == OldParentId);
                IList<ParameterSnapshot> snapshots = _IParameterRepository.Find(expr).ToList();
                for(int i=0;i < snapshots.Count; i++)
                {
                    snapshots[i].ParentId = NewParentId;
                    snapshots[i].Id = string.Empty;
                }
                _IParameterRepository.Add(snapshots);
                result = true;
            }
            catch(Exception ex)
            {
                throw;
            }
            return result;
        }

        public IEnumerable<ParameterDTO> GetParameters(string ParentId)
        {
            IEnumerable<ParameterDTO> result = null;
            try
            {
                result = this.GetSnapshots(ParentId).Select(m => _translatorService.Translate<ParameterDTO>(m)).AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Parameter", ex);
            }

            return result;
        }



        public IEnumerable<ParameterSnapshot> GetSnapshots(string ParentId)
        {
            IEnumerable<ParameterSnapshot> result = null;
            try
            {
                Expression<Func<ParameterSnapshot, bool>> expr = (x => x.ParentId == ParentId && x.IsActive == true);
                result = _IParameterRepository.Find(expr);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the Parameter", ex);
            }

            return result;
        }


        public string Add(ParameterDTO parameterdto)
        {
            string result = null;
            try
            {
                ParameterSnapshot snapshot = _translatorService.Translate<ParameterSnapshot>(parameterdto);
                result = this.Add(snapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to snapshot", ex, parameterdto);
            }
            return result;
        }

        public void Add(string ParentId,IList<ParameterDTO> parameters)
        {
            try
            {
                IList<ParameterSnapshot> snapshots = parameters.Select(s => _translatorService.Translate<ParameterSnapshot>(s)).ToList();
                for(int i =0; i < snapshots.Count; i++)
                {
                    snapshots[i].ParentId = ParentId;
                }
                _IParameterRepository.Add(snapshots);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to snapshot", ex, ParentId, parameters);
            }
        }

        public string Add(ParameterSnapshot ParameterSnapshot)
        {
            string result = null;
            try
            {
                result = _IParameterRepository.Add(ParameterSnapshot).Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Add the Parameter", ex, ParameterSnapshot);
            }
            return result;
        }


        public ParameterDTO Update(ParameterDTO parameterdto)
        {
            ParameterDTO result = null;
            try
            {
                ParameterSnapshot snapshot = _translatorService.Translate<ParameterSnapshot>(parameterdto);
                result = _translatorService.Translate<ParameterDTO>(this.Update(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate the Parameter DTO", ex, parameterdto);
            }
            return result;
        }

        public void Update(string ParentId, IList<ParameterDTO> parameters)
        {
            try
            {
                IList<ParameterSnapshot> snapshots = parameters.Select(s => _translatorService.Translate<ParameterSnapshot>(s)).ToList();
                for(int i=0;i< snapshots.Count;i++)
                {
                    snapshots[i].ParentId = ParentId;
                }
                _IParameterRepository.Update(snapshots);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to snapshot", ex, ParentId, parameters);
            }
        }
        public ParameterSnapshot Update(ParameterSnapshot ParameterSnapshot)
        {
            ParameterSnapshot result = null;
            try
            {
                result = _IParameterRepository.Update(ParameterSnapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to update the Parameter", ex, ParameterSnapshot);
            }
            return result;
        }

        public ParameterDTO Save(ParameterDTO Parametermessage)
        {
            ParameterDTO result = null;
            try
            {
                _logger.Info("Test message");
                ParameterSnapshot snapshot = _translatorService.Translate<ParameterSnapshot>(Parametermessage);
                result = _translatorService.Translate<ParameterDTO>(this.Save(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the Parameter", ex, Parametermessage);
            }
            return result;
        }


        public ParameterSnapshot Save(ParameterSnapshot Parametermessage)
        {
            ParameterSnapshot result = null;
            try
            {
                _logger.Info("Test message");
                if (Parametermessage.Id == string.Empty || Parametermessage.Id == null)
                {
                    result = _IParameterRepository.Add(Parametermessage);
                }
                else
                {
                    result = _IParameterRepository.Update(Parametermessage);
                }

            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the Parameter", ex, Parametermessage);
            }
            return result;
        }

        public ParameterDTO Delete(ParameterDTO Parametermessage)
        {
            Parametermessage.IsActive = false;
            return this.Update(Parametermessage);
        }

        public ParameterSnapshot Delete(ParameterSnapshot ParameterSnapshot)
        {
            ParameterSnapshot.IsActive = false;
            return this.Update(ParameterSnapshot);
        }

        public ParameterDTO Delete(string Id)
        {
            ParameterSnapshot snapshot = this.GetSnapshotbyId(Id);
            if (snapshot != null)
            {
                return _translatorService.Translate<ParameterDTO>(this.Delete(snapshot));
            }
            else
            {
                throw new Exception();
            }
        }

        public void DeletebyParantId(string ParentId)
        {
            IList<ParameterSnapshot> snapshots = this.GetSnapshots(ParentId).ToList();
            for(int i=0;i<snapshots.Count;i++)
            {
                snapshots[i].IsActive = false;
            }
            _IParameterRepository.Update(snapshots);
        }

       

    }
}
