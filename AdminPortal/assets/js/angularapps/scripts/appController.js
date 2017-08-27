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
        console.log($scope.years);
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