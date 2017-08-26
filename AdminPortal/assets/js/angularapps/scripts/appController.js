ngSpaApp.controller("ngSpaAppController", function ($scope, ngSpaAppService) {
    $scope.test = "hello world";
    $scope.init = function () {
        $scope.load();
    }

    $scope.load = function () {
        ngSpaAppService
            .load()
            .then(function (response) {
                $scope.YearList = response.data;
                console.log("service loaded, check below");
                console.log(response.data);
            })
    }

});