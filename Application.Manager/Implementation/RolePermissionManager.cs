using Application.Common;
using Application.DTO.Common;
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
    public class RolePermissionManager : IRolePermissionBusinessManager, IRolePermissionServiceManager
    {

        private readonly IRepository<RolePermissionSnapshot> _IRolePermissionRepository;
        private readonly IEntityTranslatorService _translatorService;
        private readonly ILogger _logger;

        public RolePermissionManager(IRepository<RolePermissionSnapshot> iRolePermissionRepository,
            IEntityTranslatorService translatorService, ILogger logger)
        {
            _translatorService = translatorService;
            _IRolePermissionRepository = iRolePermissionRepository;
            _logger = logger;
        }

        public RolePermissionDTO GetbyId(string Id)
        {
            RolePermissionDTO result = null;
            try
            {
                result = _translatorService.Translate<RolePermissionDTO>(this.GetSnapshotbyId(Id));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to RolePermission dto", ex, Id);
            }
            return result;
        }

        public RolePermissionSnapshot GetSnapshotbyId(string Id)
        {
            RolePermissionSnapshot result = null;
            try
            {
                result = _IRolePermissionRepository.GetById(Id);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Read the RolePermission", ex, Id);
            }
            return result;
        }


        public string Add(RolePermissionDTO RolePermissionmessage)
        {
            string result = null;
            try
            {
                RolePermissionSnapshot snapshot = _translatorService.Translate<RolePermissionSnapshot>(RolePermissionmessage);
                result = this.Add(snapshot);
            }
            catch (Exception ex)
            {
                int key = _logger.Error("Unable to translate to snapshot", ex, RolePermissionmessage);
                throw new CustomException(key, "Unable to translate to snapshot");

            }
            return result;
        }

        public void Add(string ParentId, IList<RolePermissionDTO> permittedRoles)
        {
            try
            {
                IList<RolePermissionSnapshot> snapshots = permittedRoles.Select(s => _translatorService.Translate<RolePermissionSnapshot>(s)).ToList();
                for (int i = 0; i < snapshots.Count; i++)
                {
                    snapshots[i].ParentId = ParentId;
                }
                _IRolePermissionRepository.Add(snapshots);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to snapshot", ex, ParentId, permittedRoles);
            }
        }

        public string Add(RolePermissionSnapshot RolePermissionSnapshot)
        {
            string result = null;
            try
            {
                result = _IRolePermissionRepository.Add(RolePermissionSnapshot).Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Add the RolePermission", ex, RolePermissionSnapshot);
            }
            return result;
        }


        public RolePermissionDTO Update(RolePermissionDTO RolePermissionmessage)
        {
            RolePermissionDTO result = null;
            try
            {
                RolePermissionSnapshot snapshot = _translatorService.Translate<RolePermissionSnapshot>(RolePermissionmessage);
                result = _translatorService.Translate<RolePermissionDTO>(this.Update(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate the RolePermission DTO", ex, RolePermissionmessage);
            }
            return result;
        }


        public RolePermissionSnapshot Update(RolePermissionSnapshot RolePermissionSnapshot)
        {
            RolePermissionSnapshot result = null;
            try
            {
                result = _IRolePermissionRepository.Update(RolePermissionSnapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to update the RolePermission", ex, RolePermissionSnapshot);
            }
            return result;
        }


        public void Update(string ParentId, IList<RolePermissionDTO> permittedRoles)
        {
            try
            {
                IList<RolePermissionSnapshot> snapshots = permittedRoles.Select(s => _translatorService.Translate<RolePermissionSnapshot>(s)).ToList();
                for (int i = 0; i < snapshots.Count; i++)
                {
                    snapshots[i].ParentId = ParentId;
                }
                _IRolePermissionRepository.Update(snapshots);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to snapshot", ex, ParentId, permittedRoles);
            }
        }

        public IEnumerable<RolePermissionDTO> GetRolePermissions()
        {
            IEnumerable<RolePermissionDTO> result = null;
            try
            {
                result = this.GetSnapshots().Select(m => _translatorService.Translate<RolePermissionDTO>(m)).AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the RolePermission", ex);
            }

            return result;
        }



        public IEnumerable<RolePermissionSnapshot> GetSnapshots()
        {
            IEnumerable<RolePermissionSnapshot> result = null;
            try
            {
                result = _IRolePermissionRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the RolePermission", ex);
            }

            return result;
        }


        public IEnumerable<RolePermissionDTO> GetRolePermissions(string ParentId)
        {
            IEnumerable<RolePermissionDTO> result = null;
            try
            {
                result = this.GetSnapshots(ParentId).Select(m => _translatorService.Translate<RolePermissionDTO>(m)).AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the RolePermission", ex);
            }

            return result;
        }



        public IEnumerable<RolePermissionSnapshot> GetSnapshots(string ParentId)
        {
            IEnumerable<RolePermissionSnapshot> result = null;
            try
            {
                Expression<Func<RolePermissionSnapshot, bool>> expr = (x => x.ParentId == ParentId && x.IsActive == true);
                result = _IRolePermissionRepository.Find(expr);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the RolePermission", ex);
            }

            return result;
        }







        public RolePermissionDTO Save(RolePermissionDTO RolePermissionmessage)
        {
            RolePermissionDTO result = null;
            try
            {
                _logger.Info("Test message");
                RolePermissionSnapshot snapshot = _translatorService.Translate<RolePermissionSnapshot>(RolePermissionmessage);
                result = _translatorService.Translate<RolePermissionDTO>(this.Save(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the RolePermission", ex, RolePermissionmessage);
            }
            return result;
        }


        public RolePermissionSnapshot Save(RolePermissionSnapshot RolePermissionmessage)
        {
            RolePermissionSnapshot result = null;
            try
            {
                _logger.Info("Test message");
                if (RolePermissionmessage.Id == string.Empty || RolePermissionmessage.Id == null)
                {
                    result = _IRolePermissionRepository.Add(RolePermissionmessage);
                }
                else
                {
                    result = _IRolePermissionRepository.Update(RolePermissionmessage);
                }

            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the RolePermission", ex, RolePermissionmessage);
            }
            return result;
        }

        public RolePermissionDTO Delete(RolePermissionDTO RolePermissionmessage)
        {
            RolePermissionmessage.IsActive = false;
            return this.Update(RolePermissionmessage);
        }

        public RolePermissionDTO Delete(string Id)
        {
            RolePermissionSnapshot snapshot = this.GetSnapshotbyId(Id);
            if (snapshot != null)
            {
                return _translatorService.Translate<RolePermissionDTO>(this.Delete(snapshot));
            }
            else
            {
                throw new Exception();
            }
        }

        public RolePermissionSnapshot Delete(RolePermissionSnapshot RolePermissionSnapshot)
        {
            RolePermissionSnapshot.IsActive = false;
            return this.Update(RolePermissionSnapshot);
        }

        public void DeletebyParantId(string ParentId)
        {
            IList<RolePermissionSnapshot> snapshots = this.GetSnapshots(ParentId).ToList();
            for (int i = 0; i < snapshots.Count; i++)
            {
                snapshots[i].IsActive = false;
            }
            _IRolePermissionRepository.Update(snapshots);
        }


        public bool CopyRoles(string OldParentId, string NewParentId)
        {
            bool result = false;
            try
            {
                Expression<Func<RolePermissionSnapshot, bool>> expr = (x => x.IsActive == true && x.ParentId == OldParentId);
                IList<RolePermissionSnapshot> snapshots = _IRolePermissionRepository.Find(expr).ToList();
                for (int i = 0; i < snapshots.Count; i++)
                {
                    snapshots[i].ParentId = NewParentId;
                    snapshots[i].Id = string.Empty;
                }
                _IRolePermissionRepository.Add(snapshots);
                result = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

    }
}
