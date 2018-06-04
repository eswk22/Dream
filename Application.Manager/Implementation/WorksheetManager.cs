using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.Worksheet;
using Application.Manager.ServiceContract;
using Application.Manager.BusinessContract;
using Application.Utility.Translators;
using Application.Utility.Logging;
using System.Linq.Expressions;
using Application.DAL.ElasticRepository;

namespace Application.Manager.Implementation
{
    public class WorksheetManager : IWorksheetBusinessManager, IWorksheetServiceManager
    {

        private readonly IElasticRepository<SheetSnapshot> _IWorksheetRepository;
        private readonly IElasticRepository<ActionResultSnapshot> _IActionResultRepository;
        private readonly IEntityTranslatorService _translatorService;
        private readonly ILogger _logger;

        public WorksheetManager(IElasticRepository<SheetSnapshot> iWorksheetRepository,
            IElasticRepository<ActionResultSnapshot> iActionResultRepository,
            IEntityTranslatorService translatorService, ILogger logger)
        {
            _translatorService = translatorService;
            _IWorksheetRepository = iWorksheetRepository;
            _IActionResultRepository = iActionResultRepository;
            _logger = logger;
        }

        #region Sheet

        public SheetDTO GetSheetbyId(string Id)
        {
            SheetDTO result = null;
            try
            {
                result = _translatorService.Translate<SheetDTO>(this.GetSheetSnapshotbyId(Id));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to Worksheet dto", ex, Id);
            }
            return result;
        }

        public SheetSnapshot GetSheetSnapshotbyId(string Id)
        {
            SheetSnapshot result = null;
            try
            {
                result = _IWorksheetRepository.GetById(Id);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Read the Worksheet", ex, Id);
            }
            return result;
        }


