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
                    $scope.load();
                    console.log("it is done");
                    $uibModalInstance.dismiss('cancel');
                })

            console.log($scope.jsonObjectForNewEmployeeModel);
        }
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