ngSpaApp.controller("ngSpaAppController", function ($scope, ngSpaAppService) {
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
                console.log("service loaded, check below");
                console.log(response.data);
            })
    }

});