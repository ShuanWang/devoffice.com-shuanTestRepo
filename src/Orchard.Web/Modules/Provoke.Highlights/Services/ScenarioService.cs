using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Orchard.ContentManagement;
using Orchard.Core.Scheduling.Models;
using Orchard.Data;
using Provoke.Highlights.Models;
using Provoke.Highlights.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Provoke.Highlights.Services
{
    public class ScenarioService : IScenarioService
    {
        private readonly IRepository<ScenarioRecord> _scenarioRepository;
        private readonly IRepository<TaskRecord> _taskRepository;
        private readonly IRepository<StepRecord> _stepRepository;
        private readonly IRepository<RelatedResourceRecord> _resourceRepository;

        public ScenarioService(
            IRepository<ScenarioRecord> scenarioRepository
            , IRepository<TaskRecord> taskRepository
            , IRepository<StepRecord> stepRepository
            , IRepository<RelatedResourceRecord> resourceRepository
        ) {
            _scenarioRepository = scenarioRepository;
            _taskRepository = taskRepository;
            _stepRepository = stepRepository;
            _resourceRepository = resourceRepository;

            
        }

        public void UpdateScenarioPart(ContentItem item, ScenarioViewModel model) {
            // At this point, Orchard has already created / update the scenario record, we just need to sort out the Task & Step records.
            var scenarioRecord = _scenarioRepository.Get(item.Id);

            var updatedTasks = (List<TaskRecord>)JsonConvert.DeserializeObject(model.TasksJson, typeof(List<TaskRecord>));
            var updatedResources = (List<RelatedResourceRecord>)JsonConvert.DeserializeObject(model.RelatedResourcesJson, typeof(List<RelatedResourceRecord>));


            var deletedTasks = scenarioRecord.Tasks.Where(deletedTaskRecord => !updatedTasks.Select(updatedTaskRecord => updatedTaskRecord.Id).Contains(deletedTaskRecord.Id));
            var deletedResources = scenarioRecord.RelatedResources.Where(deletedResourceRecord => !updatedResources.Select(updatedResourceRecord => updatedResourceRecord.Id).Contains(deletedResourceRecord.Id));
            
            foreach (var deletedTask in deletedTasks)
            {
                foreach (var stepRecord in deletedTask.Steps) {
                    _stepRepository.Delete(stepRecord);
                }
                _taskRepository.Delete(deletedTask);
            }

            foreach (var deletedResource in deletedResources)
            {
                _resourceRepository.Delete(deletedResource);
               
            }

            var mergeTasks = new List<TaskRecord>();
            foreach (var taskRecord in updatedTasks) {
                taskRecord.ScenarioRecord = scenarioRecord;

                if (taskRecord.Id == 0)
                    mergeTasks.Add(CreateTask(taskRecord));
                else
                    mergeTasks.Add(UpdateTask(taskRecord));
            }

            var mergeResourceTasks = new List<RelatedResourceRecord>();
            foreach (var resourceRecord in updatedResources)
            {
                resourceRecord.ScenarioRecord = scenarioRecord;

                if (resourceRecord.Id == 0)
                    mergeResourceTasks.Add(CreateResource(resourceRecord));
                else
                    mergeResourceTasks.Add(UpdateResource(resourceRecord));
            }

            scenarioRecord.Tasks = mergeTasks;
            scenarioRecord.RelatedResources = mergeResourceTasks;
            _scenarioRepository.Update(scenarioRecord);
        }

        private TaskRecord CreateTask(TaskRecord taskRecord) {
            _taskRepository.Create(taskRecord);

            if (taskRecord.Steps == null)
                return null;

            foreach (var stepRecord in taskRecord.Steps) {
                stepRecord.TaskRecord = taskRecord;
                _stepRepository.Create(stepRecord);
            }
            return taskRecord;
        }

        private RelatedResourceRecord CreateResource(RelatedResourceRecord resourceRecord)
        {
            _resourceRepository.Create(resourceRecord);
            return resourceRecord;
        }

        private TaskRecord UpdateTask(TaskRecord updatedTaskRecord) {
            if (updatedTaskRecord.Id <= 0)
                throw new InvalidOperationException("Task must have an Id to be updated.");

            var updatedSteps = updatedTaskRecord.Steps ?? new Collection<StepRecord>();

            foreach (var stepRecord in updatedSteps) {
                stepRecord.TaskRecord = updatedTaskRecord;

                if (stepRecord.Id == 0)
                    _stepRepository.Create(stepRecord);
                else
                    _stepRepository.Update(stepRecord);
            }

            var deletedSteps = updatedTaskRecord.Steps.Where(deletedTaskRecord => !updatedSteps.Select(sr => sr.Id).Contains(deletedTaskRecord.Id));
            foreach (var stepRecord in deletedSteps)
            {
                _stepRepository.Delete(stepRecord);
            }

            _taskRepository.Update(updatedTaskRecord);

            return _taskRepository.Get(updatedTaskRecord.Id);
        }

        private RelatedResourceRecord UpdateResource(RelatedResourceRecord updatedResourceRecord)
        {
            if (updatedResourceRecord.Id <= 0)
                throw new InvalidOperationException("Task must have an Id to be updated.");


            _resourceRepository.Update(updatedResourceRecord);

            return _resourceRepository.Get(updatedResourceRecord.Id);
        }

        public ScenarioRecord GetScenario(int scenarioId)
        {
            // It would be preferable to eager load the Tasks AND the Steps together in one query. The following query does that but also creates
            // a Cartesian product (duplicate results). Feel free to try and get it to work, but be warned - here be pain.

            //var thisShouldWorkButDoesnt =
            //    _scenarioRepository.Table
            //    .FetchMany(s => s.Tasks)
            //    .ThenFetch(t => t.Steps)  // Could also use .ThenFetchMany(t => t.Steps) but it still doesn't work.
            //    .AsEnumerable()
            //    .First();

            //DebugScenario(thisShouldWorkButDoesnt);

            var result =
                _scenarioRepository.Table
                    .Where(x => x.Id == scenarioId)
                    .FetchMany(s => s.Tasks);

            var scenarioResult = result.AsEnumerable().FirstOrDefault();
            if (scenarioResult != null) {
                scenarioResult.Tasks = OrderSteps(scenarioResult);
                scenarioResult.RelatedResources = scenarioResult.RelatedResources.OrderBy(s => s.SortOrder).ToList();
            }

            return scenarioResult;
        }

        private static List<TaskRecord> OrderSteps(ScenarioRecord result) {
            foreach (var task in result.Tasks) {
                task.Steps = task.Steps.OrderBy(s => s.SortOrder).ToList();
            }
            return result.Tasks.OrderBy(t => t.SortOrder).ToList();
        }


    }
}