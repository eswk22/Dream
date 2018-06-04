using Application.Common;
using Application.DTO;
using Application.DTO.ActionTask;
using Application.DTO.Common;
using Application.Messages;
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

    public class ActionTaskManager : IActionTaskBusinessManager, IActionTaskServiceManager
    {

        private readonly IRepository<ActionTaskSnapshot> _IActionTaskRepository;
        private readonly IEntityTranslatorService _translatorService;
        private readonly IParameterBusinessManager _parameterManager;
        private readonly IRolePermissionBusinessManager _rolePermissionManager;
        private readonly ILogger _logger;

        public ActionTaskManager(IRepository<ActionTaskSnapshot> iActionTaskRepository,
            IEntityTranslatorService translatorService, ILogger logger,
            IParameterBusinessManager parameterManager, IRolePermissionBusinessManager rolePermissionManager)
        {
            _translatorService = translatorService;
            _IActionTaskRepository = iActionTaskRepository;
            _logger = logger;
            _parameterManager = parameterManager;
            _rolePermissionManager = rolePermissionManager;
        }

        public ActionTaskDTO GetbyId(string Id)
        {
            ActionTaskDTO result = null;
            try
            {
                result = _translatorService.Translate<ActionTaskDTO>(this.GetSnapshotbyId(Id));
                if (result != null)
                {
                    result.Parameters = _parameterManager.GetParameters(result.ActionTaskId).ToArray();
                    result.PermittedRoles = _rolePermissionManager.GetRolePermissions(result.ActionTaskId).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to ActionTask dto", ex, Id);
            }
            return result;
        }
        public ActionTaskMessage GetActionMessagebyId(string actionTaskId)
        {
            ActionTaskMessage result = null;
            try
            {
                result = _translatorService.Translate<ActionTaskMessage>(this.GetbyId(actionTaskId));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to ActionTask Message", ex, actionTaskId);
            }
            return result;
        }

        public ActionTaskSnapshot GetSnapshotbyId(string Id)
        {
            ActionTaskSnapshot result = null;
            try
            {
                result = _IActionTaskRepository.GetById(Id);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Read the ActionTask", ex, Id);
            }
            return result;
        }


        public string Add(ActionTaskDTO actionTaskDTO)
        {
            string result = null;
            try
            {
                ActionTaskSnapshot snapshot = _translatorService.Translate<ActionTaskSnapshot>(actionTaskDTO);
                result = this.Add(snapshot);
                if (result != null && result != string.Empty)
                {
                    _parameterManager.Add(result, actionTaskDTO.Parameters);
                    _rolePermissionManager.Add(result, actionTaskDTO.PermittedRoles);
                }
            }
            catch (Exception ex)
            {
                int key = _logger.Error("Unable to translate to snapshot", ex, actionTaskDTO);
                throw new CustomException(key, "Unable to translate to snapshot");

            }
            return result;
        }

        public string Add(ActionTaskSnapshot ActionTaskSnapshot)
        {
            string result = null;
            try
            {
                result = _IActionTaskRepository.Add(ActionTaskSnapshot).Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Add the ActionTask", ex, ActionTaskSnapshot);
            }
            return result;
        }


        public ActionTaskDTO Update(ActionTaskDTO actionTaskDTO)
        {
            ActionTaskDTO result = null;
            try
            {
                ActionTaskSnapshot snapshot = _translatorService.Translate<ActionTaskSnapshot>(actionTaskDTO);
                result = _translatorService.Translate<ActionTaskDTO>(this.Update(snapshot));
                if (result != null)
                {
                    _parameterManager.Update(result.ActionTaskId, actionTaskDTO.Parameters);
                    _rolePermissionManager.Update(result.ActionTaskId, actionTaskDTO.PermittedRoles);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate the ActionTask DTO", ex, actionTaskDTO);
            }
            return result;
        }


        public ActionTaskSnapshot Update(ActionTaskSnapshot ActionTaskSnapshot)
        {
            ActionTaskSnapshot result = null;
            try
            {
                result = _IActionTaskRepository.Update(ActionTaskSnapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to update the ActionTask", ex, ActionTaskSnapshot);
            }
            return result;
        }


        public IEnumerable<ActionTaskDTO> Get()
        {
            IEnumerable<ActionTaskDTO> result = null;
            try
            {
                result = this.GetSnapshots().Select(m => _translatorService.Translate<ActionTaskDTO>(m)).AsEnumerable();
                foreach (ActionTaskDTO dto in result)
                {
                    dto.Parameters = _parameterManager.GetParameters(dto.ActionTaskId).ToArray();
                    dto.PermittedRoles = _rolePermissionManager.GetRolePermissions(dto.ActionTaskId).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the ActionTask", ex);
            }

            return result;
        }



        public IEnumerable<ActionTaskSnapshot> GetSnapshots()
        {
            IEnumerable<ActionTaskSnapshot> result = null;
            try
            {
                result = _IActionTaskRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to read the ActionTask", ex);
            }

            return result;
        }



        public ActionTaskDTO Publish(ActionTaskDTO actionTaskDTO,string comment)
        {
            ActionTaskDTO result = null;
            string version = "0.0";
            try
            {
                Expression<Func<ActionTaskSnapshot, bool>> expr = (x => x.IsActive == true && x.Status == "E-Published");
                var snapshot = this._IActionTaskRepository.Find(expr);
                if (snapshot != null)
                {
                    IList<ActionTaskSnapshot> snapshotlist = snapshot.ToList();
                    for(int i=0; i < snapshotlist.Count; i++)
                    {
                        snapshotlist[i].Status = "Retired";
                        this.Update(snapshotlist[i]);
                        if(snapshotlist[i].Version != string.Empty)
                        {
                            if (Convert.ToDecimal(snapshotlist[i].Version) > Convert.ToDecimal(version))
                                version = snapshotlist[i].Version;
                        }
                    }
                }
                actionTaskDTO.Status = "Published";
                actionTaskDTO.Version = (Convert.ToDecimal(version) + 1).ToString();
                result = this.Save(actionTaskDTO,comment);
            }
            catch(Exception ex)
            {
                _logger.Error("Unable to publish the ActionTask", ex, actionTaskDTO,comment);
            }
            return result;
        }


        public ActionTaskDTO Edit(string actionTaskId,string name,string ATnamespace)
        {
            ActionTaskDTO result = null;
            try
            {
                Expression<Func<ActionTaskSnapshot,bool>> expr = (x=> x.IsActive == true && x.Status.ToLower() == "published" && x.Name == name && x.module == ATnamespace);
                //Changing the status of existing orders. This should always return one record. for development purpose, included the for loop.
                var existingRecords = _IActionTaskRepository.Find(expr).ToList();
                if (existingRecords.Count > 0)
                {
                    for (int i = 0; i < existingRecords.Count; i++)
                    {
                        existingRecords[i].Status = "E-Published";
                        this.Update(existingRecords[i]);
                    }
                    var item = existingRecords[0];
                    string oldATId = existingRecords[0].Id;
                    item.Id = string.Empty;
                    item.Status = "Draft";
                    string newATId = this.Add(item);
                    //"59498e539a8c483848f1abd0" 594991f69a8c484c2030c567
                    if (newATId != string.Empty)
                    {
                        _parameterManager.CopyParameters(oldATId, newATId);
                        _rolePermissionManager.CopyRoles(oldATId, newATId);
                    }
                    result = this.GetbyId(newATId);
                }

            }
            catch(Exception ex)
            {

            }

            return result;
        }



        public ActionTaskDTO Save(ActionTaskDTO actionTaskDTO,String Comment = "")
        {
            ActionTaskDTO result = null;
            try
            {
                var snapshot = _translatorService.Translate<ActionTaskSnapshot>(actionTaskDTO);
                snapshot.Comment = Comment;
                ActionTaskSnapshot savedResult = this.Save(snapshot);
                if (savedResult != null)
                {
                    IList<ParameterDTO> parameterAddition = new List<ParameterDTO>();
                    IList<ParameterDTO> parameterUpdate = new List<ParameterDTO>();
                    if (actionTaskDTO.Parameters != null)
                    {
                        foreach (var parameter in actionTaskDTO.Parameters)
                        {
                            if (parameter.ParentId == null)
                            {
                                parameterAddition.Add(parameter);
                            }
                            else
                            {
                                parameterUpdate.Add(parameter);
                            }
                        }
                        _parameterManager.DeletebyParantId(savedResult.Id);
                        _parameterManager.Add(savedResult.Id, parameterAddition);
                        _parameterManager.Update(savedResult.Id, parameterUpdate);
                    }
                    IList<RolePermissionDTO> roleAddition = new List<RolePermissionDTO>();
                    IList<RolePermissionDTO> roleUpdate = new List<RolePermissionDTO>();
                    if (actionTaskDTO.PermittedRoles != null)
                    {
                        foreach (var role in actionTaskDTO.PermittedRoles)
                        {
                            if (role.ParentId == null)
                            {
                                roleAddition.Add(role);
                            }
                            else
                            {
                                roleUpdate.Add(role);
                            }
                        }
                        _rolePermissionManager.DeletebyParantId(savedResult.Id);
                        _rolePermissionManager.Add(savedResult.Id, roleAddition);
                        _rolePermissionManager.Update(savedResult.Id, roleUpdate);
                    }
                    result = this.GetbyId(savedResult.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the ActionTask", ex, actionTaskDTO);
            }
            return result;
        }


        public ActionTaskSnapshot Save(ActionTaskSnapshot ActionTaskmessage)
        {
            ActionTaskSnapshot result = null;
            try
            {
                if (ActionTaskmessage.Id == string.Empty || ActionTaskmessage.Id == null)
                {
                    ActionTaskmessage.CreatedOn = DateTime.UtcNow;
                    ActionTaskmessage.Version = "1.0";
                    result = _IActionTaskRepository.Add(ActionTaskmessage);
                }
                else
                {
                    ActionTaskmessage.ModifiedOn = DateTime.UtcNow;
                    result = _IActionTaskRepository.Update(ActionTaskmessage);
                }

            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the ActionTask", ex, ActionTaskmessage);
            }
            return result;
        }

        public ActionTaskDTO Delete(ActionTaskDTO ActionTaskmessage)
        {
            ActionTaskmessage.IsActive = false;
            return this.Update(ActionTaskmessage);
        }

        public IEnumerable<ActionTaskDTO> Search(string quickFilter, int page, int size, string sort, FilterDTO[] filterPerColumn,ref int rowCount)
        {

            IEnumerable<ActionTaskDTO> result = null;
            rowCount = 0;
            try
            {
                Expression<Func<ActionTaskSnapshot, bool>> expr = null;
                if(quickFilter != string.Empty && quickFilter != null)
                    expr = (x => x.Name.Contains(quickFilter) && x.IsActive == true);
                else
                    expr = (x => x.IsActive == true);
                int skip = (page - 1) * size;
                result = _IActionTaskRepository.Find(expr).Skip(skip).Take(size).Select(s=> _translatorService.Translate<ActionTaskDTO> (s));
                if(result != null)
                {
                    rowCount = _IActionTaskRepository.Count(expr);
                }
            }
            catch(Exception ex)
            {
                _logger.Error("Unable to search ActionTasks", ex);
            }
            return result;
        }

        public ActionTaskDTO Delete(string Id)
        {
            ActionTaskSnapshot snapshot = this.GetSnapshotbyId(Id);
            if (snapshot != null)
            {
                return _translatorService.Translate<ActionTaskDTO>(this.Delete(snapshot));
            }
            else
            {
                throw new Exception();
            }
        }

        public ActionTaskSnapshot Delete(ActionTaskSnapshot ActionTaskSnapshot)
        {
            ActionTaskSnapshot.IsActive = false;
            return this.Update(ActionTaskSnapshot);
        }
        public bool ExistsByName(string name)
        {
            bool result = false;
            try
            {
                Expression<Func<ActionTaskSnapshot, bool>> expr = (x => x.Name == name);
                var findResult = _IActionTaskRepository.Find(expr);
                if (findResult != null && findResult.Count() > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public ActionTaskDTO Copy(string Id, string name)
        {
            ActionTaskDTO result = null;
            try
            {
                var existingActionTask = this.GetSnapshotbyId(Id);
                if (existingActionTask != null)
                {
                    existingActionTask.Id = string.Empty;
                    existingActionTask.Name = name;
                    var newActionTask = this.Add(existingActionTask);
                    return _translatorService.Translate<ActionTaskDTO>(newActionTask);
                }
                else
                {
                    throw new Exception("Not found");
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

    }
}
