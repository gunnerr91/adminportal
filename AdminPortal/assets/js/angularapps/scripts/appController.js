ngSpaApp.controller("ngSpaAppController", function ($scope, $uibModal, ngSpaAppService) {
    $scope.test = "hello world";
    $scope.init = function () {
        $scope.load($scope.selectedYear);
        $scope.generateYearDropdown();
    }

    $scope.generateYearDropdown = function () {
        var max = new Date().getFullYear();
        var range = [];
        for (var i = 2015; i <= max; i++) {
            range.push(i);
        };
        $scope.years = range;
    }

    $scope.load = function (selectedYear) {
        ngSpaAppService
            .load(selectedYear)
            .then(function (response) {
                $scope.YearList = response.data;
            })
    }

    $scope.quickAddEmployeeModal = function () {
        var modalInstance = $uibModal.open({
            size: "md",
            animation: true,
            templateUrl: "quick-add-modal.html",
            controller: "ngQuickAddEmployeeModalController"
        })
    }

    $scope.quickAddNewEntryModal = function () {
        var modalInstance = $uibModal.open({
            size: "md",
            animation: true,
            templateUrl: "quick-add-new-entry-modal.html",
            controller: "ngQuickAddNewEntryModalController"
        })
    }

    $scope.quickEditEntryModal = function (entry, index, existingList) {
        var modalInstance = $uibModal.open({
            size: "md",
            animation: true,
            templateUrl: "quick-edit-entry-modal.html",
            controller: "ngQuickEditEntryModalController",
            resolve: {
                entry: function () {
                    return entry;
                },
                index: function () {
                    return index;
                }, 
                existingList: function () {
                    return existingList;
                } 
            }
        })
    }

    $scope.delete = function (yearId) {
        ngSpaAppService
            .deleteEntry(yearId)
            .then(function (response) {
                $scope.load($scope.selectedYear);
            })
    }
});

ngSpaApp.controller('ngQuickEditEntryModalController',
    function ($scope, $uibModalInstance, ngSpaAppService, $http, entry, index, existingList) {
        console.log(existingList);
        $scope.employeeName = entry.EmployeeName;
        $scope.yearTotal = entry.YearTotal;
        $scope.YearList = existingList;
        $scope.jsonObjectForNewEmployeeModel = {};
        $scope.save = function () {
            $scope.jsonObjectForNewEmployeeModel.EmployeeName = $scope.employeeName;
            $scope.jsonObjectForNewEmployeeModel.YearTotal = $scope.yearTotal;
            $scope.jsonObjectForNewEmployeeModel.YearId = entry.YearId;
            ngSpaAppService
                .saveEntryAmendments($scope.jsonObjectForNewEmployeeModel)
                .then(function (response) {
                    $scope.YearList[index].EmployeeName = $scope.employeeName;
                    $scope.YearList[index].YearTotal = $scope.yearTotal;
                    $uibModalInstance.dismiss('cancel');

                },
                function (response) {
                    console.log("Error occured while trying to save the entry amendments");
                }
            )

        }


        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

    });

ngSpaApp.controller('ngQuickAddNewEntryModalController',
    function ($scope, $uibModalInstance, ngSpaAppService, $http) {
        $scope.datetime_options = {
            sideBySide: true,
            format: 'DD/MM/YYYY'
        };
        var max = new Date().getFullYear();
        var range = [];
        for (var i = 2015; i <= max; i++) {
            range.push(i);
        };
        $scope.years = range;
        $scope.employees = [];
        ngSpaAppService
            .getEmployeeList()
            .then(function (response) {
                $scope.employees = response.data;
            });

        $scope.load = function (selectedYear) {
            ngSpaAppService
                .load(selectedYear)
                .then(function (response) {
                    $scope.YearList = response.data;
                })
        }

        $scope.jsonObjectForNewEmployeeModel = {};

        $scope.save = function () {
            $scope.jsonObjectForNewEmployeeModel.EmployeeId = $scope.employee;
            $scope.jsonObjectForNewEmployeeModel.BusinessYear = $scope.selectedYear;
            $scope.jsonObjectForNewEmployeeModel.CurrentSalaryStartDate = moment($scope.startDate);
            $scope.jsonObjectForNewEmployeeModel.CurrentSalaryEndDate = moment($scope.endDate);
            $scope.jsonObjectForNewEmployeeModel.OtherSalaryStartDate = moment($scope.otherSalaryStart);
            $scope.jsonObjectForNewEmployeeModel.OtherSalaryEndDate = moment($scope.otherSalaryEnd);
            $scope.jsonObjectForNewEmployeeModel.OtherSalary = $scope.otherSalary;
            $scope.jsonObjectForNewEmployeeModel.SalesCommissionBonus = $scope.amountOfSales;
            $scope.jsonObjectForNewEmployeeModel.LoyaltyBonus = $scope.loyaltyBonus;
            $scope.jsonObjectForNewEmployeeModel.ReferalBonus = $scope.references;
            $scope.jsonObjectForNewEmployeeModel.OtherBonus = $scope.otherBonus;
            $scope.jsonObjectForNewEmployeeModel.HolidayBonus = $scope.notTakenHoliday;
            $scope.jsonObjectForNewEmployeeModel.MissionBonus = $scope.missionBonus;


            ngSpaAppService
                .quickAddNewEntry($scope.jsonObjectForNewEmployeeModel, $scope.selectedYear)
                .then(function (response) {
                    $scope.load($scope.selectedYear);
                    console.log("it is done");
                    $uibModalInstance.dismiss('cancel');
                })

            console.log($scope.jsonObjectForNewEmployeeModel);
        }


        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

    });


ngSpaApp.controller('ngQuickAddEmployeeModalController',
    function ($scope, $uibModalInstance, ngSpaAppService, $http) {
        $scope.datetime_options = {
            sideBySide: true,
            format: 'DD/MM/YYYY'
        };

        $scope.jsonObjectForNewEmployeeModel = {};

        $scope.save = function () {
            $scope.jsonObjectForNewEmployeeModel.Name = $scope.employeeName;
            $scope.jsonObjectForNewEmployeeModel.Department = $scope.employeeDepartment;
            $scope.jsonObjectForNewEmployeeModel.DateJoined = moment($scope.dateJoined);
            $scope.jsonObjectForNewEmployeeModel.ContractEndDate = moment($scope.endDate);
            $scope.jsonObjectForNewEmployeeModel.DateOfBirth = moment($scope.dateOfBirth);
            $scope.jsonObjectForNewEmployeeModel.CurrentSalary = $scope.employeeSalary;

            ngSpaAppService
                .addNewEmployee($scope.jsonObjectForNewEmployeeModel)
                .then(function (response) {
                    $uibModalInstance.dismiss('cancel');
                })

        }

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };


    });