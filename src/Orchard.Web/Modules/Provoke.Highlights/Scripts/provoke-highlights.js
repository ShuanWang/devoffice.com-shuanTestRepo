
(function ($) {

    devOfficeApp.factory('highlightsService', function () {
        return {
            getModules: function() {
                return this.modules;
            },
            registerModules: function (modulesJson) {
                var newModules = [];
                for (var i = 0; i < modulesJson.length; i++) {

                    var newModule = new HighlightModule(modulesJson[i]);

                    for (var j = 0; j < newModule.tasks.length; j++) {
                        newModule.tasks[j] = new Task(newModule.tasks[j]);
                    }

                    newModules.push(newModule);
                }
                this.modules = newModules;
                return this.getModules();
            },
            modules: []
        };
    })
    .factory('highlightsAdminService', function () {
        console.log('highlights admin service');
        var service = {
                        
        }
        return service;
    });

    devOfficeApp.controller('moduleController', [
        '$scope', '$log', 'highlightsService', function ($scope, $log, highlightsService) {

            $scope.modules = [];
            $scope.currentModule = {};
            $scope.currentModuleUid = null;

            $scope.registerModules = function (modulesJson) {
                $scope.modules = highlightsService.registerModules(modulesJson);
                $scope.currentModule = $scope.modules[0] ? $scope.modules[0] : null;
                $scope.currentModuleUid = $scope.currentModule ? $scope.currentModule.uid : null;
            }

            $scope.moduleTabClick = function (module) {
                $log.log('module clicked: ', module.uid);
                $scope.currentModule = module;
                $scope.currentModuleUid = module.uid;
            }

        }
    ]);

    devOfficeApp.controller('relatedLinksController',[])

    devOfficeApp.controller('highlightController', [
        '$scope', '$log', '$timeout', '$sce', 'highlightsService', 'highlightsAdminService', 
        function ($scope, $log, $timeout, $sce, highlightsService, highlightsAdminService) {
            
            $scope.tasks = [];
            $scope.currentTaskUid;
            $scope.currentStepUid;
            $scope.newTaskDescription = '';
            $scope.selectedTaskIndex = 0;
            $scope.selectedStepIndex = 0;
            $scope.showStepReorderMessage = false;
            $scope.currentAnchorIndex = 0;
            $scope.anchorPositions = [
                'stepAnchorTopLeft', 'stepAnchorTopCenter', 'stepAnchorTopRight',
                'stepAnchorRightTop', 'stepAnchorRightCenter', 'stepAnchorRightBottom',
                'stepAnchorBottomRight', 'stepAnchorBottomCenter', 'stepAnchorBottomLeft',
                'stepAnchorLeftBottom', 'stepAnchorLeftCenter', 'stepAnchorLeftTop'
            ];

            $scope.$watch('tasks', function (newValue, oldValue) {
                // Update the stringified JSON for the entire tasks array (for use in admin)
                $scope.tasksAsJSONString = $scope.getTasksAsJSONString();
            }, true);

            $scope.$parent.$watch('currentModuleUid', function (newVal, oldVal) {
                // listen for changes to the current module uid (module navigation has occurred)
                // When we navigate to / from a module, we want to set the steps accordingly.
                $log.log($scope.currentModule.title, $scope.currentTaskUid, $scope.currentStepUid);
            });

            $scope.getTasks = function (index) {
                return $scope.tasks[index];
            }

            $scope.getJsonString = function () {
                return $scope.tasksAsJSONString;
            }

            $scope.getTasksAsJSONString = function () {
                $log.log('updating...');
                return angular.toJson($scope.tasks);
            }
            $scope.tasksAsJSONString = $scope.getTasksAsJSONString();

            $scope.initTasks = function (module) {

                // Initialize entire module, its tasks and steps
                $scope.module = module;
                $scope.tasks = module.tasks;
                $scope.currentTaskUid = getInitialCurrentTaskUid($scope.tasks);
                $scope.currentStepUid = getInitialCurrentStepUid($scope.tasks);

                $timeout(function () {
                    // Listen for changes to the current step and update the tooltip's
                    // position after its image loads. This should stay in the jQuery ready() function
                    $scope.$watch('currentStepUid', function (newVal, oldVal) {
                        // Once initial image loads, use its height/width to position the tooltip
                        $('.step-container[data-stepUid=' + $scope.currentStepUid + '] .imageContainer img').load(function () {
                            setTooltipPosition(this);
                        });
                    });

                    // Set the inital placement of the tooltip after page load
                    $('.step-container').each(function (index, element) {
                        var $element = $(element);
                        var $placementIndicator = $element.find('.step-tooltip');
                        var leftPos = $placementIndicator.attr('data-left');
                        var topPos = $placementIndicator.attr('data-top');
                        $placementIndicator.css({ top: topPos + 'px', left: leftPos + 'px' });
                    });

                    // TODO: convert to directives
                    $(".taskTabs").sortable({
                        items: '> .tab:not(.tab-new)',
                        axis: 'x',
                        containment: 'parent',
                        handle: '.tab-drag-handle',
                        tolerance: 'intersect',
                        distance: 5,
                        start: function () {
                            $log.log('moving');
                        },
                        update: function (e, ui) {
                            $log.log('update');
                            $scope.currentTaskUid = $(ui.item).attr('data-uid');
                            var currentTask = $scope.getTaskByUid($scope.currentTaskUid);
                            var firstTaskStep = currentTask ? (currentTask.steps ? currentTask.steps[0] : null) : null;
                            $scope.currentStepUid = firstTaskStep ? firstTaskStep.uid : null;
                            $log.log('REORDERED TASKS:', $scope.currentStepUid);
                            reorderList($scope.tasks, '.taskTabs .tab', 'itemId', true);
                        }
                    });

                    $(".step-list").sortable({
                        items: '> .step-list-item:not(.step-list-add)',
                        axis: 'x',
                        containment: 'parent',
                        handle: '.step-list-item-drag-handle',
                        tolerance: 'intersect',
                        distance: 5,
                        start: function () {
                            $log.log('moving');
                        },
                        update: function (e, ui) {
                            $log.log('update');
                            var taskIndex = $(ui.item).attr('taskindex');
                            $scope.currentStepUid = $(ui.item).attr('stepUid');
                            var taskId = $(ui.item).attr('taskid');
                            var stepsList = $scope.tasks[taskIndex].steps;
                            $scope.showStepReorderMessage = true;
                            reorderList(stepsList, '#task-' + taskId + ' .step-list-item:not(.step-list-add)', 'stepId', true);
                        }
                    });

                    // Done loading the modules, tasks, and steps, hide the loading spinner
                    // and show the modules.
                    $scope.isLoaded = true;
                   
                });
                //$scope.makeStepDescriptionsHtml();
            }

            $scope.addTask = function () {
                // allow only 8 tasks total
                if ($scope.tasks.length < 8) {
                    var data = {
                        Id: 0,
                        Description: $scope.newTaskDescription,
                        Title: '',
                        Duration: '',
                        SortOrder: $scope.tasks.length + 1,
                        Steps: []
                    }
                    var newTask = new Task(data);
                    $scope.tasks.push(newTask);

                    // set current task to the newly created task and focus on it in the UI
                    $scope.currentTaskUid = newTask.uid;

                    // ensure the tasks get reordered properly
                    reorderList($scope.tasks, '.taskTabs .tab', 'itemId', false);
                }
            }

            $scope.removeTask = function (index) {
                if (!confirm(confirmRemoveMessage)) {
                    return false;
                }
                if ($scope.tasks && $scope.tasks.length > 0) {
                    $scope.tasks.splice(index, 1);
                    if ($scope.tasks && $scope.tasks.length > 0) {
                        $scope.currentTaskUid = $scope.tasks[index - 1] && $scope.tasks[index - 1].uid;
                        $scope.currentStepUid = $scope.tasks[index - 1] && $scope.tasks[index - 1].steps.length > 0 &&
                            $scope.tasks[index - 1].steps[0].uid;
                    }
                    $(".tasks-message").show();

                    // ensure the tasks get reordered properly
                    reorderList($scope.tasks, '.taskTabs .tab', 'itemId', false);
                }
            }

            // Steps
            $scope.addStep = function (task, index) {
                var length = $scope.tasks[index].steps.length + 1;

                // When adding subsequent steps, populate the images by default based on the image URL
                // of the first step that was added.
                var existingImage = '';
                if ($scope.tasks && $scope.tasks[index] && $scope.tasks[index].steps && $scope.tasks[index].steps[0])
                    existingImage = $scope.tasks[index].steps[0].image;

                var data = {
                    Id: 0,
                    Title: "Step " + length,
                    Description: "",
                    SortOrder: length,
                    TopPosition: 0,
                    LeftPosition: 0,
                    Anchor: "stepAnchorRightCenter",
                    Image: existingImage
            }
                var newStep = new Step(data);
                $scope.tasks[index].steps.push(newStep);

                // set current step to the newly created step and focus on it in the UI
                $scope.currentStepUid = newStep.uid;

                $('.tasks-message').show();
            }

            $scope.removeStep = function (taskIndex, stepIndex) {
                if (!confirm(confirmRemoveMessage)) {
                    return false;
                }
                if ($scope.tasks && $scope.tasks[taskIndex].steps && $scope.tasks[taskIndex].steps.length > 0) {
                    $scope.tasks[taskIndex].steps.splice(stepIndex, 1);
                    $(".tasks-message").show();

                    reorderList(
                        $scope.tasks[taskIndex].steps,
                        '#task-' + $scope.tasks[taskIndex].id + ' .step-list-item:not(.step-list-add)',
                        'stepId', false);
                }
            }

            $scope.isImageSet = function (step) {
                return step.image != '';
            }

            $scope.logTasks = function () {
                $log.log($scope.tasks);
            }

            $scope.anchorPositionClick = function (step) {

                var currentAnchorIndex = $scope.anchorPositions.indexOf(step.anchor);
                $log.log(currentAnchorIndex, $scope.anchorPositions.length - 1);
                var nextAnchorIndex = currentAnchorIndex < $scope.anchorPositions.length - 1 ? currentAnchorIndex + 1 : 0;
                $log.log($scope.anchorPositions[currentAnchorIndex], '=>', $scope.anchorPositions[nextAnchorIndex]);
                step.anchor = $scope.anchorPositions[nextAnchorIndex];
            }

            $scope.updateMediaUrl = function (currentStep, index) {

                var url = moduleHighlights.mediaUrl;
                $log.log('media url:', url);
                $.colorbox({
                    href: url,
                    iframe: true,
                    reposition: true,
                    width: "100%",
                    height: "100%",
                    onLoad: function () { // hide the scrollbars from the main window
                        $('html, body').css('overflow', 'hidden');
                        $('#cboxClose').remove();
                    },
                    onClosed: function () {
                        $('html, body').css('overflow', '');
                        var selectedData = $.colorbox.selectedData;
                        $log.log(selectedData);

                        // NOTE: when grabbing images from the dialog, the path returned in selectedData[0].resource
                        // is simply the URI, not the full image path. This will cause the preview image not to load.
                        // To work around this, we have to prepend the entire window.location.hostname to ensure we
                        // can load the preview image.
                        var prefix = window.location.protocol + '//' + window.location.hostname;

                        if (selectedData == null) // Dialog cancelled, do nothing
                            return;
                        if (selectedData && currentStep) {
                            $log.log('updating item...');
                            currentStep.image = prefix + selectedData[0].resource;
                            //$scope.updateItemImageHelper(currentStep, index);
                            $scope.tasksAsJSONString = $scope.getTasksAsJSONString();
                        } else {
                            $log.log('adding new image...');
                            // TODO: implement step creation during task creation?
                            $("#step-image-" + currentStep.id).val(prefix + selectedData[0].resource);
                            $scope.newImageURL = prefix + selectedData[0].resource;
                        }
                        $scope.$apply();
                    }
                });
            }

            $('.save-button button.primaryAction').click(function () {
                $scope.tasksAsJSONString = $scope.getTasksAsJSONString();
                $log.log($scope.tasksAsJSONString);
            });

            function reorderList(list, jQueryElementSelector, dataAttr, forceScopeApply) {
                if (list && list.length > 0) {
                    var newSortOrder = 1;
                    $(jQueryElementSelector).each(function(index) {

                        var id = parseInt($(this).attr(dataAttr));
                        $log.log('item id: ', id, 'sortorder: ', newSortOrder);

                        // find item with matching id and update its sort order
                        var match = list.filter(function(item) {
                            //$log.log('compare ids: ', task.id, id, (task.id == id ? 'match' : ''));
                            return item.id == id;
                        });
                        $log.log('match: ', match);
                        if (match && match.length > 0) {
                            match[0].sortOrder = newSortOrder;
                            $log.log('setting match sortOrder: ', newSortOrder, ' => ', match[0].sortOrder);
                            newSortOrder++;
                        }
                    });

                    $scope.tasksAsJSONString = $scope.getTasksAsJSONString();
                    //$log.log($scope.tasksAsJSONString);

                    if (forceScopeApply)
                        $scope.$apply();
                }
            }

            $scope.getCurrentTaskStepsByTaskUid = function (taskUid) {
                var task = $scope.getTaskByUid(taskUid);
                return task ? task.steps : [];
            }

            $scope.getTaskByUid = function(taskUid) {
                var results = $scope.tasks.filter(function(task) {
                    return task.uid == taskUid;
                });
                return results && results.length > 0 ? results[0] : null;
            }

            $scope.taskHasSteps = function(task) {
                return task && task.steps && task.steps.length > 0;
            }
            

            // /FROM ADMIN

            $scope.$watch('currentStepUid', function (newVal, oldVal) {
                $log.log('current step updated: ', newVal);
                // Once initial image loads, use its height/width to position the tooltip
                $('.step-container[data-stepUid=' + $scope.currentStepUid + '] .imageContainer img').load(function () {
                    $log.log('image loaded: ', this);
                    setTooltipPosition(this);
                });
            });

            $scope.tabClick = function (task) {
                $scope.currentTaskUid = task.uid;
                $scope.currentStepUid = task.steps && task.steps.length > 0 ? task.steps[0].uid : null;
            }

            $scope.stepClick = function(step) {
                $scope.currentStepUid = step.uid;
            }

            $scope.nextStepClick = function (task, step, stepIndex) {
                // pre-condition: we know we have at least 1 step (task.steps is not empty), therefore:
                // 1 -> many steps. If we are already at the last step, nextStep will evaluate to null
                // and we will then attempt to navigate to the next task.
                var nextStep = task.steps && task.steps.length >= stepIndex + 2 ?
                    task.steps[stepIndex + 1] : ($log.log('last step, go to next task'), null);

                if (nextStep) { // we have a next step
                    $scope.currentStepUid = nextStep.uid;
                } else { // no next step, attempt to retrieve next task
                    // TODO
                }
            }

            $scope.previousStepClick = function (task, step, stepIndex) {
                // pre-condition: we know we have at least 2 steps and that the previous button
                // is only available on the 2nd+ step; therefore, we can safely assume that
                // there will always be stepIndex - 1 steps. Still, handle with caution.
                var prevStep = task.steps && stepIndex - 1 >= 0 ?
                    task.steps[stepIndex - 1] : ($log.log('last step, go to next task'), null);

                if (prevStep) { // we have a next step
                    $scope.currentStepUid = prevStep.uid;
                } else {
                    // do nothing
                }
            }
            
            //Convert the step description to HTML link if there is a URL present
            $scope.convertToHTML = function (description) {
                replacePattern = /(\b(https?):\/\/[-A-Z0-9+&amp;@#\/%?=~_|!:,.;]*[-A-Z0-9+&amp;@#\/%=~_|])/ig;
                replacedText = description.replace(replacePattern, '<a title="$1" href="$1" target="_blank">$1</a>');
                return $sce.trustAsHtml(replacedText);
            };

            // Calculate the position of the tooltip based on the dimensions of the image
            function setTooltipPosition(obj) {
                $log.log('selected step image loaded');

                var $parentStepContainer = $(obj).closest('.step-container');
                var $stepTooltip = $(obj).closest('.imageContainer').siblings('.step-tooltip');

                $parentStepContainer.css({ 'background-color': 'red !important' });

                var containerWidth = $parentStepContainer.width();
                var containerHeight = $parentStepContainer.height();
                var tooltipWidth = $stepTooltip.width();
                var tooltipHeight = $stepTooltip.height();

                $log.log(containerWidth, containerHeight);
                $log.log(tooltipWidth, tooltipHeight);

                // for each tooltip in step container

                var leftPos = $stepTooltip.attr('data-LeftPosition');
                var topPos = $stepTooltip.attr('data-TopPosition');
                $stepTooltip.css({ top: topPos + 'px', left: leftPos + 'px' });

                $log.log('current width: ', containerWidth);
                $log.log('current height: ', containerHeight);
                $log.log('left: ', leftPos + 'px');
                $log.log('top: ', topPos + 'px');
            }
        }
    ])
    .directive('provokeDraggable', ['$log', function ($log) {

        // TODO: make this more generic and reusable

        function link($scope, $element, attrs) {
            $element.draggable({
                containment: "parent",
                handle: '.drag-handle',
                drag: function (event, ui) {

                    var taskIndex = parseInt($element.attr('data-taskindex'));
                    var stepIndex = parseInt($element.attr('data-stepindex'));
                    var step = $scope.tasks[taskIndex].steps[stepIndex];
                    var leftPos = ui.position.left;
                    var topPos = ui.position.top;

                    var $parentContainer = $element.closest('.step-image-preview-container');
                    var containerWidth = $parentContainer.width();
                    var containerHeight = $parentContainer.height();
                    //var percent = percentagePosition(containerWidth, containerHeight, leftPos, topPos);

                    step.leftPosition = leftPos;
                    step.topPosition = topPos;

                    $log.log('drag dims: ', containerWidth, containerHeight);
                    $log.log('left: ', leftPos + 'px');
                    $log.log('top: ', topPos + 'px');

                    // this is required since we're in the context of a jQuery plugin
                    $scope.$apply();
                }
            });
        }

        return {
            link: link
        }
    }])
    .directive('provokeTextLimit', ['$log', function ($log) {
        $log.log('provoke-text-limit directive');

        // TODO: implement me
        // TODO: make this generic and reusable

        function link($scope, $element, attrs) {
            $log.log($element);
        }
        return {
            link: link
        };
    }]);

    function generateRandomId() {
        var one = Math.floor(100000 + Math.random() * 900000);
        var two = Math.floor(100000 + Math.random() * 900000);
        var three = Math.floor(100000 + Math.random() * 900000);
        return one + '-' + two + '-' + three;
    }

    function HighlightModule(data) {
        this.id = data.Id;
        this.uid = generateRandomId();
        this.description = data.Description;
        this.title = data.Title;
        this.tasks = data.Tasks;
    }

    function Task(data) {
        this.id = data.Id;
        this.title = data.Title || "";
        this.duration = data.Duration || "";
        this.description = data.Description || "";
        this.sortOrder = data.SortOrder;
        this.steps = [];
        this.uid = generateRandomId();
        for (var stepIndex in data.Steps) {
            this.steps.push(new Step(data.Steps[stepIndex]));
        }
    }

    function Step(data) {
        this.id = data.Id;
        this.title = data.Title;
        this.description = data.Description;
        this.sortOrder = data.SortOrder;
        this.leftPosition = data.LeftPosition;
        this.topPosition = data.TopPosition;
        this.anchor = data.Anchor;
        this.image = data.Image;
        this.uid = generateRandomId();
    }

    function getInitialCurrentTaskUid(tasks) {
        return tasks.length > 0 ? tasks[0].uid : -1;
    }

    function getInitialCurrentStepUid(tasks) {
        return tasks.length > 0 && tasks[0].steps.length > 0 ? tasks[0].steps[0].uid : -1;
    }

})(jQuery);