        public string AddSheet(SheetDTO sheetdto)
        {
            string result = null;
            try
            {
                SheetDTO snapshot = _translatorService.Translate<SheetDTO>(sheetdto);
                result = this.AddSheet(snapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to snapshot", ex, sheetdto);
            }
            return result;
        }

        public string AddSheet(SheetSnapshot sheetSnapshot)
        {
            string result = null;
            try
            {
                result = _IWorksheetRepository.Index(sheetSnapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Add the Worksheet", ex, sheetSnapshot);
            }
            return result;
        }


        public SheetDTO UpdateSheet(SheetDTO sheetDTO)
        {
            SheetDTO result = null;
            try
            {
                SheetSnapshot snapshot = _translatorService.Translate<SheetSnapshot>(sheetDTO);
                result = _translatorService.Translate<SheetDTO>(this.UpdateSheet(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate the Worksheet DTO", ex, sheetDTO);
            }
            return result;
        }


        public SheetSnapshot UpdateSheet(SheetSnapshot sheetSnapshot)
        {
            SheetSnapshot result = null;
            try
            {
                bool flag = _IWorksheetRepository.Update(sheetSnapshot);
                if(flag)
                    result = this.GetSheetSnapshotbyId(sheetSnapshot.Id);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to update the Worksheet", ex, sheetSnapshot);
            }
            return result;
        }


        public SheetDTO SaveSheet(SheetDTO sheetDTO)
        {
            SheetDTO result = null;
            try
            {
                _logger.Info("Test message");
                SheetSnapshot snapshot = _translatorService.Translate<SheetSnapshot>(sheetDTO);
                result = _translatorService.Translate<SheetDTO>(this.SaveSheet(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the Worksheet", ex, sheetDTO);
            }
            return result;
        }


        public SheetSnapshot SaveSheet(SheetSnapshot sheetSnapshot)
        {
            SheetSnapshot result = null;
            try
            {
                _logger.Info("Test message");
                if (sheetSnapshot.Id == string.Empty || sheetSnapshot.Id == null)
                {
                    sheetSnapshot.CreatedOn = DateTime.UtcNow;
                    string id = _IWorksheetRepository.Index(sheetSnapshot);
                    result = this.GetSheetSnapshotbyId(id);
                }
                else
                {
                    sheetSnapshot.ModifiedOn = DateTime.UtcNow;
                    bool flag = _IWorksheetRepository.Update(sheetSnapshot);
                    if (flag)
                    {
                        result = this.GetSheetSnapshotbyId(sheetSnapshot.Id);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the Worksheet", ex, sheetSnapshot);
            }
            return result;
        }

        public SheetDTO DeleteSheet(SheetDTO sheetDTO)
        {
            sheetDTO.IsActive = false;
            return this.UpdateSheet(sheetDTO);
        }

        public SheetSnapshot DeleteSheet(SheetSnapshot sheetSnapshot)
        {
            sheetSnapshot.IsActive = false;
            return this.UpdateSheet(sheetSnapshot);
        }

        #endregion Sheet

        #region ActionResult

        public ActionResultDTO GetActionResultbyId(string Id)
        {
            ActionResultDTO result = null;
            try
            {
                result = _translatorService.Translate<ActionResultDTO>(this.GetActionResultSnapshotbyId(Id));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to ActionResult dto", ex, Id);
            }
            return result;
        }

        public ActionResultSnapshot GetActionResultSnapshotbyId(string Id)
        {
            ActionResultSnapshot result = null;
            try
            {
                result = _IActionResultRepository.GetById(Id);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Read the ActionResult", ex, Id);
            }
            return result;
        }


        public string AddActionResult(ActionResultDTO actionResultdto)
        {
            string result = null;
            try
            {
                ActionResultDTO snapshot = _translatorService.Translate<ActionResultDTO>(actionResultdto);
                result = this.AddActionResult(snapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate to snapshot", ex, actionResultdto);
            }
            return result;
        }

        public string AddActionResult(ActionResultSnapshot actionResultSnapshot)
        {
            string result = null;
            try
            {
                result = _IActionResultRepository.Index(actionResultSnapshot);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to Add the ActionResult", ex, actionResultSnapshot);
            }
            return result;
        }


        public ActionResultDTO UpdateActionResult(ActionResultDTO actionResultDTO)
        {
            ActionResultDTO result = null;
            try
            {
                ActionResultSnapshot snapshot = _translatorService.Translate<ActionResultSnapshot>(actionResultDTO);
                result = _translatorService.Translate<ActionResultDTO>(this.UpdateActionResult(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to translate the ActionResult DTO", ex, actionResultDTO);
            }
            return result;
        }


        public ActionResultSnapshot UpdateActionResult(ActionResultSnapshot actionResultSnapshot)
        {
            ActionResultSnapshot result = null;
            try
            {
                bool flag = _IActionResultRepository.Update(actionResultSnapshot);
                if (flag)
                    result = this.GetActionResultSnapshotbyId(actionResultSnapshot.Id);
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to update the ActionResult", ex, actionResultSnapshot);
            }
            return result;
        }


        public ActionResultDTO SaveActionResult(ActionResultDTO actionResultDTO)
        {
            ActionResultDTO result = null;
            try
            {
                _logger.Info("Test message");
                ActionResultSnapshot snapshot = _translatorService.Translate<ActionResultSnapshot>(actionResultDTO);
                result = _translatorService.Translate<ActionResultDTO>(this.SaveActionResult(snapshot));
            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the ActionResult", ex, actionResultDTO);
            }
            return result;
        }


        public ActionResultSnapshot SaveActionResult(ActionResultSnapshot actionResultSnapshot)
        {
            ActionResultSnapshot result = null;
            try
            {
                _logger.Info("Test message");
                if (actionResultSnapshot.Id == string.Empty || actionResultSnapshot.Id == null)
                {
                    actionResultSnapshot.CreatedOn = DateTime.UtcNow;
                    string id = _IActionResultRepository.Index(actionResultSnapshot);
                    result = this.GetActionResultSnapshotbyId(id);
                }
                else
                {
                    actionResultSnapshot.ModifiedOn = DateTime.UtcNow;
                    bool flag = _IActionResultRepository.Update(actionResultSnapshot);
                    if (flag)
                    {
                        result = this.GetActionResultSnapshotbyId(actionResultSnapshot.Id);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error("Unable to save the ActionResult", ex, actionResultSnapshot);
            }
            return result;
        }

        public ActionResultDTO DeleteActionResult(ActionResultDTO actionResultDTO)
        {
            actionResultDTO.IsActive = false;
            return this.UpdateActionResult(actionResultDTO);
        }

        public ActionResultSnapshot DeleteActionResult(ActionResultSnapshot actionResultSnapshot)
        {
            actionResultSnapshot.IsActive = false;
            return this.UpdateActionResult(actionResultSnapshot);
        }

        #endregion ActionResult

    }
}
